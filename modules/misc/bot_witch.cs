datablock fxDTSBrickData (BrickZombieWitch_HoleSpawnData : BrickCommonZombie_HoleSpawnData)
{
    uiName = "Zombie Witch Hole";
    iconName = "Add-Ons/Gamemode_Left4Block/modules/misc/models/icons/icon_witch";
    
    holeBot = "ZombieWitchHoleBot";
};

datablock PlayerData(ZombieWitchHoleBot : CommonZombieHoleBot)
{
    shapeFile = ClawsMDts.baseShape;
    uiName = "";
    maxdamage = 2500;//Health
    hTickRate = 4000;

    hName = "Witch";//cannot contain spaces
    hMeleeCI = "Witched";
    hZombieL4BType = "Special";
	hSearch = false;
    runforce = 100 * 75;
    hAttackDamage = $Pref::L4B::Zombies::SpecialsDamage*5;//15;//Melee Damage
    resistMelee = true;
};

function ZombieWitchHoleBot::onAdd(%this,%obj)
{
    Parent::onAdd(%this,%obj);
    CommonZombieHoleBot::onAdd(%this,%obj);

    %obj.StartleLoop = %obj.getDatablock().WitchStartleLoop(%obj);
    
    if(getRandom(1,100) <= 95)
    {
        %obj.stopHoleLoop();
        serverCmdSit(%obj);
    }
}

function ZombieWitchHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	%obj.onL4BDatablockAttributes();    
    %obj.setscale("0.9 0.7 1");
}

function ZombieWitchHoleBot::onCollision(%this, %obj, %col, %fade, %pos, %norm)
{
    Parent::onCollision(%this, %obj, %col, %fade, %pos, %norm);
    
    if(%col.getType() & $TypeMasks::PlayerObjectType && minigameCanDamage(%obj,%col) && checkHoleBotTeams(%obj,%col))
    {
	    if(%obj.hMelee && %obj.lastattackis < getSimTime())
	    {
	    	%obj.hMeleeAttack(%col);
	    	%obj.lastattackis = getSimTime()+500;
	    }
    }
}

function ZombieWitchHoleBot::onBotMelee(%this,%obj,%col)
{
    CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
    %obj.playaudio(2,"hunter_hit" @ getrandom(1,3) @ "_sound");
}

function ZombieWitchHoleBot::onBotLoop(%this,%obj)
{
    %obj.hNoSeeIdleTeleport();

    if(%obj.hMelee)
    {
        if(!%obj.hFollowing)
        {
            if(%obj.raisearms)
            {   
                %obj.playthread(0,"armReadyboth");
                %obj.raisearms = 0;
            }
            %obj.playthread(1,"root");
        }
    }
    else
    {
        if(%obj.hFollowing)
        %obj.hRunAwayFromPlayer(%obj.hFollowing);
        
        %obj.playaudio(0,"witch_horrified" @ getrandom(1,2) @ "_sound");
        %obj.hFollowing = 0;
        %obj.playthread(2,"plant");
        %obj.playthread(1,"root");
        %obj.setMaxForwardSpeed(15);
        %obj.hRunAwayFromPlayer(%obj);
        %obj.MaxSpazzClick = getRandom (20,40);
        %obj.getDatablock().WitchPanicking(%obj,%count);
    }
}

function ZombieWitchHoleBot::WitchOnLoop(%this,%obj)
{
    if(%obj.getstate() $= "Dead") return;
    
    if(%obj.WitchOnLoop+4000 < getsimtime())
    {
        if(%obj.hMelee)
        {
            if(%obj.hFollowing)
            %obj.playaudio(0,"witch_attack" @ getrandom(1,2) @ "_sound");
        }
        else %obj.playaudio(0,"witch_horrified" @ getrandom(1,2) @ "_sound");

        %obj.WitchOnLoop = getsimtime();
    }
}

function ZombieWitchHoleBot::WitchPanicking(%this,%obj,%count)
{
    %time = getRandom(100,200);
    if(isObject(%obj) && %count != %obj.MaxSpazzClick)
    {
        %obj.activateStuff();
        cancel(%obj.WitchPanicking);
        %obj.WitchPanicking = %obj.getDataBlock().schedule(%time,WitchPanicking,%obj,%count+1);
    }
}

