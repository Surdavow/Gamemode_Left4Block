datablock TSShapeConstructor(NewMDts) 
{
	baseShape = "./models/newm.dts";
	sequence0 = "./models/animations/m_root.dsq root";
	sequence1 = "./models/animations/m_run.dsq run";
	sequence2 = "./models/animations/m_back.dsq back";
	sequence3 = "./models/animations/m_side.dsq side";
	sequence4 = "./models/animations/m_crouch.dsq crouch";
	sequence5 = "./models/animations/m_crouchrun.dsq crouchrun";
	sequence6 = "./models/animations/m_crouchback.dsq crouchback";
	sequence7 = "./models/animations/m_crouchside.dsq crouchside";
	sequence8 = "./models/animations/m_look.dsq look";
	sequence9 = "./models/animations/m_headside.dsq headside";
	sequence10 = "./models/animations/m_jump.dsq jump";
	sequence11 = "./models/animations/m_jump.dsq standjump";
	sequence12 = "./models/animations/m_fall.dsq fall";
	sequence13 = "./models/animations/m_land.dsq land";
	sequence14 = "./models/animations/m_armattack.dsq armattack";
	sequence15 = "./models/animations/m_armreadyleft.dsq armreadyleft";
	sequence16 = "./models/animations/m_armreadyright.dsq armreadyright";
	sequence17 = "./models/animations/m_armreadyboth.dsq armreadyboth";
	sequence18 = "./models/animations/m_spearready.dsq spearready";
	sequence19 = "./models/animations/m_spearthrow.dsq spearthrow";
	sequence20 = "./models/animations/m_talk.dsq talk";
	sequence21 = "./models/animations/m_death1.dsq death1";
	sequence22 = "./models/animations/m_shiftup.dsq shiftup";
	sequence23 = "./models/animations/m_shiftdown.dsq shiftdown";
	sequence24 = "./models/animations/m_shiftaway.dsq shiftaway";
	sequence25 = "./models/animations/m_shiftto.dsq shiftto";
	sequence26 = "./models/animations/m_shiftleft.dsq shiftleft";
	sequence27 = "./models/animations/m_shiftright.dsq shiftright";
	sequence28 = "./models/animations/m_rotcw.dsq rotcw";
	sequence29 = "./models/animations/m_rotccw.dsq rotccw";
	sequence30 = "./models/animations/m_undo.dsq undo";
	sequence31 = "./models/animations/m_plant.dsq plant";
	sequence32 = "./models/animations/m_sit.dsq sit";
	sequence33 = "./models/animations/m_wrench.dsq wrench";
	sequence34 = "./models/animations/m_activate.dsq activate";
	sequence35 = "./models/animations/m_activate2.dsq activate2";
	sequence36 = "./models/animations/m_leftrecoil.dsq leftrecoil";	
	sequence37 = "./models/animations/m_lookarmedboth.dsq lookarmedboth";	
	sequence38 = "./models/animations/m_lookarmed.dsq lookarmed";
	sequence39 = "./models/animations/m_weaponfire1.dsq weaponfire1";
	sequence40 = "./models/animations/m_handgunmagout.dsq handgunmagout";
	sequence41 = "./models/animations/m_handgunmagin.dsq handgunmagin";
	sequence42 = "./models/animations/m_shotgunfire1.dsq shotgunfire1";
	sequence43 = "./models/animations/m_riflefire1.dsq riflefire1";
	sequence44 = "./models/animations/m_shotgunpump1.dsq shotgunpump1";
	sequence44 = "./models/animations/m_meleeswing1.dsq meleeswing1";
	sequence45 = "./models/animations/m_meleeswing2.dsq meleeswing2";
	sequence46 = "./models/animations/m_meleeswing3.dsq meleeswing3";
	sequence47 = "./models/animations/m_meleeraise.dsq meleeraise";
};

