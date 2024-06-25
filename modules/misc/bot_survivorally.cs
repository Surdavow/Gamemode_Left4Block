if(LoadRequiredAddOn("Support_BotHolePlus") != $Error::None) return;

datablock fxDTSBrickData (BrickSurvivorBot_HoleSpawnData)
{
	brickFile = "Add-ons/Bot_Hole/4xSpawn.blb";
	category = "Special";
	subCategory = "Holes";
	uiName = "Survivor Bot Hole";
	iconName = "Add-Ons/Bot_Blockhead/icon_blockhead";

	bricktype = 2;
	cancover = 0;
	orientationfix = 1;
	indestructable = 1;

	isBotHole = 1;
	holeBot = "SurvivorHoleBot";
};

datablock PlayerData(SurvivorHoleBot : SurvivorPlayer)
{
	uiName = "";

	maxdamage = 200;//Bot Health
	jumpSound = "";//Removed due to bots jumping a lot
	
	//Hole Attributes
	isHoleBot = 1;

	//Spawning option
	hSpawnTooClose = 0;//Doesn't spawn when player is too close and can see it
	  hSpawnTCRange = 8;//above range, set in brick units
	hSpawnClose = 0;//Only spawn when close to a player, can be used with above function as long as hSCRange is higher than hSpawnTCRange
	  hSpawnCRange = 32;//above range, set in brick units

	hType = "Survivors"; //Enemy,Friendly, Neutral
	  hNeutralAttackChance = 0; //0 to 100, Chance that this type will attack neutral bots, ie horses/vehicles/civilians
	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Survivor";//cannot contain spaces
	hTickRate = 3000;
	
	//Wander Options
	hWander = 1;//Enables random walking
	  hSmoothWander = 1;//This is in addition to regular wander, makes them walk a bit longer, and a bit smoother
	  hReturnToSpawn = 1;//Returns to spawn when too far
	  hSpawnDist = 48;//Defines the distance bot can travel away from spawnbrick
	  hGridWander = 0;//Locks the bot to a grid, overwrites other settings
	
	//Searching options
	hSearch = 1;//Search for Players
	  hSearchRadius = 64;//in brick units
	  hSight = 1;//Require bot to see player before pursuing
	  hStrafe = 1;//Randomly strafe while following player
	hSearchFOV = 0;//if enabled disables normal hSearch
	  hFOVRange = 0.7; // determines the angle we can see the player at, 0.7 is about normal fov for players, if 0 we can see all in front, if 1 we could only see someone perfectly ahead, can be negative
	  // hFOVRadius = 6; // no longer used, we now use searchRadius
	  hHearing = 1;//If it hears a player it'll look in the direction of the sound

	  hAlertOtherBots = 1;//Alerts other bots when he sees a player, or gets attacked

	//Attack Options
	hMelee = 0;//Melee
	  hAttackDamage = 15;//Melee Damage
	hShoot = 1;
	  hWep = "gunItem";
	  hShootTimes = 6;//Number of times the bot will shoot between each tick
	  hMaxShootRange = 256;//The range in which the bot will shoot the player
	  hAvoidCloseRange = 1;//
		hTooCloseRange = 14;//in brick units

	//Misc options
	hActivateDirection = 0; // 0 1 2, determines the direction you have to face the bot to activate him, both, front back
	hMoveSlowdown = 1; // bool, determines wether the bot will slow down when following enemies
	hAvoidObstacles = 1;
	hSuperStacker = 1;//When enabled makes the bots stack a bit better, in other words, jumping on each others heads to get to a player
	hSpazJump = 1;//Makes bot jump when the user their following is higher than them

	hAFKOmeter = 1;//Determines how often the bot will wander or do other idle actions, higher it is the less often he does things

	hIdle = 1;// Enables use of idle actions, actions which are done when the bot is not doing anything else
	  hIdleAnimation = 0;//Plays random animations/emotes, sit, click, love/hate/etc
	  hIdleLookAtOthers = 1;//Randomly looks at other players/bots when not doing anything else
	    hIdleSpam = 0;//Makes them spam click and spam hammer/spraycan
	  hSpasticLook = 1;//Makes them look around their environment a bit more.
	hEmote = 1;

    hooks_ifWander = "SurvivorBot_ifWander";
};

