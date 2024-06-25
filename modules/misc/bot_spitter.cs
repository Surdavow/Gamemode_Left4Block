datablock ParticleData(SpitAcidBallHitParticle)
{
	dragCoefficient      = 4;
	gravityCoefficient   = 0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	spinRandomMin 	     = -90;
	spinRandomMax 		= 90;
	lifetimeMS           = 2000;
	lifetimeVarianceMS   = 800;
	textureName          = "base/data/particles/dot";
	colors[0]     = "0.530 0.825 0.591 0.7";
	colors[1]     = "0.697 0.770 0.380 0.0";
	sizes[0]      = 0.20;
	sizes[1]      = 0.18;
	times[0]	  = 0.0;
	times[1]	  = 1.0;
};

datablock ParticleEmitterData(SpitAcidBallHitEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 1;
   ejectionVelocity = 6;
   velocityVariance = 2.0;
   ejectionOffset   = 0.2;
   thetaMin         = 0;
   thetaMax         = 60;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "SpitAcidBallHitParticle";

   uiName = "";
};

datablock ParticleData(SpitAcidBallParticle)
{
	dragCoefficient      = 5;
	gravityCoefficient   = 0.05;
	inheritedVelFactor   = 0.0;
	constantAcceleration = 0.0;
	spinRandomMin 	     = 0;
	spinRandomMax 		= 0;
	lifetimeMS           = 300;
	lifetimeVarianceMS   = 200;
	textureName          = "base/data/particles/dot";
	colors[0]     = "0.6 0.790 0.4 0.1";
	colors[1]     = "0.5 0.770 0.300 0.0";
	sizes[0]      = 0.8;
	sizes[1]      = 0.0;
	times[0]	  = 0.0;
	times[1]	  = 1;

	useInvAlpha		= false;
};

datablock ParticleData(SpitAcidBallTrailParticle)
{
	dragCoefficient      = 5;
	gravityCoefficient   = 0.05;
	inheritedVelFactor   = 0.0;
	constantAcceleration = 0.0;
	spinRandomMin 	     = 0;
	spinRandomMax 		= 0;
	lifetimeMS           = 700;
	lifetimeVarianceMS   = 200;
	textureName          = "base/data/particles/dot";
	colors[0]     = "0.6 0.790 0.6 0.1";
	colors[1]     = "0.435 0.676 0.472 0.2";
	colors[2]     = "0.697 0.770 0.300 0.0";
	sizes[0]      = 0.5;
	sizes[1]      = 0.15;
	sizes[2]      = 0.0;
	times[0]	  = 0.0;
	times[1]	  = 0.2;
	times[2]	  = 1.0;

	useInvAlpha		= false;

};

datablock ParticleEmitterData(SpitAcidBallTrailEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 1;
   ejectionVelocity = 0;
   velocityVariance = 0;
   ejectionOffset   = 0.1;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 180;
   overrideAdvance = false;
   particles = "SpitAcidBallTrailParticle SpitAcidBallParticle";
   uiName = "";
};

datablock ParticleData(SpitAcidStatusParticle)
{
	dragCoefficient      = 0.2;
	gravityCoefficient   = 0.5;
	inheritedVelFactor   = 0.6;
	constantAcceleration = 0.0;
	spinRandomMin 	     = -90;
	spinRandomMax 		= 90;
	lifetimeMS           = 1300;
	lifetimeVarianceMS   = 400;
	textureName          = "base/data/particles/dot";
	colors[0]     = "0.6 0.7 0.3 0.3";
	colors[1]     = "0.75 0.8 0.4 0.4";
	colors[2]     = "0.3 0.79 0.2 0.0";
	sizes[0]      = 0.01;
	sizes[1]      = 0.25;
	sizes[2]      = 0.05;
	times[0]	  = 0.0;
	times[1]	  = 0.3;
	times[2]	  = 1;
};

datablock ParticleEmitterData(SpitAcidStatusEmitter)
{
   ejectionPeriodMS = 30;
   periodVarianceMS = 2;
   ejectionVelocity = 0;
   velocityVariance = 0.0;
   ejectionOffset   = 1.0;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "SpitAcidStatusParticle";
   uiName = "";
};

datablock ParticleData(SpitAcidPulseParticle)
{
	dragCoefficient      = 0.8;
	gravityCoefficient   = -1.0;
	inheritedVelFactor   = 1;
	constantAcceleration = 0.0;
	spinRandomMin 	     = -90;
	spinRandomMax 		= 90;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 300;
	textureName          = "base/data/particles/cloud";
	colors[0]     = "0.8 0.9 0.6 0.2";
	colors[1]     = "0.7 0.7 0.7 0.0";
	sizes[0]      = 2.0;
	sizes[1]      = 0.01;
	times[0]	  = 0.0;
	times[1]	  = 1.0;

};