datablock PlayerData(SurvivorPlayer : PlayerStandardArmor)
{
	shapeFile = NewMDts.baseshape;
	canJet = false;
	runForce = 100 * 45;
	jumpforce = 100*9.25;
	jumpDelay = 25;
	minimpactspeed = 15;
	speedDamageScale = 0.25;
	mass = 105;
	airControl = 0.05;

	cameramaxdist = 2.25;
    cameraVerticalOffset = 1;
    cameraHorizontalOffset = 0.75;
    cameratilt = 0;
    maxfreelookangle = 2;

    maxForwardSpeed = 9;
	maxSideSpeed = 8;
	maxBackwardSpeed = 7;

 	maxForwardCrouchSpeed = 5;
	maxSideCrouchSpeed = 4;
	maxBackwardCrouchSpeed = 3;
    
	groundImpactMinSpeed = 5;
	groundImpactShakeFreq = "4.0 4.0 4.0";
	groundImpactShakeAmp = "1.0 1.0 1.0";
	groundImpactShakeDuration = 0.8;
	groundImpactShakeFalloff = 15;

	upMaxSpeed = 150;
	upResistSpeed = 10;
	upResistFactor = 0.25;	
	horizMaxSpeed = 150;
	horizResistSpeed = 10;
	horizResistFactor = 0.25;

	uiName = "Survivor Player";
	usesL4DItems = true;
	isSurvivor = true;
	hType = "Survivors";
	enableRBlood = true;
	usesL4Bappearance = true;
	renderFirstPerson = true;
	maxtools = 5;
	maxWeapons = 5;

	boundingBox = "4.5 4 10.6";
	crouchboundingBox = "4.5 4 9";
	
	jumpSound 		= JumpSound;
	PainSound		= "";
	DeathSound		= "";
	useCustomPainEffects = true;
	rechargeRate = 0.025;
	maxenergy = 100;
	showEnergyBar = false;
};

datablock PlayerData(SurvivorPlayerDowned : SurvivorPlayer)
{	
   	runForce = SurvivorPlayer.runForce;
   	maxForwardSpeed = 0;
   	maxBackwardSpeed = 0;
   	maxSideSpeed = 0;
   	maxForwardCrouchSpeed = 0;
   	maxBackwardCrouchSpeed = 0;
   	maxSideCrouchSpeed = 0;
   	jumpForce = 0;
	rechargerate = 0;
	isDowned = true;
	uiName = "";
};

function SurvivorPlayer::L4BAppearance(%this,%obj,%client) 
{ 
	Parent::L4BAppearance(%this,%obj,%client); 

	switch(%client.Pack)
	{
		case 0:
		default: %obj.unHideNode($pack[%client.pack]);
				 %obj.setNodeColor($pack[%client.pack],%client.PackColor);
	}

	switch(%client.secondPack)
	{
		case 0:
		default: %obj.unHideNode($secondpack[%client.secondPack]);
				 %obj.setNodeColor($secondpack[%client.secondPack],%client.secondPackColor);
	}

	//Don't bother if they already have the hat
	if((isObject(%obj.getmountedImage(2)) && %obj.getmountedImage(2).getName() $= $L4BHat[%client.hat] @ "image") || %obj.limbDismemberedLevel[0]) return;

	switch$(%client.hat)
	{
		case 1: if(%client.accent)
				{
					%obj.mountImage("helmetimage",2,1,addTaggedString(getColorName(%client.hatColor)));	
					%obj.currentHat = "helmet";
				}		
				else
				{
					%obj.mountImage("hoodieimage",2,1,addTaggedString(getColorName(%client.hatColor)));	
					%obj.currentHat = "hoodie";
				}
		default: %obj.mountImage($L4BHat[%client.hat] @ "image",2,1,addTaggedString(getColorName(%client.hatColor)));
				 %obj.currentHat = $L4BHat[%client.hat];
	}
}

function SurvivorPlayer::RbloodDismember(%this,%obj,%limb,%doeffects,%position)
{
	if(%limb != 7 || %limb != 8) Parent::RbloodDismember(%this,%obj,%limb,%doeffects,%position);
}

