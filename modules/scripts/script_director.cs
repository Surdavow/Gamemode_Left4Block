package L4B_Director
{
    function MiniGameSO::Reset(%minigame,%client)
	{
        Parent::Reset(%minigame,%client);
        %minigame.director(5);        
	}	

    function MiniGameSO::endGame(%minigame)
    {
		Parent::endGame(%minigame);
        %minigame.director(5);
        %minigame.director.delete();
        showAreaZones(true);
    }

	function MiniGameSO::checkLastManStanding(%minigame)
	{
	    if(%minigame.RespawnTime > 0 || isEventPending(%minigame.resetSchedule)) return;
	
	    for(%i = 0; %i < %minigame.numMembers; %i++) if(isObject(%player = %minigame.member[%i].player) && !%player.hIsInfected && !%player.getdataBlock().isDowned) %livePlayerCount++;
	
	    if(!%livePlayerCount)
	    {	        
            if(isObject(%minigame.director))
            {
	            %minigame.director(false);
                %minigame.director.l4bMusic("game_lose_sound",false,"Music");
            }
	        %minigame.scheduleReset(12000);
	    }
	}    
};

if(isPackage(L4B_Director)) deactivatePackage(L4B_Director);
activatePackage(L4B_Director);

registerOutputEvent(Minigame, "Director", "List Disable 0 Enable 1 Horde 2 Tank 3 Panic 4 Safehouse 5 Reset",0);
registerOutputEvent(Minigame, "DirectorCCR", "bool",0);

function MinigameSO::Director(%minigame,%type,%client)
{
    switch(%type)
    {
        case 0: if(isObject(%minigame.director)) %minigame.director.stop();       
        
        case 1: if(!isObject(%minigame.director))
                {
                    %minigame.director = new ScriptObject(Director);
                    %minigame.director.minigame = %minigame;
                }
                
                %minigame.director.canChangeRound = true;
                %minigame.director.roundUpdate();
        
        case 2: if(isObject(%minigame.director))
                {
                    %minigame.director.roundType = "horde";
                    %minigame.director.canChangeRound = true;
                    %minigame.director.roundUpdate();
                }

        case 3: if(isObject(%minigame.director))
                {
                    %minigame.director.roundType = "panic";
                    %minigame.director.canChangeRound = true;
                    %minigame.director.roundUpdate();
                }

        case 4: if(isObject(%minigame.director))
                {
                    %minigame.director.roundType = "tank";
                    %minigame.director.canChangeRound = true;
                    %minigame.director.roundUpdate();
                }

        case 5: if(!isObject(%minigame.director))
                {
                    %minigame.director = new ScriptObject(Director);
                    %minigame.director.minigame = %minigame;
                }

                if(isObject(%brickgroup = %client.brickgroup) && %brickgroup.getCount())
                for(%i = 0; %i < %brickgroup.getcount(); %i++) if(isObject(%brick = %brickgroup.getObject(%i)))
                {            
                    if(%brick.getName() $= "_breakbrick")
                    {
                        %brick.setRendering(1);
                        %brick.setRaycasting(1);
                        %brick.setColliding(1);
                        %brick.setName("");
                    }
                    if(%brick.getdataBlock().isOpen) %brick.door(close);
                }

                if(isObject(DecalGroup)) DecalGroup.deleteAll();
                if(isObject(%minigame.director.ZombieGroup)) %minigame.director.ZombieGroup.delete();
                if(isObject(AreaZoneGroup)) for(%i = 0; %i < AreaZoneGroup.getCount(); %i++)
                if(isObject(%zone = AreaZoneGroup.getObject(%i)))
                {
                    %zone.firstentry = false;
                    %zone.presenceallentered = false;
                    for(%j = 0; %j < %zone.simset.getCount(); %j++)
                    if(isObject(%brick = %zone.simset.getObject(%j)) && %brick.getdataBlock().ZoneBrickType $= "item") %brick.setItem(none);
                }        

                showAreaZones(0);
                %minigame.director.schedule(1900,l4bMusic,"musicdata_L4D_safearea" @ getRandom(1,4),true,"Music");
                %minigame.director.l4bMusic("game_start_sound",false,"Stinger1");
        default:     
    }
}

