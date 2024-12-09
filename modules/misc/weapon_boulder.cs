AddDamageType("BoulderDirect",'<bitmap:Add-ons/Gamemode_Left4Block/modules/misc/icons/ci/CI_Boulder> %1',       '%2 <bitmap:Add-ons/Gamemode_Left4Block/modules/misc/icons/ci/CI_Boulder> %1',1,1);
AddDamageType("BoulderRadius",'<bitmap:Add-ons/Gamemode_Left4Block/modules/misc/icons/ci/CI_Boulder> %1', '%2 <bitmap:Add-ons/Gamemode_Left4Block/modules/misc/icons/ci/CI_Boulder> %1',1,0);

datablock DebrisData(boulder1debris)
{
   emitters = "";

	shapeFile = "./models/boulderpiece1.dts";
	lifetime = 10;
	spinSpeed			= 2000.0;
	minSpinSpeed = -100.0;
	maxSpinSpeed = 100.0;
	elasticity = 0.5;
	friction = 0.4;
	numBounces = 5;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 1;
};
datablock ExplosionData(boulder1debrisExplosion)
{
   particleEmitter = "";

   debris = boulder1debris;
   debrisNum = 2;
   debrisNumVariance = 6;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisThetaMin = 0;
   debrisThetaMax = 180;
   debrisVelocity = 3;
   debrisVelocityVariance = 1;
};

datablock DebrisData(boulder2debris)
{
   emitters = "";

	shapeFile = "./models/boulderpiece2.dts";
	lifetime = 10;
	spinSpeed			= 2000.0;
	minSpinSpeed = -100.0;
	maxSpinSpeed = 100.0;
	elasticity = 0.5;
	friction = 0.4;
	numBounces = 5;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 1;
};
datablock ExplosionData(boulder2debrisExplosion)
{
   particleEmitter = "";

   debris = boulder2debris;
   debrisNum = 2;
   debrisNumVariance = 6;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisThetaMin = 0;
   debrisThetaMax = 180;
   debrisVelocity = 3;
   debrisVelocityVariance = 1;
};

datablock DebrisData(boulder3debris)
{
   emitters = "";

	shapeFile = "./models/boulderpiece3.dts";
	lifetime = 10;
	spinSpeed			= 2000.0;
	minSpinSpeed = -100.0;
	maxSpinSpeed = 100.0;
	elasticity = 0.5;
	friction = 0.4;
	numBounces = 5;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 1;
};
datablock ExplosionData(boulder3debrisExplosion)
{
   particleEmitter = "";

   debris = boulder3debris;
   debrisNum = 2;
   debrisNumVariance = 6;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisThetaMin = 0;
   debrisThetaMax = 180;
   debrisVelocity = 3;
   debrisVelocityVariance = 1;
};

datablock DebrisData(boulder4debris)
{
   emitters = "";

	shapeFile = "./models/boulderpiece4.dts";
	lifetime = 10;
	spinSpeed			= 2000.0;
	minSpinSpeed = -100.0;
	maxSpinSpeed = 100.0;
	elasticity = 0.5;
	friction = 0.4;
	numBounces = 5;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 1;
};
datablock ExplosionData(boulder4debrisExplosion)
{
   particleEmitter = "";

   debris = boulder4debris;
   debrisNum = 2;
   debrisNumVariance = 6;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisThetaMin = 0;
   debrisThetaMax = 180;
   debrisVelocity = 3;
   debrisVelocityVariance = 1;
};

datablock ExplosionData(BoulderExplosion : spearExplosion)
{
   soundProfile = "boulder_hit_sound";

   damageRadius = 3;
   radiusDamage = 100;

   impulseRadius = 5;
   impulseForce = 1000;

   //subExplosion[0] = boulder1debrisExplosion;
   //subExplosion[1] = boulder2debrisExplosion;
   //subExplosion[2] = boulder3debrisExplosion;
   //subExplosion[3] = boulder4debrisExplosion;

   faceViewer     = true;
   explosionScale = "5 5 5";

   shakeCamera = true;
   camShakeFreq = "2 2 2";
   camShakeAmp = "1.25 1.25 1.25";
   camShakeDuration = 4;
   camShakeRadius = 1.25;
};

datablock ExplosionData(BoulderExplosion1 : spearExplosion)
{
   soundProfile = "";
   //impulse
   impulseRadius = 0;
   impulseForce = 0;

   //radius damage
   radiusDamage        = 0;
   damageRadius        = 0;
};