function SurvivorPlayer::onImpact(%this, %obj, %col, %vec, %force)
{	
	if(%obj.getState() !$= "Dead")
	{				
		%zvector = getWord(%vec,2);

		if(%zvector > %this.minImpactSpeed) 
		{
			%force = %force*(%force*0.15);					
			if(%zvector > %this.minImpactSpeed*1.333) %force = %force*(%force*0.03);
			%obj.playthread(0,"land");
		}
	}

	Parent::onImpact(%this, %obj, %col, %vec, %force);	
}

function SurvivorPlayer::onEnterLiquid(%this, %obj, %cov, %type)
{
	cancel(%obj.oxygenTick);
	%obj.oxygenTick = %obj.schedule(2500, oxygenTick);
	Parent::onEnterLiquid(%this, %obj, %cov, %type);
}
function SurvivorPlayer::onLeaveLiquid(%this, %obj, %type)
{
	%obj.schedule(150,checkIfUnderwater);
	Parent::onLeaveLiquid(%this, %obj, %cov, %type);
}

function SurvivorPlayer::onAdd(%this,%obj)
{	
	Parent::onAdd(%this,%obj);	
	if(isObject(%obj.client)) %obj.client.deletel4bMusic("Music");
}

function SurvivorPlayer::onTrigger (%this, %obj, %triggerNum, %val)
{
	Parent::onTrigger (%this, %obj, %triggerNum, %val);

	if(%obj.getClassName() $= "Player" && %obj.getState() !$= "Dead")
	{		
		%triggerTime = getSimTime();

		switch(%triggerNum)
		{
			case 0: if(!isObject(%obj.getMountedImage(0)))//Left click
					{
						if(%val)
						{
							%eye = %obj.getEyePoint(); //eye point
							%scale = getWord (%obj.getScale (), 2);
							%len = $Game::BrickActivateRange * %scale; //raycast length
							%masks = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;
							%vec = %obj.getEyeVector(); //eye vector
							%beam = vectorScale(%vec,%len); //lengthened vector (for calculating the raycast's endpoint)
							%end = vectorAdd(%eye,%beam); //calculated endpoint for raycast
							%ray = containerRayCast(%eye,%end,%masks,%obj); //fire raycast
							%ray = isObject(firstWord(%ray)) ? %ray : 0; //if raycast hit - keep ray, else set it to zero

							if(!%ray) //if Beam Check fcn returned "0" (found nothing)
							return Parent::onTrigger (%this, %obj, %triggerNum, %val); //stop here

							%target = firstWord(%ray);
							if(%target.getType() & $TypeMasks::PlayerObjectType)
							{
								L4B_SaveVictim(%obj,%target);
								L4B_ReviveDowned(%obj);
							}
							if(%target.getType() & $TypeMasks::VehicleObjectType && %target.getdatablock().image !$= "")
							{
								%obj.mountImage(%target.getdatablock().image, 0);
								%target.delete();
							}
						}
						else
						{
							if(%obj.issavior)
							{
								if(%obj.getmountedimage(0) == 0)
								%obj.stopthread(1);
								%obj.issavior = 0;
								%obj.isSaving.isBeingSaved = 0;
								%obj.isSaving = 0;
							}
							%obj.savetimer = 0;
							cancel(%obj.savesched);
						}
					}

			case 4:	if(%val)//Right click
					{    							
                    	if(%triggerTime - %obj.lastmeleetime > 3500)//Reset the delay if the player waits long enough, 3.5 seconds
                    	{
                        	%obj.lastmeleecount = 0;
	                        %obj.lastmeleetime = 0;
						}

						if(%obj.lastmeleetime < %triggerTime)//Meleeing
						{
							%obj.lastmeleecount++;
							%obj.lastmeleetime = (%triggerTime+400)+(40*%obj.lastmeleecount);
							
							%obj.playthread(3,"activate2");
							serverPlay3D("melee_swing" @ getRandom(1,2) @ "_sound",%obj.getHackPosition());

							%pos = %obj.getEyePoint();
							%radius = 5;
							%length = 5 * getWord(%obj.getScale(),2);
							%eyeVec = %obj.getEyeVector();
							%mask = $TypeMasks::PlayerObjectType;


							initContainerRadiusSearch(%pos,%radius,%mask);
							while(%hit = containerSearchNext())
							{
								%obscure = containerRayCast(%obj.getEyePoint(),%hit.getWorldBoxCenter(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
								%dot = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%hit.getposition(),%obj.getposition())));
		
								if(%hit == %obj || isObject(%obscure) || ContainerSearchCurrRadiusDist() > 5 || %dot < 0.5) continue;
								if(%hit.getState() $= "Dead" || !miniGameCanDamage(%obj,%hit) || %hit.getDatablock().resistMelee) continue;

								%obj.stresslevel++;

								%hit.spawnExplosion("SecondaryMeleeProjectile",%hit.getScale());
								serverPlay3D("melee_hit" @ getRandom(1,8) @ "_sound",%hit.getHackPosition());
								cancel(hLastFollowSched);
								cancel(hSpecialSched);
								%hit.playThread(3,"zstumble" @ getRandom(1,3));
								%hit.applyimpulse(%hit.getPosition(),VectorAdd(VectorScale(%obj.getForwardVector(),"500"),"0 0 250"));
								%hit.damage(%obj, %hit.getposition(), 15*getWord(%obj.getScale(),0), $DamageType::default);

								if(%hit.getClassName() $= "AIPlayer")
								{
									%hit.setMoveY(-0.5);
									%hit.setMoveX(0);
									%hit.setAimObject(%obj);
								}
							}												
						}
					}

			default:
		}
	}
}