function MinigameSO::DirectorCCR(%minigame,%bool)
{
    if(!isObject(%director = %minigame.director)) return;
    return %minigame.director.canChangeRound = %bool;
}

function Director::onRemove(%obj)
{
    cancel(%obj.roundUpdateSched);
}

function Director::Stop(%obj)
{
    %obj.canChangeRound = false;
    %obj.roundType = "";    
    %obj.deletel4bMusic("Music");
    %obj.deletel4bMusic("Music2");
    %obj.deletel4bMusic("Stinger1");
    %obj.deletel4bMusic("Stinger2");
    %obj.deletel4bMusic("Stinger3");
    %obj.deletel4bMusic("Ambience"); 
    cancel(%obj.roundUpdateSched);
}

function Director::roundUpdate(%obj)
{
    if(!isObject(%obj) || !isObject(%minigame = %obj.minigame))
    { 
        if(isObject(%obj)) %obj.delete();
        return;
    }

    %obj.interval++;

    if(%obj.roundType $= "Panic")
    {
        %obj.spawnZombies("Horde",10);
        %obj.spawnZombies("special",2);      
        if(getRandom(1,10) == 1 ) %obj.spawnZombies("Tank",1);
    }
    else switch(%obj.interval)
    {
        case 1: %cue = true;
        case 2: if(getRandom(1,2) == 1) %cue = true;
                %obj.spawnZombies("horde",getRandom(2,5));
                %obj.spawnZombies("special",getRandom(1,2));

        case 3: for(%i=0; %i < %minigame.numMembers; %i++)
                {
                    %fixcount = %i+1;
                    if(isObject(%mgmember = %minigame.member[%i]))
                    {                        
                        if(isObject(%mgmember.player) && %mgmember.player.getdataBlock().isSurvivor)
                        {                              
                                %health += 100-%mgmember.player.getdamagelevel();
                                %stresslevel += %mgmember.player.survivorStress;
                                %mgmember.player.survivorStress = 0;
                        }
                        else %stresslevel += 20;
                                                
                        %health = mClamp(%health,0,%obj.survivorHealthMax);
                        %stresslevel = mClamp(%stresslevel,0,%obj.survivorStressMax);                        
                        %obj.survivorHealthAverage = %health;
                        %obj.survivorStressAverage = %stresslevel;
                        %obj.survivorHealthMax = 100*%fixcount;
                        %obj.survivorStressMax = 20*%fixcount;                        

                        if(%obj.survivorStressAverage > %obj.survivorStressMax/1.5 && %obj.survivorHealthAverage < %obj.survivorHealthMax/2.5) %stressed = true;
                    }
                }

                if(!%obj.canChangeRound)
                {
                    if(%obj.roundType $= "")
                    {
                        %chance = getRandom(1,100);
                        if(%chance <= 80) %obj.roundType = "horde";
                        if(%chance <= 40) %obj.roundType = "tank";
                        if(%stressed) %obj.roundType = "break";
                    }

                    switch$(%obj.roundType)
                    {
                        case "break":   %music = "musicData_L4D_quarantine_" @ getRandom(1,3);
                                        %obj.canChangeRound = true;

                        case "horde":   if(%obj.spawnZombies("horde",getRandom(25,35)))
                                        {
                                            %minigame.L4B_ChatMessage("[They're coming...]","hordeincoming" @ getrandom(1,9) @ "_sound",true);                                           
                                            %obj.canChangeRound = false;
                                        }

                        case "tank":    if(%obj.tankRoundCount > 1 || !%obj.spawnZombies("Tank",1))
                                        {
                                            %obj.canChangeRound = false;
                                            %obj.tankRoundCount++;
                                        }

                        case "panic":   %minigame.L4B_ChatMessage("[They're coming!] <bitmapk:Add-Ons/Gamemode_Left4Block/modules/add-ins/player_l4b/icons/ci_skull2>","hordeincoming" @ getrandom(1,9) @ "_sound",true); 
                                        %minigame.l4bMusic("musicData_L4D_horde_urgent",false,"Music2");
                                        %obj.canChangeRound = false;
                    }
                }                            

                if(getRandom(1,2) == 1) %cue = true;
    }

    if(%music !$= "" && !%obj.MusicActive)
    {
        %minigame.deletel4bMusic("Music");
        %minigame.deletel4bMusic("Music2");
        %minigame.l4bMusic(%music,true,"Music");
        %obj.MusicActive = true;
    }
    
    if(%cue && %obj.MusicActive) switch$(%obj.roundType)
    {
        case "Break":   if(getRandom(1,2) == 1) %minigame.l4bMusic("zombiechoir_0" @ getrandom(1,6) @ "_sound",false,"Stinger1"); 
                        if(getRandom(1,8) == 1) %minigame.l4bMusic("aglimpseofhell_" @ getrandom(1,3) @ "_sound",false,"Stinger2");
        case "Horde":   if(getRandom(1,2) == 1) %minigame.l4bMusic("hordeslayer_0" @ getrandom(1,3) @ "_sound",false,"Stinger1"); 
                        if(getRandom(1,10) == 1 || %interval == 3) %minigame.l4bMusic("horde_danger_sound",false,"Stinger2");
    }    
    
    if(%obj.interval >= 3) %obj.interval = 0;    

    cancel(%obj.roundUpdateSched);
    %obj.roundUpdateSched = %obj.scheduleNoQuota(10000,roundUpdate,%minigame);
}

