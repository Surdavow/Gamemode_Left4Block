if(LoadRequiredAddOn("Support_BotHolePlus") != $Error::None) return;

$AFKBotSet = new SimSet(AFKBotSet);
MissionCleanup.add($AFKBotSet);

//
// Datablock stuff. Mostly ripped from SurvivorHoleBot.
//

datablock PlayerData(AFKPlayerHoleBot : SurvivorHoleBot)
{
    isAFKPlayer = 1;
    hName = "AFK" SPC "Player";
    //Minigame and spawnbrick dependencies need to be removed.
    hooks_hLoop = "AFKPlayerHoleBot_loop";
    hooks_guardAtSpawn = -1;
    hooks_ifWander = "AFKPlayerHoleBot_ifWander";
};
function AFKPlayerHoleBot::onDisabled (%this, %obj, %state)
{
    parent::onDisabled(%this, %obj, %state);
    %client = %obj.afk_client;
    %client.camera.setMode("Corpse", %obj);
    %client.setControlObject(%client.camera);
}
function AFKPlayerHoleBot::onAdd(%this, %obj)
{
    //
    // Needs to be it's own function to remove the appearance changes brought about by the SurviviorHoleBot function.
    //
    
    Armor::onAdd(%this, %obj);

    //Can't assign the variables to the %obj.vars ScriptObject for some reason. Thanks.
    %obj.hIsImmune = 1;
    %obj.hHumanFellow = 0;
    %obj.hHelpTarget = 0;
    %obj.botherHumanCheckTimes = 8; //Needs to be moderately high to be effective. At least have it at 4 or so.
    %obj.botherHumanDistance = 4;
    %obj.hHelpTarget_previousHSearch = 0;

    if(getMiniGameFromObject(%obj))
    {
        %buddy = L4B_attemptFindHuman(%obj); //Right off the bat, find a cuddle buddy.
        if(isObject(%buddy))
        {
            L4B_survivorBotRunSchedule(%obj, %buddy);
        }
    }
}
function AFKPlayerHoleBot::GiveHealingStuff(%this,%obj,%human)
{    
    SurvivorHoleBot::GiveHealingStuff(%this,%obj,%human);
}
function AFKPlayerHoleBot::onBotLoop( %this, %obj )
{
    SurvivorHoleBot::onBotLoop(%this, %obj);
}
function AFKPlayerHoleBot::onBotCollision( %this, %obj, %col, %normal, %speed )
{
    SurvivorHoleBot::onBotCollision(%this, %obj, %col, %normal, %speed);
}
function AFKPlayerHoleBot::onDamage(%this,%obj,%am)
{
    SurvivorHoleBot::onDamage(%this, %obj, %am);
}
function AFKPlayerHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
    SurvivorHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}

//
// Item saving and loading functions.
//

function L4B_generateItemString(%player)
{
	if(!isObject(%player))
	{
		return;
	}
	%itemString = "";
	for(%i = 0; %i < %player.getDatablock().maxTools; %i++)
	{
        if(%itemString $= "")
        {
            %itemString = %player.tool[%i];
        }
        else
        {
            %itemString = %itemString SPC %player.tool[%i];
        }
	}
    return %itemString;
}

function L4B_loadItemString(%player, %string)
{
	if(!isObject(%player))
	{
		return;
	}
	%player.clearTools();
    for(%i = 0; %i < %player.getDatablock().maxTools; %i++)
    {
        if(%i > getWordCount(%string))
        {
            break;
        }
        %image = getWord(%string, %i);
        %player.tool[%i] = %image;
        %player.weaponCount++;
        messageClient(%player.client,'MsgItemPickup', '', %i, %image);
    }
}

//
// Spawning functions.
//