function SurvivorPlayer::onNewDataBlock(%this,%obj)
{	
	Parent::onNewDataBlock(%this,%obj);

	if(!%obj.isMounted())
	{
		%obj.playthread(0,"root");
		%obj.playthread(3,"root");
		%obj.playthread(2,"root");
	}

	if(!isObject(%obj.billboardbot))
	{
		%obj.billboardbot = new Player() 
		{ 
			dataBlock = "EmptyPlayer";
			source = %obj;
			slotToMountBot = 5;
			lightToMount = "blankBillboard";
		};
	}
	
	%obj.SurvivorStress = 0;
	%obj.hType = "Survivors";
}

function SurvivorPlayer::OnCollision(%this, %obj, %col, %vec, %speed)
{
	Parent::OnCollision(%this, %obj, %col, %vec, %speed);

	if(isObject(%col) && (%col.getType() & $TypeMasks::PlayerObjectType))//Leaving this here for now, it doesn't work properly yet.
	if(%col.getdataBlock().isSurvivor)
	{
		%obj.disablePlayerCollision();
		%col.disablePlayerCollision();

		%obj.enablePlayerCollision();
		%col.enablePlayerCollision();
	}
}

function SurvivorPlayer::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{	
	//If the survivor doesn't take enough lethal damage or is not underwater, down them and avoid damaging them further.
	if(%obj.getstate() !$= "Dead" && %obj.getdamagelevel()+%damage >= %obj.getdataBlock().maxDamage && %damage <= %obj.getDatablock().maxDamage/1.333 && %obj.getWaterCoverage() != 1)
	{
		%obj.setdamagelevel(0);
		%obj.setenergylevel(100);
		%obj.isdowned = 1;
		%obj.setDatablock("SurvivorPlayerDowned");
		return;
	}

	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);

	if(%damageType $= $DamageType::Fall)//Extra fall damage effects
	{
		serverPlay3D("impact_fall_sound",%obj.getPosition());
		serverPlay3D("victismoked_sound",%obj.getPosition());

		if(isObject(ZombieHitProjectile))
		%p = new Projectile()
		{
			dataBlock = "ZombieHitProjectile";
			initialPosition = %obj.getPosition();
			sourceObject = %obj;
		};
		MissionCleanup.add(%p);
		%p.explode();
	}	

	if(%damageType == $DamageType::Fall && %obj.getState() $= "Dead")//Dismember legs and stops audio
	{
		%obj.stopAudio(0);
		%this.RbloodDismember(%obj,7,true,%position);
		%this.RbloodDismember(%obj,8,true,%position);
		%this.RbloodDismember(%obj,6,true,%position);
	}
}

