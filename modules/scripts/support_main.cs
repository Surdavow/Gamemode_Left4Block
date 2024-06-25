function Armor::L4BAppearance(%this,%obj,%client)
{
	%obj.hideNode("ALL");
	%obj.unHideNode((%client.chest ? "femChest" : "chest"));
	%obj.unHideNode("rhand");
	%obj.unHideNode("lhand");
	%obj.unHideNode("rarm");
	%obj.unHideNode("larm");
	%obj.unHideNode("headskin");
	%obj.unHideNode("pants");
	%obj.unHideNode("rshoe");
	%obj.unHideNode("lshoe");
	%obj.setHeadUp(0);

	%headColor = %client.headcolor;
	%chestColor = %client.chestColor;
	%rarmcolor = %client.rarmColor;
	%larmcolor = %client.larmColor;
	%rhandcolor = %client.rhandColor;
	%lhandcolor = %client.lhandColor;
	%hipcolor = %client.hipColor;
	%rlegcolor = %client.rlegColor;
	%llegColor = %client.llegColor;		
	%faceName = %client.faceName;
	%decalName = %client.decalName;

	if(%obj.getDatablock().hType $= "Zombie")
	{
		%obj.unhidenode("gloweyes");
		%obj.setnodeColor("gloweyes","1 1 0 1");

		if(%obj.getclassname() $= "Player")
		{
			%skin = %client.headColor;
			%zskin = getWord(%skin,0)/2.75 SPC getWord(%skin,1)/1.5 SPC getWord(%skin,2)/2.75 SPC 1;
			%client.zombieskincolor = %zskin;

			%headColor = %zskin;
			if(%client.chestColor $= %skin) %chestColor = %zskin;
			if(%client.rArmColor $= %skin) %rarmcolor = %zskin;
			if(%client.lArmColor $= %skin) %larmcolor = %zskin;
			if(%client.rhandColor $= %skin) %rhandcolor = %zskin;
			if(%client.lhandColor $= %skin) %lhandcolor = %zskin;
			if(%client.hipColor $= %skin) %hipcolor = %zskin;
			if(%client.rLegColor $= %skin) %rlegcolor = %zskin;
			if(%client.lLegColor $= %skin) %llegColor = %zskin;
			%faceName = "asciiTerror";
		}
	}		

	%obj.setFaceName(%faceName);
	%obj.setDecalName(%decalName);
	%obj.setNodeColor("headskin",%headcolor);
	%obj.setNodeColor((%client.chest ? "femChest" : "chest"),%chestcolor);
	%obj.setNodeColor("pants",%hipcolor);
	%obj.setNodeColor("rarm",%rarmcolor);
	%obj.setNodeColor("larm",%larmcolor);
	%obj.setNodeColor("rhand",%rhandcolor);
	%obj.setNodeColor("lhand",%lhandcolor);
	%obj.setNodeColor("rshoe",%rlegcolor);
	%obj.setNodeColor("lshoe",%llegcolor);
	%obj.setNodeColor("headpart1",%headcolor);
	%obj.setNodeColor("headpart2",%headcolor);
	%obj.setNodeColor("headpart3",%headcolor);
	%obj.setNodeColor("headpart4",%headcolor);
	%obj.setNodeColor("headpart5",%headcolor);
	%obj.setNodeColor("headpart6",%headcolor);
	%obj.setNodeColor("chestpart1",%chestcolor);
	%obj.setNodeColor("chestpart2",%chestcolor);
	%obj.setNodeColor("chestpart3",%chestcolor);
	%obj.setNodeColor("chestpart4",%chestcolor);
	%obj.setNodeColor("chestpart5",%chestcolor);
	%obj.setNodeColor("pants",%hipcolor);
	%obj.setNodeColor("pantswound",%hipcolor);

	%bloodcolor = "1 0.5 0.5 1";
	%obj.setNodeColor("rarmSlim",%bloodcolor);
	%obj.setNodeColor("larmSlim",%bloodcolor);
	%obj.setNodeColor("headskullpart1",%bloodcolor);
	%obj.setNodeColor("headskullpart2",%bloodcolor);
	%obj.setNodeColor("headskullpart3",%bloodcolor);
	%obj.setNodeColor("headskullpart4",%bloodcolor);
	%obj.setNodeColor("headskullpart5",%bloodcolor);
	%obj.setNodeColor("headskullpart6",%bloodcolor);
	%obj.setNodeColor("headstump",%bloodcolor);
	%obj.setNodeColor("legstumpr",%bloodcolor);
	%obj.setNodeColor("legstumpl",%bloodcolor);
	%obj.setNodeColor("skeletonchest",%bloodcolor);
	%obj.setNodeColor("skeletonchestpiece1",%bloodcolor);
	%obj.setNodeColor("skeletonchestpiece2",%bloodcolor);		
	%obj.setNodeColor("skelepants",%bloodcolor);
	%obj.setNodeColor("organs","1 0.6 0.5 1");
	%obj.setNodeColor("brain","1 0.75 0.746814 1");		

	for(%limb = 0; %limb <= 8; %limb++) if(%obj.limbDismemberedLevel[%limb]) %this.RbloodDismember(%obj,%limb,false);
}

