datablock ItemData(PropaneTankItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/propanetankbox.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Propane Tank";
	iconName = "./icons/Icon_Propane";
	doColorShift = true;
	colorShiftColor = "0.471 0.471 0.471 1.000";

	image = PropaneTankImage;
	canDrop = true;
	throwableExplosive = 1;
};

datablock ShapeBaseImageData(PropaneTankImage)
{
   	shapeFile = "./models/propanetank.dts";
   	emap = true;
   	vehicle = PropaneTankCol;

   	mountPoint = 0;
   	offset = "-0.53 0.05 -0.6";
   	rotation = "0 0 1 180";

   	correctMuzzleVector = true;
   	className = "WeaponImage";

   	item = PropaneTankItem;
   	ammo = " ";
   	projectile = "";
   	projectileType = "";

   	melee = false;
   	armReady = 2;

	doColorShift = true;
	colorShiftColor = "0.471 0.471 0.471 1.000";

	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.1;
	stateTransitionOnTimeout[0]	= "Ready";
	stateSequence[0]		= "ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]			= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]	= true;

	stateName[5]			= "Fire";
	stateTransitionOnTimeout[5]	= "Ready";
	stateTimeoutValue[5]		= 0.5;
	stateFire[5]			= true;
	stateSequence[5]		= "fire";
	stateScript[5]			= "onFire";
	stateWaitForTimeout[5]		= true;
	stateAllowImageChange[5]	= false;
};

datablock WheeledVehicleData(PropaneTankCol)
{
	category = "Vehicles";
	displayName = "";
	shapeFile = "./models/PropaneTankCol.dts";
	emap = true;
	minMountDist = 0;
   
   	numMountPoints = 0;

	image = PropaneTankImage;
	maxDamage = 1;
	destroyedLevel = 1;
	energyPerDamagePoint = 1;
	speedDamageScale = 1.04;
	collDamageThresholdVel = 20.0;
	collDamageMultiplier   = 0.02;
    doColorShift = true;
    colorShiftColor = "0.471 0.471 0.471 1.000";
	massCenter = "0 0 0";

	maxSteeringAngle = 0.9785;
	integration = 4;

	cameraRoll = false;
	cameraMaxDist = 13;
	cameraOffset = 7.5;
	cameraLag = 0.0;   
	cameraDecay = 0.75;
	cameraTilt = 0.4;
   	collisionTol = 0.1; 
   	contactTol = 0.1;

	useEyePoint = false;	


	numWheels = 0;

	mass = 800;
	density = 5.0;
	drag = 1.6;
	bodyFriction = 0.6;
	bodyRestitution = 0.6;
	minImpactSpeed = 10;
	softImpactSpeed = 10;
	hardImpactSpeed = 15;
	groundImpactMinSpeed    = 10.0;

	// Engine
	engineTorque = 12000;
	engineBrake = 2000; 
	brakeTorque = 50000;
	maxWheelSpeed = 0;

	rollForce		= 900;
	yawForce		= 600;
	pitchForce		= 1000;
	rotationalDrag		= 0.2;

	// Energy
	maxEnergy = 5;
	jetForce = 3000;
	minJetEnergy = 30;
	jetEnergyDrain = 2;

	splash = PropaneTankSplash;
	splashVelocity = 4.0;
	splashAngle = 67.0;
	splashFreqMod = 300.0;
	splashVelEpsilon = 0.60;
	bubbleEmitTime = 1.4;

	mediumSplashSoundVelocity = 10.0;   
	hardSplashSoundVelocity = 20.0;   
	exitSplashSoundVelocity = 5.0;

	softImpactSound = slowImpactSound;
	hardImpactSound = fastImpactSound;

	justcollided = 0;

   	uiName = "";
	rideable = true;
	lookUpLimit = 0.65;
	lookDownLimit = 0.45;

	paintable = true;
   
	damageEmitterOffset[0] = "0.0 0.0 0.0 ";
	damageLevelTolerance[0] = 0.99;

	damageEmitterOffset[1] = "0.0 0.0 0.0 ";
	damageLevelTolerance[1] = 1.0;

   	numDmgEmitterAreas = 1;

  	initialExplosionProjectile = "";
	finalExplosionProjectile = "";
   	finalExplosionOffset = 0;

   	burnTime = 100;

   	minRunOverSpeed    = 15;
   	runOverDamageScale = 5;
   	runOverPushScale   = 2;
};

datablock ExplosionData(PropaneTankFinalExplosion : vehicleFinalExplosion)
{
   //explosionShape = "";
   lifeTimeMS = 150;

   debris = "";
   debrisNum = 1;
   debrisNumVariance = 0;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisThetaMin = 0;
   debrisThetaMax = 20;
   debrisVelocity = 18;
   debrisVelocityVariance = 3;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "7.0 8.0 7.0";
   camShakeAmp = "10.0 10.0 10.0";
   camShakeDuration = 0.75;
   camShakeRadius = 15.0;

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 20;
   lightStartColor = "0.45 0.3 0.1";
   lightEndColor = "0 0 0";

   //impulse
   impulseRadius = 10;
   impulseForce = 1000;
   impulseVertical = 2000;

   //radius damage
   radiusDamage        = 1000;
   damageRadius        = 10.0;

   //burn the players?
   playerBurnTime = 4000;

};

