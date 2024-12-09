AddDamageType("Molotov",'<bitmap:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci/ci_molotov> %1','%2 <bitmap:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci/ci_molotov> %1',1,1);
AddDamageType("Flamer",'<bitmap:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci/ci_fire> %1','%2 <bitmap:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci/ci_fire> %1',1,1);

datablock ParticleData(flamerFleshBurningParticle)
{
	dragCoefficient      = 1;
	gravityCoefficient   = -3;
	inheritedVelFactor   = 0.7;
	constantAcceleration = 0.0;
	lifetimeMS           = 425;
	lifetimeVarianceMS   = 355;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 16.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.7 0.6 0.3 0.5";
	colors[1]     = "0.9 0.4 0.1 0.8";
	colors[2]     = "0.9 0.2 0.1 0.2";
	colors[3]     = "0.9 0.1 0.1 0";
	sizes[0]      = 0.75;
	sizes[1]      = 0.97;
	sizes[2]      = 1.45;
	sizes[3]      = 1.85;
   times[0] = 0.0;
   times[1] = 0.3;
   times[2] = 0.6;
   times[3] = 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(flamerFleshBurningEmitter)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 0;
	ejectionVelocity = 1;
	velocityVariance = 0.4;
	ejectionOffset   = 0.4;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	orientOnVelocity = true;
	particles = flamerFleshBurningParticle;
};

datablock ParticleData(flamerRubbishBurningParticle)
{
	dragCoefficient      = 1;
	gravityCoefficient   = -2;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 525;
	lifetimeVarianceMS   = 455;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 16.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.7 0.6 0.3 0.5";
	colors[1]     = "0.9 0.4 0.1 0.8";
	colors[2]     = "0.9 0.2 0.1 0.2";
	colors[3]     = "0.9 0.1 0.1 0";
	sizes[0]      = 0.67;
	sizes[1]      = 1.1;
	sizes[2]      = 1.2;
	sizes[3]      = 1.5;
	times[0] = 0.0;
	times[1] = 0.3;
	times[2] = 0.6;
	times[3] = 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(flamerRubbishBurningEmitter)
{
   ejectionPeriodMS = 12;
   periodVarianceMS = 0;
   ejectionVelocity = 5;
   velocityVariance = 1;
   ejectionOffset   = 0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   orientOnVelocity = true;
   particles = flamerRubbishBurningParticle;
};

datablock projectileData(flamerRubbishProjectile)
{
	projectileShapeName 	= "base/data/shapes/empty.dts";
	particleEmitter     	= flamerRubbishBurningEmitter;
	isBallistic 			= 1;
	gravityMod 				= 1;
	lifeTime 				= 12000;
	explodeOnDeath 			= 1;
	explosion 				= "";
	muzzleVelocity = 5;
};

datablock ShapeBaseImageData(flamerFleshBurningImage)
{
	emap = false;
	mountPoint = $backSlot;
	eyeOffset = "0 0 -0.4";
	doColorShift = true;
	shapeFile = "base/data/shapes/empty.dts";
	offset = "0 0 -0.8";
	rotation = "1 0 0 90";
	
	stateName[0]					= "FireA";
	stateEmitter[0]					= flamerFleshBurningEmitter;
	stateEmitterTime[0]				= 100;
	stateTransitionOnTimeout[0]     = "FireB";
	stateTimeoutValue[0]            = 100;
	
	stateName[1]					= "FireB";
	stateEmitter[1]					= flamerFleshBurningEmitter;
	stateEmitterTime[1]				= 100;
	stateTransitionOnTimeout[1]     = "FireA";
	stateTimeoutValue[1]            = 100;
};

datablock ParticleData(molotovExplosionSmokeParticle)
{
	dragCoefficient      = 5;
	gravityCoefficient   = -0.1;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.0;
	lifetimeMS           = 4700;
	lifetimeVarianceMS   = 900;
	textureName          = "base/data/particles/cloud";
	spinSpeed			= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]			= "0.1 0.1 0.1 0.2";
	colors[1]			= "0.2 0.2 0.2 0.1";
	colors[2]			= "0.4 0.4 0.4 0";
	sizes[0]			= 2;
	sizes[1]			= 5;
	sizes[2]			= 7;
	times[0]			= 0;
	times[1]			= 0.1;
	times[2]			= 1;
	useInvAlpha 		= true;
};

