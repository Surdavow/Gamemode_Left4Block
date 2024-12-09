datablock fxDTSBrickData (BrickZombieJockey_HoleSpawnData : BrickCommonZombie_HoleSpawnData)
{
	uiName = "Zombie Jockey Hole";
	iconName = "Add-Ons/Gamemode_Left4Block/modules/misc/icons/icon_Jockey";

	holeBot = "ZombieJockeyHoleBot";
};

datablock PlayerData(ZombieJockeyHoleBot : CommonZombieHoleBot)
{
	uiName = "Jockey Infected";
	jumpForce = 90*8;
	minImpactSpeed = 10;
	airControl = 0.1;
	speedDamageScale = 0.01;

    maxForwardSpeed = 10;
    maxBackwardSpeed = 8;
    maxSideSpeed = 9;

 	maxForwardCrouchSpeed = 7;
    maxBackwardCrouchSpeed = 5;
    maxSideCrouchSpeed = 6;

	cameramaxdist = 4;
    cameraVerticalOffset = 1;
    cameraHorizontalOffset = 0.6;
    cameratilt = 0.1;
    maxfreelookangle = 2;

	maxdamage = 100;//Health
	jumpForce = 100 * 10; //8.3 * 90;
	hTickRate = 4000;

	hName = "Jockey";//cannot contain spaces
	hAttackDamage = $Pref::L4B::Zombies::SpecialsDamage;
	
	hIsInfected = 2;
	hZombieL4BType = "Special";
	hPinCI = "<bitmapk:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci_jockey2>";
	SpecialCPMessage = "Right click to leap <br>\c6Jump on non-infected to control them";
	hBigMeleeSound = "";

	rechargeRate = 1.5;
	maxenergy = 100;
	showEnergyBar = true;
};

function ZombieJockeyHoleBot::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

function ZombieJockeyHoleBot::doDismount(%this, %obj, %forced) 
{ 
	if(isObject(%obj.hEating)) return Parent::doDismount(%this, %obj, %forced);
}

function ZombieJockeyHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
	%obj.setscale("0.75 0.75 0.75");
}

function ZombieJockeyHoleBot::onPinLoop(%this,%obj,%col)
{
	if(L4B_SpecialsPinCheck(%obj,%col))
	{
		if(%obj.getClassName() $= "AIPlayer") %obj.hRunAwayFromPlayer(%col);
		%col.damage(%obj.hFakeProjectile, %col.getposition(), $Pref::L4B::Zombies::SpecialsDamage/2.5, $DamageType::Jockey);
		%obj.playthread(2,"zAttack" @ getRandom(1,3));
		%obj.playThread(3,talk);
		%col.playThread(2,plant);
		%obj.playaudio(1,"melee_hit" @ getrandom(1,8) @ "_sound");

		%this.schedule(750,onPinLoop,%obj,%col);
	}
}

function ZombieJockeyHoleBot::OnCollision(%this, %obj, %col, %fade, %pos, %norm)
{
	Parent::OnCollision(%this, %obj, %col, %fade, %pos, %norm);	

	if(%obj.getState $= "Dead" || %col.getdatablock().isDowned) return;
	
	if(getWord(%obj.getvelocity(),2) != 0) if((%oScale = getWord(%obj.getScale(),0)) == 0.75) %obj.SpecialPinAttack(%col);
	else if(checkHoleBotTeams(%obj,%col)) %obj.hJump();
}

function ZombieJockeyHoleBot::onBotLoop(%this,%obj)
{	
	switch$(%obj.hState)
	{
		case "Wandering":	%obj.setMaxForwardSpeed(9);
							%obj.isStrangling = false;
							%obj.hNoSeeIdleTeleport();
							%obj.playThread(0,talk);
							%sound = "jockey_lurk" @ getrandom(1,4) @ "_sound";				
		case "Following": 	%sound = "jockey_recognize" @ getrandom(1,2) @ "_sound";
	}

	if(getsimtime() >= %obj.LastLoopSound+4000)
	{
		%obj.playaudio(0,%sound);		
		%obj.LastLoopSound = getSimTime();
	}
}