datablock ProjectileData(PropaneTankFinalExplosionProjectile : vehicleFinalExplosionProjectile)
{
   directDamage        = 0;
   radiusDamage        = 0;
   damageRadius        = 0;
   explosion           = PropaneTankFinalExplosion;

   directDamageType  = $DamageType::PropaneTankExplosion;
   radiusDamageType  = $DamageType::PropaneTankExplosion;

    brickExplosionRadius = 8;
   brickExplosionImpact = false;
   brickExplosionForce  = 30;          
   brickExplosionMaxVolume = 30;  
   brickExplosionMaxVolumeFloating = 60;

   explodeOnDeath		= 1;

   armingDelay         = 0;
   lifetime            = 10;
};

function PropaneTankItem::onPickup(%this, %obj, %user, %amount)
{  
	Parent::onPickup(%this, %obj, %user, %amount);
	
	for(%i=0;%i<%user.getDatablock().maxTools;%i++)
	{
		%toolDB = %user.tool[%i];
		if(%toolDB $= %this.getID())
		{
			servercmdUseTool(%user.client,%i);
			break;
		}
	}
}

function PropaneTankImage::onFire(%this, %obj, %slot)
{
	%obj.sourcerotation = %obj.gettransform();
	%muzzlepoint = %obj.getHackPosition();
	%muzzlevector = vectorScale(%obj.getEyeVector(),3);
	%muzzlepoint = VectorAdd (%muzzlepoint, %muzzlevector);
	%playerRot = rotFromTransform (%obj.getTransform());

	%item = new WheeledVehicle(ExplosiveItemVehicle) 
	{  
		rotation = getwords(%obj.sourcerotation,3,6);
		datablock  = %this.vehicle;
		sourceObject = %obj.client.player;
		minigame = getMinigameFromObject(%obj);
		brickGroup = %obj.client.brickGroup;
	};

	%item.schedule(60000,delete);
	%item.startfade(5000,55000,1);
	%item.setTransform (%muzzlepoint @ " " @ %playerRot);
	%item.applyimpulse(%item.gettransform(),vectoradd(vectorscale(%obj.getEyeVector(),10000),"0" SPC "0" SPC 5000));

	%obj.tool[%currSlot] = 0;
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);

	%obj.droppedExplosiveVeh = 1;
	%obj.playthread(1,root);
	%obj.unMountImage(0);
}

function PropaneTankCol::onAdd(%this,%obj)
{
	%obj.setNodeColor("ALL",%this.image.item.colorShiftColor);
	
	Parent::onAdd(%this,%obj);
}
function PropaneTankCol::onCollision(%this, %obj, %col, %fade, %pos, %norm)
{
	Parent::onCollision(%this, %obj, %col, %fade, %pos, %norm);
}

function PropaneTankCol::onDamage(%this,%obj)
{
	%c = new projectile()
	{
		datablock = PropaneTankFinalExplosionProjectile;
		initialPosition = %obj.getPosition();
		client = %obj.creator.client;
		sourceObject = %obj.sourceObject;
		damageType = $DamageType::Boomer;
	};

	Parent::onDamage(%this,%obj);
}

function PropaneTankImage::onMount(%this,%obj,%slot)
{
	Parent::onMount(%this,%obj,%slot);	
	%obj.playThread(1, armReadyRight);
}

function PropaneTankImage::onUnMount(%this,%obj,%slot)
{
	Parent::onUnMount(%this,%obj,%slot);

	if(!%obj.droppedExplosiveVeh )
	{
		%obj.sourcerotation = %obj.gettransform();
		%muzzlepoint = %obj.getHackPosition();
		%muzzlevector = vectorScale(%obj.getEyeVector(),3);
		%muzzlepoint = VectorAdd (%muzzlepoint, %muzzlevector);
		%playerRot = rotFromTransform (%obj.getTransform());
	
		%item = new WheeledVehicle(ExplosiveItemVehicle) 
		{  
			rotation = getwords(%obj.sourcerotation,3,6);
			datablock  = %this.vehicle;
			sourceObject = %obj.client.player;
			minigame = getMinigameFromObject(%obj);
			brickGroup = %obj.client.brickGroup;
		};
	
		%item.schedule(60000,delete);
		%item.startfade(5000,55000,1);
		%item.setTransform (%muzzlepoint @ " " @ %playerRot);
		%item.applyimpulse(%item.gettransform(),vectoradd(vectorscale(%obj.getEyeVector(),10000),"0" SPC "0" SPC 5000));
	}

	%obj.playThread(3,jump);
	%obj.playThread(2,activate2);
	%obj.droppedExplosiveVeh = 0;
}