//Modified "spawnHoleBot" function from holes.cs to ignore pre-existing bots.
function L4B_AFKReplacePlayer(%target)
{
    %data = AFKPlayerHoleBot;
    %obj = new AIPlayer()
	{
		dataBlock = %data;
		path = "";
				
		//Apply attributes to Bot
		Name = %target.client.name;
		hType = %data.hType;
		hSearchRadius = %data.hSearchRadius;
		hSearch = %data.hSearch;
		hSight = %data.hSight;
		hWander = %data.hWander;
		hGridWander = %data.hGridWander;
		hReturnToSpawn = 0;
		hSpawnDist = %data.hSpawnDist;
		hMelee = %data.hMelee;
		hAttackDamage = %data.hAttackDamage;
		hSpazJump = %data.hSpazJump;
		hSearchFOV = %data.hSearchFOV;
		hFOVRadius = %data.hFOVRadius;
		hTooCloseRange = %data.hTooCloseRange;
		hAvoidCloseRange = %data.hAvoidCloseRange;
		hShoot = %data.hShoot;
		hMaxShootRange = %data.hMaxShootRange;
		hStrafe = %data.hStrafe;
		hAlertOtherBots = %data.hAlertOtherBots;
		hIdleAnimation = %data.hIdleAnimation;
		hSpasticLook = %data.hSpasticLook;
		hAvoidObstacles = %data.hAvoidObstacles;
		hIdleLookAtOthers = %data.hIdleLookAtOthers;
		hIdleSpam = %data.hIdleSpam;
		hAFKOmeter = %data.hAFKOmeter + getRandom(0, 2);
		hHearing = %data.hHearing;
		hIdle = %data.hIdle;
		hSmoothWander = %data.hSmoothWander;
		hEmote = %data.hEmote;
		hSuperStacker = %data.hSuperStacker;
		hNeutralAttackChance = %data.hNeutralAttackChance;
		hFOVRange = %data.hFOVRange;
		hMoveSlowdown = %data.hMoveSlowdown;
		hMaxMoveSpeed = 1.0;
		hActivateDirection = %data.hActivateDirection;

        hooks_hLoop = "AFKPlayerHoleBot_loop";
        //The hook declarations below aren't strictly necessary, but I'm including them anyway just in case.
        hooks_ifSearch_Radius = "AFKPlayerHoleBot_ifSearch_Radius";
        hooks_ifSearch_FOV = "AFKPlayerHoleBot_ifSearch_FOV";
        hooks_ifWander = "AFKPlayerHoleBot_ifWander";
        hooks_guardAtSpawn = -1;
		
		isHoleBot = 0; //These hax.
        isAFKPlayer = 1;
        isBot = 1;
	};
    if(isObject(%obj))
	{
        //Apply the appearance of the AFKing player.
        //Not really the way this function was meant to be used, but ^\(0v0)/^
        %obj.fixAppearance(%target.client);

        //Fake projectile is so they can correctly damage using melee
        //Thanks for breaking my Hit Direction Indicator. :P
		%obj.hFakeProjectile = new scriptObject(){};
		%obj.hFakeProjectile.sourceObject = %obj;
		%obj.hFakeProjectile.client = %obj;

        %damageType = %obj.getDataBlock().hMeleeCI;
		if(strlen(%damageType))
        {
			eval("%obj.hDamageType = $DamageType::" @ %damageType @ ";");
        }
		else
        {
			%obj.hDamageType = $DamageType::HoleMelee;
        }

        %obj.setTransform(%target.getTransform());
        %obj.setVelocity(%target.getVelocity());
        %obj.setDamageLevel(%target.getDamagePercent());
        %obj.setEnergyLevel(%target.getEnergyLevel());

        %obj.setMoveSlowdown(%obj.hMoveSlowdown);
		%obj.setMoveTolerance(0.25); //Set the move tolerance to default with our wrapped function
		%obj.hGridPosition = %target.getPosition();

        %obj.minigame = getMiniGameFromObject(%target); //This is important.
        %obj.afk_client = %target.client;
        %item_string = %target.client.afkItemString;

        %target.delete(); //Trololololol

		//This is done so they can use certain functions meant to be called on a client
		%obj.player = %obj;

        MissionCleanup.add(%obj);
        $AFKBotSet.add(%obj);

        //Finally, run the hole loop.
        %obj.hLastSpawnTime = getSimTime();
        %obj.hLoopActive = 1;

        //Select a weapon for the bot to use. Really simple right now.
        for(%i = 0; %i < getWordCount(%item_string); %i++)
        {
            %item = getWord(%item_string, %i);
            if(%item.className $= "Weapon")
            {
                %obj.setWeapon(%item.image);
            }
        }

		%obj.hSched = AFKPlayerHoleBot_loop(%obj);
	    return %obj;
	}
    else
    {
        error("ERROR: AFK System failed to create a bot for player" SPC %target.client.name);
    }
}

