datablock DebrisData(FlashgrenadePinDebris)
{
	shapeFile = "./models/flashgrenadepin.dts";
	lifetime = 5.0;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 200.0;
	elasticity = 0.5;
	friction = 0.2;
	numBounces = 3;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 2;
};

datablock ParticleData(FlashgrenadeExplosionParticle)
{
	dragCoefficient		= 1.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= -0.2;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 4000;
	lifetimeVarianceMS	= 3990;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	animateTexture		= false;

	textureName		= "base/data/particles/cloud";
	colors[0]	= "0.2 0.2 0.2 1.0";
	colors[1]	= "0.25 0.25 0.25 0.2";
   	colors[2]	= "0.4 0.4 0.4 0.0";

	sizes[0]	= 2.0;
	sizes[1]	= 10.0;
	sizes[2]	= 13.0;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(flashgrenadeExplosionEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;
   lifeTimeMS	   = 21;
   ejectionVelocity = 10;
   velocityVariance = 1.0;
   ejectionOffset   = 1.0;
   thetaMin         = 0;
   thetaMax         = 0;
   phiReferenceVel  = 0;
   phiVariance      = 90;
   overrideAdvance = false;
   particles = "flashgrenadeExplosionParticle";
};

datablock ParticleData(flashgrenadeExplosionParticle2)
{
	dragCoefficient		= 1.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= -0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 4000;
	lifetimeVarianceMS	= 3990;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/cloud";

	// Interpolation variables
	colors[0]	= "0.2 0.2 0.2 0.0";
	colors[1]	= "0.25 0.25 0.25 0.2";
   colors[2]	= "0.4 0.4 0.4 0.0";

	sizes[0]	= 2.0;
	sizes[1]	= 10.0;
   sizes[2]	= 3.0;

	times[0]	= 0.0;
	times[1]	= 0.1;
   times[2]	= 1.0;
};

datablock ParticleEmitterData(flashgrenadeExplosionEmitter2)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 0;
   lifetimeMS       = 120;
   ejectionVelocity = 15;
   velocityVariance = 5.0;
   ejectionOffset   = 0.0;
   thetaMin         = 85;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "flashgrenadeExplosionParticle2";
};



datablock ParticleData(flashgrenadeExplosionParticle3)
{
	dragCoefficient		= 1.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= -0.2;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 4000;
	lifetimeVarianceMS	= 3990;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/cloud";

	// Interpolation variables
	colors[0]	= "0.2 0.2 0.2 0.0";
	colors[1]	= "0.25 0.25 0.25 0.2";
   colors[2]	= "0.4 0.4 0.4 0.0";

	sizes[0]	= 2.0;
	sizes[1]	= 10.0;
   sizes[2]	= 13.0;

	times[0]	= 0.0;
	times[1]	= 0.1;
   times[2]	= 1.0;
};

datablock ParticleEmitterData(flashgrenadeExplosionEmitter3)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   lifetimeMS       = 150;
   ejectionVelocity = 20;
   velocityVariance = 5.0;
   ejectionOffset   = 0.0;
   thetaMin         = 85;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "flashgrenadeExplosionParticle3";
};

datablock ParticleData(flashgrenadeExplosionParticle4)
{
	dragCoefficient		= 0.1;
	windCoefficient		= 0.0;
	gravityCoefficient	= 4.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1000;
	lifetimeVarianceMS	= 500;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/chunk";
	//animTexName		= "~/data/particles/cloud";

	// Interpolation variables
	colors[0]	= "0.1 0.1 0.1 1.0";
	colors[1]	= "0.2 0.2 0.2 0.0";
	sizes[0]	= 0.5;
	sizes[1]	= 0.13;
	times[0]	= 0.0;
	times[1]	= 1.0;
};

datablock ParticleEmitterData(flashgrenadeExplosionEmitter4)
{
   ejectionPeriodMS = 1;
   timeMultiple     = 10;
   periodVarianceMS = 0;
   lifetimeMS       = 15;
   ejectionVelocity = 35;
   velocityVariance = 5.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "flashgrenadeExplosionParticle4";
};

datablock ExplosionData(flashgrenadeExplosion)
{
   explosionShape = "Add-Ons/Weapon_Rocket_Launcher/explosionSphere1.dts";
   lifeTimeMS = 150;

   soundProfile = "flashgrenade_fire_sound";
   
   emitter[0] = flashgrenadeExplosionEmitter3;
   emitter[1] = flashgrenadeExplosionEmitter2;
   emitter[2] = flashgrenadeExplosionEmitter4;

   particleEmitter = flashgrenadeExplosionEmitter;
   particleDensity = 10;
   particleRadius = 1.0;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "30.0 30.0 30.0";
   camShakeAmp = "7 7 7";
   camShakeDuration = 4;
   camShakeRadius = 20.0;

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 0;
   lightStartColor = "0.45 0.3 0.1";
   lightEndColor = "0 0 0";

   //impulse
   impulseRadius = 1;
   impulseForce = 0;

   //radius damage
   damageRadius = 15;
   radiusDamage = 0;

};

