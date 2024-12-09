datablock ParticleData(BileStatusParticle)
{
	dragCoefficient      = 0.2;
	gravityCoefficient   = 1;
	inheritedVelFactor   = 0.6;
	constantAcceleration = 0.0;
	spinRandomMin 	     = -90;
	spinRandomMax 		= 90;
	lifetimeMS           = 350;
	lifetimeVarianceMS   = 150;
	textureName          = "base/data/particles/dot";
	colors[0]     = 33/255 SPC 158/255 SPC 11/255 SPC 0.3;
	colors[1]     = 33/255 SPC 158/255 SPC 11/255 SPC 0.3;
	colors[2]     = 33/255 SPC 158/255 SPC 11/255 SPC 0.3;
//	colors[3]     = "0.4 0.6 0.4 0.0";
	sizes[0]      = 0.08;
	sizes[1]      = 0.1;
	sizes[2]      = 0.16;
//	sizes[3]      = 0.0;
	times[0]	  = 0.0;
	times[1]	  = 0.3;
	times[2]	  = 1;
//	times[3]	  = 1.0;
	useInvAlpha = true;

};

datablock ParticleEmitterData(BileStatusEmitter)
{
   ejectionPeriodMS = 6;	//7
   periodVarianceMS = 5;
   ejectionVelocity = 0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.9;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "BileStatusParticle";

   uiName = "";
};

datablock ParticleData(BilePulseParticle)
{
	dragCoefficient      = 0.2;
	gravityCoefficient   = 0.5;
	inheritedVelFactor   = 0.6;
	constantAcceleration = 0.0;
	spinRandomMin 	     = -90;
	spinRandomMax 		= 90;
	lifetimeMS           = 500;
	lifetimeVarianceMS   = 250;
	textureName          = "base/data/particles/cloud";
	colors[0]     = 33/255 SPC 158/255 SPC 11/255 SPC 0.3;
	colors[1]     = 33/255 SPC 158/255 SPC 11/255 SPC 0.3;
	colors[2]     = 33/255 SPC 158/255 SPC 11/255 SPC 0.3;
//	colors[3]     = "0.4 0.6 0.4 0.0";
	sizes[0]      = 1.04;
	sizes[1]      = 1.08;
	sizes[2]      = 1.06;
//	sizes[3]      = 0.0;
	times[0]	  = 0.0;
	times[1]	  = 0.3;
	times[2]	  = 1;
//	times[3]	  = 1.0;
useInvAlpha = true;

};

datablock ParticleEmitterData(BilePulseEmitter)
{
   ejectionPeriodMS = 15;	//7
   periodVarianceMS = 10;
   ejectionVelocity = 0.25;
   velocityVariance = 0.0;
   ejectionOffset   = 1;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "BilePulseParticle";

   uiName = "";
};

datablock ShapeBaseImageData(BileStatusPlayerImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 2;
	offset = "0 0 -0.3";
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
	colorShiftColor = "1 1 1 1";

	stateName[0]                   = "Status";
	stateTimeoutValue[0]           = 0.5;
	stateEmitter[0]                = "BileStatusEmitter";
	stateEmitterTime[0]            = 1;
	stateTransitionOnTimeout[0]    = "Pulse";
	
	stateName[1]                   = "Pulse";
	stateEmitter[1]                = "BilePulseEmitter";
	stateEmitterTime[1]            = 1;
	stateTimeoutValue[1]           = 0.5;
	stateTransitionOnTimeout[1]    = "Status";
	stateScript[1]					= "onPulse";
};


datablock ParticleData(BoomerBoomParticle)
{
   dragCoefficient      = 0.1;
   gravityCoefficient   = 1;
   inheritedVelFactor   = 0.9;
   constantAcceleration = 0.0;
   spinRandomMin       = -90;
   spinRandomMax     = 90;
   lifetimeMS           = 600;
   lifetimeVarianceMS   = 500;
   textureName          = "base/data/particles/cloud";
   colors[0]     = "0.3 0.15 0.05 0.3";
   colors[1]     = "0.3 0.1 0.05 0.15";
   colors[2]     = "0.3 0.05 0.05 0";
// colors[3]     = "0.4 0.6 0.4 0.0";
   sizes[0]      = 2.5;
   sizes[1]      = 2.25;
   sizes[2]      = 2;
// sizes[3]      = 0.0;
   times[0]   = 0.0;
   times[1]   = 0.3;
   times[2]   = 1;
// times[3]   = 1.0;

	useInvAlpha = true;

};

