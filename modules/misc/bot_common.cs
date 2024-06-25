datablock fxDTSBrickData (BrickCommonZombie_HoleSpawnData)
{
	brickFile = "Add-ons/Bot_Hole/4xSpawn.blb";
	category = "Special";
	subCategory = "Holes - L4B";
	uiName = "Common Zombie Hole";
	iconName = "Add-Ons/Gamemode_Left4Block/modules/misc/icons/icon_zombie";

	bricktype = 2;
	cancover = 0;
	orientationfix = 1;
	indestructable = 1;

	isBotHole = 1;
	holeBot = "CommonZombieHoleBot";
};

datablock TSShapeConstructor(ZombieMDts) 
{
	baseShape = "./models/oldm.dts";
	sequence0 = "./models/animations/m_root.dsq root";
	sequence1 = "./models/animations/m_run.dsq run";
	sequence2 = "./models/animations/m_back.dsq back";
	sequence3 = "./models/animations/m_side.dsq side";
	sequence4 = "./models/animations/m_prone.dsq crouch";
	sequence5 = "./models/animations/m_pronerun.dsq crouchrun";
	sequence6 = "./models/animations/m_proneback.dsq crouchback";
	sequence7 = "./models/animations/m_proneside.dsq crouchside";
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
	sequence38 = "./models/animations/m_zattack1.dsq zattack1";
	sequence39 = "./models/animations/m_zattack2.dsq zattack2";
	sequence40 = "./models/animations/m_zattack3.dsq zattack3";
	sequence41 = "./models/animations/m_zstumble1.dsq zstumble1";
	sequence43 = "./models/animations/m_zstumble2.dsq zstumble2";
	sequence44 = "./models/animations/m_zstumble3.dsq zstumble3";
};

datablock PlayerData(CommonZombieHoleBot : SurvivorPlayer)
{
	shapeFile = ZombieMDts.baseShape;
	boundingBox = VectorScale ("1.25 1.25 2.65", 4);
	crouchBoundingBox = VectorScale ("1.25 1.25 1.00", 4);	
	renderFirstPerson = false;
	thirdpersononly = true;
	jumpForce = 9.5*100;
	minImpactSpeed = 20;
	airControl = 0.1;
	speedDamageScale = 0.5;
	isSurvivor = false;

    maxForwardSpeed = 11;
    maxSideSpeed = 10;
	maxBackwardSpeed = 9;

 	maxForwardCrouchSpeed = 7;
	maxSideCrouchSpeed = 6;
    maxBackwardCrouchSpeed = 5;

	cameramaxdist = 4;
    cameraVerticalOffset = 1;
    cameraHorizontalOffset = 0.6;
    cameratilt = 0.1;
    maxfreelookangle = 2;

	rideable = false;
	canRide = false;

	uiName = "Infected";
	maxTools = 0;
	maxWeapons = 0;
	maxdamage = 100;//Health

	rechargeRate = 1.15;
	maxenergy = 100;
	showEnergyBar = true;
	
	useCustomPainEffects = true;
	jumpSound = "";
	PainSound		= "";
	DeathSound		= "";

	hIsInfected = 1;
	hZombieL4BType = "Normal";
	hPinCI = "";
	hBigMeleeSound = "";
	
	//Hole Attributes
	isHoleBot = 1;

	//Spawning option
	hSpawnTooClose = 0;//Doesn't spawn when player is too close and can see it
	hSpawnTCRange = 0;//above range, set in brick units
	hSpawnClose = 0;//Only spawn when close to a player, can be used with above function as long as hSCRange is higher than hSpawnTCRange
	hSpawnCRange = 0;//above range, set in brick units

	hType = "Zombie"; //Enemy,Friendly, Neutral
	hNeutralAttackChance = 100;
	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Infected";//cannot contain spaces
	hTickRate = 8000;

	//Wander Options
	hWander = 1;//Enables random walking
	hSmoothWander = 1;
	hReturnToSpawn = 0;//Returns to spawn when too far
	hSpawnDist = 32;//Defines the distance bot can travel away from spawnbrick
	hMoveSlowdown = false;

	//Searching options
	hSearch = 1;//Search for Players
	hSearchRadius = 512;//in brick units
	hSight = 1;//Require bot to see player before pursuing
	hStrafe = 1;//Randomly strafe while following player
	hSearchFOV = 1;//if enabled disables normal hSearch
	hFOVRadius = 10;//max 10
	hAlertOtherBots = 1;//Alerts other bots when he sees a player, or gets attacked

	//Attack Options
	hMelee = 1;
	hAttackDamage = $Pref::L4B::Zombies::NormalDamage;
	hShoot = 1;
	hWep = "";
	hShootTimes = 4;
	hMaxShootRange = 60;
	hAvoidCloseRange = 0;
	hTooCloseRange = 0;
	
	//Misc options
	hAvoidObstacles = 1;
	hSuperStacker = 0;
	hSpazJump = 0;

	hAFKOmeter = 1;
	hIdle = 1;
	hIdleAnimation = 1;
	hIdleLookAtOthers = 1;
	hIdleSpam = 0;
	hSpasticLook = 1;
	hEmote = 1;
};