function SurvivorPlayer::onDamage(%this,%obj,%damage)
{
	Parent::onDamage(%this,%obj,%damage);

	if(%obj.getState() $= "Dead" || !%damage) return;
	
    %simTime = getSimTime();
	%obj.stressLevel = %obj.stressLevel+(%damage*0.25);
	
	if(%obj.lastdamagetime < %simTime)
	{
		%obj.lastdamagetime = %simTime+500;			
		
		if(%damage >= 5) %sound = "survivor_pain" @ getRandom(1, 12) @ "_sound";
		if(%damage >= 20) %sound = "survivor_painmed" @ getRandom(1, 5) @ "_sound";
		if(%damage >= 30 || %obj.getDamagePercent() > 0.75) %sound = "survivor_painhigh" @ getRandom(1, 6) @ "_sound";
		if(%obj.getWaterCoverage() == 1) %sound = "survivor_pain_underwater_sound";

		if(%sound !$= "") %obj.playaudio(0,%sound);
	}		
}

function SurvivorPlayerDowned::L4BAppearance(%this,%obj,%client) { SurvivorPlayer::L4BAppearance(%this,%obj,%client); }

function SurvivorPlayerDowned::onImpact(%this, %obj, %col, %vec, %force) { SurvivorPlayer::onImpact(%this, %obj, %col, %vec, %force); }

function SurvivorPlayerDowned::onDisabled(%this,%obj) 
{
	Parent::onDisabled(%this,%obj);
	
	//if(isObject(%client = %obj.client))
	//{
		//%client.delayMusicTime = getSimTime();
		//%client.l4bMusic("musicData_L4D_death",false,"Music");
		//%client.deletel4bMusic("Stinger1");
		//%client.deletel4bMusic("Stinger2");
		//%client.deletel4bMusic("Stinger3");
	//}	 
	
	if(%obj.getWaterCoverage() == 1)
	{
		%obj.playaudio(0,"survivor_death_underwater" @ getRandom(1, 2) @ "_sound");
		%obj.emote(oxygenBubbleImage, 1);
		serverPlay3D("drown_bubbles_sound",%obj.getPosition());
		serverPlay3D("die_underwater_bubbles_sound",%obj.getPosition());
	}
	else %obj.playaudio(0,"survivor_death" @ getRandom(1, 24) @ "_sound");

	if(isObject(%obj.billboardbot.lightToMount)) %obj.billboardbot.delete();
}

function SurvivorPlayer::onDisabled(%this,%obj,%state) { SurvivorPlayerDowned::onDisabled(%this,%obj,%state); }

function SurvivorPlayerDowned::RbloodDismember(%this,%obj,%limb,%doeffects,%position) { SurvivorPlayer::RbloodDismember(%this,%obj,%limb,%doeffects,%position); }

function SurvivorPlayerDowned::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	if(!%obj.isBeingStrangled) %damage = %damage/3.25;//Make the downed player last a little longer if they aren't pinned	
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}

function SurvivorPlayerDowned::onDamage(%this, %obj, %delta)
{
	Parent::onDamage(%this, %obj, %delta);
	if(%delta > %this.maxDamage/15 && %obj.getState () !$= "Dead") %obj.playaudio(0, "survivor_painhigh" @ getRandom(1,6) @ "_sound");
}

function SurvivorPlayerDowned::onNewDataBlock(%this,%obj)
{	
	
	if(!%obj.hEater) %obj.playthread(0,sit);
	%obj.lastcry = getsimtime();
	%obj.playaudio(0,"survivor_painhigh1_sound");
	%this.DownLoop(%obj);

	if(isObject(%obj.billboardbot.lightToMount)) %obj.billboardbot.lightToMount.setdatablock("incappedBillboard");
	
	if(%obj.getClassName() $= "Player" && isObject(%minigame = getMinigameFromObject(%obj)))
	{		
		%minigame.L4B_ChatMessage("<color:FFFF00><bitmapk:Add-Ons/Gamemode_Left4Block/modules/add-ins/player_l4b/icons/ci_down>" SPC %obj.client.name,"victineedshelp_sound",true);
		%minigame.checkLastManStanding();
	}
	
	Parent::onNewDataBlock(%this,%obj);
}