datablock ParticleEmitterData(molotovExplosionSmokeEmitter)
{
	ejectionPeriodMS	= 3;
	periodVarianceMS	= 0;
	ejectionVelocity	= 4;
	velocityVariance	= 0;
	ejectionOffset  	= 0.1;
	thetaMin			= 0;
	thetaMax			= 90;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	particles			= molotovExplosionSmokeParticle;
};
datablock explosionData(molotovExplosion)
{
	lifetimeMS 				= 150;
	
	particleEmitter 		= molotovExplosionSmokeEmitter;
	particleDensity 		= 12;
	particleRadius 			= 0.5;
	soundProfile 			= "";

	
	emitter[0]				= "";
	
	debris 					= "";
	debrisNum 				= 0;
	debrisNumVariance 		= 0;

	radiusDamage = 0;
	damageRadius = 0;
	
	impulseForce = 0;
	impulseRadius = 0;
	
	faceViewer = 1;
	
	explosionScale = "1 1 1";
	
	shakeCamera = true;
	camShakeRadius = 20;
	camShakeAmp = "1 2 1";
	camShakeDuration = 3;
	camShakeFallOff = 10;
	camShakeFreq = "2 4 2";
	cameraShakeFalloff = 1;
};
datablock ParticleData(molotovSparkParticle)
{
	dragCoefficient      = 1;
	gravityCoefficient   = -1;
	inheritedVelFactor   = 0.4;
	constantAcceleration = 0.0;
	lifetimeMS           = 425;
	lifetimeVarianceMS   = 255;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 16.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.7 0.6 0.3 0.8";
	colors[1]     = "0.9 0.4 0.1 0.5";
	colors[2]     = "0.9 0.2 0.1 0.2";
	colors[3]     = "0.9 0.1 0.1 0";
	sizes[0]      = 0.18;
	sizes[1]      = 0.22;
	sizes[2]      = 0.29;
	sizes[3]      = 0.17;
   times[0] = 0.0;
   times[1] = 0.3;
   times[2] = 0.6;
   times[3] = 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(molotovSparkEmitter)
{
   ejectionPeriodMS = 8;
   periodVarianceMS = 0;
   ejectionVelocity = 0.1;
   velocityVariance = 0;
   ejectionOffset   = 0.05;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   orientOnVelocity = true;
   particles = molotovSparkParticle;
};
datablock ProjectileData(molotovProjectile : gunProjectile)
{
	projectileShapeName 	= "./models/molotov_proj.dts";
	directDamage        	= 5;
	explodeOnDeath = 1;
	explosion 				= molotovExplosion;
	directDamageType    	= $DamageType::Flamer;
	radiusDamageType    	= $DamageType::Flamer;
	particleEmitter     	= molotovSparkEmitter;
	uiName 					= "";
	muzzleVelocity 			= 35;
	verticalImpulse 		= 20;
	impactImpulse			= 20;
	lifeTime				= 20000;
	sound 					= "";
	sProjectile 			= 0;
	noBulletWhiz = 1;
	gravityMod 				= 1;
	isBallistic 			= 1;
	
	sound = "molotov_loop_sound";
	
	impactImpulse = 0;
	verticalImpulse = 0;
};
datablock ItemData(molotovItem)
{
	category = "Weapon";
	className = "Weapon";
	
	weaponClass = "slot3";

	shapeFile = "./models/molotov.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Molotov";
	iconName = "./icons/icon_molotov";
	fakeIcon = c4Icon;
	doColorShift = true;
	colorShiftColor = "0.4 0.2 0 1";
	ammo = 1;
	canPutInSuitcase = 0;
	isLargeWeapon = 0;

	image = molotovImage;
	canDrop = true;
};
datablock ShapeBaseImageData(molotovImage)
{
	className = "WeaponImage";
	shapeFile = "./models/molotov.dts";
	emap = true;
	mountPoint = 0;
	offset = "-0.01 0.035 0";
	eyeOffset = "0 0 0";
	rotation = "0 0 1 20";
	eyeRotation = "0 0 0 0";
	item = molotovItem;
	ammo = " ";
	
	armReady = true;
	
	projectile = molotovProjectile;
	projectileType = Projectile;
	
	
	doColorShift = true;
	colorShiftColor = molotovItem.colorShiftColor;

	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.3;
	stateTransitionOnTimeout[0]	= "FlickA";
	stateSequence[0]		= "ready";
	stateSound[0]			= weaponSwitchSound;
	
	stateName[1]			= "FlickA";
	stateTimeoutValue[1]		= 0.1;
	stateTransitionOnTimeout[1]	= "FlickB";
	stateSequence[1]		= "ready";
	stateScript[1]			= "onFlick";
	stateEmitter[1]                 = molotovSparkEmitter;
	stateEmitterNode[1]             = "mountPoint";
	stateEmitterTime[1]             = "2";
	stateSound[1]			= "molotov_light_sound";
	
	stateName[7]			= "FlickB";
	stateTimeoutValue[7]		= 0.1;
	stateTransitionOnTimeout[7]	= "Ready";
	stateSequence[7]		= "ready";
	stateEmitter[7]                 = molotovSparkEmitter;
	stateEmitterNode[7]             = "mountPoint";
	stateEmitterTime[7]             = "2";
	stateSound[7]			= "molotov_loop_sound";

	stateName[2]			= "Ready";
	stateTransitionOnTriggerDown[2]	= "Charge";
	stateTransitionOnTimeout[2]	= "Ready";
	stateTimeoutValue[2]		= 0.15;
	stateWaitForTimeout[2]		= 0;
	stateAllowImageChange[2]	= true;
	stateEmitter[2]                 = molotovSparkEmitter;
	stateEmitterNode[2]             = "mountPoint";
	stateEmitterTime[2]             = "2";
	stateSound[2]			= "molotov_loop_sound";
	
	stateName[3]                    = "Charge";
	stateTransitionOnTimeout[3]	= "Armed";
	stateTimeoutValue[3]            = 0.25;
	stateWaitForTimeout[3]		= false;
	stateTransitionOnTriggerUp[3]	= "AbortCharge";
	stateScript[3]                  = "onCharge";
	stateAllowImageChange[3]        = false;
	stateEmitter[3]                 = molotovSparkEmitter;
	stateEmitterNode[3]             = "mountPoint";
	stateEmitterTime[3]             = "0.6";
	stateSound[3]			= "molotov_loop_sound";
	
	stateName[4]			= "AbortCharge";
	stateTransitionOnTimeout[4]	= "Ready";
	stateTimeoutValue[4]		= 0.1;
	stateWaitForTimeout[4]		= true;
	stateScript[4]			= "onAbortCharge";
	stateAllowImageChange[4]	= false;
	stateEmitter[4]                 = molotovSparkEmitter;
	stateEmitterNode[4]             = "mountPoint";
	stateEmitterTime[4]             = "0.4";
	stateSound[4]			= "molotov_loop_sound";

	stateName[5]			= "Armed";
	stateWaitFotTimeout[5]		= 0;
	stateTransitionOnTimeout[5]	= "Armed";
	stateTimeoutValue[5]		= 0.1;
	stateTransitionOnTriggerUp[5]	= "Fire";
	stateAllowImageChange[5]	= false;
	stateEmitter[5]                 = molotovSparkEmitter;
	stateEmitterNode[5]             = "mountPoint";
	stateEmitterTime[5]             = "0.2";
	stateSound[5]			= "molotov_loop_sound";

	stateName[6]			= "Fire";
	stateTransitionOnTimeout[6]	= "FlickA";
	stateTimeoutValue[6]		= 1.5;
	stateFire[6]			= true;
	stateSequence[6]		= "ready";
	stateScript[6]			= "onFire";
	stateWaitForTimeout[6]		= true;
	stateAllowImageChange[6]	= false;
	stateSound[6]			= "";
};

datablock ParticleData(flamerBurningParticle)
{
	dragCoefficient      = 1;
	gravityCoefficient   = -1;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 525;
	lifetimeVarianceMS   = 455;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 16.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.7 0.6 0.3 0.5";
	colors[1]     = "0.9 0.4 0.1 0.8";
	colors[2]     = "0.9 0.2 0.1 0.2";
	colors[3]     = "0.9 0.1 0.1 0";
	sizes[0]      = 0.45;
	sizes[1]      = 0.67;
	sizes[2]      = 1.05;
	sizes[3]      = 1.85;
   times[0] = 0.0;
   times[1] = 0.3;
   times[2] = 0.6;
   times[3] = 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(flamerBurningAEmitter)
{
   ejectionPeriodMS = 12;
   periodVarianceMS = 0;
   ejectionVelocity = 5;
   velocityVariance = 1;
   ejectionOffset   = 0.1;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   orientOnVelocity = true;
   particles = flamerBurningParticle;
};
datablock ParticleEmitterData(flamerBurningBEmitter)
{
   ejectionPeriodMS = 12;
   periodVarianceMS = 0;
   ejectionVelocity = 0;
   velocityVariance = 0;
   ejectionOffset   = 1;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   orientOnVelocity = true;
   particles = flamerBurningParticle;
};
datablock ParticleData(flamerSparkParticle)
{
	dragCoefficient      = 4;
	gravityCoefficient   = -0.4;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 325;
	lifetimeVarianceMS   = 255;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 16.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.7 0.6 0.3 0.8";
	colors[1]     = "0.9 0.4 0.1 0.8";
	colors[2]     = "0.9 0.2 0.1 0.2";
	colors[3]     = "0.9 0.1 0.1 0";
	sizes[0]      = 0.15;
	sizes[1]      = 0.17;
	sizes[2]      = 0.26;
	sizes[3]      = 0.14;
   times[0] = 0.0;
   times[1] = 0.3;
   times[2] = 0.6;
   times[3] = 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(flamerSparkEmitter)
{
   ejectionPeriodMS = 8;
   periodVarianceMS = 0;
   ejectionVelocity = 9;
   velocityVariance = 0.2;
   ejectionOffset   = 0;
   thetaMin         = 0;
   thetaMax         = 20;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   orientOnVelocity = true;
   particles = flamerSparkParticle;
};
datablock DebrisData(flamerSparkDebris)
{
	shapeFile = "base/data/shapes/empty.dts";
	lifetime = 2;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 200.0;
	elasticity = 0.5;
	friction = 0;
	numBounces = 1;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 6;
	emitters[0] = flamerSparkEmitter;
};
datablock explosionData(flamerSparkExplosion)
{
	lifetimeMS = 50;
	debris 					= flamerSparkDebris;
	debrisNum 				= 4;
	debrisNumVariance 		= 3;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 140;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 6;
	debrisVelocityVariance 	= 2;
	explosionScale = "0.1 0.1 0.1";
	faceViewer = 1;
	offset = 0.3;
};
datablock explosionData(flamerExplosion)
{
	soundProfile = "";
	lifetimeMS = 4000;
	emitter[0] = flamerBurningAEmitter;
	emitter[1] = flamerBurningBEmitter;
	subExplosion[0] = flamerSparkExplosion;
};
datablock projectileData(flamerFakeProjectile)
{
	explosion = flamerExplosion;
	lifetimeMS = 10000;
};

function flamerRubbishProjectile::onCollision(%db,%proj,%hit,%fade,%pos,%normal)
{
	flamerProjectileExplode(%proj);
}
function flamerProjectileExplode(%proj)
{
	%mask = $TypeMasks::fxBrickObjectType | $TypeMasks::StaticObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType;
	if(!isObject($flamerFireSet)) $flamerFireSet = new simSet();
	%set = $flamerFireSet;
	%cnt = %set.getCount();
	%pos = %proj.getPosition();
	%attacker = %proj.client;
	%ignore = %proj.ignoreVehicles;
	%attackerPl = %proj.sourceObject;
	if(%cnt != 0)
	{
		%safety = 0;
		%ready = 0;
		while(!%ready && %safety < 12)
		{
			%pass = 1;
			for(%i=%cnt-1;%i>=0;%i--)
			{
				%obj = %set.getObject(%i);
				%fpos = %obj.position;
				%dist = vectorDist(%pos,%fpos);
				if(%dist < 1)
				{
					%pass = 0;
					%tmp = vectorAdd(%pos,(getRandom()-0.5)*2 SPC (getRandom()-0.5)*3 SPC 0);
					%ray = containerRayCast(%tmp,vectorAdd(%tmp,"0 0 -1.5"),%mask);
					if(getWord(%ray,0))
						%pos = %tmp;
					break;
				}
			}
			if(%pass) %ready = 1;
			%safety++;
		}
	}
	%damageType = %proj.damageType;
	if(%damageType $= "")
		%damageType = $DamageType::Flamer;
	%proj.delete();
	if(%pass)
	{
		%ray = containerRayCast(%pos,vectorAdd(%pos,"0 0 -1.5"),%mask);
		if(getWord(%ray,0)) %pos = getWords(%ray,1,3);
	}
	%p = new projectile()
	{
		datablock = flamerFakeProjectile;
		initialPosition = %pos;
		initialVelocity = "0 0 -1";
	};
	%p.explode();
	%exp = new simSet()
	{
		tick = 0;
		sourceObject = %attackerPl;
		beGentleWith = %attackerPl;
		client = %attacker;
		position = %pos;
		ignoreVehicles = %ignore;
		damageType = %damageType;
	};
	$flamerFireSet.add(%exp);
	if(!isEventPending($flamerFireGlobalLoop)) flamerExplosionLoop();
	flameSetAdd(getFlameSet(%exp),%exp);
}
function flamerExplosionLoop()
{
	cancel($flamerFireGlobalLoop);
	%set = $flamerFireSet;
	%cnt = %set.getCount();
	
	if(%cnt == 0)
		return;
	%sparks = 0;
	for(%i=%cnt-1;%i>=0;%i--)
	{
		%exp = %set.getObject(%i);
		%pos = %exp.position;
		%tick = %exp.tick;
		
		if(!getRandom(0,3) && %sparks > 0)
		{
			%p = new projectile()
			{
				datablock = flamerSparkProjectile;
				initialPosition = vectorAdd(%pos,"0 0 0.1");
				initialVelocity = "0 0 10";
			};
			%p.explode();
			%sparks--;
		}
		
		%dist = 2.5;
		%max = 10;
		%dmg = 3;
		%mask = $TypeMasks::PlayerObjectType;// | $TypeMasks::CorpseObjectType;
		initContainerRadiusSearch(%pos,mSqrt(%dist*%dist+%dist*%dist)+0.1,%mask);
		while(%hit = containerSearchNext())
		{
			if(%hit.getDatablock().noBurning)
				continue;
			%hPos = %hit.getPosition();
			%hDist = vectorDist(%pos,%hPos);
			if(%exp.beGentleWith == %hit)
			{
				if(%hDist > %dist/2)
					continue;
			}
			else if(%hDist > %dist)
				continue;
			
			%fact = ((%dist-%hDist)/%dist);
			if(%exp.beGentleWith == %hit)
				%fact *= 0.5;
			
			if(getSimTime()-%hit.lastFireDmg > 100)
			{
				cancel(%hit.burnSchedRem);
				%hit.lastFireDmg = getSimTime();
				%dmgd = %dmg*%fact;
				if(minigameCanDamage(%exp.client,%hit) == 1)
				{
					%hit.lastFireAttacker = %exp.client;
					%hit.lastBurnDmgType = %exp.damageType;
					%hit.flamer_burnStart(mCeil(%dmgd)*3);
				}
			}
		}
			
		%exp.tick++;
		if(%exp.tick > 60)
		{
			%clumpSet = %exp.set;
			%exp.delete();
			flameSetFlamePop(%clumpSet);
			%i--;
			continue;
		}
	}
	$flamerFireGlobalLoop = schedule(50,0,flamerExplosionLoop);
}

function molotovImage::onFlick(%this, %obj, %slot)
{
	%obj.playthread(2, shiftRight);
}

function molotovImage::onRemove(%this, %obj, %slot)
{
	%obj.playthread(2, shiftRight);
}

function molotovImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
	%obj.lastPipeSlot = %obj.currTool;
}

function molotovImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, root);
}