function CommonZombieHoleBot::onAdd(%this,%obj)
{			
	Parent::onAdd(%this,%obj);
	%obj.hDefaultL4BAppearance();
}

function CommonZombieHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	%obj.onL4BDatablockAttributes();
	%obj.hDefaultL4BAppearance();
	%obj.setscale("1 1 1");
	%obj.schedule(10,setenergylevel,%this.maxenergy);
}

function CommonZombieHoleBot::onDamage(%this,%obj)
{
	if(%obj.getstate() $= "Dead") return;

	if(%obj.lastdamage+1250 < getsimtime())
	{
		%obj.lastdamage = getsimtime();
		%obj.playthread(2,"plant");

		if(%obj.raisearms)
		{
			%obj.raisearms = 0;
			%obj.playthread(1,"root");
		}

		if(%obj.getWaterCoverage() == 1)
		{
			%obj.emote(oxygenBubbleImage, 1);
			serverPlay3D("drown_bubbles_sound",%obj.getPosition());
		}
		else if(%obj.isBurning)
		{
			switch(%obj.chest)	
			{
				case 0: %obj.playaudio(0,"zombiemale_ignite" @ getrandom(1,5) @ "_sound");
				case 1: %obj.playaudio(0,"zombiefemale_ignite1" @ getrandom(1,5) @ "_sound");
			}

			%obj.MaxSpazzClick = getrandom(16,32);
			%obj.hSpazzClick();
			
		}
		else switch(%obj.chest)	
		{
			case 0: %obj.playaudio(0,"zombiemale_pain" @ getrandom(1,8) @ "_sound");
			case 1: %obj.playaudio(0,"zombiefemale_pain" @ getrandom(1,8) @ "_sound");
		}
	}

	Parent::OnDamage(%this,%obj);
}

function CommonZombieHoleBot::RbloodDismember(%this,%obj,%limb,%doeffects,%position)
{
	Parent::RbloodDismember(%this,%obj,%limb,%doeffects,%position);

	if(%obj.nolegs && %obj.getState() !$= "Dead")//Ouch there goes my legs
	switch(%obj.chest)
	{
		case 0: %obj.playaudio(0,"zombiemale_ignite" @ getrandom(1,5) @ "_sound");
		case 1: %obj.playaudio(0,"zombiefemale_ignite" @ getrandom(1,5) @ "_sound");
	}
}

function CommonZombieHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{	
	%limb = %obj.rgetDamageLocation(%position);
	if(%damageType !$= $DamageType::FallDamage || %damageType !$= $DamageType::Impact)
	if(!%limb) %damage = %damage*2;

	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}

function CommonZombieHoleBot::RBloodSimulate(%this, %obj, %position, %damagetype, %damage)
{
	Parent::RBloodSimulate(%this, %obj, %position, %damagetype, %damage);
}

function CommonZombieHoleBot::onImpact(%this, %obj, %col, %vec, %force)
{
	if(%obj.getState() !$= "Dead")
	{				
		%zvector = getWord(%vec,2);

		if(%zvector > %this.minImpactSpeed) 
		{
			%force = %force*(%force*0.025);					
			if(%zvector > %this.minImpactSpeed*1.333) %force = %force*(%force*0.01);
			%obj.playthread(0,"land");
		}
	}	

	Parent::onImpact(%this, %obj, %col, %vec, %force);	
}

