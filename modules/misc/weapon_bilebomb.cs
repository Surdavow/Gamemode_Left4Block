datablock ParticleData(BileBombParticle)
{
	dragCoefficient      = 4;
	gravityCoefficient   = 0.01;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS = 15000;
	lifetimeVarianceMS = 500;
	textureName = "base/data/particles/cloud";
	
	spinSpeed = 25.0;
	spinRandomMin = -500.0;
	spinRandomMax = 500.0;
	
	colors[0] = 33/255 SPC 158/255 SPC 11/255 SPC 0.5;
	colors[1] = 33/255 SPC 158/255 SPC 11/255 SPC 0.25;
	colors[2] = 33/255 SPC 158/255 SPC 11/255 SPC 0.1;
	sizes[0] = 5.0;
	sizes[1] = 4.0;
	sizes[2] = 3.0;
	
	useInvAlpha = true;
};

datablock ParticleEmitterData(BileBombEmitter)
{
	lifeTimeMS = 50;

	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 1;
	velocityVariance = 1.0;
	ejectionOffset   = 2.0;
	thetaMin         = 89;
	thetaMax         = 90;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "BileBombParticle";

	emitterNode = HalfEmitterNode;
};

datablock ExplosionData(BileBombExplosion)
{
	lifeTimeMS = 15000;	
	emitter[0] = BileBombEmitter;
	particleEmitter = BileBombEmitter;
	particleDensity = 100;
	particleRadius = 1;	
	faceViewer = true;
	explosionScale = "1 1 1";
};

datablock ProjectileData(BileBombProjectile)
{
		projectileShapeName				= "./models/BileBombProjectile.dts";
		directDamage					= 0;
		
		brickExplosionImpact			= false;

		
		impactImpulse					= 0;
		verticalImpulse					= 0;
		explosion						= "BileBombExplosion";
		particleEmitter					= "";
		
		muzzleVelocity					= 25;
		velInheritFactor				= 0;
		
		armingDelay						= 0;
		lifetime						= 20000;
		fadeDelay						= 20000;
		bounceElasticity				= 0.4;
		bounceFriction					= 0.3;
		isBallistic						= true;
		gravityMod						= 1.0;
		
		hasLight						= false;
		lightRadius						= 3.0;
		lightColor						= "0 0 0.5";
		
		uiName							= "";
};

datablock ProjectileData(BileBombFakeProjectile)
{
	isDistraction = 1;
	distractionLifetime = 15;
	distractionFunction = BileBombDistract;
	distractionDelay = 0;
	DistractionRadius = 100;

	projectileShapeName				= "base/data/shapes/empty.dts";
	directDamage        = 0;
	explosion           = "";
	particleEmitter     = "";

	explodeOnDeath = 1;
	brickExplosionImpact = false;

	sound = "";

	muzzleVelocity      = 30;
	velInheritFactor    = 0;
	explodeOnDeath = false;

	armingDelay         = 15000; //4 second fuse 
	lifetime            = 15000;
	fadeDelay           = 13000;
	bounceElasticity    = 0.4;
	bounceFriction      = 0.5;
	isBallistic         = false;
	gravityMod = 0;

	hasLight    = false;


	uiName = "";
};

datablock ItemData(BileBombItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./models/BileBombItem.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Bile Bomb";
	iconName = "./icons/icon_BileBomb";
	doColorShift = false;

	 // Dynamic properties defined by the scripts
	image = BileBombImage;
	canDrop = true;
};