function Director::RoundEnd(%obj)
{
    if(!isObject(%obj) || !isObject(%minigame = %obj.minigame) || %obj.canChangeRound) return;    
    
    %obj.roundType = "";
    %obj.MusicActive = false;
    %obj.canChangeRound = true;
    %obj.l4bMusic("drum_suspense_end_sound",false,"Stinger1");
    %obj.deletel4bMusic("Music");
    %obj.deletel4bMusic("Music2");
    %obj.deletel4bMusic("Stinger1");
    %obj.deletel4bMusic("Stinger2");
    %obj.deletel4bMusic("Stinger3");     
}

function Director::l4bMusic(%obj, %datablock, %loopable, %type)
{
    if(!isObject(%obj) || !isObject(%minigame = %obj.minigame)) return;

    for(%i = 0; %i < %minigame.numMembers; %i++)   
    if(isObject(%mgmember = %minigame.member[%i]))
    {
        if((isObject(%player = %mgmember.player) && %player.isBeingStrangled)) return;
        %mgmember.l4bMusic(%dataBlock,%loopable,%type);        
    }
}

function Director::deletel4bMusic(%obj, %type)
{
    if(!isObject(%obj) || !isObject(%minigame = %obj.minigame)) return;
    
    for(%i = 0; %i < %minigame.numMembers; %i++)
    if(isObject(%mgmember = %minigame.member[%i])) %mgmember.deletel4bMusic(%type);
}

function GameConnection::l4bMusic(%client, %datablock, %loopable, %type)
{  
    if(!isObject(%datablock)) return;
    if(isObject(%client.l4bMusic[%type])) %client.l4bMusic[%type].delete();

    switch$(%type)
    {
        case "Music": %channel = 10;
        case "Music2": %channel = 10;
        case "Stinger1": %channel = 11;
        case "Stinger2": %channel = 11;
        case "Stinger3": %channel = 11;
        case "Ambience": %channel = 12;
        default: return;
    }

    %client.l4bMusic[%type] = new AudioEmitter(l4b_music)
    {
        profile = %datablock;
        isLooping= %loopable;
        position = "9e9 9e9 9e9";
        is3D = false;
        useProfileDescription = false;
        type = %channel;
    };
    %client.l4bMusic[%type].setNetFlag(6, true);
    %client.l4bMusic[%type].scopeToClient(%client);
}

function GameConnection::deletel4bMusic(%client, %type)
{
    if(isObject(%client.l4bMusic[%type])) %client.l4bMusic[%type].delete();   
}

function GameConnection::l4bmusicCatchUp(%client)
{   
    %music_tags = "Music Music2 Stinger1 Stinger2 Stinger3 Ambience";
    for(%i = 0; %i < getWordCount(%music_tags); %i++)
    if(isObject(%music_object = $L4B_Music[getWord(%music_tags, %i)])) %music_object.scopeToClient(%client);
}