datablock ParticleEmitterData(BoomerBoomEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 1;
   ejectionVelocity = 15;
   velocityVariance = 10;
   ejectionOffset   = 1;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "BoomerBoomParticle";

   uiName = "";
};

datablock ExplosionData(BoomerExplosion)
{
	soundProfile		= "boomer_explode_sound";

   explosionShape = "";
   explosion           = "BoomerExplosion";

   particleEmitter = BoomerBoomEmitter;
   particleDensity = 300;
   particleRadius = 0.25;

   lifeTimeMS = 500;

   subExplosion[0] = RBloodOrganExplosion;
   subExplosion[1] = RBloodBrainExplosion;

   lightStartRadius = 10;
   lightEndRadius = 20;
   lightStartColor = "0.2 1 0.4 1";
   lightEndColor = "0 0 0 1";

   damageRadius = 8;
   radiusDamage = 20;

   impulseRadius = 8;
   impulseForce = 2500;

   faceViewer     = true;
   explosionScale = "2 2 2";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "8.0 10.0 8.0";
   camShakeDuration = 1.5;
   camShakeRadius = 20.0;
};

datablock ProjectileData(BoomerProjectile)
{
	uiname							= "";
	lifetime						= 10;
	fadeDelay						= 10;
	explodeondeath						= true;

   directDamage        = 0;
   directDamageType = $DamageType::Boomer;
   radiusDamageType = $DamageType::Boomer;

   impactImpulse	   = 1;
   verticalImpulse	   = 1;

	explosion	= BoomerExplosion;
};

datablock ParticleData(bloodStreakParticle)
{
	dragCoefficient      = 1;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 400;
	lifetimeVarianceMS   = 100;
	textureName          = "base/data/particles/cloud";
	spinSpeed			= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]			= "0.3 0.1 0 0.08";
	colors[1]			= "0.3 0.1 0 0.2";
	colors[2]			= "0.3 0.1 0 0.01";
	colors[3]			= "0.3 0.1 0 0";
	sizes[0]			= 0.85;
	sizes[1]			= 0.95;
	sizes[2]			= 0.65;
	sizes[3]			= 0.0;

	useInvAlpha = true;
};

datablock ParticleEmitterData(bloodStreakEmitter)
{
	lifeTimeMS			= 3500;

	ejectionPeriodMS	= 12;
	periodVarianceMS	= 0;
	ejectionVelocity	= 0;
	velocityVariance	= 0.0;
	ejectionOffset		= 0.1;
	thetaMin			= 0;
	thetaMax			= 180;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= "bloodStreakParticle";
};

datablock ParticleData(bloodExplosionParticle)
{
	dragCoefficient      = 1;
	gravityCoefficient   = 0.4;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 5000;
	lifetimeVarianceMS   = 100;
	textureName          = "base/data/particles/cloud";
	spinSpeed			= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]				= "0.5 0.3 0 0.5";
	colors[1]				= "0.5 0.3 0 0";
	sizes[0]			= 5.25;
	sizes[1]			= 4.25;

	useInvAlpha = true;
};

datablock ParticleEmitterData(bloodExplosionEmitter)
{
	ejectionPeriodMS	= 1;
	periodVarianceMS	= 0;
	ejectionVelocity	= 4;
	velocityVariance	= 1.0;
	ejectionOffset  	= 0.0;
	thetaMin			= 89;
	thetaMax			= 90;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= "bloodExplosionParticle";
};