function molotovImage::onFire(%this, %obj, %slot)
{
	Parent::onFire(%this, %obj, %slot);
	
	%currSlot = %obj.lastPipeSlot;
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%obj.client);
	serverPlay3D("molotov_throw_sound",%obj.getPosition());

	%obj.playthread(2, spearThrow);
}

function molotovProjectile::onExplode(%db,%proj,%expos,%b)
{
	parent::onExplode(%db,%proj,%expos,%b);
	molotov_explode(%expos,%proj,%proj.client);
}
function molotovProjectile::damage(%this,%obj,%col,%fade,%pos,%normal)
{
	if(!%col.getDatablock().noBurning) Parent::damage(%this,%obj,%col,%fade,%pos,%normal);	
}
function molotov_explode(%pos,%obj,%cl)
{
	createFireCircle(%pos,10,30,%cl,%obj,$DamageType::Molotov);
	serverPlay3D("molotov_explode_sound",%pos);
}

function createFireCircle(%pos,%rad,%amt,%cl,%obj,%damageType)
{
	for(%i=0;%i<%amt;%i++)
	{
		%rnd = getRandom();
		%dist = getRandom()*%rad;
		%x = mCos(%rnd*$PI*2)*%dist;
		%y = mSin(%rnd*$PI*2)*%dist;
		%p = new projectile()
		{
			datablock = flamerRubbishProjectile;
			initialPosition = %pos;
			initialVelocity = %x SPC %y SPC (getRandom()*4);
			client = %cl;
			sourceObject = %obj;
			damageType = %damageType;
		};
	}
}
function player::flamer_burnStart(%pl,%tick)
{
	if(%pl.getDatablock().noBurning) return;
	
	if(!%pl.isBurning)
	{
		%pl.setTempColor("0.1 0.1 0.1 1");
		%pl.flamer_burn(%tick);
		%pl.flamerBurnTickAdd = 0;
	}
	else %pl.flamerBurnTickAdd = %tick;
}
function player::flamer_burn(%pl,%tick)
{
	if(%pl.getDataBlock().hType $= "Zombie")
	{
		cancel(%pl.flamerClearBurnSched);
		%pl.isBurning = 1; 

		cancel(%pl.burnSched);
		if(!isObject(%pl.getMountedImage(3)))
		%pl.mountImage(flamerFleshBurningImage,3);
		
		if(!%pl.isPlayingBurningSound)
		{
			%pl.playAudio(3,"fire_fleshLoop_sound");
			%pl.isPlayingBurningSound = 1;
		}
		
		%dmg = mClamp(%pl.getdataBlock().maxDamage/25,10,%pl.getdataBlock().maxDamage);
		if(%pl.isCrouched())
		%dmg *= 0.47619;
		if(!%pl.noFireBurning)
		{
			%pl.damage(%pl.lastFireAttacker,%pl.getPosition(),%dmg,%pl.lastBurnDmgType);
			if(%pl.getclassname() $= "AIPlayer" && %pl.hZombieL4BType $= "Normal")
			{
				%pl.hRunAwayFromPlayer(%pl);
				%pl.stopHoleLoop();
					
				%pl.MaxSpazzClick = getrandom(16,32);
				L4B_SpazzZombie(%pl,0);
			}
			%pl.playThread(2,plant);
		}
	
		%pl.burnSched = %pl.schedule(500,flamer_burn,%tick);
	}
	else
	{
		cancel(%pl.flamerClearBurnSched);
		%pl.isBurning = 1;
		if(%pl.flamerBurnTickAdd > 0 && %pl.flamerBurnTickAdd > %tick)
		{
			%tick = mFloor(%pl.flamerBurnTickAdd);
			%pl.flamerBurnTickAdd = 0;
		}
		cancel(%pl.burnSched);
		if(!isObject(%pl.getMountedImage(3))) %pl.mountImage(flamerFleshBurningImage,3);

		if(!%pl.isPlayingBurningSound)
		{
			%pl.playAudio(3,"fire_fleshLoop_sound");
			%pl.isPlayingBurningSound = 1;
		}

		%dmg = mClamp(%tick,3,6)*1.75;
		if(%pl.isCrouched()) %dmg *= 0.47619;
		if(%pl.getDamagePercent() < 1)
		{
			%pl.damage(%pl.lastFireAttacker,%pl.getPosition(),%dmg,%pl.lastBurnDmgType);
			%pl.playThread(2,plant);
		}

		if(%tick <= 0)
		{
			%pl.unMountImage(3);
			%pl.isBurning = 0;
			%pl.playAudio(3,napalmFireEndSound);
			%pl.isPlayingBurningSound = 0;

			if(%pl.getDamagePercent() < 1) %pl.flamerClearBurnSched = %pl.schedule(1500,flamer_clearBurn);
			return;
		}
		%pl.burnSched = %pl.schedule(500,flamer_burn,%tick--);
	}
}
function player::flamer_clearBurn(%pl)
{
	cancel(%pl.flamerClearBurnSched);
	%p = new projectile()
	{
		initialPosition = vectorAdd(%pl.getPosition(),"0 0 1");
		datablock = PlayerSootProjectile;
		scale = "0.7 0.7 0.7";
	};
	%p.explode();
	%pl.clearTempColor();
}