$L4B_lastSupportMessageTime = getSimTime();
function MiniGameSO::L4B_ChatMessage(%miniGame, %text, %sound, %bypassdelay)
{
    if(!%bypassdelay && getSimTime() - $L4B_lastSupportMessageTime < 15000) return;    
    announce(%text);
    %miniGame.l4bMusic(%sound,false,"Stinger3");
    $L4B_lastSupportMessageTime = getSimTime();
}

function Director::SafehouseCheck(%obj)
{
	if(!isObject(%obj) || !isObject(%minigame = %obj.minigame)) return;

    for(%i = 0; %i < %minigame.numMembers; %i++)
	{
		%client = %minigame.member[%i];
		if(isObject(%player = %client.player) && !%player.hIsInfected && %player.getdataBlock().getname() !$= "SurvivorPlayerDowned") %livePlayerCount++;
		if(isObject(%player) && %player.InSafehouse) %safehousecount++;
	}
	
	if(%safehousecount >= %livePlayerCount && isObject(%minigame))
	{
		if(isEventPending(%minigame.resetSchedule))	return;

		%minigame.scheduleReset(8000);
		%minigame.l4bMusic("game_win_sound",false,"Music");
		%minigame.deletel4bMusic("Stinger1");
		%minigame.deletel4bMusic("Stinger2");
		%minigame.deletel4bMusic("Stinger3");	

    	for(%i=0;%i<%minigame.numMembers;%i++)
    	if(isObject(%member = %minigame.member[%i]) && isObject(%member.player))
        {
            if(%member.player.hType $= "Survivors") %member.player.emote(winStarProjectile, 1);	
            %member.Camera.setOrbitMode(%member.player, %member.player.getTransform(), 0, 5, 0, 1);
            %member.setControlObject(%member.Camera);
        }
		return true;
	}
}