function ZombieJockeyHoleBot::onBotFollow(%this,%obj,%targ)
{
	if((isObject(%obj) && %obj.getState() !$= "Dead" && %obj.hLoopActive && !%obj.isStrangling) && (isObject(%targ) && %targ.getState() !$= "Dead")) 
	%this.schedule(500,onBotFollow,%obj,%targ);
	else return;
	
	if(%targ != %obj.hIgnore)
	{
		if(VectorDist(%obj.getposition(), %targ.getposition()) < 20 && getWord(%obj.getvelocity(),2) <= 5)
		{	
		if(!%obj.raisearms)
		{	
			%obj.playthread(1,"armReadyboth");
			%obj.raisearms = true;
		}
		
		%obj.hJump();
		%obj.schedule(325,hShootAim,%targ);
		%this.schedule(375,onTrigger,%obj,4,1);
		}
		else if(%obj.raisearms)
		{	
			%obj.playthread(1,"root");
			%obj.raisearms = false;
		}
	}
	else 
	{
		%obj.stopHoleLoop();
		%obj.hRunAwayFromPlayer(%targ);
		%obj.schedule(2000,startHoleLoop);
	}
}

function ZombieJockeyHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	%limb = %obj.rgetDamageLocation(%position);
	if((%damageType !$= $DamageType::FallDamage || %damageType !$= $DamageType::Impact) && %limb) %damage = %damage/8;
	
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}

function ZombieJockeyHoleBot::onDamage(%this,%obj,%delta)
{	
	Parent::onDamage(%this,%obj,%source,%pos,%damage,%type);

	if(%obj.lastdamage+500 < getsimtime())
	{
		if(%obj.getstate() !$= "Dead") %obj.playaudio(0,"jockey_pain" @ getrandom(1,4) @ "_sound");
		else %obj.playaudio(0,"jockey_death" @ getrandom(1,3) @ "_sound");

		if(%obj.raisearms)
		{
			%obj.raisearms = 0;
			%obj.playthread(1,"root");			
		}

		if(isObject(%obj.hEating))
		{
			%obj.hEating.isBeingStrangled = false;
			L4B_SpecialsPinCheck(%obj,%obj.hEating);
			%obj.addvelocity("0 0 15");
		}

		%obj.playthread(2,"plant");
		%obj.lastdamage = getsimtime();
	}	
}

function ZombieJockeyHoleBot::onTrigger(%this, %obj, %triggerNum, %val)
{	
	Parent::onTrigger (%this, %obj, %triggerNum, %val);

	if(isObject(%obj) && %obj.getstate() !$= "Dead")
	{
		if(%val) switch(%triggerNum)
		{
			case 0: CommonZombieHoleBot::onTrigger(%this, %obj, %triggerNum, %val);
			case 4: if(%obj.GetEnergyLevel() >= %this.maxenergy && !%obj.isStrangling)
					{
						%normVec = VectorNormalize(vectoradd(%obj.getEyeVector(),"0 0 0.005"));
						%obj.setvelocity(vectorscale(%normVec,20));
						%obj.playthread(2,"activate2");
						%obj.setenergylevel(0);
					}
		}
	}
}

function ZombieJockeyHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{
	%decal = "AAA-None";
	%hat = 0;	
	%pack = 0;
	%pack2 = 0;
	%accent = 0;
	%hatColor = getRandomBotRGBColor();
	%packColor = getRandomBotRGBColor();
	%shirtColor = getRandomBotRGBColor();
	%accentColor = getRandomBotRGBColor();
	%pantsColor = getRandomBotPantsColor();
	%shoeColor = getRandomBotPantsColor();	
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

	%obj.llegColor =  %llegColor;
	%obj.secondPackColor =  %pack2Color;
	%obj.lhand =  "0";
	%obj.hip =  "0";
	%obj.faceName =  %face;
	%obj.rarmColor =  %skinColor;
	%obj.hatColor =  %hatcolor;
	%obj.hipColor =  %pantsColor;
	%obj.chest =  "0";
	%obj.rarm =  "0";
	%obj.packColor =  %packColor;
	%obj.pack =  "0";
	%obj.decalName =  %decal;
	%obj.larmColor =  %skinColor;
	%obj.secondPack =  "0";
	%obj.larm =  "0";
	%obj.chestColor =  %skinColor;
	%obj.accentColor =  %accentColor;
	%obj.rhandColor =  %skinColor;
	%obj.rleg =  "0";
	%obj.rlegColor =  %rlegColor;
	%obj.accent =  "1";
	%obj.headColor =  %skinColor;
	%obj.rhand =  "0";
	%obj.lleg =  "0";
	%obj.lhandColor =  %skinColor;
	%obj.hat =  "0";

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}