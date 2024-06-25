datablock fxDTSBrickData (BrickZombieCharger_HoleSpawnData : BrickCommonZombie_HoleSpawnData)
{
	uiName = "Zombie Charger Hole";
	iconName = "Add-Ons/Gamemode_Left4Block/modules/misc/icons/icon_charger";

	holeBot = "ZombieChargerHoleBot";
};

datablock TSShapeConstructor(ChargerMDts : ZombieMDts) 
{
	baseShape = "./models/mcharger.dts";
	sequence45 = "./models/animations/m_chargermelee3.dsq chargermelee3";
	sequence46 = "./models/animations/m_chargermelee2.dsq chargermelee2";
	sequence47 = "./models/animations/m_chargermelee1.dsq chargermelee1";
	sequence48 = "./models/animations/m_chargeridle.dsq chargeridle";
	sequence49 = "./models/animations/m_chargersmash.dsq chargersmash";
};

datablock PlayerData(ZombieChargerHoleBot : CommonZombieHoleBot)
{
	shapeFile = ChargerMDts.baseShape;
	uiName = "Charger Infected";
	minImpactSpeed = 25;
	airControl = 0.1;
	speedDamageScale = 0.2;
	maxdamage = 100;//Health
	hName = "Charger";//cannot contain spaces
	hTickRate = 4000;
	hMeleeCI = "Charger";
	hAttackDamage = $Pref::L4B::Zombies::SpecialsDamage;

	cameramaxdist = 4;
    cameraVerticalOffset = 1;
    cameraHorizontalOffset = 0.6;
    cameratilt = 0.1;
    maxfreelookangle = 2;

    maxForwardSpeed = 8;
    maxBackwardSpeed = 7;
    maxSideSpeed = 6;

 	maxForwardCrouchSpeed = 6;
    maxBackwardCrouchSpeed = 5;
    maxSideCrouchSpeed = 4;

	hIsInfected = 2;
	hZombieL4BType = "Special";
	resistMelee = true;
	hPinCI = "<bitmapk:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci_charger2>";
	SpecialCPMessage = "Right click to charge <br>\c6Charge to pin non-infected";
	hBigMeleeSound = "charger_punch1_sound";

	rechargeRate = 0.75;
	maxenergy = 100;
	showEnergyBar = true;
};

function ZombieChargerHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);	
	%obj.setscale("1.15 1.15 1.15");
}

function ZombieChargerHoleBot::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}


function ZombieChargerHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	%limb = %obj.rgetDamageLocation(%position);
	if(%damageType !$= $DamageType::FallDamage || %damageType !$= $DamageType::Impact)
	if(%limb) %damage = %damage/4;
	else %damage = %damage/2;
	
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}

function ZombieChargerHoleBot::onImpact(%this, %obj, %col, %vec, %force)
{
	Parent::onImpact(%this, %obj, %col, %vec, %force);

	if((%class = %col.getClassName) !$= "AIPlayer" || %class !$= "Player")
	{
		%obj.playThread(3,"zstumble" @ getrandom(1,3));
		cancel(%obj.StartAfterCharge);
		if(!%obj.hLoopActive) %obj.StartAfterCharge = %obj.schedule(4000,startHoleLoop);
		%this.onDamage(%obj,10);
		%obj.setMoveY(-0.375);
		%obj.setMoveX(0);
		%obj.setAimObject(%col);
	}
}