function Director::spawnZombies(%obj,%type,%amount,%spawnzone,%count)
{
    if(!isObject(%obj) || !isObject(%minigame = %obj.minigame)) return;
    
    if(!isObject(%spawnzone))//Just in case the zone wasn't listed then we can just choose from the area zone group and prioritize spawns
    {
        if(%type $= "Horde" || %type $= "Wander") %priority = 2;//Common zombies don't get the high priority so just make them spawn whereever
        else %priority = 1;//Specials, tanks and witches spawn with higher priority

        switch(%priority)
        {
            case 1: if(%minigame.numMembers) for(%i=0;%i <= %minigame.numMembers;%i++)//Create a list if there are lone survivors
                    if(isObject(%minigame.member[%i]) && isObject(%player = %minigame.member[%i].player) && %player.getdataBlock().isSurvivor && %player.currentZone.presencecount == 1)
                    {
                        %lonelysurvivorzone[%lsz++] = %player.currentZone;
                        %spawnzone = %lonelysurvivorzone[getRandom(1,%lsz)];
                    }

                    if(!%lsz)//Make a generic list in case there are no lone survivors
                    {
                        for(%i = 0; %i < AreaZoneGroup.getcount(); %i++) if(isObject(AreaZoneGroup.getObject(%i)) && AreaZoneGroup.getObject(%i).presencecount)
                        %activezones[%az++] = AreaZoneGroup.getObject(%i);
                        %spawnzone = %activezones[getRandom(1,%az)];
                    }
                    
                    //If there are no active zones, don't spawn anything
                    if(!%spawnzone) return;

            case 2: for(%i = 0; %i < AreaZoneGroup.getcount(); %i++) if(isObject(AreaZoneGroup.getObject(%i)) && AreaZoneGroup.getObject(%i).presencecount)
                    {
                        %activezones[%az++] = AreaZoneGroup.getObject(%i);
                        %spawnzone = %activezones[getRandom(1,%az)];
                    }

                    //If there are no active zones, don't spawn anything
                    if(!%spawnzone) return;
        }
    }    
    
    for(%i = 0; %i < %spawnzone.simset.getcount(); %i++)
    if(isObject(%setbrick = %spawnzone.simset.getObject(%i)) && %setbrick.getdataBlock().ZoneBrickType $= "spawner" && strstr(strlwr(%setbrick.getName()),"_" @ strlwr(%type)) != -1)
    {
        %spawnlist[%sb++] = %setbrick;
        %spawnlist[%sb].currentset = %spawnzone;
    }

    if(%sb && %count < %amount)
    {
        %random = getRandom(1,%sb);
        %spawnbrick = %spawnlist[%random];
        %zone = %spawnlist[%random].currentset;

        switch$(%type)
        {
            case "Wander": %bottype = "CommonZombieHoleBot";
            case "Horde": %bottype = "CommonZombieHoleBot";
                            //if(getRandom(1,8) == 1) %bottype = $hZombieUncommonType[getRandom(1,$hZombieUncommonTypeAmount)];
                            //if(getRandom(1,16) == 1 && $L4B_CurrentMonth == 10) %bottype = "SkeletonHoleBot";
            case "Tank": %bottype = "ZombieTankHoleBot";
            case "Witch": %bottype = "ZombieWitchHoleBot";    
            case "Special": %bottype = $hZombieSpecialType[getRandom(1,$hZombieSpecialTypeAmount)];
            
        }

        %bot = new AIPlayer()
        {
            dataBlock = %bottype;
            path = "";
            spawnBrick = %spawnbrick;
            spawnType = %type;
            Name = %bottype.hName;
            hType = %bottype.hType;
            hSearchRadius = %bottype.hSearchRadius;
            hSearch = %bottype.hSearch;
            hSight = %bottype.hSight;
            hWander = %bottype.hWander;
            hGridWander = false;
            hReturnToSpawn = false;
            hSpawnDist = %bottype.hSpawnDist;
            hMelee = %bottype.hMelee;
            hAttackDamage = %bottype.hAttackDamage;
            hSpazJump = false;
            hSearchFOV = %bottype.hSearchFOV;
            hFOVRadius = %bottype.hFOVRadius;
            hTooCloseRange = %bottype.hTooCloseRange;
            hAvoidCloseRange = %bottype.hAvoidCloseRange;
            hShoot = %bottype.hShoot;
            hMaxShootRange = %bottype.hMaxShootRange;
            hStrafe = %bottype.hStrafe;
            hAlertOtherBots = %bottype.hAlertOtherBots;
            hIdleAnimation = %bottype.hIdleAnimation;
            hSpasticLook = %bottype.hSpasticLook;
            hAvoidObstacles = %bottype.hAvoidObstacles;
            hIdleLookAtOthers = %bottype.hIdleLookAtOthers;
            hIdleSpam = %bottype.hIdleSpam;
            hAFKOmeter = %bottype.hAFKOmeter + getRandom( 0, 2 );
            hHearing = %bottype.hHearing;
            hIdle = %bottype.hIdle;
            hSmoothWander = %bottype.hSmoothWander;
            hEmote = %bottype.hEmote;
            hSuperStacker = %bottype.hSuperStacker;
            hNeutralAttackChance = %bottype.hNeutralAttackChance;
            hFOVRange = %bottype.hFOVRange;
            hMoveSlowdown = false;
            hMaxMoveSpeed = 1;
            hActivateDirection = %bottype.hActivateDirection;
            hGridPosition = %spawnbrick.getPosition();
            currentZone = %zone;
            isHoleBot = 1;
        };

        if(strlen(%bottype.hMeleeCI)) eval("%bot.hDamageType = $DamageType::" @ %bottype.hMeleeCI @ ";");
        else %bot.hDamageType = $DamageType::HoleMelee;        
        
        if(!isObject(%obj.ZombieGroup))
        {
            %obj.ZombieGroup = new SimGroup();
            missionCleanup.add(%obj.ZombieGroup);
        }
        
        %obj.ZombieGroup.add(%bot);
        %bot.doMRandomTele(%spawnbrick);

        cancel(%minigame.spawn[%type]);
        %minigame.spawn[%type] = %minigame.scheduleNoQuota(50,spawnZombies,%type,%amount,%spawnzone,%count++);
        return true;
    }
    else return false;
}