//
// Command for leaving the keyboard and returning.
//

function serverCmdAFK(%client)
{
    %time_remaining = %client.nextAFKTime - getSimTime();
    if(%time_remaining > 0)
    {
        %to_seconds = mFloor(%time_remaining / 1000) + 1;
        commandToClient (%client, 'CenterPrint', "<color:FFFFFF>Please wait another" SPC %to_seconds SPC "seconds.", %to_seconds);
        return;
    }
    else
    {
        %client.nextAFKTime = getSimTime() + 5000;
    }

    if(isObject(%client.afk_bot))
    {
        if(%client.afk_bot.getState() $= "Dead")
        {
            %client.camera.setMode("Corpse", %client.afk_bot);
            %client.setControlObject(%client.camera);
            return;
        }
        //Already AFK, return them back to normal.
        %bot = %client.afk_bot;
        %transform = %bot.getTransform();
        %velocity = %bot.getVelocity();
        %damagePercent = %bot.getDamagePercent();
        %damageFlash = %bot.getDamageFlash();
        %energyLevel = %bot.getEnergyLevel();
        %client.afk_bot.delete();

        %client.spawnPlayer();
        %player = %client.player;
        %player.setTransform(%transform);
        %player.setVelocity(%velocity);
        %player.setEnergyLevel(%energyLevel);
        %player.setDamageFlash(%damageFlash);
        %player.setDamageLevel(%damagePercent);
        if(strLen(%client.afkItemString))
        {
            L4B_loadItemString(%player, %client.afkItemString);
        }
    }
    else if(!isObject(%client.afk_bot) && isObject(%client.player))
    {
        //Not AFK, run the process.
        %client.afkItemString = L4B_generateItemString(%client.player);
        %client.afk_bot = L4B_AFKReplacePlayer(%client.player);

        %camera = %client.camera;
		%camera.setMode("Observer");
		%camera.setOrbitMode(%client.afk_bot, %client.afk_bot.getEyeTransform(), 5, 15, 15, 1);
		%client.setControlObject(%camera);
    }
    else
    {
        %client.setControlObject(%client.camera);
    }
}

package Gamemode_Left4Block_AFKSystem
{
    function GameConnection::spawnPlayer(%client)
    {
        if(isObject(%client.afk_bot))
        {
            %client.afk_bot.delete();
        }
        parent::spawnPlayer(%client);
    }
    function Observer::onTrigger(%this, %obj, %trigger, %state)
    {
        if(isObject(%obj.getControllingClient().afk_bot))
        {
            return;
        }
        parent::onTrigger(%this, %obj, %trigger, %state);
    }
    function holeZombieInfect(%obj, %col)
    {
        if(%col.isAFKPlayer)
        {
            return;
        }
        parent::holeZombieInfect(%obj, %col);
    }
};
activatePackage(Gamemode_Left4Block_AFKSystem);

//
// hLoop and derivitives.
//