function SurvivorPlayerDowned::DownLoop(%this,%obj)
{ 
	if(isobject(%obj) && %obj.getstate() !$= "Dead" && %obj.getdataBlock().isDowned)
	{
		if(!%obj.isBeingSaved)
		{
			%obj.addhealth(-5);
			%obj.setdamageflash(0.25);

			if(%obj.lastcry+10000 < getsimtime())
			{
				%obj.lastcry = getsimtime();
				%obj.playaudio(0,"survivor_painhigh1_sound");
				%obj.playthread(3,"plant");
			}			
		}
	
		cancel(%obj.downloop);
		%obj.downloop = %this.schedule(1000,DownLoop,%obj);
	}
	else return;
}

function Player::sapHealth(%obj,%threshold)
{
	if(isObject(%obj) && %obj.getState() !$= "Dead" && %obj.getDamageLevel() > 50 && (%obj.getDataBlock().maxDamage-%obj.getDamageLevel()) >= %threshold)
	{
		%obj.addhealth(-0.5);		
		%obj.playthread(3,"plant");
		%obj.playaudio(0,"norcough" @ getrandom(1,3) @ "_sound");

		cancel(%obj.sapHealthSchedule);
		%obj.sapHealthSchedule = %obj.schedule(getRandom(2000,3000),sapHealth,%threshold);
	}
}

function L4B_SaveVictim(%obj,%target)
{	
	if(miniGameCanDamage(%obj,%target) && %target.getState() !$= "Dead" && !%obj.getState() !$= "Dead")
	if(%target.isBeingStrangled && !%obj.isBeingStrangled && !%obj.hIsInfected)
	{
		%target.isBeingStrangled = 0;
		%target.hEater.SmokerTongueTarget = 0;
		%obj.playthread(3,activate2);
		%target.playthread(3,plant);

		if(%target.isHoleBot)
		%target.resetHoleLoop();

		if(%target.getClassName() $= "Player" && %obj.getClassName() $= "Player" && $Pref::Server::L4B2Bots::VictimSavedMessages)
		{
			chatMessageTeam(%target.client,'fakedeathmessage',"<color:00FF00>" @ %obj.client.name SPC "<bitmapk:add-ons/gamemode_left4block/add-ins/player_survivor/icons/CI_VictimSaved>" SPC %target.client.name);
			%target.client.centerprint("<color:FFFFFF>You were saved by " @ %obj.client.name,5);
			%obj.client.centerprint("<color:FFFFFF>You saved " @ %target.client.name,5);

			if(isObject(%minigame = %obj.client.minigame))
			%minigame.L4B_PlaySound("victisaved_sound");
		}

		if(%target.hEater.getDataBlock().getName() !$= "ZombieChargerHoleBot")
		{
			L4B_SpecialsPinCheck(%target.hEater, %target);
		}
		else 
		{
			return;
		}
	}
}

function Survivor_ReviveDownedCounter(%obj,%amount)
{
	%client = %obj.client;
	%per = %amount/4*100;
	%maxcounters = 20;
	%char = "|";for(%a =0; %a<%per/10; %a++){%fchar = %char @ %fchar;}
	centerprint(%client,"<just:center><color:00fa00>Get Up! <color:FFFFFF>: <just:left><color:FFFF00>" @ %fchar,1);
}