datablock ParticleEmitterData(SpitAcidPulseEmitter)
{
   ejectionPeriodMS = 70;	//7
   periodVarianceMS = 5;
   ejectionVelocity = 0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.6;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "SpitAcidPulseParticle";
   uiName = "";
};

datablock ShapeBaseImageData(SpitAcidStatusPlayerImage)
{
   	shapeFile = "base/data/shapes/empty.dts";
   	emap = true;

   	mountPoint = 2;
   	offset = "0 -0.25 -0.75";
   	eyeOffset = 0;
   	rotation = "0 0 0";

   	correctMuzzleVector = true;

   	className = "WeaponImage";

   	item = "";
   	ammo = " ";
   	projectile = "";
   	projectileType = Projectile;

   	melee = false;
   	armReady = false;

   	doColorShift = false;

	stateName[0]                   = "Wait";
	stateTimeoutValue[0]           = 0.3;
	stateEmitter[0]                = SpitAcidStatusEmitter;
	stateEmitterTime[0]            = 1;
	stateTransitionOnTimeout[0]    = "Poison";

	stateName[1]                   = "Poison";
	stateEmitter[1]                = SpitAcidPulseEmitter;
	stateEmitterTime[1]            = 0.6;
	stateTimeoutValue[1]           = 0.1;
	stateTransitionOnTimeout[1]    = "Wait";
};

datablock ExplosionData(SpitAcidBallExplosion)
{
   explosionShape = "base/data/shapes/empty.dts";
   lifeTimeMS = 500;

   soundProfile = "spit_hit_sound";

   particleEmitter = SpitAcidBallHitEmitter;
   particleDensity = 25;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

	damageRadius = 10;	//4
	radiusDamage = 1;

	impulseRadius = 0.1;
	impulseForce = 1000;

};

	datablock ProjectileData(SpitterSpitProjectile)
{
   	directDamage        = 1;
   	directDamageType  = $DamageType::SpitAcidBall;
   	radiusDamageType  = $DamageType::SpitAcidBall;
   	explosion           = "SpitAcidBallExplosion";
	particleEmitter = "SpitAcidBallTrailEmitter";

	impactImpulse	   = 0;
	verticalImpulse	   = 0;

  	 muzzleVelocity      = 30;	//50
  	 velInheritFactor    = 0.5;

 	  armingDelay         = 0;
 	  lifetime            = 5000;	//1200
 	  fadeDelay           = 1000;
	   bounceElasticity    = 0;
	   bounceFriction      = 0;
	   isBallistic         = true;
	   gravityMod = 1;

 	  hasLight    = false;
  	 lightRadius = 3.0;
  	 lightColor  = "0 0 0.5";

   	uiName = "";
};

	datablock ProjectileData(SpitterSpewedProjectile)
{
   	directDamage        = 1;
   	directDamageType  = $DamageType::SpitAcidBall;
   	radiusDamageType  = $DamageType::SpitAcidBall;
   	explosion           = "SpitAcidBallExplosion";
	particleEmitter = "SpitAcidBallTrailEmitter";

	impactImpulse	   = 0;
	verticalImpulse	   = 0;

  	 muzzleVelocity      = 30;	//50
  	 velInheritFactor    = 0.5;

 	  armingDelay         = 0;
 	  lifetime            = 5000;	//1200
 	  fadeDelay           = 1000;
	   bounceElasticity    = 0.2;
	   bounceFriction      = 0.5;
	   isBallistic         = true;
	   gravityMod = 1;

 	  hasLight    = false;

   	uiName = "";
};

datablock fxDTSBrickData (BrickZombieSpitter_HoleSpawnData : BrickCommonZombie_HoleSpawnData)
{
	uiName = "Zombie Spitter Hole";
	iconName = "Add-Ons/Gamemode_Left4Block/modules/misc/icons/icon_Spitter";

	holeBot = "ZombieSpitterHoleBot";
};

datablock PlayerData(ZombieSpitterHoleBot : CommonZombieHoleBot)
{
	uiName = "Spitter Infected";
	minImpactSpeed = 16;
	speedDamageScale = 2;

	maxdamage = 100;//Health

    maxForwardSpeed = 8;
    maxBackwardSpeed = 7;
    maxSideSpeed = 6;

 	maxForwardCrouchSpeed = 6;
    maxBackwardCrouchSpeed = 5;
    maxSideCrouchSpeed = 4;

	cameramaxdist = 4;
    cameraVerticalOffset = 1;
    cameraHorizontalOffset = 0.6;
    cameratilt = 0.1;
    maxfreelookangle = 2;

	hName = "Spitter";//cannot contain spaces
	hTickRate = 4000;
	hShoot = 1;
	hMaxShootRange = 100;//The range in which the bot will shoot the player
	hTooCloseRange = 50;//in brick units
	hAttackDamage = $Pref::L4B::Zombies::SpecialsDamage;

	hIsInfected = 1;
	hZombieL4BType = "Special";
	hPinCI = "<bitmapk:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci_spitter2>";
	SpecialCPMessage = "Right click to spit";
	hBigMeleeSound = "";
	hNeedsWeapons = 1;

	rechargeRate = 0.75;
	maxenergy = 100;
	showEnergyBar = true;
};

function ZombieSpitterHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

function ZombieSpitterHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
	%obj.setscale("1 0.95 1.15");
}

function ZombieSpitterHoleBot::onBotLoop(%this,%obj)
{
	if(%obj.getstate() !$= "Dead" && %obj.lastIdle+5000 < getsimtime())
	{
		switch$(%obj.hState)
		{
			case "Wandering":	%obj.isStrangling = false;
								%obj.hNoSeeIdleTeleport();
								%obj.playaudio(0,"spitter_lurk" @ getrandom(1,3) @ "_sound");

			case "Following": 	%obj.playaudio(0,"spitter_recognize" @ getrandom(1,3) @ "_sound");
		}

		%obj.playthread(3,plant);
		%obj.lastIdle = getsimtime();		
	}	
}


function ZombieSpitterHoleBot::onBotFollow( %this, %obj, %targ )
{
	if(!isObject(%obj) || %obj.getState() $= "Dead" || %obj.hState !$= "Following" || !isObject(%targ)) return;

	cancel(%obj.hLastFollowSched);
	%obj.hLastFollowSched = %this.schedule(750,onBotFollow,%obj,%targ);

	if((%distance = vectorDist(%obj.getposition(),%targ.getposition())) > 10 && %distance < 25)
	{
		if(%obj.GetEnergyLevel() >= %this.maxenergy) 
		{
            %cansee = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%targ.getposition(),%obj.getposition()))) > 0.5;
            %obscure = containerRayCast(%obj.getEyePoint(),%targ.getMuzzlePoint(2),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);

            if(!isObject(%obscure) && %cansee) %this.onTrigger(%obj,4,1);
		}
		
		%obj.setMoveX(getRandom(-1,1));
		%obj.setMoveY(0);
		%obj.setaimobject(%targ);
	}
	else if(%distance < 12) %obj.hRunAwayFromPlayer(%targ);
	
}

	function ZombieSpitterHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
}

function ZombieSpitterHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	%limb = %obj.rgetDamageLocation(%position);
	if(%damageType !$= $DamageType::FallDamage || %damageType !$= $DamageType::Impact)
	if(%limb) %damage = %damage/3;
	else %damage = %damage*2.5;
	
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);

	if(%obj.lastDamaged < getSimTime())
	{
		for(%i = 0; %i < getRandom(1,5); %i++)
		{			
			doBloodExplosion(%position, getWord(%obj.getScale(), 2));
			%this.doSplatterBlood(%obj,5);
		}
		serverPlay3D("blood_impact" @ getRandom(1,4) @ "_sound", %position);
		%obj.lastDamaged = getSimTime()+50;
	}
}

function ZombieSpitterHoleBot::onDamage(%this,%obj)
{
	Parent::OnDamage(%this,%obj);

    if(%obj.getstate() !$= "Dead")
	{	
		if(%delta > 5 && %obj.lastdamage+1000 < getsimtime())
		{
			%obj.playaudio(0,"spitter_pain" @ getrandom(1,3) @ "_sound");			

			%obj.playthread(2,"plant");			

			if(%obj.raisearms)
			{
				%obj.raisearms = false;	
				%obj.playthread(1,"root");
			}
			%obj.lastdamage = getsimtime();
		}
	}
	else
	{
		for(%i=0;%i<25;%i++)
		{
			%rnd = getRandom();
			%dist = getRandom()*15;
			%x = mCos(%rnd*$PI*3)*%dist;
			%y = mSin(%rnd*$PI*3)*%dist;
			%p = new projectile()
			{
				datablock = SpitterSpewedProjectile;
				initialPosition = %obj.getPosition();
				initialVelocity = %x SPC %y SPC (getRandom()*4);
				client = %obj.client;
				sourceObject = %obj;
				damageType = $DamageType::SpitAcidBall;
			};
		}
		%obj.playaudio(0,"spitter_death" @ getrandom(1,2) @ "_sound");
	}
}

function ZombieSpitterHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{
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

	%obj.llegColor =  %lLegColor;
	%obj.secondPackColor =  "0 0.435 0.831 1";
	%obj.lhand =  "0";
	%obj.hip =  "0";
	%obj.rarmColor =  %skinColor;
	%obj.hatColor =  "1 1 1 1";
	%obj.hipColor =  %pantsColor;
	%obj.chest =  "1";
	%obj.decalName =  "witch";
	%obj.rarm =  "0";
	%obj.packColor =  "0.2 0 0.8 1";
	%obj.pack =  "0";
	%obj.larmColor =  %skinColor;
	%obj.secondPack =  "0";
	%obj.larm =  "0";
	%obj.chestColor =  %skinColor;
	%obj.accentColor =  "0.990 0.960 0 0.700";
	%obj.rhandColor =  %skinColor;
	%obj.rleg =  "0";
	%obj.rlegColor =  %rLegColor;
	%obj.accent =  "1";
	%obj.headColor =  %skinColor;
	%obj.faceName = %face;
	%obj.rhand =  "0";
	%obj.lleg =  "0";
	%obj.lhandColor =  %skinColor;
	%obj.hat =  0;

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

function ZombieSpitterHoleBot::Spit(%this,%obj,%limit,%count)
{
	if(isObject(%obj) && %obj.getState() !$= "Dead" && %count <= 3)
	{
		%obj.setenergylevel(0);
		%obj.playaudio(2,"spitter_spit_sound");
		%obj.playthread(0,"plant");
		%obj.spitschedule = %this.schedule(100,Spit,%obj,%limit,%count+1);

		%pm = new projectile()
		{
			dataBlock = "SpitterSpitProjectile";
			initialVelocity = vectorAdd(vectorscale(%obj.getEyeVector(),50),"0 0 2.5");
			initialPosition = vectorAdd(%obj.getMuzzlePoint(2),"0 0 0.45");
			sourceObject = %obj;
			client = %obj.client;
		};
		MissionCleanup.add(%pm);
	}
}


function ZombieSpitterHoleBot::onTrigger (%this, %obj, %triggerNum, %val)
{	
	Parent::onTrigger(%this, %obj, %triggerNum, %val);

	CommonZombieHoleBot::onTrigger (%this, %obj, %triggerNum, %val);
	if(%obj.getstate() !$= "Dead") switch(%triggerNum)
	{
		case 4: if(%val && %obj.GetEnergyLevel() >= %this.maxenergy) %this.Spit(%obj,3);
		default:
	}	
}	

function SpitterSpitProjectile::onExplode(%obj,%this)
{
	for(%i=0;%i<4;%i++)
	{
		%rnd = getRandom();
		%dist = getRandom()*15;
		%x = mCos(%rnd*$PI*3)*%dist;
		%y = mSin(%rnd*$PI*3)*%dist;
		%p = new projectile()
		{
			datablock = SpitterSpewedProjectile;
			initialPosition = %this.getPosition();
			initialVelocity = %x SPC %y SPC (getRandom()*4);
			client = %this.sourceObject.client;
			sourceObject = %this.sourceObject;
			damageType = $DamageType::SpitAcidBall;
		};
	}
	Parent::onExplode(%obj,%this);
}

function SpitterSpitProjectile::damage(%this,%obj,%col,%fade,%pos,%normal)
{
   	%damageType = $DamageType::SpitAcidBall;
   	%scale = getWord(%obj.getScale(), 2);
   	%directDamage = mClampF(%this.directDamage, -100, 100) * %scale;
	
	if((%col.getType() & $TypeMasks::PlayerObjectType) && checkHoleBotTeams(%obj.sourceObject,%col)) %col.damage(%obj, %pos, %directDamage, %damageType);
}

function SpitterSpewedProjectile::damage(%this,%obj,%col,%fade,%pos,%normal)
{	
   	%damageType = $DamageType::SpitAcidBall;	
   	%scale = getWord(%obj.getScale(), 2);
   	%directDamage = mClampF(%this.directDamage, -100, 100) * %scale;

   	if(%col.getType() & $TypeMasks::PlayerObjectType)
	if(checkHoleBotTeams(%obj.sourceObject,%col))
	%col.damage(%obj, %pos, %directDamage, %damageType);
}

//Don't package this since we have our own damage system, which I got from Badspot's modification topic
function SpitterSpewedProjectile::radiusDamage(%this, %obj, %col, %distanceFactor, %pos, %damageAmt)
{
   	if(%distanceFactor <= 0) return;
   	else if(%distanceFactor > 1) %distanceFactor = 1;
   	%damageAmt *= %distanceFactor;   
	if(%damageAmt && (%col.getType() & $TypeMasks::PlayerObjectType) && checkHoleBotTeams(%obj.sourceObject,%col)) %col.damage(%obj, %pos, %directDamage, $DamageType::SpitAcidBall);
   
}