function ZombieChargerHoleBot::onPinLoop(%this,%obj,%col)
{
	if(L4B_SpecialsPinCheck(%obj,%col))
	{
		if((%force = mFloor(vectorDot(getWords(%obj.getVelocity(),0,1), %obj.getForwardVector()))) < 5)
		{
			if(!%obj.hitMusic && isObject(%col.client)) 
			{
				%col.client.l4bMusic("charger_pin_sound", true, "Music");
				%obj.hitMusic = true;
			}

			if(%obj.getclassname() $= "AIPlayer")
			{
				%obj.stopHoleLoop();
				%obj.hClearMovement();
			}

			%obj.mountImage(HateImage, 3);
			%obj.setenergylevel(0);
			%obj.playThread(1,"chargersmash");
			%obj.schedule(100,playaudio,0,"charger_pummel" @ getrandom(1,4) @ "_sound");
			%obj.schedule(250,playaudio,3,"charger_smash_sound");
			%this.schedule(250,RBloodSimulate,%col, %col.gethackposition(), 1, 25);
			%col.schedule(250,playthread,3,"plant");
			%col.schedule(250,damage,%obj, %col.getposition(), $Pref::L4B::Zombies::SpecialsDamage, $DamageType::Charger);			
			%obj.schedule(250,spawnExplosion,pushBroomProjectile,"0.5 0.5 0.5");
		}
		%this.schedule(1250,onPinLoop,%obj,%col);
	}
}

function ZombieChargerHoleBot::onBotLoop(%this,%obj)
{	
	if(%obj.getstate() !$= "Dead" && %obj.lastIdle+5000 < getsimtime())
	{
		%obj.playthread(3,plant);
		%obj.lastIdle = getsimtime();
	
		switch$(%obj.hState)
		{
			case "Wandering":	%obj.isStrangling = false;
								%obj.hNoSeeIdleTeleport();
								%obj.setMaxForwardSpeed(9);
								%obj.playaudio(0,"charger_lurk" @ getrandom(1,4) @ "_sound");		
								%obj.playthread(1,"root");
								%obj.raisearms = 0;

			case "Following": 	if(!isEventPending(%obj.SpecialSched)) %obj.playaudio(0,"charger_recognize" @ getrandom(1,4) @ "_sound");
		}

		if(%obj.hEating) %obj.hClearMovement();
	}
}

function ZombieChargerHoleBot::onBotFollow(%this,%obj,%targ )
{
	if((isObject(%obj) && %obj.getState() !$= "Dead" && %obj.hLoopActive && !isEventPending(%obj.SpecialSched)) && (isObject(%targ) && %targ.getState() !$= "Dead")) 
	%this.schedule(750,onBotFollow,%obj,%targ);
	else return;	

	%distance = vectorDist(%obj.getposition(),%targ.getposition());
	
	if(%distance < 75)
	{				
		if(%obj.GetEnergyLevel() >= %this.MaxEnergy && !%obj.isStrangling) 
		{
			%obj.setaimobject(%targ);
			%this.onTrigger(%obj,4,1);
		}
		if(%distance < 3.5)
		{
			%this.onTrigger(%obj,0,true);
			%obj.setMoveX(0);
			%obj.setMoveY(1);
			%obj.setmoveobject(%targ);
		}
	}
}	

function ZombieChargerHoleBot::onDamage(%this,%obj,%delta)
{
	Parent::onDamage(%this,%obj,%delta);	

    if(%obj.lastdamage+1000 < getsimtime())
	{			
		if(%obj.getstate() !$= "Dead") %obj.playaudio(0,"charger_pain" @ getrandom(1,4) @ "_sound");
		else 
		{
			%obj.playaudio(0,"charger_die" @ getrandom(1,2) @ "_sound");

			if(isObject(%obj.hEating))
			{
				%obj.hEating.isBeingStrangled = 0;
				L4B_SpecialsPinCheck(%obj,%obj.hEating);
			}			
		}

		%obj.playthread(2,"plant");
		%obj.lastdamage = getsimtime();

		if(%obj.raisearms)
		{
			%obj.raisearms = false;	
			%obj.playthread(1,plant);
		}		
	}
}	

function ZombieChargerHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{	
	%hatColor = getRandomBotRGBColor();
	%packColor = getRandomBotRGBColor();
	%pack2Color = getRandomBotRGBColor();
	%accentColor = getRandomBotRGBColor();
	%pantsrandmultiplier = getrandom(2,8)*0.25;
	%pantsColorRand = getRandomBotRGBColor();
	%pantsColor = getWord(%pantsColorRand,0)*%pantsrandmultiplier SPC getWord(%pantsColorRand,1)*%pantsrandmultiplier SPC getWord(%pantsColorRand,2)*%pantsrandmultiplier SPC 1;
	%shoeColor = %pantsColor;
	%shirtColor = %skinColor;
	%larmColor = %shirtColor;
	%chargerhandColor = getWord(%skinColor,0)*0.5 SPC getWord(%skinColor,1)*0.5 SPC getWord(%skinColor,2)*0.5 SPC 1;
	%rarmColor = %chargerhandColor;
	%handColor = %skinColor;

	%rLegColor = getRandom(0,1);
	if(%rLegColor) %rLegColor = %shoeColor; else %rLegColor = %skinColor;
	%lLegColor = getRandom(0,1);
	if(%lLegColor) %lLegColor = %shoeColor;

	%obj.accentColor = %accentColor;
	%obj.accent =  0;
	%obj.hatColor = %hatColor;
	%obj.hat = 0;
	%obj.headColor = %skinColor;
	%obj.faceName = %face;
	%obj.chest =  0;
	%obj.decalName = "worm_engineer";
	%obj.chestColor = %shirtColor;
	%obj.pack =  %pack;
	%obj.packColor =  %packColor;
	%obj.secondPack =  0;
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

function ZombieChargerHoleBot::L4BAppearance(%this,%client,%obj)
{
	%obj.hideNode("ALL");
	%obj.unHideNode("chest");
	%obj.unHideNode("LchargerArm");
	%obj.unHideNode("RarmCharger");	
	%obj.unHideNode("RarmCharger");	
	%obj.unHideNode(("rarm"));
	%obj.unHideNode("headskin");
	%obj.unHideNode("pants");
	%obj.unHideNode("rshoe");
	%obj.unHideNode("lshoe");
	%obj.unhidenode("gloweyes");
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

	if(%obj.getDatablock().hType $= "Zombie" && %obj.getclassname() $= "Player")
	{
		%skin = %client.headColor;
		%zskin = getWord(%skin,0)/2.75 SPC getWord(%skin,1)/1.5 SPC getWord(%skin,2)/2.75 SPC 1;			

		%headColor = %zskin;
		if(%client.chestColor $= %skin) %chestColor = %zskin;
		if(%client.rArmColor $= %skin) %rarmcolor = %zskin;
		if(%client.lArmColor $= %skin) %larmcolor = %zskin;
		if(%client.rhandColor $= %skin) %rhandcolor = %zskin;
		if(%client.lhandColor $= %skin) %lhandcolor = %zskin;
		if(%client.hipColor $= %skin) %hipcolor = %zskin;
		if(%client.rLegColor $= %skin) %rlegcolor = %zskin;
		if(%client.lLegColor $= %skin) %llegColor = %zskin;
	}

	%obj.setFaceName("asciiTerror");
	%obj.setDecalName(%client.decalName);	
	%obj.setNodeColor("LchargerArm",%larmColor);
	%obj.setNodeColor("RarmCharger",%rarmColor);
	%obj.setNodeColor("headskin",%headColor);
	%obj.setNodeColor("chest",%chestColor);
	%obj.setNodeColor("pants",%hipColor);
	%obj.setNodeColor("rarm",%rarmColor);
	%obj.setNodeColor("rarmSlim",%rarmColor);
	%obj.setNodeColor("larmSlim",%larmColor);
	%obj.setNodeColor("rshoe",%rlegColor);
	%obj.setNodeColor("lshoe",%llegColor);
	%obj.setNodeColor("headpart1",%headColor);
	%obj.setNodeColor("headpart2",%headColor);
	%obj.setNodeColor("headpart3",%headColor);
	%obj.setNodeColor("headpart4",%headColor);
	%obj.setNodeColor("headpart5",%headColor);
	%obj.setNodeColor("headpart6",%headColor);
	%obj.setNodeColor("chestpart1",%chestColor);
	%obj.setNodeColor("chestpart2",%chestColor);
	%obj.setNodeColor("chestpart3",%chestColor);
	%obj.setNodeColor("chestpart4",%chestColor);
	%obj.setNodeColor("chestpart5",%chestColor);
	%obj.setNodeColor("pants",%hipColor);
	%obj.setNodeColor("pantswound",%hipColor);	
	%obj.setnodeColor("gloweyes","1 1 0 1");	
	%obj.setNodeColor("headskullpart1","1 0.5 0.5 1");
	%obj.setNodeColor("headskullpart2","1 0.5 0.5 1");
	%obj.setNodeColor("headskullpart3","1 0.5 0.5 1");
	%obj.setNodeColor("headskullpart4","1 0.5 0.5 1");
	%obj.setNodeColor("headskullpart5","1 0.5 0.5 1");
	%obj.setNodeColor("headskullpart6","1 0.5 0.5 1");
	//%obj.setNodeColor("headstump","1 0 0 1");
	%obj.setNodeColor("legstumpr","1 0 0 1");
	%obj.setNodeColor("legstumpl","1 0 0 1");
	%obj.setNodeColor("skeletonchest","1 0.5 0.5 1");
	%obj.setNodeColor("skelepants","1 0.5 0.5 1");
	%obj.setNodeColor("organs","1 0.6 0.5 1");
	%obj.setNodeColor("brain","1 0.75 0.746814 1");
}

function ZombieChargerHoleBot::onTrigger (%this, %obj, %triggerNum, %val)
{	
	Parent::onTrigger(%this,%obj,%triggerNum,%val);

	if(!isObject(%obj) || %obj.getState() $= "Dead") return;

	if(isObject(%obj.hFollowing)) %targ = %obj.hFollowing;
	else if(isObject(%obj.lastactivated))
	{
		if(%obj.lastactivated.getType() & $TypeMasks::PlayerObjectType) %targ = %obj.lastactivated;
		else return;
	}
	else return;

	if(%val) switch(%triggerNum)
	{
		case 0: if(!isEventPending(%obj.MeleeSched))
				{
					%obj.playthread(2,"chargermelee" @ getRandom(1,3));
					cancel(%obj.MeleeSched);
					%obj.MeleeSched = %this.schedule(350,Melee,%obj,%targ);
				}

		case 4: if(%obj.GetEnergyLevel() >= %this.MaxEnergy)
				{
					if(!isEventPending(%obj.SpecialSched))
					{
						%obj.playthread(1,"armReadyright");
						%obj.playaudio(0,"charger_warn" @ getrandom(1,3) @ "_sound");
						%obj.setMaxForwardSpeed(9);
						%obj.SpecialSched = %this.schedule(1000,Special,%obj);
					}
				}
	}		
}

function ZombieChargerHoleBot::Melee(%this,%obj,%targ)
{
	CommonZombieHoleBot::Melee(%this,%obj,%targ);
}

function ZombieChargerHoleBot::Special(%this,%obj)
{
	if(isObject(%obj) && %obj.getState() !$= "Dead")
	{
		%obj.WalkAfterCharge = %obj.schedule(6000,setMaxForwardSpeed,9);
		%obj.playaudio(0,"charger_charge" @ getrandom(1,2) @ "_sound");
		%obj.mountImage(HateImage, 3);
		%obj.setMaxForwardSpeed(50);
		%obj.setenergylevel(0);

		if(%obj.getClassName() $= "AIPlayer")
		{
			%obj.stopHoleLoop();
			%obj.schedule(250,hShootAim,%obj.hFollowing);
			%obj.schedule(500,clearAim);
			%obj.StartAfterCharge = %obj.schedule(4000,startHoleLoop);
			%obj.setmoveY(1);
		}
	}
}

function ZombieChargerHoleBot::onBotMelee(%this,%obj,%col)
{
	%obj.bigZombieMelee(%col);
}