function L4B_ReviveDowned(%obj)
{
	if(%obj.getDatablock().getName() !$= "DownPlayerSurvivorArmor")
	{
		%eyeVec = %obj.getEyeVector();
		%startPos = %obj.getEyePoint();
		%endPos = VectorAdd(%startPos,vectorscale(%eyeVec,3));

		%mask = $TypeMasks::PlayerObjectType;
		%target = ContainerRayCast(%startPos, %endPos, %mask,%obj);
		if(%target)
		{
			if(%target.isdowned)
			{	
				if(%obj.savetimer < 4)
				{
					%obj.savetimer += 1;
					%target.isBeingSaved = %obj;
					if(%obj.issavior != 1)
					{
						%obj.issavior = 1;
						%obj.isSaving = %target;
						%obj.playthread(1,"armreadyright");
					}
					Survivor_ReviveDownedCounter(%obj, %obj.savetimer);
					Survivor_ReviveDownedCounter(%target, %obj.savetimer);
					%obj.savesched = schedule(1000,0,L4B_ReviveDowned,%obj);
				}
				else
				{
					Survivor_ReviveDownedCounter(%obj, %obj.savetimer);
					%obj.savetimer = 0;
					%target.isdowned = 0;
					%obj.isSaving = 0;
					%target.lastdamage = getsimtime();
					%target.sethealth(25);

					%target.SetDataBlock("SurvivorPlayerLow");

					%target.playthread(0,root);
					%obj.playthread(1,root);
					
					cancel(%target.energydeath1);
					cancel(%target.energydeath2);
					cancel(%target.energydeath3);

					if(%target.getClassName() $= "Player")
					{
						%target.client.centerprint("<color:00fa00>You were saved by " @ %obj.client.name,5);
						chatMessageTeam(%target.client,'fakedeathmessage',"<color:00fa00>" @ %obj.client.name SPC "<bitmapk:add-ons/gamemode_left4block/add-ins/player_survivor/icons/CI_VictimSaved>" SPC %target.client.name);
						%target.client.play2d("victirevived_sound");
						%obj.client.play2d("victirevived_sound");
					}

					return;
				}
			}
		}
	}
}