datablock ParticleData(bloodChunksParticle)
{
	dragCoefficient			= 0;
	gravityCoefficient		= 3;
	inheritedVelFactor		= 0.2;
	constantAcceleration	= 0.0;
	lifetimeMS				= 7500;
	lifetimeVarianceMS		= 300;
	textureName				= "base/data/particles/chunk";
	spinSpeed				= 190.0;
	spinRandomMin			= -290.0;
	spinRandomMax			= 290.0;
	colors[0]				= "0.3 0 0.0 1";
	colors[1]				= "0.3 0 0.0 0";
	sizes[0]				= 0.7;
	sizes[1]				= 0.6;

	useInvAlpha				= true;
};

datablock ParticleEmitterData(bloodChunksEmitter)
{
	lifeTimeMS			= 100;
	ejectionPeriodMS	= 1;
	periodVarianceMS	= 0;
	ejectionVelocity	= 17;
	velocityVariance	= 16.0;
	ejectionOffset		= 1.0;
	thetaMin			= 0;
	thetaMax			= 180;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= "bloodChunksParticle";
};

datablock ParticleData(bloodSprayParticle)
{
	dragCoefficient			= 2;
	gravityCoefficient		= 2;
	inheritedVelFactor		= 0.2;
	constantAcceleration	= 0.0;
	lifetimeMS				= 5840;
	lifetimeVarianceMS		= 200;
	textureName				= "base/data/particles/dot";
	spinSpeed				= 0.0;
	spinRandomMin			= 0.0;
	spinRandomMax			= 0.0;
	colors[0]				= "0.5 0 0.0 1";
	colors[1]				= "0.5 0 0.0 0.5";
	colors[2]				= "0.5 0 0.0 0";
	sizes[0]				= 0.2;
	sizes[1]				= 0.2;
	sizes[2]				= 0.2;
	useInvAlpha				= true;
};

datablock ParticleEmitterData(bloodSprayEmitter)
{
	lifeTimeMS			= 1000;
	ejectionPeriodMS	= 1;
	periodVarianceMS	= 0;
	ejectionVelocity	= 18;
	velocityVariance	= 7.0;
	ejectionOffset		= 1.0;
	thetaMin			= 0;
	thetaMax			= 180;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= "bloodSprayParticle";
};

//explosion
//////////////////////////////////////////

///////////////////////////
//Boomer Vomit Datablocks//
///////////////////////////
datablock ParticleData(BoomerVomitBallHitParticle)
{
	dragCoefficient      = 4;
	gravityCoefficient   = 0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	spinRandomMin 	     = -90;
	spinRandomMax 		= 90;
	lifetimeMS           = 2000;
	lifetimeVarianceMS   = 800;
	textureName          = "base/data/particles/cloud";
	colors[0]     = 33/255 SPC 158/255 SPC 11/255 SPC 0.7;
	colors[1]     = 33/255 SPC 158/255 SPC 11/255 SPC 0;
	sizes[0]      = 0.5;
	sizes[1]      = 0.1;
	times[0]	  = 0.0;
	times[1]	  = 1.0;

	useInvAlpha		= true;

};

datablock ParticleEmitterData(BoomerVomitBallHitEmitter)
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
   particles = "BoomerVomitBallHitParticle";

   uiName = "";
};

datablock ParticleData(BoomerVomitBallParticle)
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
	colors[0]     = 33/255 SPC 255/255 SPC 11/255 SPC 0.3;
	colors[1]     = 33/255 SPC 200/255 SPC 11/255 SPC 0.3;
	colors[2]     = 33/255 SPC 158/255 SPC 11/255 SPC 0;
	sizes[0]      = 0.8;
	sizes[1]      = 0.0;
//	sizes[2]      = 0.0;
	times[0]	  = 0.0;
	times[1]	  = 1;
//	times[2]	  = 1.0;

	useInvAlpha		= true;

};