function ZombieWitchHoleBot::onBotFollow( %this, %obj, %targ )
{
    Parent::onBotFollow(%this,%obj,%targ);

	if(!%obj.startMusic)
	{
		if(isObject(%minigame = getMiniGameFromObject(%obj)))
        %minigame.DirectorMusic("musicdata_L4D_witch",true,1,%client);

		%obj.startMusic = 1;
	}

    %obj.setMaxForwardSpeed(20);
    if(!%obj.raisearms)
    {   
        %obj.playthread(1,"armReadyboth");
        %obj.raisearms = 1;
    }
    
}

function ZombieWitchHoleBot::OnRemove(%this,%obj)
{
    if(isObject(%minigame = getMiniGameFromObject(%obj))) %minigame.RoundEnd();
    Parent::OnRemove(%this,%obj);
}

function ZombieWitchHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	if(%damageType !$= $DamageType::FallDamage || %damageType !$= $DamageType::Impact) %damage = %damage/2.5;
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}

function ZombieWitchHoleBot::OnDamage(%this,%obj,%am)
{
    if(%obj.getstate() $= "Dead") return;

    if(%obj.lastdamage+750 < getsimtime())//Check if the chest is the male variant and add a 1 second cooldown
    {
        %obj.playaudio(0,"witch_pain" @ getrandom(1,3) @ "_sound");
        %obj.lastdamage = getsimtime();
    }

    if(isObject(getMiniGameFromObject(%obj)))
    {
        %attacker = %obj.hAttacker;
        %obj.hSearch = 1;

        if(!%obj.hLoopActive)
        %obj.startHoleLoop();

        cancel(%obj.StartleLoop);
        %obj.getDataBlock().WitchOnLoop(%obj);
        
        %obj.setMaxForwardSpeed(20);
        %obj.onBotFollow();

        if(%attacker && %attacker.client && %attacker.getclassname() $= "player" && !%obj.hWhoAttacked)
        {
            chatMessageTeam(%attacker.client,'fakedeathmessage',"<color:FFFF00>" @ %attacker.client.name SPC "<bitmapk:Add-Ons/Gamemode_Left4Block/modules/misc/models/icons/ci_skull2>" SPC %obj.getDatablock().hName);
            %minigame = %attacker.client.minigame;
            MinigameSO::L4B_PlaySound(%col.client.minigame,"victim_needshelp_sound");
            %obj.hWhoAttacked = 1;
        }

        if(%obj.GetDamageLevel() >= 750 && %obj.hMelee)
        %obj.hMelee = 0;
    }
    
    Parent::onDamage(%this,%obj,%Am);
}

function ZombieWitchHoleBot::onDisabled(%this,%obj)
{
    	if(%obj.getstate() !$= "Dead")
    	return;

        %obj.playaudio(0,"witch_death" @ getrandom(1,3) @ "_sound");

        if(isObject(%minigame = getMiniGameFromObject(%obj)) && !%minigame.finalround) %minigame.RoundEnd();

    	Parent::OnDisabled(%this,%obj);
}