//spear trail
datablock ParticleData(boulderTrailParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 600;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= " ";

	// Interpolation variables
	colors[0]	= "0.5 0.5 0.5 0.1";
	colors[1]	= "0.25 0.25 0.25 0.05";
	colors[2]	= "0.05 0.05 0.05 0";
	sizes[0]	= 3;
	sizes[1]	= 1;
	sizes[2]	= 0.1;
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(boulderTrailEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;

   ejectionVelocity = 0.25; //0.25;
   velocityVariance = 0.10; //0.10;

   ejectionOffset = 0;

   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = boulderTrailParticle;

   useEmitterColors = true;
   uiName = "";
};

datablock ProjectileData(BoulderProjectile)
{
   projectileShapeName = "./models/BoulderProjectile.dts";
   directDamage        = 30;
   directDamageType  = $DamageType::BoulderDirect;
   radiusDamageType  = $DamageType::BoulderRadius;
   impactImpulse	   = 500;
   verticalImpulse	   = 150;
   explosion           = BoulderExplosion;
   particleEmitter     = boulderTrailEmitter;

   brickExplosionRadius = 0;
   brickExplosionImpact = true; //destroy a brick if we hit it directly?
   brickExplosionForce  = 0;
   brickExplosionMaxVolume = 0;
   brickExplosionMaxVolumeFloating = 0;

   muzzleVelocity      = 20;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 20000;
   fadeDelay           = 19500;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod = 1;

   hasLight    = false;
   uiName = "";
};

datablock ProjectileData(BoulderProjectile1 : BoulderProjectile)
{
   directDamage        = 0;
   impactImpulse	   = 0;
   verticalImpulse	   = 0;
   explosion           = BoulderExplosion1;

};

datablock ShapeBaseImageData(BoulderImage)
{
   shapeFile = "./models/Boulder.dts";
   emap = true;
   mountPoint = 0;
   offset = "-1.625 0 0";
   rotation				= eulerToMatrix( "90 0 90" );
   correctMuzzleVector = true;
   className = "WeaponImage";

   // Projectile && Ammo.
   item = BoulderItem;
   ammo = " ";
   projectile = "";
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = true;
   //raise your arm up or not
   armReady = true;

   isChargeWeapon = true;

   //casing = " ";
   doColorShift = true;
   colorShiftColor = "0.400 0.196 0 1.000";

	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 1.5;
	stateTransitionOnTimeout[0]	= "Ready";
	stateSequence[0]		= "ready";
   stateScript[0]			= "onActivate";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]			= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]	= true;

	stateName[2]			= "Fire";
	stateTransitionOnTimeout[2]	= "Ready";
	stateFire[2]			= true;
	stateSequence[2]		= "fire";
	stateScript[2]			= "onFire";
	stateWaitForTimeout[2]		= true;
   stateTimeoutValue[2]		= 5;
	stateAllowImageChange[2]	= false;
	stateSound[2]				= "";
};

function BoulderImage::onActivate(%this, %obj, %slot)
{
   %obj.playaudio(1, "tank_rock_grab_sound");
   %obj.playthread(1, spearReady);
   %obj.spawnExplosion(BoulderProjectile1,1); //boom
   %obj.setenergylevel(0);
}

function BoulderProjectile::onExplode(%this,%obj)
{
	Parent::onExplode(%this,%obj);
}

function BoulderImage::onFire(%this, %obj, %slot)
{
   %obj.schedule(75,TankThrowBoulder);
   %obj.playthread(1, spearThrow);
   %obj.playaudio(1, "tank_rock_toss" @ getRandom(1,3) @ "_sound");
   return;
}


function Player::TankThrowBoulder(%obj)
{
   %obj.playthread(2, "activate2");
   %obj.playthread(3, "activate2");
   %obj.playthread(0, "jump");

   if(isObject(%targ = %obj.hFollowing))
   %velocity = vectorscale(%obj.getEyeVector(),10+vectorDist(%obj.getHackPosition(),%targ.getHackPosition())*0.75);
   else %velocity = vectorscale(%obj.getEyeVector(),75);
   
   %p = new Projectile()
   {
      dataBlock = "BoulderProjectile";
      initialVelocity = vectorAdd(%velocity,"0 0 2.5");
      initialPosition = %obj.getHackPosition();
      sourceObject = %obj;
      client = %obj.client;
      scale = "4 4 4";
   };
   MissionCleanup.add(%p);
   %obj.unMountImage(0);
}