//projectile
AddDamageType("flashgrenadeDirect",   '<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_flashgrenade> %1',    '%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_flashgrenade> %1',1,1);
AddDamageType("flashgrenadeRadius",   '<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_flashgrenade> %1',    '%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_flashgrenade> %1',1,0);
datablock ProjectileData(flashgrenadeProjectile)
{
	projectileShapeName = "./models/flashgrenadeProjectile.dts";
	directDamage        = 0;
	directDamageType  = $DamageType::flashgrenadeDirect;
	radiusDamage = 0;
	radiusDamageType  = $DamageType::flashgrenadeRadius;
	impactImpulse	   = 200;
	verticalImpulse	   = 200;
	explosion           = flashgrenadeExplosion;	
	brickExplosionImpact = false; //destroy a brick if we hit it directly?

	muzzleVelocity      = 30;
	velInheritFactor    = 0;
	explodeOnDeath = true;

	armingDelay         = 3500; 
	lifetime            = 4000;
	fadeDelay           = 3500;
	bounceElasticity    = 0.25;
	bounceFriction      = 0.3;
	isBallistic         = true;
	gravityMod = 1.0;
	hasLight    = false;	

	isDistraction = true;
	distractionLifetime = 3;
	distractionDelay = 0;
	DistractionFunction = "BileBombDistract";
	DistractionRadius = 50;

	uiName = "Flash Grenade";
};

datablock ItemData(flashgrenadeItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	shapeFile = "./models/flashgrenade.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Flash Grenade";
	iconName = "./icons/icon_flashgrenade";
	doColorShift = false;

	image = flashgrenadeImage;
	canDrop = true;
};

datablock ShapeBaseImageData(flashgrenadeImage)
{
	shapeFile = flashgrenadeItem.shapeFile;
	emap = true;
	mountPoint = 0;
	offset = "0 0 0";	
	correctMuzzleVector = true;
	className = "WeaponImage";

	item = flashgrenadeItem;
	ammo = " ";
	projectile = flashgrenadeProjectile;
	projectileType = Projectile;

	casing = flashgrenadePinDebris;
	shellExitDir        = "-2.0 1.0 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 15.0;	
	shellVelocity       = 7.0;


	melee = false;   
	armReady = true;
	doColorShift = false;	

	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.1;
	stateTransitionOnTimeout[0]	= "Ready";
	stateSequence[0]		= "ready";
	stateSound[0]					= "weaponSwitchSound";

	stateName[1]			= "Ready";
	stateTransitionOnTriggerDown[1]	= "Pindrop";
	stateAllowImageChange[1]	= true;

	stateName[2]			= "Pindrop";
	stateTransitionOnTimeout[2]	= "Pinfallen";
	stateAllowImageChange[2]	= false;
	stateTimeoutValue[2]		= 0.2;
	stateSound[2]				= "flashgrenade_pin_sound";
	stateSequence[2]                = "Pinhide";
	stateEjectShell[2]       = true;

	stateName[3]			= "Pinfallen";
	stateTransitionOnTriggerDown[3]	= "Charge";
	stateAllowImageChange[3]	= false;

	stateName[4]                    = "Charge";
	stateTransitionOnTimeout[4]	= "Armed";
	stateTimeoutValue[4]            = 0.7;
	stateWaitForTimeout[4]		= false;
	stateTransitionOnTriggerUp[4]	= "AbortCharge";
	stateScript[4]                  = "onCharge";
	stateAllowImageChange[4]        = false;

	stateName[5]			= "AbortCharge";
	stateTransitionOnTimeout[5]	= "Pinfallen";
	stateTimeoutValue[5]		= 0.3;
	stateWaitForTimeout[5]		= true;
	stateScript[5]			= "onAbortCharge";
	stateAllowImageChange[5]	= false;

	stateName[6]			= "Armed";
	stateTransitionOnTriggerUp[6]	= "Fire";
	stateAllowImageChange[6]	= false;

	stateName[7]			= "Fire";
	stateTransitionOnTimeout[7]	= "Done";
	stateTimeoutValue[7]		= 0.5;
	stateFire[7]			= true;
	stateSequence[7]		= "fire";
	stateScript[7]			= "onFire";
	stateWaitForTimeout[7]		= true;
	stateAllowImageChange[7]	= false;

	stateName[8]					= "Done";
	stateScript[8]					= "onDone";
};

function flashgrenadeImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function flashgrenadeImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2,"");
}

function flashgrenadeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	serverPlay3D("flashgrenade_bounce_sound",%obj.getPosition());
}

function flashgrenadeImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2,spearThrow);
	Parent::OnFire(%this, %obj, %slot);

	%obj.tool[%obj.currTool] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%obj.currTool,0);
	serverCmdUnUseTool(%obj.client);
}

function flashgrenadeImage::onDone(%this,%obj,%slot)
{
	%obj.unMountImage(%slot);
}

function flashGrenadeProjectile::onExplode(%this,%obj)
{
   parent::onExplode(%this, %obj);

	initContainerRadiusSearch(%obj.getWorldBoxCenter(),50,$TypeMasks::PlayerObjectType);
	while((%target = ContainerSearchNext()) != 0)
	{
		%obscure = containerRayCast(%obj.getWorldBoxCenter(),%target.getEyePoint(),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);
		%dot = vectorDot(%target.getEyeVector(),vectorNormalize(vectorSub(%obj.getWorldBoxCenter(),%target.getEyePoint())));
		%distance = containerSearchCurrDist()/2.5;

		if(%dot > 0.1 && !isObject(%obscure) && miniGameCanDamage(%obj,%target) == 1 && %target.getState() !$= "Dead")
		{			
			switch$(%target.getClassName())
			{
				case "Player":	%target.setWhiteout(%dot/%distance);
				case "AIPlayer": 	%target.mountImage("sm_stunImage",2);
								 	%target.stopHoleLoop();
								 	%target.schedule(4000,resetHoleLoop);
								 	%target.setActionThread("sit",1);
			}
		}
		
	}
}