function AFKPlayerHoleBotVarArray_New(%bot)
{
    %object = new ScriptObject();
    MissionCleanup.add(%object);
    %object.data = %bot.getDataBlock();
    %object.mount = %bot.getObjectMount();
    %object.wander = %bot.hWander;
    %object.gridWander = %bot.hWander;
    %object.search = %bot.hSearch;
    %object.strafe = %bot.strafe;
    %object.tickrate = %bot.data.hTickRate;
    %object.spastic = %bot.hSpasticLook;
    %object.idleAnim = %bot.hIdleAnimation;
    %object.AFKScale = %bot.hAFKOmeter;
    %object.idle = %bot.hIdle;
    %object.scale = getWord(%bot.getScale(), 0);
    //%object.minigame = getMiniGameFromObject(%bot);
    //%object.isHost = %bot.spawnBrick.getGroup().client == %object.minigame.owner;
    //%object.brickGroup = %bot.spawnBrick.getGroup();
    //Circumstancial variables.
    %object.avoid = 0;
    %object.noStrafe = 0;
    %object.onlyJump = 0;
    %object.isLookingAtPlayer = 0;
    %object.alreadyLooked = 0;
    
    if(!%object.AFKScale)
    {
        %object.AFKScale = 1;
    }
    return %object;
}

function AFKPlayerHoleBot_loop(%obj)
{
	//Check if we're currently active and not dead, we need to exist
	if(%obj.isDisabled() || !%obj.hLoopActive)
    {
        return;
    }
    HoleBot_init(%obj);

    //Lay out all the options we have so we can alter them as the function progresses
	%obj.inHoleLoop = 1;
    %obj.vars = AFKPlayerHoleBotVarArray_New(%obj);
	
	// if we're in water set our move tolerance to be a bit higher
    HoleBot_waterMovementTolerance(%obj);

	// let's figure out when to get out of the vehicle
    HoleBot_ifMount(%obj);

	%obj.playThread(3,root);
    HoleBot_ifTurret(%obj);
		
	//If we have certain idle actions activate this function gives you back your weapon/fixes your hands
    HoleBot_ifIsSpazzing(%obj);

	//If no tickrate create one, useful for when bots are changed into other datablocks ie horse
	if(!%obj.vars.tickrate)
	{
		%obj.vars.tickrate = 3000;
	}
	// %customLoop = %obj.getDatablock().hCustomLoop;
	
	//Again clear movement and reattach jump/crouch images, there is probably a better way to do this
    HoleBot_movementReset(%obj);
	
	// check if we're a jet datablock and we're falling to our death
	HoleBot_jetCheck(%obj);
	
	if(%obj.vars.search)
	{	
		//Radius Check
        AFKPlayerHoleBot_ifSearch_Radius(%obj); //Modded.
		//Fov Check
		AFKPlayerHoleBot_ifSearch_FOV(%obj); //Modded.
	}

	//If there's a custom script tag on the datablock, then call the function
	// if(%customLoop)
    //Lol, I wouldn't have had to make this support script if Rotondo followed through with this idea.
	%obj.getDataBlock().onBotLoop(%obj);

	//If we have a brick target that takes precedence over wandering
    if(HoleBot_ifPathBrick(%obj) == 1)
    {
        %obj.hSched = %obj.scheduleNoQuota(%obj.vars.tickrate + getRandom(0, 750), hLoop);
        return;
    }
    
	%wander_result = AFKPlayerHoleBot_ifWander(%obj);
    if(%wander_result == 1)
    {
        //We're returning to our spawn.
        %obj.hSched = %obj.scheduleNoQuota(%obj.vars.tickrate + getRandom(0, 1000), hLoop);
        return;
    }
    else if(%wander_result == 2)
    {
        //We have a target, trigger a moment of realization.
        %obj.hSched = %obj.scheduleNoQuota(%obj.vars.tickrate / 2, hLoop);
        return;
    }
	
	// head turn is out of all
	// random head turn, only if hIdleLookAtOthers is enabled && !%obj.hHeadTurn 
    HoleBot_randomHeadTurn(%obj);
	
	// randomly reset the being sprayed flag so the bot doesn't just spray all the time
    HoleBot_sprayPaintReset(%obj);
	
	HoleBot_ifIdle(%obj);
	
	//%obj.hIsBeingSprayed = 0;
	
	//Pop quiz time, did anyone notice I misspelt emote a few lines back.. er not that it's really a word
    HoleBot_wanderAvoidObstacle(%obj);
	
	//Spazz click check, blocklanders do it why not bots?
    HoleBot_wanderSpazzClick(%obj);
	
	//hmm well apparently emote is an official word, or at least far as I can tell, well it's in the dictionary anyway
	%obj.hSched = scheduleNoQuota(%obj.vars.tickrate + getRandom(0, 1000), %obj, AFKPlayerHoleBot_loop, %obj);
	return;
}