function CommonZombieHoleBot::onDisabled(%this,%obj)
{
	Parent::OnDisabled(%this,%obj);	

	if(isObject(%obj.client)) commandToClient(%obj.client,'SetVignette',$EnvGuiServer::VignetteMultiply,$EnvGuiServer::VignetteColor);

	if(isObject(%minigame = getMiniGameFromObject(%obj))) 
	{
		for(%i=0;%i<getRandom(1,5);%i++)
		{
			%pos = %obj.getHackPosition();
			%vec = %obj.getVelocity();

			%item = new Item()
			{
				dataBlock = "CashItem";
				position = %pos;
				spawnBrick = 0;
			};

			if(isObject(ClientGroup.getObject(0).player))
			%itemvec = vectorAdd(vectorScale(VectorNormalize(vectorSub(ClientGroup.getObject(0).player.getposition(),%obj.getMuzzlePoint(2))),getRandom(75,100)),"0 0 10");
			else %itemvec = vectorAdd(%obj.getVelocity(),getRandom(-8,8) SPC getRandom(-8,8) SPC 10);

			%item.setVelocity(%itemvec);
			%item.schedulePop();
		}
		
		if(%obj.spawnType $= "Horde")
		{
			%minigame.hordecount--;
			if(%minigame.hordecount <= 0 && %minigame.directorMusicActive)
			{
				%minigame.directorMusicActive = false;
				%minigmae.hordecount = 0;

				if(%minigame.RoundType $= "Horde") %minigame.RoundEnd();
				else
				{
    				%minigame.l4bMusic("drum_suspense_end_sound",false,"Stinger1");
					%minigame.deletel4bMusic("Music");
	    			%minigame.deletel4bMusic("Music2");
				}
			}
		}
	}	

	if(%obj.getWaterCoverage() == 1) serverPlay3D("die_underwater_bubbles_sound",%obj.getPosition());	
	else switch(%obj.chest)
	{
		case 0: %obj.playaudio(0,"zombiemale_death" @ getrandom(1,10) @ "_sound");
		case 1: %obj.playaudio(0,"zombiefemale_death" @ getrandom(1,10) @ "_sound");
	}
}

function CommonZombieHoleBot::onBotLoop(%this,%obj)
{
	if(%obj.getWaterCoverage() == 1) %obj.damage(%obj,%obj.getPosition(),%obj.getdatablock().maxDamage/1.25,$DamageType::Suicide);

	switch$(%obj.hState)
	{
		case "Following": 	if(getRandom(1,2) == 1) %obj.hSpazzClick();							
							%obj.playthread(2,plant);

							switch(%obj.chest)
							{
								case 0: %obj.playaudio(0,"zombiemale_attack" @ getrandom(1,10) @ "_sound");
								case 1: %obj.playaudio(0,"zombiefemale_attack" @ getrandom(1,12) @ "_sound");
							}

							if(isObject(%minigame = getMiniGameFromObject(%obj)) && !%obj.hasSpottedOnce)
							{
								%minigame.hordecount++;
								%obj.hasSpottedOnce = true;
								if(%miniGame.hordecount >= 15 && !%minigame.directorMusicActive)
								{
									%minigame.directorMusicActive = true;
									%minigame.l4bMusic("musicData_L4D_horde_combat" @ getRandom(1,3),true,"Music");
    								%minigame.l4bMusic("drum_suspense_end_sound",false,"Stinger1");
								}
							}

		default: 	if(isObject(%minigame = getMiniGameFromObject(%obj)) && %obj.hasSpottedOnce) 
				 	{
				 		%obj.hasSpottedOnce = false;
				 		%minigame.hordecount--;
				 		if(%minigame.hordecount <= 0 && %minigame.directorMusicActive)
				 		{
				 			%minigame.directorMusicActive = false;
				 			%minigmae.hordecount = 0;

				 			if(%minigame.RoundType $= "Horde") %minigame.RoundEnd();
				 			else
				 			{
				 				%minigame.l4bMusic("drum_suspense_end_sound",false,"Stinger1");
				 				%minigame.deletel4bMusic("Music");
				 				%minigame.deletel4bMusic("Music2");
				 			}
				 		}
				 	}

				 	if(!isObject(%obj.distraction)) %obj.hSearch = 1;
					if(%obj.raisearms)
					{	
						%obj.playthread(1,"root");
						%obj.raisearms = false;
					}
				 	%obj.hNoSeeIdleTeleport();		
	}
}