package L4B_MainPackage
{
	function ProjectileData::radiusDamage(%this, %obj, %col, %distanceFactor, %pos, %damageAmt)
	{			
		if(%col.getType() & $TypeMasks::PlayerObjectType && %col.getDamageLevel()+%damageAmt > %col.getdataBlock().maxDamage && (vectorDist(%pos, %col.getHackPosition()) / getWord(%col.getScale(), 2)) < %obj.getdataBlock().damageRadius) 
		%col.markForGibExplosion = true;

		Parent::radiusDamage(%this, %obj, %col, %distanceFactor, %pos, %damageAmt);
	}	

    function Armor::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
    {
        if(isObject(%obj.hEating) && %obj.heating.getclassname() $= "Player") %victim = %obj.heating;        
        
        Parent::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType);		

		//Rblood simulation
		if($Pref::L4B::Blood::BloodDamageThreshold && %damage > $Pref::L4B::Blood::BloodDamageThreshold && %this.enableRBlood && (%damageType != $DamageType::Lava && %damageType != $DamageType::Suicide))
		{
			if(%this.hZombieL4BType $= "Normal") %damage = %damage*3;
			%this.RBloodSimulate(%obj,%position,%damagetype,%damage);
		}

        if(isObject(%sourceObject) && %sourceObject.getClassName() $= "Player") %source = %sourceObject;		
        else if(isObject(%sourceObject.sourceObject) && %sourceObject.sourceObject.getClassName() $= "Player") %source = %sourceObject.sourceObject;

        if(!isObject(%minigame = getMiniGameFromObject(%obj)) || %obj.hType !$= "Zombie" || !isObject(%source)) return;
        
        //When the bot is a special and dies
        if(%obj.getdataBlock().hZombieL4BType $= "Special")
        {
            if(%obj.getDataBlock().getName() !$= "ZombieChargerHoleBot" && isObject(%victim))
            {                
                chatMessageTeam(%victim.client,'fakedeathmessage',"<color:00FF00> "@ %source.client.name @ " <bitmapk:add-ons/Gamemode_Left4Block/modules/add-ins/player_l4b/icons/CI_VictimSaved> " @ %victim.client.name);
                %victim.isBeingStrangled = false;
                L4B_SpecialsPinCheck(%obj,%victim);
            }            

            if(%obj.getState() $= "Dead") %minigame.L4B_ChatMessage("\c0" @ %source.client.name SPC "<bitmapk:" @ $DamageType::MurderBitmap[%damageType] @ ">" SPC %obj.getdataBlock().hName @ "","victim_revived_sound",true);
        }

        //Protection!
		if(%obj.getState() $= "Dead" && isObject(%target = %obj.hFollowing) && %target.getClassName() $= "Player" && %source !$= %target && !L4B_isInFOV(%target, %obj))
        %minigame.L4B_ChatMessage("<color:00FF00>" @ %source.client.name SPC "protected" SPC %target.client.name,"victim_revived_sound",false);
    }

	function minigameCanDamage(%objA, %objB)
	{
        if((!isObject(%objA) || !isObject(%objB)) || (!isObject(getMiniGameFromObject(%objA)) || !isObject(getMiniGameFromObject(%objA))) || (getMiniGameFromObject(%objA) !$= getMiniGameFromObject(%objB))) return false; 
        if(!miniGameFriendlyFire(%objA,%objB)) Parent::minigameCanDamage(%objA,%objB);
        else return false;
	}

    function GameConnection::resetVehicles(%client)
    {
	    if(!isObject(MissionCleanup))
	    {
	    	if(getBuildString() !$= "Ship") error("ERROR: GameConnection::ResetVehicles() - MissionCleanUp group not found!");	    	
	    	return;
	    }
	
        for(%i = 0; %i < MissionCleanup.getCount(); %i++) if(isObject(%obj = MissionCleanup.getObject(%i)) && (%obj.getType() & ($TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType)) && isObject(%obj.spawnBrick) && %obj.spawnBrick.getGroup() == %client.brickGroup)
        %obj.spawnBrick.schedule(10, spawnVehicle);
    }

    function GameConnection::onClientLeaveGame (%client)
    {
        Parent::onClientLeaveGame(%client);

        %client.deletel4bMusic("Music");
        %client.deletel4bMusic("Musi2");
        %client.deletel4bMusic("Stinger1");
        %client.deletel4bMusic("Stinger2");
        %client.deletel4bMusic("Stinger3");
        %client.deletel4bMusic("Ambience");        
    }

	function fxDTSBrickData::onColorChange (%data, %brick)
	{
		if(isObject(%brick.interactiveshape)) %brick.interactiveshape.setnodecolor("ALL",getwords(getColorIdTable(%brick.colorid),0,2) SPC "1");

		Parent::onColorChange (%data, %brick);
	}

	function Armor::onCollision(%this,%obj,%col)
	{
		Parent::onCollision(%this,%obj,%col,%a,%b,%c,%d);

		if(isObject(%col) && %col.getClassName() $= "StaticShape" && %col.getdatablock().isInteractiveShape) %col.getdatablock().CheckConditions(%col,%obj);
	}

	function Projectile::onAdd(%obj)
	{
		if(%obj.getdataBlock().isDistraction) %obj.schedule(%obj.getDataBlock().distractionDelay,%obj.getDataBlock().distractionFunction,0);

		Parent::onAdd(%obj);
	}

	function ProjectileData::onCollision (%this, %obj, %col, %fade, %pos, %normal, %velocity)
	{
		if(%this.directDamage && %col.getType() & $TypeMasks::PlayerObjectType)
		{
			if(!%obj.sourceObject.hIsInfected && %col.isBeingStrangled && %col.hEater.getDataBlock().getName() $= "ZombieSmokerHoleBot")
			{
				%col.hEater.isBeingStrangled = false;
				%obj.isStrangling = false;
				L4B_SpecialsPinCheck(%col.hEater,%col);
				%col.hEater.damage(%obj, %pos, %this.directDamage/2, %this.directDamageType);
			}
		}		

		Parent::onCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity);
	}

	function WeaponImage::onMount(%this,%obj,%slot)
	{		
		Parent::onMount(%this,%obj,%slot);

		if(%obj.getdataBlock().isSurvivor && !%obj.getdataBlock().isDowned)
		switch(%obj.getMountedImage(0).armReady)
		{
			case 1: %obj.setarmthread("lookarmed");
			case 2: %obj.setarmthread("lookarmedboth");
					%obj.schedule(1,playthread,1,"armreadyboth");
		}		
	}

	function WeaponImage::onUnMount(%this,%obj,%slot)
	{		
		Parent::onUnMount(%this,%obj,%slot);
		if(%obj.getdataBlock().isSurvivor && !%obj.getdataBlock().isDowned) %obj.setarmthread("look");
	}	
	
	function AIPlayer::setCrouching(%player,%bool)
	{
		if(%player.nolegs) %bool = true; 
		Parent::setCrouching(%player,%bool);
	}

	function AIPlayer::setJumping(%player,%bool)
	{
		if(%player.nolegs) %bool = false;
		Parent::setJumping(%player,%bool);
	}

	function AIPlayer::hLoop(%obj)
	{
		if(!isObject(%obj)) return;
		else Parent::hLoop(%obj);

		if(%obj.getDataBlock().hZombieL4BType !$= "")//Bypass the default garbage
		{
			%obj.setMoveTolerance(2.5);
			%obj.setMoveSlowdown(0);
		}
	}

	function AIPlayer::hMeleeAttack(%obj,%col)
	{
		if((!isObject(%obj) || %obj.getState() $= "Dead")) return;

		if(!isObject(%col)) return;

		%cansee = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%col.getposition(),%obj.getposition())));
		if(VectorDist(%obj.getposition(), %col.getposition()) > 2.5 || %cansee < 0.5) return;

		if(%col.getType() & $TypeMasks::VehicleObjectType || %col.getType() & $TypeMasks::PlayerObjectType)
		{
			if(%obj.hState $= "Following" || %obj.Distraction)
			{
				%damage = %obj.hAttackDamage*getWord(%obj.getScale(),0);
				%damagefinal = getRandom(%damage/4,%damage);
				%obj.hlastmeleedamage = %damagefinal;
				%obj.lastattacked = getsimtime()+1000;

				%obj.playthread(2,activate2);
				%col.damage(%obj.hFakeProjectile, %col.getposition(), %damagefinal, %obj.hDamageType);
				%obj.getDataBlock().onBotMelee(%obj,%col);				
			}
		}
	}

	function AIPlayer::hFindClosestPlayerFOV(%bot,%range,%hear)
	{
		%type = $TypeMasks::PlayerObjectType;
		%scale = getWord(%bot.getScale(),0);

		%pos = %bot.getPosition();
		if(%bot.hSearchRadius == 0)	%bot.hFinalRadius = 0;
		else %bot.hFinalRadius = brickToRadius( %bot.hSearchRadius )*%scale;
		if(%bot.hSearchRadius == -2) %bot.hFinalRadius = 2000000000;

		%n = 0;
		initContainerRadiusSearch( %pos, %bot.hFinalRadius, %type );
		while((%target = containerSearchNext()) != 0 )
		{
			if(%bot == %target || !%bot.hFOVCheck(%target)) continue;

			if(%target.getState() !$= "Dead" && !%target.isCloaked && %bot.hIgnore != %target && checkHoleBotTeams(%bot,%target) && hLOSCheck(%bot,%target) && miniGameCanDamage(%target,%bot))
			switch$(%bot.getdataBlock().hZombieL4BType)
			{
				case "Special": if(%target.getdataBlock().isDowned) return %target;
								else return %target;
				case "Normal": 	if(%target.BoomerBiled) return %target;
								else return %target;
				default: return %target;
			}	
		}

		//Hearing
		if(%bot.hHearing && %hear)
		{
			%pos = %bot.getPosition();
			//%radius = 8*%scale;

			initContainerRadiusSearch( %pos, %bot.hFinalRadius/2, %type );
			while((%target = containerSearchNext()) != 0)
			{
				%target = %target.getID();
				%speed = vectorLen( %target.getVelocity() );

				if(%bot != %target && %speed > 3 && %target.getState() !$= "Dead" && !%target.isCloaked && checkHoleBotTeams(%bot,%target, 1) && miniGameCanDamage( %target, %bot ) == 1)
				{
					%bot.clearMoveY();
					%bot.clearMoveX();
					%bot.maxYawSpeed = 10;
					%bot.maxPitchSPeed = 10;
					%bot.setAimLocation( vectorAdd(%target.getPosition(), "0 0 2") );

					if(%bot.hEmote)
						%bot.emote(wtfImage);
					cancel(%bot.hFOVSchedule);
					%bot.hFOVSchedule = %bot.scheduleNoQuota( 500, hDoHoleFOVCheck, 0, 0, 0 );//%bot.hFOVRadius,0,0);

					return -1;
				}
			}
		}
		return 0;
	}

	function Player::ActivateStuff (%player)//Not parenting, I made an overhaul of this function so it might cause compatibility issues...
	{
		%client = %player.client;
		%player.lastActivated = 0;

		if(isObject(%client.miniGame) && %client.miniGame.WeaponDamage && getSimTime() - %client.lastF8Time < 5000 || %player.getDamagePercent () >= 1) return false;
		
		if(getSimTime() - %player.lastActivateTime <= 320) %player.activateLevel += 1;
		else %player.activateLevel = 0;		
		%player.lastActivateTime = getSimTime();
		
		if(%player.activateLevel > 4) %player.playThread (3, activate2);
		else %player.playThread (3, activate);		

		%start = %player.getEyePoint ();
		%vec = %player.getEyeVector ();
		%scale = getWord (%player.getScale (), 2);
		%end = VectorAdd (%start, VectorScale (%vec, 3 * %scale));
		%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::ItemObjectType;

		if(%player.isMounted()) %exempt = %player.getObjectMount();
		else %exempt = %player;
		%search = containerRayCast (%start, %end, %mask, %exempt);
		
		if(isObject(%search))
		{
			%pos = getWords (%search, 1, 3);
			switch$(%search.getclassname()) 
			{
				case "fxDtsBrick":%search.onActivate (%player, %client, %pos, %vec);
				case "Item":if(%search.canPickup)
							{
								if(%search.getdataBlock().throwableExplosive)
								{									            
									%player.mountImage(%search.getdataBlock().image,0);
									%search.delete();
									return;
								}

								for(%i=0;%i<%player.getdataBlock().maxTools;%i++)
								if(%search.getDataBlock() == %player.tool[%i]) return;
								%player.pickup(%search);
							}
				case "WheeledVehicle": 	if(isFunction(%search, onActivate)) %search.onActivate(%player, %client, %pos, %vec);
										if(isObject(%search.getdatablock().image))
										{
											%player.mountImage(%search.getdataBlock().image,0);
											%search.delete();
										}

				default: if(isFunction(%search, onActivate)) %search.onActivate(%player, %client, %pos, %vec);
			}

			%player.lastActivated = %search;
			return true;
		}
		else return false;	
	}

	function Player::PutItemInSlot(%obj,%slot,%item)
	{
		if(!%obj.tool[%slot])
		{
			%obj.tool[%slot] = %item.getDataBlock();
			messageClient(%obj.client,'MsgItemPickup','',%slot,%item.getDataBlock());

			if(isObject(%brick = %item.spawnBrick) && strstr(strlwr(%brick.getName()), "nodis") != -1)
			{
				%item.fadeOut();
				%item.respawn();
			}
			else %item.delete();

			return true;
		}
		else 
		{
			%obj.client.centerprint("<color:FFFFFF><font:impact:40>Drop slot<color:FFFF00>" SPC %slot+1 SPC "<color:FFFFFF>to pickup <color:FFFF00>" @ %item.getdataBlock().uiName,2);
			return false;
		}
	}

	function Player::Pickup(%obj,%item)
	{
		if(%obj.getClassName() $= "Player" && %item.getdataBlock().getName() $= "CashItem")
		{
			serverPlay3D("cash_pickup_sound",%item.getposition());
			%item.delete();
			if(isObject(%obj.client)) %obj.client.incscore(5);
			return;
		}
		
		if(%obj.getDatablock().isSurvivor)
		{
			if(!isObject(%item) || !%item.canPickup || !miniGameCanUse(%obj,%item)) return;
			
			if(%item.getdatablock().throwableExplosive)
			{
				ServerCmdUnUseTool(%obj.client);
				%obj.mountImage(%item.getdatablock().image, 0);
				
				%item.delete();
				return;
			}

			if(%item.getdataBlock().L4Bitemslot $= "NoSlot") return Parent::pickup(%obj,%item);

			switch$(%item.getdataBlock().L4Bitemslot)
			{
        	    case "Secondary": %obj.PutItemInSlot(1,%item);
				case "Grenade": %obj.PutItemInSlot(2,%item);
				case "Medical": %obj.PutItemInSlot(3,%item);
				case "Medical_Secondary": %obj.PutItemInSlot(4,%item);
				default: %obj.PutItemInSlot(0,%item);
			}
			return;
		}

		Parent::Pickup(%obj,%item);
	}

	function Armor::onRemove(%this,%obj)
	{
		if(isObject(%obj.hatprop)) %obj.hatprop.delete();
		for(%i = 0; %i < %obj.getMountedObjectCount(); %i++) if(isObject(%obj.getMountedObject(%i)) && %obj.getMountedObject(%i).getDataBlock().getName() $= "EmptyPlayer") %obj.getMountedObject(%i).delete();
		
		Parent::onRemove(%this,%obj);
	}

	function Armor::onImpact(%this, %obj, %col, %vec, %force)
	{
		Parent::onImpact(%this, %obj, %col, %vec, %force);

		if(%force < 40) serverPlay3D("impact_medium" @ getRandom(1,3) @ "_sound",%obj.getPosition());
		else serverPlay3D("impact_hard" @ getRandom(1,3) @ "_sound",%obj.getPosition());

		%oScale = getWord(%obj.getScale(),2);
		%forcescale = %force/25 * %oscale;
		%obj.spawnExplosion(pushBroomProjectile,%forcescale SPC %forcescale SPC %forcescale);

		if(%obj.getState() !$= "Dead" && getWord(%vec,2) > %obj.getdataBlock().minImpactSpeed)
        {
			serverPlay3D("impact_fall_sound",%obj.getPosition());
			if(%obj.getClassName() $= "Player") %obj.spawnExplosion("ZombieHitProjectile",%force/15 SPC %force/15 SPC %force/15);

		}		
	}

	function Armor::onNewDatablock(%this, %obj)
	{		
		Parent::onNewDatablock(%this, %obj);

		if(%this.hType !$= "Zombie")
		{
			%obj.hZombieL4BType = "";
			%obj.hIsInfected = "";

			if(%obj.getdatablock().hType !$= "") %obj.hType = %obj.getdatablock().hType;
			if(isObject(%obj.client) && %obj.getDamagePercent() == 1) commandToClient( %obj.client, 'SetVignette', $EnvGuiServer::VignetteMultiply, $EnvGuiServer::VignetteColor);
		}
	}

	function serverCmdUseTool(%client, %tool)
	{		
		if(!%client.player.isBeingStrangled) return parent::serverCmdUseTool(%client, %tool);
	}

	function ServerCmdDropTool (%client, %position)
	{
		if(isObject (%player = %client.player) && isObject(%item = %player.tool[%position]) && %item.canDrop)
		%player.playthread(3,"activate");
		%bool = Parent::ServerCmdDropTool (%client, %position);
	}	

	function holeZombieInfect(%obj, %col)
	{					
		return;
		if(fileName(%col.getDataBlock().shapeFile) $= "skeleton.dts") return;

		if(%col.getDataBlock().usesL4Bappearance)
		{		
			%col.isBeingStrangled = false;

			%col.setDataBlock(CommonZombieHoleBot);
			switch$(%col.getclassname())
			{
				case "AIPlayer": %col.hChangeBotToInfectedAppearance();

				case "Player": 	if(isObject(%minigame = getMiniGameFromObject(%obj)))
								{
							   		%col.client.deletel4bMusic("Music");
									%col.client.deletel4bMusic("Music2");									
									%minigame.L4B_ChatMessage("\c0" @ %obj.getDatablock().hName SPC "<bitmapk:Add-Ons/Gamemode_Left4Block/add-ins/player_l4b/icons/ci_infected>" SPC %col.client.name,"survivor_left4dead_sound",true);
							   		%minigame.checkLastManStanding();
								}

							   	for(%i = 0; %i < %col.getdatablock().maxTools; %i++) 
							   	{
									%col.tool[%i] = 0;
									messageClient(%col.client,'MsgItemPickup','',%i,0);
							   	}
							   
							   	if(isObject(%col.getMountedImage(0)))
							   	{
									ServerCmdDropTool(%col.client, %col.getHackPosition());
							   		%col.playthread(1,root);
									L4B_ZombieDropLoot(%col,%col.getMountedImage(0).item,100);
							   	}
			}
		}
	}

	function Observer::onTrigger(%this, %obj, %a, %b)
	{
		%client = %obj.getControllingClient();

		if(isObject(%client.player) && %client.player.isBeingStrangled)
		{
			if(%b) %client.player.activateStuff();	
			return;
		}

		Parent::onTrigger(%this, %obj, %a, %b);
	}

	function fxDTSBrick::onActivate (%obj, %player, %client, %pos, %vec)
	{
		if(%obj.getdataBlock().IsZoneBrick && %obj.getgroup().bl_id == %player.client.bl_id)
		{
			if(strstr(strlwr(%obj.getName()),"_main") != -1)
			if(%player.client.currAreaZone !$= %obj.AreaZone)
			{
				%player.client.currAreaZone = %obj.AreaZone;
				%player.client.centerprint("\c2Set current checker <br>\c2" @ %obj.AreaZone.ParBrick,3);

				%removenoctnum = getSubStr(%obj.getName(), 0, strstr(strlwr(%obj.getName()),"_ct")+3);
				%num = strreplace(%obj.getName(), %removenoctnum, "");
				%player.client.AZCount = %num;
			}
		}

		if(%player.hType $= "Zombie" && %obj.getdatablock().isDoor)
		{
			%obj.playSound("BrickRotateSound");
			
			if(%obj.getdatablock().uiName !$= "L4D Door" && %player.breakDoorTime < getSimTime())
			{
				if(%obj.breakDoorCount < 10) %obj.breakDoorCount++;
				else
				{
					%obj.zfakeKillBrick();
					%obj.breakDoorCount = 0;
				}

				%obj.playSound("BrickBreakSound");
				%player.breakDoorTime = getSimTime()+250;
			}
			return;
		}

		Parent::onActivate (%obj, %player, %client, %pos, %vec);
	}

	function fxDTSBrickData::onPlayerTouch(%data,%obj,%player)
	{
		Parent::onPlayerTouch(%data,%obj,%player);

		if(%player.getDatablock().isSurvivor) %obj.processInputEvent("onSurvivorTouch");
		
		if(%player.hType $= "zombie") 
		{
			%obj.processInputEvent("onZombieTouch");
			if(%player.getDatablock().getName() $= "ZombieTankHoleBot") %obj.processInputEvent("onTankTouch");
		}			
	}

	function GameConnection::createPlayer (%client, %spawnPoint)
	{
		Parent::createPlayer (%client, %spawnPoint);
		
		if(%client.team $= "zombie" && isObject(%client.Player))
		{
			%client.player.setDataBlock($hZombieSpecialType[getRandom(1,$hZombieSpecialTypeAmount)]);
			commandToClient(%client, 'ShowEnergyBar', true);
		}
	}

	function GameConnection::applyBodyColors(%client,%o) 
	{
		Parent::applyBodyColors(%client,%o);
		
		if(isObject(%player = %client.player) && %player.getDataBlock().usesL4Bappearance) %player.getDataBlock().L4BAppearance(%player,%client);
	}

	function GameConnection::applyBodyParts(%client,%o) 
	{
		Parent::applyBodyParts(%client,%o);
		
		if(isObject(%player = %client.player) && %player.getDataBlock().usesL4Bappearance) %player.getDataBlock().L4BAppearance(%player,%client);
	}	
};
if(isPackage(holeZombiePackage)) deactivatePackage(holeZombiePackage);
if(isPackage(BotHolePackage))
{
	deactivatePackage(L4B_MainPackage);
	deactivatePackage(BotHolePackage);
	activatePackage(BotHolePackage);
	activatePackage(L4B_MainPackage);
}