function L4B_isInFOV(%viewer, %target)
{	
	return vectorDot(%viewer.getEyeVector(), vectorNormalize(vectorSub(%target.getPosition(), %viewer.getPosition()))) >= 0.7;
}

function L4B_isPlayerObstructed(%viewer, %target)
{
    //Check if there's anything blocking line-of-sight between the viewer and the target, then return the result.
    return ContainerRayCast(%viewer.getEyePoint(), %target.getHackPosition(), $TypeMasks::FxBrickObjectType | $TypeMasks::DebrisObjectType | $TypeMasks::InteriorObjectType, %viewer);
}

function SurvivorBot_ifWander(%obj)
{
    //If we have a human fellow, following them is more important then wandering.
    if(%obj.hHumanFellow && !obj.hFollowing)
    {
        //Don't spazz out if we need to keep up with them.
        if(!L4B_isPlayerObstructed(%obj, %obj.hHumanFellow) && getWord(%obj.hHumanFellow.getVelocity(), 0) == 0 && getWord(%obj.hHumanFellow.getVelocity(), 1) == 0)
        {
            HoleBot_ifIdle(%obj);
            HoleBot_wanderSpazzClick(%obj);
        }
        else
        {
            %obj.setAimObject(%obj.hHumanFellow);
        }

        //Signal to the modified hLoop to schedule another loop and skip the remaining code.
	    return 1;
    }
    else if(%obj.vars.wander)
    {
        %obj.hIsRunning = 0;
        //Er again, but still...
        %obj.setMoveObject("");
        %obj.clearMoveY();
        %obj.clearMoveX();
        %obj.setImageTrigger(0, 0);
        %obj.hResetHeadTurn();
        
        //Again converting to brick units, easier for players
        %returnDist = brickToMetric(%obj.hSpawnDist);
        %avoid = 0;
        
        //if 0 we assume they want to always return to brick
        // if returnDistance is zero then our bot will always try to get back to spawn since the distance between bot and spawn is never truly zero
        if(%returnDist <= 0)
        {
            %returnDist = 1.6;
        }
            
        %returnDist = %returnDist * %obj.vars.scale;
        
        if(%returnDist < 1)
        {
            %returnDist = 1;
        }

        if(HoleBot_tooFarFromSpawn(%obj, %returnDist) == 1)
        {
            //Returning to spawn, signal that to hLoop.
            return 1;
        }
        
        //If spawn distance is set to 0 we don't randomly wander. We assume the user wants them to act as guard bots
        HoleBot_guardAtSpawn(%obj, %returnDist);
        
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

//I can't get bots to stop squirming around when they're not moving towards their human target. Temporary fix until I can figure out this stupidity.
package Package_Left4Block_SurvivorBot_SquirmFix
{
    function AIPlayer::hAvoidObstacle( %obj, %noStrafe, %onlyJump, %onStuck )
    {
        if(isObject(%obj.hHumanFellow) && !obj.hFollowing)
        {
            return;
        }
        parent::hAvoidObstacle( %obj, %noStrafe, %onlyJump, %onStuck );
    }
};
activatePackage(Package_Left4Block_SurvivorBot_SquirmFix);

function SurvivorHoleBot::onAdd(%this, %obj)
{
	armor::onAdd(%this, %obj);

    %obj.hIsImmune = 1;
    %obj.setRandomAppearance(0); //Placeholder.
    
    //Can't assign the variables to the %obj.vars ScriptObject for some reason. Thanks.
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

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

//
// Radius search functions.
//

//Largely copy and pasted from Bot_Hole's "hFindClosestPlayer" function.
function L4B_pairableHumanSearch(%bot, %mode)
{
    initContainerRadiusSearch(%bot.getPosition(), 2000000000, $TypeMasks::PlayerObjectType);
    if(%mode == 0) //Single humans only.
    {
        while((%target = containerSearchNext()) != 0)
        {
            if(%target != %bot && %target.getClassName() $= "Player" && %target.getState() !$= "Dead" && %bot.hFollowing !$= %target && !isObject(%target.survivorBotBuddy) && %bot.hIgnore !$= %target && !%target.hIsInfected)
            {
                return %target;
            }
        }
    }
    else if(%mode == 1) //Friended and single humans.
    {
        while((%target = containerSearchNext()) != 0)
        {
            if(%target != %bot && %target.getClassName() $= "Player" && %target.getState() !$= "Dead" && %bot.hFollowing !$= %target && %bot.hIgnore !$= %target && !%target.hIsInfected)
            {
                return %target;
            }
        }
    }
}

function L4B_troubledPlayerSearch(%bot)
{
    initContainerRadiusSearch(%bot.getPosition(), $EnvGuiServer::VisibleDistance, $TypeMasks::PlayerObjectType);
    while((%target = containerSearchNext()) != 0)
    {
        if(%target != %bot && %target.getState() !$= "Dead" && %bot.hIgnore !$= %target && !%target.hIsInfected)
        {
            if(%target.getName() $= "DownPlayerSurvivorArmor")
            {
                if(%target.isBeingStrangled)
                {
                    //Both downed and strangled is top priority, return immediately.
                    return %target;
                }
                //Downed players capable of self-defense can wait.
                if(!%downed_player)
                {
                    %downed_player = %target;
                }
            }
            else if(%target.isBeingStrangled)
            {
                //Strangled players are the second biggest priority, return immediately.
                return %target;
            }
        }
    }
    //Nobody found but downies found, start getting them up.
    return %downed_player;
}

//
// Single Search functions.
//

function L4B_attemptFindHuman(%obj)
{
    //Search for singles first.
    //Obstruction checks removed to aid in teleportation.
    %local_single = L4B_pairableHumanSearch(%obj, 0);
    if(isObject(%local_single))// && !L4B_isPlayerObstructed(%obj, %local_single))
    {
        L4B_survivorBotPairUp(%obj, %local_single);
        return %local_single;
    }
    else
    {
        //Settle for the friended ones.
        %targ = L4B_pairableHumanSearch(%obj, 1);
        if(isObject(%targ))// && !L4B_isPlayerObstructed(%obj, %targ))
        {
            //Should not use the "L4B_survivorBotPairUp" function. We are simpily a follower, not their buddy.
            %obj.hHumanFellow = %targ; 
            return %targ;
        }
    }
    return 0;
}

function L4B_survivorBotPairUp(%obj, %targ)
{
    %obj.hHumanFellow = %targ;
    %targ.survivorBotBuddy = %obj;
}

//
// Main code.
//

function L4B_botHumanCheckup(%obj, %human, %minDist)
{
    if(%obj.hFollowing || !isObject(%human) || %human.getState() $= "Dead")
    {
        //If our hands are tied or the human is no longer with us, don't do this stuff.
        if(isEventPending(%obj.hHumanSched))
        {
            cancel(%obj.hHumanSched);
        }
        return;
    }

    %obj.hHumanLastLocation = %human.getPosition();

    if(vectorDot(%human.getEyeVector(), vectorSub(%human.getPosition(), %obj.getPosition())) >= 0.4)
    {
        //We're in our human's line of fire, move out of the way.
        %obj.setAimLocation(%human.getEyeVector());
        %obj.setMoveY(-%obj.hMaxMoveSpeed / 2);
    }

    %distance = VectorDist(%obj.getPosition(), %human.getPosition());
    %max_dist = %minDist * 2;
    %is_downed = %human.getDatablock().getName() $= "DownPlayerSurvivorArmor";
    if(%distance <= %max_dist && %distance >= %minDist && !%is_downed)
    {
        //We're far enough away but not too far, maintain our distance.
        if(getWord(%human.getVelocity(), 0) > 0 && getWord(%human.getVelocity(), 1) > 0)
        {
            //Our human is still moving, slow down but don't stop completely.
            %obj.setMoveSlowdown(%obj.hMoveSlowdown);
        }
        else
        {
            //Stop completely.
            %obj.hClearMovement(); //Doesn't work.
            //The bot won't stop moving towards the human unless you do this crap. Stupid and hacky, the underlying issue needs to be found. TODO
            %obj.setMoveX(0);
            %obj.setMoveY(0);
        }
    }
    else if(%distance < %minDist && !%is_downed)
    {
        //We're too close, back up.
        %obj.setMoveY(-%obj.hMaxMoveSpeed / 2);
    }
    else if(%distance > %max_dist || %is_downed)
    {
        //We're too far away or our human is downed, move in.
        %obj.setMoveObject(%human);
        if(L4B_isPlayerObstructed(%obj, %human)) //There's probably obstacles in the way, try to get around them.
        {
            %obj.setAimObject(%human);
            %obj.hAvoidObstacle();
        }
    }
    
    if( (getWord(%human.getPosition(), 2) - getWord(%obj.getPosition(), 2) ) > 1 )
    {
        //They're above us or jumping, try jumping ourselves.
        %obj.hJump(500);
    }

    if(%distance > 50 && getWord(%human.getVelocity(), 2) == 0)
    {
        %obj.setTransform(%human.getPosition() SPC rotFromTransform(%obj.getTransform()));
    }

    //Our human is all beat up, make them feel better.
    if(%distance <= 8 && %human.getDamageLevel() >= %human.getdatablock().maxdamage/2)
    {
        if(!%obj.GiveHealingItem)
        {
            %obj.GiveHealingItem = 1;
            %obj.setAimObject(%human);

            switch(getRandom(1,2))
            {
                case 1: %healingitem = "ZombiePillsImage";
                case 2: %healingitem = "GauzeImage";
                default: %healingitem = "ZombiePillsImage";
            }

            %obj.mountImage(%healingitem,0);

            cancel(%obj.GiveHealingStuff);
            if(isObject(%obj) && %obj.getState() !$= "Dead")
                %obj.GiveHealingStuff = %obj.getdatablock().schedule(1000,GiveHealingStuff,%obj,%human);
        }
    }
    else %obj.GiveHealingItem = 0;
}

function SurvivorHoleBot::GiveHealingStuff(%this,%obj,%human)
{    
    %item = new item()
    {
        dataBlock = %obj.getmountedImage(0).item;
        position = %obj.getHackPosition();
        dropped = 1;
        canPickup = 1;
        client = %obj;
        minigame = getMiniGameFromObject(%obj);
    };

    %obj.unMountImage(0);
    %obj.playthread(1,"Activate");
    %obj.sourcerotation = %obj.gettransform();
	%muzzlepoint = %obj.getHackPosition();
	%muzzlevector = vectorScale(%obj.getEyeVector(),1.5);
	%muzzlepoint = VectorAdd (%muzzlepoint, %muzzlevector);
	%playerRot = rotFromTransform (%obj.getTransform());

    %item.schedulePop();
	%item.setTransform (%muzzlepoint @ " " @ %playerRot);
    %item.applyimpulse(%item.gettransform(),vectoradd(vectorscale(%obj.getEyeVector(),10),"0" SPC "0" SPC 5));

    %obj.setWeapon( %obj.hLastWeapon );
}

function L4B_survivorBotRunSchedule(%obj, %targ)
{
    for(%i = 0; %i < %obj.botherHumanCheckTimes; %i++)
    {
        %obj.hHumanSched = schedule( (%obj.vars.tickRate / %obj.botherHumanCheckTimes) * %i , %obj.getID(), L4B_botHumanCheckup, %obj, %targ, %obj.botherHumanDistance);
    }
}

function SurvivorHoleBot::onBotLoop( %this, %obj )
{
    if(!getMiniGameFromObject(%obj) || %obj.hFollowing)
    {
        return;
    }

    //Before everything, check if someone is in need of help.
    if(!%obj.hHelpTarget)
    {
        %obj.hHelpTarget = L4B_troubledPlayerSearch(%obj);
    }
    if(%obj.hHelpTarget)
    {
        if(!isObject(%obj.hHelpTarget) || %obj.hHelpTarget.getState() $= "Dead" || %obj.hHelpTarget.hIsInfected)
        {
            //We've lost 'em. Move on.
            %obj.hHelpTarget = 0;
            if(%obj.hHelpTarget_previousHSearch)
            {
                %obj.hSearchRadius = getWord(%obj.hHelpTarget_previousHSearch, 0);
		        %obj.hSearchFOV = getWord(%obj.hHelpTarget_previousHSearch, 1);
		        %obj.hFOVRadius = getWord(%obj.hHelpTarget_previousHSearch, 2);
                %obj.hHelpTarget_previousHSearch = 0;
            }
        }
        else
        {
            if(%obj.hHelpTarget_previousHSearch == 0)
            {
                %obj.hHelpTarget_previousHSearch = %obj.hSearchRadius SPC %obj.hSearchFOV SPC %obj.hFOVRadius;
            }
            %obj.hSearchRadius = 0;
		    %obj.hSearchFOV = 0;
		    %obj.hFOVRadius = 0;
            
            %obj.setMoveObject(%obj.hHelpTarget);
            %obj.hAvoidObstacle();
        }
        return;
    }

    //We're hurt, heal up.
    if(!%obj.GiveHealingItem && %obj.getDamageLevel() > %this.MaxDamage/4)
    {
        %obj.mountImage("gc_SyringeAntidoteImage",0);
        %obj.schedule(260,playThread,1,"shiftDown");
        %obj.schedule(250,setImageTrigger,0,1);
        %obj.schedule(300,setImageTrigger,0,0);

        %obj.schedule(500,unmountImage,0);
        %obj.schedule(750,setWeapon,%obj.hLastWeapon);
    }

    %human = %obj.hHumanFellow;
    %human_exists = isObject(%human);

    //Will hopefully prevent a crashing issue.
    if(%human_exists && %human.getState() $= "Dead")
    {
        %obj.hHumanFellow = 0;
        %human = 0;
        %human_exists = 0;
    }

    if(!%obj.hHumanFellow == %human.survivorBotBuddy)
    {
        //Our human buddy already has another friend, search for someone else.
        %local_single = L4B_pairableHumanSearch(%obj, 0);
        if(%local_single && !L4B_isPlayerObstructed(%obj, %local_single))
        {
            L4B_survivorBotPairUp(%obj, %local_single);
            %human = %obj.hHumanFellow;
        }
    }

    if(%human_exists && !L4B_isPlayerObstructed(%obj, %human))
    {
        //Monitor our human and determine if we should follow them, get out of their way, rescue them, etc.
        L4B_survivorBotRunSchedule(%obj, %human);
    }
    else if(%human_exists && L4B_isPlayerObstructed(%obj, %human))
    {
        //Our human buddy is lost, go to their last known location and look for them.
        if(%obj.hHumanLastLocation)
        {
            //Already there and we still can't see them, suffer separation anxiety.
            if(VectorDist(%obj.getPosition(), %obj.hHumanLastLocation) <= 5)
            {
                %obj.hHumanFellow = 0;
                %obj.hHumanLastLocation = 0;
                //Future-proofing.
                %human = 0;
                %human_exists = 0;
            }
            else //We aren't there yet, keep moving.
            {
                %obj.setMoveDestination(%obj.hHumanLastLocation);
                %obj.hAvoidObstacle();
            }
        }
    }
    else //No human buddy, search for one.
    {
        L4B_attemptFindHuman(%obj);
    }
}

function SurvivorHoleBot::onBotCollision( %this, %obj, %col, %normal, %speed )
{
    if(%obj.getstate() $= "Dead")
    
    if(%col.getClassName() $= "Player" && %col.getState() !$= "Dead" && %obj.hFollowing != %col)
    {
        if(%col.getDatablock().getName() $= "DownPlayerSurvivorArmor")
        {
            %col.isdowned = 0;
			%obj.isSaving = 0;
			%col.SetDataBlock("SurvivorPlayerLow");

            if(%col.billboard && $L4B_hasSelectiveGhosting)
            Billboard_DeallocFromPlayer($L4B::Billboard_SO, %col);
            
            %col.lastdamage = getsimtime();
            %col.sethealth(25);

            %col.playthread(0,root);
            %obj.playthread(1,root);
            %obj.playthread(2,"activate2");

            cancel(%col.energydeath1);
            cancel(%col.energydeath2);
            cancel(%col.energydeath3);

            if(%col.getClassName() $= "Player")
            {
                %col.client.centerprint("<color:00fa00>You were saved by " @ %this.hName,5);
                chatMessageTeam(%col.client,'fakedeathmessage',"<color:00fa00>" @ %this.hName SPC "<bitmapk:add-ons/package_left4block/icons/CI_VictimSaved>" SPC %col.client.name);
                %col.client.play2d("victim_revived_sound");
            }
        }
        //We've bumped a human ally, give them some room.
        %obj.setAimObject(%col);
        %obj.setMoveY(-%obj.hMaxMoveSpeed / 2);
    }

    if(%col.getType() & $TypeMasks::PlayerObjectType && %col.hType $= "Zombie" && %col.getState() !$= "Dead")
    {
        %obj.setAimObject(%col);
        //luacall(Survivor_Rightclick,%obj);	
        %obj.setMoveY(-%obj.hMaxMoveSpeed);
    }
}

function SurvivorHoleBot::onDamage(%this,%obj,%am)
{
	Parent::onDamage(%this,%obj,%am);
	if(%obj.getstate() $= "Dead")
    {
	    %obj.playaudio(0,"survivor_death" @ getRandom(1, 8) @ "_sound");

	    if(%obj.getWaterCoverage() == 1 && %obj.getenergylevel() == 0)
	    {
	    	%obj.playaudio(0,"survivor_death_underwater" @ getRandom(1, 2) @ "_sound");
	    	%obj.emote(oxygenBubbleImage, 1);
	    	serverPlay3D("drown_bubbles_sound",%obj.getPosition());
	    	serverPlay3D("die_underwater_bubbles_sound",%obj.getPosition());
	    }
        return;
    }
	
	if(!%obj.getWaterCoverage() $= 1)
	{
		if(%am >=5 && %obj.lastdamage+500 < getsimtime())
		{
			%obj.playaudio(0,"survivor_pain" @ getRandom(1, 4) @ "_sound");

			if(%am >= 20)
			{
				%obj.playaudio(0,"survivor_painmed" @ getRandom(1, 4) @ "_sound");

				if(%am >= 30)
				%obj.playaudio(0,"survivor_painhigh" @ getRandom(1, 4) @ "_sound");
			}

			%obj.lastdamage = getsimtime();
		}
	}
	else %obj.playaudio(0,"survivor_pain_underwater_sound");
}

function SurvivorHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	if(%damageType $= $DamageType::Fall)	
	{
		serverPlay3D("impact_fall_sound",%obj.getPosition());
		serverPlay3D("victim_smoked_sound",%obj.getPosition());
    }

	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}