function ZombieWitchHoleBot::WitchStartleLoop(%this,%obj)
{
    if(!isObject(%obj.spawnbrick) || !isObject(%obj.spawnbrick) || %obj.getState() $= "Dead") return;

    %time = 1000;
    %pos = %obj.getPosition();
    %radius = 4;
    %searchMasks =  $TypeMasks::PlayerObjectType;
    InitContainerRadiusSearch(%pos, %radius, %searchMasks);
    while (%targetid = containerSearchNext())
    {
        if(%targetid == %obj) continue;
        if(isObject(containerRayCast(%obj.getEyePoint(),vectorAdd(%targetid.getPosition(),"0 0 1.9"),$TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType,%obj))) continue;

        if(%targetid.getState() !$= "Dead" && minigameCanDamage(%obj,%targetid) && isObject(getMiniGameFromObject(%targetid)) && checkHoleBotTeams(%obj,%targetid))
        {
            %obj.StartleCount = mClamp(%obj.StartleCount+2, 0, 11);
            %obj.setaimobject(%targetid);

            %target = %targetid;
        }
    }
    %obj.StartleCount = mClamp(%obj.StartleCount--, 0, 11);

    if(!%obj.startlecount)
    {
        if(%obj.CryDelay+getRandom(5000,6000) < getsimtime())
        {
            serverCmdSit(%obj);
            %obj.PlayThreadOnce = 1;
            %obj.playaudio(0,"witch_cry" @ getrandom(1,2) @ "_sound");
            %obj.playthread(2,"Plant");

            %obj.CryDelay = getsimtime();
        }

        if(%obj.hSpawnCue+15000 < getsimtime())
        {
            %obj.playaudio(3,strlwr(%obj.name) @ "_spawn" @ getRandom(1,2) @ "_sound");
            %obj.hSpawnCue = getsimtime();
        }
    }
    else
    {
        %obj.playaudio(0,"witch_surprised_sound");
        %time = 1000;

        if(%obj.startlecount > 1)
        {
            %obj.playaudio(0,"witch_growl0_sound");
            %time = 750;

            if(%obj.startlecount > 3)
            {
                %obj.playaudio(0,"witch_growl1_sound");
                %time = 1000;

                if(!%obj.hLoopActive && %obj.startlecount < 7)
                %obj.setcrouching(0);

                if(%obj.StartleCount > 5)
                {
                    %obj.playaudio(0,"witch_growl2_sound");
                    %obj.mountImage(HateImage, 3);

                    if(!%obj.PlayThreadOnce)
                    {
                        %obj.playthread(1,"root");
                        %obj.PlayThreadOnce = 1;
                    }
                    if(!%obj.hLoopActive)
                    {
                        serverCmdSit(%obj);
                        %obj.setcrouching(1);
                    }
                    
                    if(%obj.StartleCount > 7)
                    {
                        %obj.playaudio(0,"witch_growl3_sound");

                        if(%obj.StartleCount > 8)
                        {
                            %time = 2000;
                            %obj.playaudio(0,"witch_angry" @ getrandom(1,2) @ "_sound");
                            %obj.hJump();
                            %obj.setcrouching(0);

                            if(%obj.hAngryCue+4000 < getSimTime())
                            {
                                serverPlay3D("witch_cue_angry" @ getRandom(1,2) @ "_sound",%obj.getPosition());
                                %obj.hAngryCue = getsimtime();
                            }

                            if(%obj.StartleCount == 10)
                            {
                                if(%target.client && %target.getclassname() $= "player")
                                {
                                    chatMessageTeam(%target.client,'fakedeathmessage',"<color:FFFF00>" @ %target.client.name SPC "<bitmapk:Add-Ons/Gamemode_Left4Block/modules/misc/models/icons/ci_witchclose>" SPC %obj.getDatablock().hName);
                                    %minigame = %target.client.minigame;
                                    MinigameSO::L4B_PlaySound(%col.client.minigame,"victim_needshelp_sound");
                                }
                                %obj.hSearch = 1;
                                cancel(%obj.StartleLoop);
                                %obj.playthread(2,"Plant");

                                %obj.startHoleLoop();
                                %obj.getDataBlock().WitchOnLoop(%obj);
                                return;
                            }
                        }
                    }  
                }
            }
        }

        %obj.playthread(2,"Plant");
    }
    cancel(%obj.StartleLoop);
    %obj.StartleLoop = %obj.getDataBlock().schedule(%time,WitchStartleLoop,%obj);
}

function ZombieWitchHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{
        %skinColor = "0.1 0.1 0.1 1";
        %obj.hipColor =  "0.3 0.3 0.3 1";
        %obj.decalName = "witch";
        %obj.faceName = %face;
        %obj.larmColor =  %skinColor;
        %obj.secondPack =  "0";
        %obj.larm =  "1";
        %obj.chestColor =  %skinColor;
        %obj.accentColor =  "0 0 0 0";
        %obj.rhandColor =  %skinColor;
        %obj.rleg =  "0";
        %obj.rlegColor =  %skinColor;
        %obj.accent =  "0";
        %obj.headColor =  %skinColor;
        %obj.rhand =  "0";
        %obj.lleg =  "0";
        %obj.lhandColor =  %skinColor;
        %obj.hat =  "0";
        %obj.llegColor =  %skinColor;
        %obj.secondPackColor =  "0 0 0 0";
        %obj.lhand =  "0";
        %obj.hip =  "0";
        %obj.rarmColor =  %skinColor;
        %obj.hatColor =  "0 0 0 0";
        %obj.chest =  "1";
        %obj.rarm =  "1";
        %obj.packColor =  "0 0 0 0";
        %obj.pack =  "0";

        GameConnection::ApplyBodyParts(%obj);
        GameConnection::ApplyBodyColors(%obj);
}

function ZombieWitchHoleBot::L4BAppearance(%this,%obj,%client)
{
    Parent::L4BAppearance(%this,%obj,%client);
}