function AFKPlayerHoleBot_ifSearch_Radius(%obj)
{
    //Radius Check
    if(%obj.hSearchFOV != 1)
    {
        %target = %obj.hFindClosestPlayer();
        if(isObject(%target) && hLOSCheck(%obj, %target))
        {
            //if we've found someone set wander to 0 so we don't do any idle actions or wander around
            %obj.vars.wander = 0;
            %obj.vars.idle = 0;
            %obj.hFollowPlayer(%target, 1);
        }
        else if(%obj.hIgnore != %obj.hFollowing && %obj.hSawPlayerLastTick && isObject(%obj.hFollowing) && !%obj.hFollowing.isDisabled() && checkHoleBotTeams(%obj, %obj.hFollowing, 1))
        {
            if(%obj.hIsRunning)
            {
                %chaseDist = 64;
            }
            else
            {
                %chaseDist = 128;
            }
            if(vectorDist(%obj.getPosition(), %obj.hFollowing.getPosition()) <= %chaseDist * %obj.vars.scale)
            {
                //We saw someone last tick so we should go to where we last saw him/towards him. Might redo the way this works
                %obj.vars.wander = 0;
                %obj.vars.idle = 0;
                %obj.hFollowPlayer(%obj.hFollowing, 1);
            }
        }
    }
}

function AFKPlayerHoleBot_ifSearch_FOV(%obj)
{
    if(%obj.hSearchFOV)
    {
        if(%obj.hSawPlayerLastTick && isObject(%obj.hFollowing) && checkHoleBotTeams(%obj,%obj.hFollowing, 1) && !%obj.hFollowing.isDisabled() && %obj.hIgnore != %obj.hFollowing)
        {
            if(%obj.hIsRunning)
            {
                %chaseDist = 64;
            }
            else
            {
                %chaseDist = 128;
            }

            if(vectorDist(%obj.getPosition(), %obj.hFollowing.getPosition()) <= %chaseDist * %obj.vars.scale)
            {
                //We saw someone last tick so we should go to where we last saw him/towards him. Might redo the way this works
                %obj.vars.wander = 0;
                %obj.vars.idle = 0;
                %obj.hFollowPlayer(%obj.hFollowing, 1);
            }
        }

        if(%obj.hDoHoleFOVCheck(%obj.hFOVRadius, 1, 1))
        {
            %obj.vars.wander = 0;
            %obj.vars.idle = 0;
        }
        else
        {
            cancel(%obj.hFOVSchedule);
            %obj.hFOVSchedule = %obj.scheduleNoQuota(%obj.vars.tickrate / 2, hDoHoleFOVCheck, %obj.hFOVRadius, 0, 1);
        }
    }
}

function AFKPlayerHoleBot_ifWander(%obj)
{
    if(%obj.vars.wander)
    {
        %obj.hIsRunning = 0;
        //Er again, but still...
        %obj.setMoveObject("");
        %obj.clearMoveY();
        %obj.clearMoveX();
        %obj.setImageTrigger(0, 0);
        %obj.hResetHeadTurn();
        
        %avoid = 0;
        
        HoleBot_wanderRandomJet(%obj);
        
        //If we just saw a target, better act fast and irrational
        if(HoleBot_wanderTargetCheck(%obj) == 1)
        {
            return 2;
        }

        //Already did return check and lost target check, set to wander
        %obj.hState = "Wandering";
        
        //Grid walking
        HoleBot_gridWander(%obj);
        //Smooth wandering
        HoleBot_smoothWander(%obj);
        //More conventional bot movement, mixing between the two creates a good variety of movement
        HoleBot_mixedWander(%obj);
    }
}