datablock ShapeBaseImageData(BileBombImage)
{
   // Basic Item properties
   shapeFile = "./models/BileBombItem.dts";
   emap = true;
   mountPoint = 0;
   offset = "-0.05 0.1 0";
   correctMuzzleVector = true;
   className = "WeaponImage";

   item = BileBombItem;
   ammo = " ";
   projectile = BileBombProjectile;
   projectileType = Projectile;

   melee = false;
   armReady = true;
   doColorShift = false;

	// States
	stateName[0]					= "Activate";
	stateSequence[0]				= "ready";
	stateSound[0]					= weaponSwitchSound;
	stateTimeoutValue[0]			= 0.1;
	stateTransitionOnTimeout[0]		= "Ready";

	stateName[1]					= "Ready";
	stateAllowImageChange[1]		= true;
	stateTransitionOnTriggerDown[1]	= "Charge";

	stateName[2]					= "Charge";
	stateTimeoutValue[2]			= 0.3;
	stateTransitionOnTimeout[2]		= "Charged";
	stateTransitionOnTriggerUp[2]	= "AbortCharge";
	stateScript[2]					= "onCharge";
	stateWaitForTimeout[2]			= true;

	stateName[3]					= "Charged";
	stateAllowImageChange[3]		= false;
	stateTransitionOnTriggerUp[3]	= "Fire";

	stateName[4]					= "Fire";
	stateAllowImageChange[4]		= false;
	stateFire[4]					= true;
	stateScript[4]					= "onFire";
	stateTimeoutValue[4]			= 0.5;
	stateTransitionOnTimeout[4]		= "Done";
	stateWaitForTimeout[4]			= true;
	
	stateName[5]					= "Done";
	stateScript[5]					= "onDone";
	
	stateName[6]					= "AbortCharge";
	stateScript[6]					= "onChargeAbort";
	stateTimeoutValue[6]			= 0;
	stateTransitionOnTimeout[6]		= "Ready";
};

function BileBombImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
	%obj.lastBileSlot = %obj.currTool;
}

function BileBombImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, root);
}

function BileBombImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	Parent::onFire(%this, %obj, %slot);
	%obj.hasBileBomb = 0;
	
	%currSlot = %obj.lastBileSlot;
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	%obj.unMountImage(0);
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%obj.client);
}

function Projectile::BileBombDistract(%obj, %count)
{
	if(isObject(%obj) || %count < %obj.getDatablock().distractionLifetime)
	%obj.ContinueSearch = %obj.schedule(1000,BileBombDistract,%count+1);

	%pos = %obj.getPosition();
	%radius = %obj.getDatablock().DistractionRadius;

	%searchMasks = $TypeMasks::PlayerObjectType;
	InitContainerRadiusSearch(%pos, %radius, %searchMasks);

	while((%targetid = containerSearchNext()) != 0 )
	{
		if(!%targetid.getState() !$= "Dead" && %targetid.getClassName() $= "AIPlayer" && %targetid.hZombieL4BType $= "Normal" && !%targetid.isBurning)
		{
			if(%count < %obj.getDatablock().distractionLifetime)
			{
				if(!%targetid.Distraction)
				{
					%targetid.Distraction = %obj.getID();
					%targetid.hSearch = 0;
					%targetid.hFollowing = 0;
				}
				else if(%targetid.Distraction == %obj.getID())
				{
					%targetid.setaimobject(%obj);
					%targetid.setmoveobject(%obj);
					%time1 = getRandom(1000,4000);
					%targetid.getDataBlock().schedule(%time1,onBotFollow,%targetid,%obj);
				}
			}
			else
			{
				%targetid.hSearch = 1;
				%targetid.Distraction = 0;
			}

			if(%obj.getdataBlock().getID() == BileBombFakeProjectile.getID() && ContainerSearchCurrRadiusDist() <= 4 && %targetid.hType $= "zombie")
			{
				%targetid.hType = "Biled" @ getRandom(1,9999);
				%targetid.mountImage(BileStatusPlayerImage, 2);
				%targetid.BoomerBiled = 1;
			}
		}
	}
}

function BileBombProjectile::onCollision(%this, %obj)
{
	%pos = %obj.getPosition();
	serverPlay3d("Bilejar_explode_sound",%pos);

   %p = new Projectile()
   {
      dataBlock = BileBombFakeProjectile;
      initialPosition = %pos;
      initialVelocity = "0 0 1";
      sourceObject = %obj.sourceObject;
      client = %obj.client;
      sourceSlot = 0;
      originPoint = %obj.originPoint;
   };
   if(isObject(%p))
   {
      MissionCleanup.add(%p);
      %p.setScale(%obj.getScale());
   } 
}