function CommonZombieHoleBot::onBotFollow(%this,%obj,%targ)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead" || !isObject(%obj.hFollowing) || %obj.hFollowing.getState() $= "Dead") return;

	cancel(%obj.hLastFollowSched);
	%obj.hLastFollowSched = %this.schedule(750,onBotFollow,%obj);

	if((%distance = vectordist(%obj.getposition(),%obj.hFollowing.getposition())) < 20)
	{		
		if(!%obj.raisearms)
		{	
			%obj.playthread(1,"armReadyboth");
			%obj.raisearms = true;
		}

		if(%distance < 5)
		{
			%this.onTrigger(%obj,0,true);
			%obj.setMoveX(0);
			%obj.setMoveY(1);
			%obj.setmoveobject(%obj.hFollowing);
		}
	}
	else if(%obj.raisearms)
	{	
		%obj.playthread(1,"root");
		%obj.raisearms = false;
	}
}

function CommonZombieHoleBot::onBotMelee(%this,%obj,%col)
{			
	%obj.stopaudio(1);
	%obj.playaudio(1,"melee_hit" @ getrandom(1,8) @ "_sound");
	
	if(%col.getType() & $TypeMasks::PlayerObjectType)
	{
		%meleeimpulse = mClamp(%obj.hLastMeleeDamage,1,10);		
		if(%col.getClassName() $= "Player") %col.spawnExplosion("ZombieHitProjectile",%meleeimpulse/4 SPC %meleeimpulse/4 SPC %meleeimpulse/4);
		%col.playthread(3,"plant");
		%col.StunnedSlowDown(3);
	}
}

function CommonZombieHoleBot::onTrigger(%this,%obj,%triggerNum,%val)
{
	Parent::onTrigger(%this, %obj, %triggerNum,%val);

	if(!isObject(%obj) || %obj.getState() $= "Dead") return;
		
	if(%val) switch(%triggerNum)
	{
		case 0: if(isObject(%obj.hFollowing)) %targ = %obj.hFollowing;
				else if(isObject(%obj.lastactivated) && %obj.lastactivated.getType() && $TypeMasks::PlayerObjectType) %targ = %obj.lastactivated;
				else return;
			
				if(!isEventPending(%obj.MeleeSched))
				{
					%obj.playthread(2,"zAttack" @ getRandom(1,3));
					cancel(%obj.MeleeSched);
					%obj.MeleeSched = %this.schedule(350,Melee,%obj,%targ);
				}
	}
}

function CommonZombieHoleBot::Melee(%this,%obj,%targ)
{
	if(isObject(%obj) && %obj.getState() !$= "Dead") %obj.hMeleeAttack(%targ);
}

function CommonZombieHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{	
	//if(getRandom(1,10) == 1)
	//{ 
	//	L4B_pushClientSnapshot(%obj,0,true);
	//	return;
	//}

	%shirtColor = getRandomBotRGBColor();
	%accentColor = getRandomBotRGBColor();
	%pantsColor = getRandomBotPantsColor();
	%shoeColor = getRandomBotPantsColor();
	%hatColor = getRandomBotRGBColor();
	%packColor = getRandomBotRGBColor();
	
	%handColor = %skinColor;
	%larmColor = %shirtColor;
	%rarmColor = %shirtColor;
	%rLegColor = %shoeColor;
	%lLegColor = %shoeColor;

	if(getRandom(1,4) == 1)
	{
		if(getRandom(1,0)) %larmColor = %skinColor;
		if(getRandom(1,0)) %rarmColor = %skinColor;
		if(getRandom(1,0)) %rLegColor = %skinColor;
		if(getRandom(1,0)) %lLegColor = %skinColor;
	}

	%pack2 = 0;
	%accent = 0;
	%obj.accentColor = %accentColor;
	%obj.accent =  %accent;
	%obj.hatColor = %hatColor;
	%obj.hat = %hat;
	%obj.headColor = %skinColor;
	%obj.faceName = %face;
	%obj.chest =  %chest;
	%obj.decalName = %decal;
	%obj.chestColor = %shirtColor;
	%obj.pack =  0;
	%obj.packColor =  %packColor;
	%obj.secondPack =  %pack2;
	%obj.secondPackColor =  %packColor;
	%obj.larm =  "0";
	%obj.larmColor = %larmColor;
	%obj.lhand =  0;
	%obj.lhandColor = %handColor;
	%obj.rarm =  "0";
	%obj.rarmColor = %rarmColor;
	%obj.rhandColor = %handColor;
	%obj.rhand = 0;
	%obj.hip =  "0";
	%obj.hipColor = %pantsColor;
	%obj.lleg =  0;
	%obj.llegColor = %lLegColor;
	%obj.rleg =  0;
	%obj.rlegColor = %rLegColor;

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

function CommonZombieHoleBot::L4BAppearance(%this,%obj,%client) { SurvivorPlayer::L4BAppearance(%this,%obj,%client); }