function cleanFlameSet()
{
	%tailClear = 1;
	for(%i=$flameSetCnt-1;%i>=0;%i--)
	{
		%set = $flameSet[%i];
		if(%set $= "")
		{
			if(%tailClear == 1) $flameSetCnt--;
			continue;
		}
		if(%set.getCount() == 0)
		{
			if(isObject(%set.soundObj)) %set.soundObj.delete();
			%set.delete();
			$flameSet[%i] = "";
			if(%tailClear == 1) $flameSetCnt--;
			
		}
		else %tailClear = 0;
	}
}
function getFlameSet(%exp)
{
	%pos = %exp.position;
	for(%i=0;%i<$flameSetCnt;%i++)
	{
		if($flameSet[%i] $= "")
			continue;
		%med = $flameSet[%i].median;
		if(vectorDist(%pos,%med) < 6)
			return $flameSet[%i];
	}
	$flameSet[%i = $flameSetCnt] = new simSet();
	$flameSetCnt++;
	return $flameSet[%i];
}
function flameSetAdd(%set,%exp)
{
	%set.add(%exp);
	flameSetGenerateMedian(%set);
	flameSetPlayAudio(%set);
	%exp.set = %set;
}
function flameSetGenerateMedian(%set)
{
	%cnt = %set.getCount();
	%xAv = 0;
	%yAv = 0;
	%zAv = 0;
	for(%i=0;%i<%cnt;%i++)
	{
		%pos = %set.getObject(%i).position;
		%x = getWord(%pos,0);
		%y = getWord(%pos,1);
		%z = getWord(%pos,2);
		%xAv += %x;
		%yAv += %y;
		%zAv += %z;
	}
	%av = (%xAv/%cnt) SPC (%yAv/%cnt) SPC (%zAv/%cnt);
	%set.median = %av;
}
function flameSetPlayAudio(%set)
{
	return;
	if(isObject(%set.soundObj)) %set.soundObj.setTransform(%set.median);
	else
	{
		%soundObj = %set.soundObj = new aiPlayer()
		{
			datablock = emptyPlayer;
			position = %set.median;
			source = %set;
		};
		%soundObj.setDamageLevel(100);
		%soundObj.playAudio(0,"fire_loop" @ getRandom(1,2) @ "_sound");
	}
}
function flameSetDestroy(%set)
{
	if(isObject(%set.soundObj)) %set.soundObj.delete();
	if(isObject(%set.debugObj)) %set.debugObj.delete();
	serverPlay3D("fire_end_sound",%set.median);
	cleanFlameSet();
}
function flameSetFlamePop(%set)
{
	if(%set.getCount() == 0)
	{
		flameSetDestroy(%set);
		return 0;
	}
	else
	{
		flameSetGenerateMedian(%set);
		flameSetPlayAudio(%set);
		return 1;
	}
}

package swol_sweps_extpack
{
	function player::emote(%pl,%im,%override)
	{
		if(%pl.getDatablock().isEmptyPlayer) return;
		if(%pl.isBurning) if(isObject(%im)) if(%im.getClassName() !$= "ProjectileData") return;			
	
		parent::emote(%pl,%im,%override);
	}

	function armor::onDisabled(%db,%pl,%bool)
	{
		if(%db.isEmptyPlayer)
			return;
		return parent::onDisabled(%db,%pl,%bool);
	}
	function armor::onMount(%db,%pl,%obj,%node)
	{
		if(%db.isEmptyPlayer)
			return;
		return parent::onMount(%db,%pl,%obj,%node);
	}
};
activatePackage(swol_sweps_extpack);