datablock ParticleData(BoomerVomitBallTrailParticle)
{
	dragCoefficient      = 5;
	gravityCoefficient   = 0.05;
	inheritedVelFactor   = 0.0;
	constantAcceleration = 0.0;
	spinRandomMin 	     = 0;
	spinRandomMax 		= 0;
	lifetimeMS           = 700;
	lifetimeVarianceMS   = 200;
	textureName          = "base/data/particles/cloud";
	colors[0]     = 33/255 SPC 158/255 SPC 11/255 SPC 0.3;
	colors[1]     = 33/255 SPC 158/255 SPC 11/255 SPC 0.3;
	colors[2]     = 33/255 SPC 158/255 SPC 11/255 SPC 0;
	sizes[0]      = 0.9;
	sizes[1]      = 0.4;
	sizes[2]      = 0.0;
	times[0]	  = 0.0;
	times[1]	  = 0.2;
	times[2]	  = 1.0;

	useInvAlpha		= true;

};

datablock ParticleEmitterData(BoomerVomitBallTrailEmitter)
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
   particles = "BoomerVomitBallTrailParticle BoomerVomitBallParticle";

   uiName = "";
};


datablock ExplosionData(BoomerVomitBallExplosion)
{
   	explosionShape = "base/data/shapes/empty.dts";
   	lifeTimeMS = 500;
   	soundProfile = "spit_hit_sound";
   	particleEmitter = BoomerVomitBallHitEmitter;
   	particleDensity = 25;
   	particleRadius = 0.2;
   	faceViewer     = true;
   	explosionScale = "2.5 2.5 2.5";
	damageRadius = 0;
	radiusDamage = 0;
	impulseRadius = 0;
	impulseForce = 0;
};

datablock ProjectileData(BoomerVomitProjectile)
{
   	directDamage        = 0;
	radiusDamage		= 0;
   	explosion           = "BoomerVomitBallExplosion";
	particleEmitter = "BoomerVomitBallTrailEmitter";
	impactImpulse	   = 0;
	verticalImpulse	   = 0;
	muzzleVelocity      = 15;	//50
	velInheritFactor    = 0.5;
	armingDelay         = 0;
	lifetime            = 5000;	//1200
	fadeDelay           = 1000;
	bounceElasticity    = 0;
	bounceFriction      = 0;
	isBallistic         = true;
	gravityMod = 1;

	hasLight    = false;
   	uiName = "";
};

datablock fxDTSBrickData (BrickZombieBoomer_HoleSpawnData : BrickCommonZombie_HoleSpawnData)
{
	uiName = "Zombie Boomer Hole";
	iconName = "Add-Ons/Gamemode_Left4Block/modules/misc/models/icons/icon_boomer";
	holeBot = "ZombieBoomerHoleBot";
};

datablock TSShapeConstructor(BoomerMDts : ZombieMDts) 
{
	baseShape = "./models/mboomer.dts";
	sequence45 = "./models/animations/m_boomerwarn.dsq boomerwarn";
	sequence46 = "./models/animations/m_boomervomit.dsq boomervomit";
};

datablock PlayerData(ZombieBoomerHoleBot : CommonZombieHoleBot)
{
	shapeFile = BoomerMDts.baseShape;
	uiName = "Boomer Infected";
	jumpForce = 9*175;
	minImpactSpeed = 20;
	airControl = 0.01;
	speedDamageScale = 10;
	mass = 250;

	cameramaxdist = 2;
    cameraVerticalOffset = 1.1;
    cameraHorizontalOffset = 0.6;
    cameratilt = 0.1;
    maxfreelookangle = 2;

    maxForwardSpeed = 8;
    maxBackwardSpeed = 6;
    maxSideSpeed = 7;

 	maxForwardCrouchSpeed = 6;
    maxBackwardCrouchSpeed = 4;
    maxSideCrouchSpeed = 5;
	
	hIsInfected = 1;
	hZombieL4BType = "Special";
	hPinCI = "<bitmapk:Add-Ons/Gamemode_Left4Block/modules/misc/models/icons/ci_boomer2>";
	SpecialCPMessage = "Right click to vomit";
	hBigMeleeSound = "";
	hNeedsWeapons = 1;
	maxdamage = 100;
	hTickRate = 4000;
	hShoot = 1;
	hMaxShootRange = 2.5;//The range in which the bot will shoot the player
	hMoveSlowdown = 1;
	hName = "Boomer";//cannot contain spaces
	hAttackDamage = $Pref::L4B::Zombies::SpecialsDamage/3;

	rechargeRate = 0.5;
	maxenergy = 100;
	showEnergyBar = true;
};

function ZombieBoomerHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
	%obj.setscale("1.6 2 1.1");
}

function ZombieBoomerHoleBot::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

	function ZombieBoomerHoleBot::onBotLoop(%this,%obj)
{
	if(%obj.getstate() !$= "Dead" && !isEventPending(%obj.vomitschedule) && %obj.lastIdle+5000 < getsimtime())
	{
		switch$(%obj.hState)
		{
			case "Wandering":	%obj.isStrangling = false;
								%obj.hNoSeeIdleTeleport();
								%obj.playaudio(0,"boomer_lurk" @ getrandom(1,4) @ "_sound");
			case "Following": 	%obj.playaudio(0,"boomer_recognize" @ getrandom(1,4) @ "_sound");
		}

		%obj.playthread(3,plant);
		%obj.lastIdle = getsimtime();		
	}
}

function ZombieBoomerHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
}

function ZombieBoomerHoleBot::onBotFollow( %this, %obj, %targ )
{
	if((isObject(%obj) && %obj.getState() !$= "Dead" && %obj.hLoopActive) && (isObject(%targ) && %targ.getState() !$= "Dead") && (%distance = vectorDist(%obj.getposition(),%targ.getposition())) < 30)
	%this.schedule(750,onBotFollow,%obj,%targ);
	else return;
		
	if(%distance > 10 &&%distance < 20)
	{
		if(%obj.GetEnergyLevel() >= %this.maxenergy && !isEventPending(%obj.SpecialSched))
		{
			%this.onTrigger(%obj,4,1);
			%obj.setMoveX(0);
			%obj.setMoveY(0);
			%obj.setJumping(0);
			%obj.setCrouching(0);
			%obj.setaimobject(%targ);
		}
	}
	else if(%distance < 10)
	{		
		if(%obj.GetEnergyLevel() >= %this.maxenergy)
		{
			%obj.stopHoleLoop();
			%obj.hRunAwayFromPlayer(%targ);
			%obj.schedule(1500,startHoleLoop);
		}
		else
		{
			%this.onTrigger(%obj,0,true);
			%obj.setMoveX(0);
			%obj.setMoveY(1);
			%obj.setmoveobject(%targ);
		}
	}
}

function ZombieBoomerHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	%limb = %obj.rgetDamageLocation(%position);
	if(%damageType !$= $DamageType::FallDamage || %damageType !$= $DamageType::Impact)
	if(!%limb) %damage = %damage*2;
	else %damage = %damage/3;
	
	
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}

function ZombieBoomerHoleBot::onDamage(%this,%obj,%delta)
{	
	Parent::onDamage(%this,%obj,%delta);

	if(%delta > 5 && %obj.lastdamage+500 < getsimtime())
	{			
		if(%obj.getstate() !$= "Dead")
		{
			%obj.playaudio(0,"boomer_pain" @ getrandom(1,4) @ "_sound");
			if(%Obj.GetDamageLevel() > %obj.getDatablock().maxDamage/2) %obj.playaudio(3,"boomer_indigestion_loop_sound");
			%obj.playthread(2,"plant");
		}				 
		%obj.lastdamage = getsimtime();	
	}

	if(%obj.getState() $= "Dead") 
	{
		%obj.hideNode("ALL");
		%obj.schedule(50,delete);
		%this.doSplatterBlood(%obj,30);

		%datablock = "bloodHeadDebrisProjectile RBloodOrganProjectile 0 0 bloodHandDebrisProjectile bloodHandDebrisProjectile 0 bloodFootDebrisProjectile bloodFootDebrisProjectile";
		for(%i = 0; %i < getWordCount(%datablock); %i++) if(isObject(getWord(%datablock, %i)) && !%obj.limbDismemberedLevel[%i]) doGibLimbExplosion(getWord(%datablock, %i),%obj.getHackPosition(), getWord(%obj.getScale(), 2));
		for (%j = 0; %j < getRandom(10,15); %j++) doGibLimbExplosion("bloodDismemberProjectile",%obj.getHackPosition(), getWord(%obj.getScale(), 2));
	
		%b = new projectile()
		{
			datablock = BoomerProjectile;
			initialPosition = %obj.getPosition();
			sourceObject = %obj;
			scale = "1 1 1";
			damageType = $DamageType::Boomer;
		};
		%obj.unMountImage(0);
	}	
}