registerOutputEvent(fxDTSBrick, "RandomizeZombieSpecial");
registerOutputEvent(fxDTSBrick, "RandomizeZombieUncommon");
registerInputEvent ("fxDTSBrick", "onZombieTouch", "Self fxDTSBrick" TAB "Player Player" TAB "Bot Bot" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent ("fxDTSBrick", "onTankTouch", "Self fxDTSBrick" TAB "Player Player" TAB "Bot Bot" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent ("fxDTSBrick", "onDoorClose", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent ("fxDTSBrick", "onDoorOpen", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerOutputEvent ("fxDTSBrick", "zfakeKillBrick");

registerOutputEvent ("Player", "Safehouse","bool");
registerInputEvent ("fxDTSBrick", "onSurvivorTouch", "Self fxDTSBrick" TAB "Player Player" TAB "Bot Bot" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerOutputEvent(Player,RemoveItem,"datablock ItemData",1);

function Player::StunnedSlowDown(%obj)
{						
	if(!isObject(%obj) || %obj.getstate() $= "Dead") return;

	//%obj.SetTempSpeed(0.5);
	//talk(%obj.CurrentSpeedPenalty);

	//%obj.resetSpeedSched = %obj.schedule(2000,SetSpeed,true,%obj.CurrentSpeedPenalty);
}

function Player::SetTempSpeed(%obj,%slowdowndivider)
{						
	if(!isObject(%obj) || %obj.getstate() $= "Dead") return;

	%datablock = %obj.getDataBlock();
	%obj.setMaxForwardSpeed(%datablock.MaxForwardSpeed*%slowdowndivider);
	%obj.setMaxSideSpeed(%datablock.MaxSideSpeed*%slowdowndivider);
	%obj.setMaxBackwardSpeed(%datablock.maxBackwardSpeed*%slowdowndivider);

	%obj.setMaxCrouchForwardSpeed(%datablock.maxForwardCrouchSpeed*%slowdowndivider);
  	%obj.setMaxCrouchBackwardSpeed(%datablock.maxSideCrouchSpeed*%slowdowndivider);
  	%obj.setMaxCrouchSideSpeed(%datablock.maxSideCrouchSpeed*%slowdowndivider);

 	%obj.setMaxUnderwaterBackwardSpeed(%datablock.MaxUnderwaterBackwardSpeed*%slowdowndivider);
  	%obj.setMaxUnderwaterForwardSpeed(%datablock.MaxUnderwaterForwardSpeed*%slowdowndivider);
  	%obj.setMaxUnderwaterSideSpeed(%datablock.MaxUnderwaterForwardSpeed*%slowdowndivider);
}

function Player::SetSpeed(%obj,%bool,%slowdowndivider)
{						
	if(!isObject(%obj) || %obj.getstate() $= "Dead") return;

	if(!%bool) %slowdowndivider = 1;

	if(!%obj.CurrentSpeedPenalty) %obj.CurrentSpeedPenalty = 1;
	else %obj.CurrentSpeedPenalty = %slowdowndivider;

	%datablock = %obj.getDataBlock();
	%obj.setMaxForwardSpeed(%datablock.MaxForwardSpeed*%slowdowndivider);
	%obj.setMaxSideSpeed(%datablock.MaxSideSpeed*%slowdowndivider);
	%obj.setMaxBackwardSpeed(%datablock.maxBackwardSpeed*%slowdowndivider);

	%obj.setMaxCrouchForwardSpeed(%datablock.maxForwardCrouchSpeed*%slowdowndivider);
  	%obj.setMaxCrouchBackwardSpeed(%datablock.maxSideCrouchSpeed*%slowdowndivider);
  	%obj.setMaxCrouchSideSpeed(%datablock.maxSideCrouchSpeed*%slowdowndivider);

 	%obj.setMaxUnderwaterBackwardSpeed(%datablock.MaxUnderwaterBackwardSpeed*%slowdowndivider);
  	%obj.setMaxUnderwaterForwardSpeed(%datablock.MaxUnderwaterForwardSpeed*%slowdowndivider);
  	%obj.setMaxUnderwaterSideSpeed(%datablock.MaxUnderwaterForwardSpeed*%slowdowndivider);
}

function Player::Safehouse(%player,%bool)
{
	%minigame = getMiniGameFromObject(%player);
	if(%player.hType !$= "Survivors" || isEventPending(%minigame.resetSchedule)) return;

	if(%bool) %player.InSafehouse = 1;
	else %player.InSafehouse = 0;
}

function Player::RemoveItem(%player,%item,%client)  
{
	if(isObject(%player)) for(%i=0;%i<%player.dataBlock.maxTools;%i++)
    {
        %tool = %player.tool[%i];
        if(%tool == %item)
        {
            %player.tool[%i] = 0;
            messageClient(%client,'MsgItemPickup','',%i,0);
            if(%player.currTool == %i)
            {
                %player.updateArm(0);
                %player.unMountImage(0);
            }
        }
    }
}

function Player::checkIfUnderwater(%obj)
{
	if(%obj.getWaterCoverage() == 0)
	{
		if(%obj.oxygenCount == 6 && %obj.getState() !$= "Dead") 
		%obj.playaudio(0,"survivor_painhigh" @ getRandom(1,6) @ "_sound");
		%obj.oxygenCount = 0;
	}
   	cancel(%obj.oxygenTick);
}

function Player::oxygenTick(%obj)
{   
	if(!isObject(%obj) && %obj.getState() $= "Dead") return;
	
	if(%obj.getWaterCoverage() == 1)
	{
		%obj.oxygenCount = mClamp(%obj.oxygenCount++, 0, 6);	

		if(%obj.oxygenCount == 6) %obj.Damage(%obj, %obj.getPosition (), 25, $DamageType::Suicide);
		
		%obj.lastwatercoverage = getsimtime();
		%bubblepitch = 0.125*%obj.oxygenCount;
		%obj.emote(oxygenBubbleImage, 1);
		%obj.playthread(3,plant);

		if($oldTimescale $= "")
		$oldTimescale = getTimescale();
  		setTimescale(0.25+%bubblepitch);
  		serverPlay3D("drown_bubbles_sound",%obj.getPosition());
  		setTimescale($oldTimescale);
	}
	
	cancel(%obj.oxygenTick);
	%obj.oxygenTick = %obj.schedule(2500, oxygenTick);
}

function Armor::onBotMelee(%obj,%col)
{

}

function Armor::onPinLoop(%this,%obj,%col)
{

}