//if(LoadRequiredAddOn("Gamemode_Slayer") != $Error::None) return;
//
//function MiniGameSO::Reset(%minigame,%client)
//{
//	Parent::Reset(%minigame,%client);
//
//	if(isObject(%minigameteam = %minigame.teams))
//	{
//		%teamsammount = %minigameteam.getCount();
//		for(%i = 0; %i < %teamsammount; %i++)
//		{				
//			%teams = %minigameteam.getObject(%i);
//			
//			if(%teams.name $= "Survivors")
//			%survivorteam = %teams;
//
//			for(%j=0;%j<%minigame.numMembers;%j++)
//			{
//				if(isObject(%infectedclient = %minigame.member[%j]) && %infectedclient.getClassName() $= "GameConnection" && %infectedclient.isInInfectedTeam)
//				{
//					%survivorteam.addMember(%infectedclient, "Reset Minigame", 1, 1);
//					%infectedclient.isInInfectedTeam = 0;
//				}
//			}
//		}
//	}
//}
//
//function holeZombieInfect(%obj, %col)
//{			
//	if(isObject(%minigameteam = getMinigameFromObject(%col).teams) && %col.getClassName() $= "Player")
//	{
//		%minigame = getMinigameFromObject(%col);
//		%minigame.L4B_PlaySound("survivor_turninfected" @ getRandom(1,3) @ "_sound",%col.client);
//
//		%teamsammount = %minigameteam.getCount();
//		for(%i = 0; %i < %teamsammount; %i++)
//		{
//			%teams = %minigameteam.getObject(%i);
//			if(%teams.name $= "Infected") %infectedteam = %teams;
//
//			if(isObject(%infectedteam))
//			{
//				%infectedteam.addMember(%col.client, "No Immunity", 1, 1);
//				%clName = %col.client.name;
//
//				%col.client.isInInfectedTeam = 1;
//				L4B_checkAnyoneNotZombie(%col,%minigame);
//				break;
//			}
//		}
//	}
//
//	Parent::holeZombieInfect(%obj, %col);
//}
//
//function L4B_CheckAnyoneNotZombie(%obj,%minigame)
//{
//	%survivorteam = "";
//	for(%i = 0; %i < %teamsammount = %minigame.teams.getCount(); %i++)
//	{
//		%teams = %minigame.teams.getObject(%i);
//		if(%teams.name $= "Survivors")
//		{
//			%survivorteam = %teams;
//			break;
//		}
//	}
//	if(%survivorteam !$= "" && %survivorteam.numMembers <= 0) %minigame.endRound(%minigame.victoryCheck_Lives());
//}