function ZombieBoomerHoleBot::RbloodDismember(%this,%obj,%limb,%doeffects,%position)
{
	return;	
}

function ZombieBoomerHoleBot::onTrigger (%this, %obj, %triggerNum, %val)
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
					%obj.playthread(2,"zAttack" @ getRandom(1,3));
					cancel(%obj.MeleeSched);
					%obj.MeleeSched = %this.schedule(350,Melee,%obj,%targ);
				}

		case 4: if(%obj.GetEnergyLevel() >= %this.maxenergy)
				{
					%obj.setenergylevel(0);
					%obj.stopaudio(0);
					%obj.playaudio(0,"boomer_warn_sound");
					%obj.playthread(1,"boomerwarn");
					if(%obj.getclassname() $= "AIPlayer") %obj.stopHoleLoop();
					%randomtime = getRandom(600,1000);
					%obj.SpecialSched = %this.schedule(%randomtime,Special,%obj,10);
				}
	}
}

function ZombieBoomerHoleBot::Melee(%this,%obj,%targ)
{
	CommonZombieHoleBot::Melee(%this,%obj,%targ);
}

function ZombieBoomerHoleBot::Special(%this,%obj,%limit,%count)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead") return;
	
	if(%count <= 10)
	{
		if(%count == 1)
		{
			%obj.playaudio(0,"boomer_vomit" @ getrandom(1,4) @ "_sound");
			%obj.playthread(1,"boomervomit");
		}

		%obj.playthread(2,"plant");
		%obj.SpecialSched = %this.schedule(100,Special,%obj,10,%count+1);
		%p = new Projectile()
		{
			dataBlock = "BoomerVomitProjectile";
			initialVelocity = VectorAdd(vectorScale(%obj.getEyeVector(),20),"0 0 2.5");
			initialPosition = vectorAdd(%obj.getMuzzlePoint(2),"0 0 0.45");
			sourceObject = %obj;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	else if(%obj.getclassname() $= "AIPlayer") %obj.schedule(1500,startHoleLoop);
}

function ZombieBoomerHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{
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

	%obj.llegColor =  %lLegColor;
	%obj.secondPackColor =  "0 0.435 0.831 1";
	%obj.lhand =  "0";
	%obj.hip =  "0";
	%obj.faceName =  %face;
	%obj.rarmColor =  %rarmColor;
	%obj.hatColor =  %hatColor;
	%obj.hipColor =  %pantsColor;
	%obj.chest =  "0";
	%obj.rarm =  "0";
	%obj.packColor =  "0.2 0 0.8 1";
	%obj.pack =  %pack;
	%obj.decalName =  %decal;
	%obj.larmColor =  %larmColor;
	%obj.secondPack =  "0";
	%obj.larm =  "0";
	%obj.chestColor =  %shirtColor;
	%obj.accentColor =  "0.990 0.960 0 0.700";
	%obj.rhandColor =  %skinColor;
	%obj.rleg =  "0";
	%obj.rlegColor =  %rLegColor;
	%obj.accent =  "1";
	%obj.headColor =  %skinColor;
	%obj.rhand =  "0";
	%obj.lleg =  "0";
	%obj.lhandColor =  %skinColor;
	%obj.hat =  0;

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

function ZombieBoomerHoleBot::L4BAppearance(%this,%obj,%client)
{
	%obj.unhideNode("ALL");

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

	%obj.setnodeColor("gloweyes","1 1 0 1");
	%obj.setFaceName("asciiTerror");
	%obj.setDecalName(%client.decalName);
	%obj.setNodeColor("headskin",%headColor);
	%obj.setNodeColor("boomerchest",%chestColor);
	%obj.setNodeColor("boomercheststomach",%headColor);
	%obj.setNodeColor("pants",%hipColor);
	%obj.setNodeColor("rarm",%rarmColor);
	%obj.setNodeColor("larm",%larmColor);
	%obj.setNodeColor("rhand",%rhandColor);
	%obj.setNodeColor("lhand",%lhandColor);
	%obj.setNodeColor("rshoe",%rlegColor);
	%obj.setNodeColor("lshoe",%llegColor);
	%obj.setNodeColor("pants",%hipColor);	
}

function BoomerProjectile::onExplode(%this,%obj)
{
	Parent::onExplode(%this,%obj);

    InitContainerRadiusSearch(%obj.getPosition(), 5, $TypeMasks::PlayerObjectType);
    while ((%targetid = containerSearchNext()) != 0)
    {
        if((%targetid.getType() & $TypeMasks::PlayerObjectType) && checkHoleBotTeams(%obj.sourceObject,%targetid) && miniGameCanDamage(%obj.sourceObject,%targetid))
        {
			%targetid.setWhiteout(2);
			if(%targetid.BoomerBiled) return Parent::onExplode(%this,%obj);
			else
			{
				if(!%targetid.BoomerBiled)
				{
					if(isObject(%targetid.client) && isObject(%minigame = getMiniGameFromObject(%targetid))) 
					{
						%minigame.L4B_ChatMessage("<color:FFFF00>" @ %obj.sourceObject.getDatablock().hName SPC %obj.sourceObject.getdataBlock().hPinCI SPC %targetid.client.name,"victim_needshelp_sound",true);
						if(%miniGame.DirectorStatus != 2 && %minigame.RoundType !$= "Horde") %minigame.schedule(1000,HordeRound);
					}
					%targetid.vomitbot = new Player() 
					{ 
						dataBlock = "EmptyPlayer";
						source = %targetid;
						slotToMountBot = 2;
						imageToMount = "BileStatusPlayerImage";
					};
					%targetid.BoomerBiled = true;
				}
			}
        }
    }
}

function BoomerVomitProjectile::onExplode(%this,%obj)
{
	Parent::onExplode(%this,%obj);

    InitContainerRadiusSearch(%obj.getPosition(), 1, $TypeMasks::PlayerObjectType);
    while ((%targetid = containerSearchNext()) != 0)
    {
        if((%targetid.getType() & $TypeMasks::PlayerObjectType) && checkHoleBotTeams(%obj.sourceObject,%targetid) && miniGameCanDamage(%obj.sourceObject,%targetid))
        {
			%targetid.setWhiteout(2);
			if(%targetid.BoomerBiled) return Parent::onExplode(%this,%obj);
			else
			{
				if(!%targetid.BoomerBiled)
				{
					if(isObject(%targetid.client) && isObject(%minigame = getMiniGameFromObject(%targetid))) 
					{
						%minigame.L4B_ChatMessage("<color:FFFF00>" @ %obj.sourceObject.getDatablock().hName SPC %obj.sourceObject.getdataBlock().hPinCI SPC %targetid.client.name,"victim_needshelp_sound",true);
						if(%miniGame.DirectorStatus != 2 && %minigame.RoundType !$= "Horde") %minigame.schedule(1000,HordeRound);
					}
					%targetid.vomitbot = new Player() 
					{ 
						dataBlock = "EmptyPlayer";
						source = %targetid;
						slotToMountBot = 2;
						imageToMount = "BileStatusPlayerImage";
					};
					%targetid.BoomerBiled = true;
				}
			}
        }
    }
}

function BileStatusPlayerImage::onPulse(%this,%obj,%slot)
{
	if(%obj.PulseCount <= 15) %obj.PulseCount++;
	else 
	{
		%obj.source.BoomerBiled = false;
		%obj.delete();	
	}
}