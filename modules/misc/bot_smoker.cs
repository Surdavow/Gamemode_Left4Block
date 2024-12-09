datablock StaticShapeData(SmokerTongueShape)
{
	shapeFile = "./models/tongue.dts";
	isSmokerTongue = 1;
};

datablock ParticleData(SmokerSpores)
{
	dragCoefficient      = 4;
	gravityCoefficient   = 0.2;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 800;
	textureName = "base/data/particles/cloud";
	
	spinSpeed = 10.0;
	spinRandomMin = -50.0;
	spinRandomMax = 50.0;
	
	colors[0] = 33/255 SPC 33/255 SPC 33/255 SPC 0.5;
	colors[1] = 33/255 SPC 33/255 SPC 33/255 SPC 0.25;
	colors[2] = 33/255 SPC 33/255 SPC 33/255 SPC 0;
	sizes[0] = 12.0;
	sizes[1] = 8.0;
	sizes[2] = 4.0;
	
	useInvAlpha = true;
};

datablock ParticleEmitterData(SmokerSporeEmitter)
{
	lifeTimeMS = 8000;
	
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;
   ejectionVelocity = 15;
   velocityVariance = 12;
   ejectionOffset   = 0.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
	particles = SmokerSpores;
	
	emitterNode = HalfEmitterNode;
};

datablock ProjectileData(SmokerSporeProjectile)
{
   projectileShapeName				= "base/data/shapes/empty.dts";
   directDamage        = 0;
   explosion           = "";
   particleEmitter     = "SmokerSporeEmitter";

   brickExplosionImpact = false;

   sound = "smoker_explode_sound";

   muzzleVelocity      = 0;
   velInheritFactor    = 0;
   explodeOnDeath = false;

   armingDelay         = 0; //4 second fuse 
   lifetime            = 8000;
   fadeDelay           = 7500;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod = 0;
   uiName = "";
};

datablock ShapeBaseImageData(ZombieSmokerConstrictImage)
{
	shapeFile = "./models/constricted.dts";
	emap = true;
	mountPoint = $BackSlot;
	offset = "0 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 0");
	firstPerson = false;
		
	className = "WeaponImage";
	item = "";
		
	armReady = false;
   doColorShift = true;
   colorShiftColor = "1 0 0 1";
};

datablock fxDTSBrickData (BrickZombieSmoker_HoleSpawnData : BrickCommonZombie_HoleSpawnData)
{
	uiName = "Zombie Smoker Hole";
	iconName = "Add-Ons/Gamemode_Left4Block/modules/misc/icons/icon_smoker";

	holeBot = "ZombieSmokerHoleBot";
};

datablock TSShapeConstructor(SmokerMDts : ZombieMDts) 
{
	baseShape = "./models/msmoker.dts";
};

datablock PlayerData(ZombieSmokerHoleBot : CommonZombieHoleBot)
{
	shapeFile = SmokerMDts.baseShape;
	uiName = "Smoker Infected";
	minImpactSpeed = 24;
	speedDamageScale = 2;

	maxdamage = 100;//Health

	hName = "Smoker";//cannot contain spaces
	hAttackDamage = $Pref::L4B::Zombies::SpecialsDamage/2.5;
	hTickRate = 4000;

    maxForwardSpeed = 8;
    maxBackwardSpeed = 7;
    maxSideSpeed = 6;

 	maxForwardCrouchSpeed = 6;
    maxBackwardCrouchSpeed = 5;
    maxSideCrouchSpeed = 4;

	cameramaxdist = 4;
    cameraVerticalOffset = 0.9;
    cameraHorizontalOffset = 0.6;
    cameratilt = 0.1;
    maxfreelookangle = 2;

	hIsInfected = 2;
	hZombieL4BType = "Special";
	hPinCI = "<bitmapk:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci_smoker2>";
	SpecialCPMessage = "Right click to shoot your tongue <br>\c6Pin non-infected by hitting them with your tongue";
	hBigMeleeSound = "";
	hNeedsWeapons = 1;

	rechargeRate = 0.75;
	maxenergy = 100;
	showEnergyBar = true;
};

function ZombieSmokerHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

function ZombieSmokerHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);

	%obj.setscale("0.95 1 1.25");
}

function ZombieSmokerHoleBot::onBotLoop(%this,%obj)
{	
	if(%obj.getstate() !$= "Dead" && %obj.lastIdle+5000 < getsimtime())
	{
		%obj.playthread(3,plant);
		%obj.lastIdle = getsimtime();
	
		switch$(%obj.hState)
		{
			case "Wandering":	%obj.isStrangling = false;
								%obj.hNoSeeIdleTeleport();

								switch(getRandom(0,1))
								{
									case 0: %obj.playaudio(0,"smoker_lurk" @ getrandom(1,4) @ "_sound");
									case 1: %obj.playaudio(0,"smoker_gasp" @ getrandom(1,5) @ "_sound");
								}

			case "Following": 	%obj.playaudio(0,"smoker_recognize" @ getrandom(1,4) @ "_sound");
		}
	}
}

function ZombieSmokerHoleBot::onRemove(%this,%obj)
{
	if(isObject(%obj.tongue)) %obj.tongue.delete();
	Parent::onRemove(%this,%obj);
}

function ZombieSmokerHoleBot::onBotMelee(%this,%obj,%col)
{
	if(%obj.isStrangling && !%obj.hitMusic && %col == %obj.hEating && isObject(%col.client)) 
	{
		%col.client.l4bMusic("musicData_smoker_pin", true, "Music");
		%obj.hitMusic = true;
	}

	%meleeimpulse = mClamp(%obj.hLastMeleeDamage, 1, 10);
	%obj.playthread(2,"zAttack" @ getRandom(1,3));
	%obj.setaimobject(%col);
	
	if(%col.getType() & $TypeMasks::PlayerObjectType)
	{
		if(%col.getClassName() $= "Player") %col.spawnExplosion("ZombieHitProjectile",%meleeimpulse/2 SPC %meleeimpulse/2 SPC %meleeimpulse/2);
		%col.playthread(3,"plant");		
		%obj.playaudio(1,"melee_hit" @ getrandom(1,8) @ "_sound");
	}
}

function ZombieSmokerHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	%limb = %obj.rgetDamageLocation(%position);
	if(%damageType !$= $DamageType::FallDamage || %damageType !$= $DamageType::Impact)
	if(%limb) %damage = %damage/4;
	
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}

function ZombieSmokerHoleBot::onDamage(%this,%obj,%delta)
{	
    Parent::OnDamage(%this,%obj,%delta);
	
	if(%delta > 5 && %obj.lastdamage+500 < getsimtime())
	{			
		if(%obj.getstate() !$= "Dead") %obj.playaudio(0,"smoker_pain" @ getrandom(1,4) @ "_sound");

		%obj.playthread(2,"plant");
		%obj.lastdamage = getsimtime();	
	}

	if(%obj.getState() $= "Dead")
	{
		%obj.playaudio(0,"smoker_death" @ getrandom(1,2) @ "_sound");
		if(isObject(%obj.tongue)) %obj.tongue.delete();		
	
		%p = new projectile()
		{
			datablock = SmokerSporeProjectile;
			initialPosition = %obj.getposition();
			client = %obj.client;
			sourceObject = %obj;
		};
		%p.schedule(1000,SmokerExplosionCoughEffect);
		%obj.unMountImage(0);		
	}
}

function ZombieSmokerHoleBot::onBotFollow( %this, %obj, %targ )
{
	if((isObject(%obj) && %obj.getState() !$= "Dead" && %obj.hLoopActive && !%obj.isStrangling && !isEventPending(%obj.SpecialSched)) && (isObject(%targ) && %targ.getState() !$= "Dead")) 
	%this.schedule(750,onBotFollow,%obj,%targ);
	else return;

	%distance = vectorDist(%obj.getposition(),%targ.getposition());

	if(%distance < 75)
	{	
		if(%distance < 10)
		{
			%this.onTrigger(%obj,0,true);
			%obj.setMoveX(0);
			%obj.setMoveY(1);
			%obj.setmoveobject(%targ);			
		}
		else if(%obj.GetEnergyLevel() >= %this.maxenergy && !%obj.isStrangling)
		{
            %cansee = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%targ.getposition(),%obj.getposition()))) > 0.5;
            %obscure = containerRayCast(%obj.getEyePoint(),%targ.getMuzzlePoint(2),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);

            if(!isObject(%obscure) && %cansee)
			{
				%obj.schedule(250,hClearMovement);
				%obj.schedule(500,setaimobject,%targ);
				%this.onTrigger(%obj,4,1);
				%obj.schedule(700,hShootAim,%targ);
			}
		}
	}		
}

function ZombieSmokerHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{	
	%pack2 = 0;
	%accent = 0;
	%hatColor = getRandomBotColor();
	%packColor = getRandomBotColor();
	%shirtColor = getRandomBotColor();
	%accentColor = getRandomBotColor();
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
	%obj.secondPackColor =  "0 0.435 0.831 1";
	%obj.lhand =  "0";
	%obj.hip =  "0";
	%obj.faceName =  %face;
	%obj.rarmColor =  %rarmColor;
	%obj.hatColor =  %hatcolor;
	%obj.hipColor =  %pantsColor;
	%obj.chest =  "0";
	%obj.rarm =  "0";
	%obj.packColor =  "1 1 1 1";
	%obj.pack =  %pack;
	%obj.decalName =  "classicshirt";
	%obj.larmColor =  %larmColor;
	%obj.secondPack =  "0";
	%obj.larm =  "0";
	%obj.chestColor =  %shirtColor;
	%obj.accentColor =  "1 1 1 1";
	%obj.rhandColor =  %skinColor;
	%obj.rleg =  "0";
	%obj.rlegColor =  %rlegColor;
	%obj.accent =  "1";
	%obj.headColor =  %skinColor;
	%obj.rhand =  "0";
	%obj.lleg =  "0";
	%obj.lhandColor =  %skinColor;
	%obj.hat = 0;
	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

function ZombieSmokerHoleBot::L4BAppearance(%this,%obj,%client)
{
	Parent::L4BAppearance(%this,%obj,%client);
	%obj.unhidenode("smokerboil");
	
	switch$(%obj.getClassName())
	{
		case "Player": %obj.setNodeColor("smokerboil",%client.zombieskincolor);
		case "AIPlayer": %obj.setNodeColor("smokerboil",%client.headcolor);
	}
}

function ZombieSmokerHoleBot::onTrigger(%this,%obj,%triggerNum,%val)
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

		case 4: if(%obj.GetEnergyLevel() >= %this.maxenergy && !%obj.isStrangling && !isEventPending(%obj.SpecialSched))
				{
					if(%obj.getClassName() $= "AIPlayer")
					{							
						%obj.hClearMovement();
						%obj.stopHoleLoop();
					}				
					%obj.SpecialSched = %obj.getDatablock().schedule(750,Special,%obj);
					%obj.playaudio(0,"smoker_warn" @ getrandom(1,3) @ "_sound");
				}
	}	
}

function ZombieSmokerHoleBot::Special(%this, %obj)
{
	if(!isObject(%obj) || %obj.getstate() $= "Dead") return;		
	if(isObject(%obj.GHRope)) %obj.GHRope.delete();
	%obj.playthread(3,"Plant");
	%obj.playaudio(1,"smoker_launch_tongue_sound");
		%obj.setenergylevel(0);
	
	%p = new Projectile()
	{
		dataBlock = "SmokerTongueProjectile";
		initialVelocity = vectorAdd(vectorscale(%obj.getEyeVector(),125),"0 0 3.75");
		initialPosition = vectorAdd(%obj.getMuzzlePoint(2),"0 0 0.35");
		sourceObject = %obj;
		client = %obj.client;
	};
	MissionCleanup.add(%p);

	%tongue = new StaticShape()
	{
		dataBlock = SmokerTongueShape;
		source = %obj;
		end = %p;
	};	
	%obj.tongue = %tongue;
	%p.tongue = %tongue;
}

function SmokerTongueShape::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	%obj.setNodeColor("ALL","1 0 0 1");
	%this.onTongueLoop(%obj);
	MissionCleanup.add(%obj);
}

function SmokerTongueShape::onTongueLoop(%this,%obj)
{		
	if(!isObject(%obj)) return;
	
	%smoker = %obj.source;
	%end = %obj.end;

	if((isObject(%end) && (%end.getClassName() $= "Player" || %end.getClassName() $= "AIPlayer") && !L4B_SpecialsPinCheck(%smoker,%end)) || !isObject(%smoker) || !isObject(%end))
	{
		%obj.delete();
		return;
	}

	if(%end.getType() & $TypeMasks::ProjectileObjectType) %endpos = %end.getPosition();
	else if(%end.getType() & $TypeMasks::PlayerObjectType)
	{
		%endpos = vectorSub(%end.getmuzzlePoint(2),"0 0 0.2");

		if(%smoker.getclassname() $= "AIPlayer")
		{ 
			%smoker.setaimlocation(%endpos);
			%smoker.hClearMovement();
		}

		if(%end.lastchokecough+getrandom(250,500) < getsimtime() && getWord(%end.getVelocity(), 2) >= 0.75)
		{			
			if(%smoker.lastdamage+getRandom(500,100) < getsimtime())
			{
				%smoker.ChokeUpCount++;
				%smoker.playaudio(0,"smoker_pain" @ getrandom(1,4) @ "_sound");				
				%smoker.playthread(3,"plant");
				%smoker.playthread(2,"Shiftup");
				%smoker.lastdamage = getsimtime();
			}

			if(%smoker.ChokeUpCount > 5)
			{
				%end.playaudio(0,"norm_cough" @ getrandom(1,3) @ "_sound");
				%end.playthread(2,"plant");		
				%end.lastchokecough = getsimtime();
				
				%end.damage(%smoker.hFakeProjectile, %end.getposition(), %end.getdataBlock().maxDamage/15, $DamageType::SmokerConstrict);

				if(!%smoker.hitMusic && isObject(%end.client)) 
				{
					%end.playthread(3,"talk");
					%end.client.l4bMusic("musicData_smoker_pin", true, "Music");
					%smoker.hitMusic = true;
				}				
			}			
		}

		if(vectorDist(%end.getposition(),%smoker.getposition()) > 2)
		{ 
			if(getWord(%end.getVelocity(), 2) <= 0.75 && !%obj.hitMusic) %end.playthread(1,"activate2");

			%DisSub = vectorSub(%end.getPosition(),%smoker.getposition());
			%DistanceNormal = vectorNormalize(%DisSub);

			if(getWord(%end.getvelocity(),2) != 0) %force = 10;
			else %force = 5;
			%newvelocity = vectorscale(%DistanceNormal,-%force);

			if(vectorDist(%end.gethackposition(),%smoker.gethackposition()) > 5)
			%zinfluence = getWord(%end.getVelocity(), 2) + getWord(%newvelocity, 2)/10;
			else %zinfluence = getWord(%end.getVelocity(), 2);

			%end.setVelocity(getWords(%newvelocity, 0, 1) SPC %zinfluence);
		}
		else if(%smoker.lastattacked+300 < getsimtime())
		{
			%smoker.hMeleeAttack(%end);
			%smoker.lastattacked = getSimTime();
		}
	}

	if(%smoker.getclassname() $= "AIPlayer") %smoker.setaimlocation(%endpos);

	%head = %smoker.getmuzzlePoint(2);
	%vector = vectorNormalize(vectorSub(%endpos,%head));
	%relative = "0 1 0";
	%xyz = vectorNormalize(vectorCross(%relative,%vector));
	%u = mACos(vectorDot(%relative,%vector)) * -1;
	%obj.setTransform(vectorScale(vectorAdd(vectorAdd(%head,"0 0 0.5"),%endpos),0.5) SPC %xyz SPC %u);
	%obj.setScale(0.2 SPC vectorDist(%head,%endpos) * 2 SPC 0.2);

	%obj.TongueLoop = %this.schedule(33,onTongueLoop,%obj);
}

function SmokerTongueShape::onRemove(%this,%obj)
{		
	%smoker = %obj.source;
	%end = %obj.end;

	if(isObject(%smoker))
	{
		%smoker.isStrangling = false;

		serverPlay3D("smoker_tongue_reel_sound",%smoker.getposition());			
		if(%smoker.getClassName() $= "AIPlayer" && %smoker.getState() !$= "Dead")
		{
			%smoker.startHoleLoop();
			%smoker.hRunAwayFromPlayer(%smoker);				
		}
	}		
	
	if((isObject(%end) && (%end.getClassName() $= "Player" || %end.getClassName() $= "AIPlayer")))
	{
		%end.isBeingStrangled = false;
		if(isObject(%end.getMountedImage(3)) && %end.getMountedImage(3).getName() $= "ZombieSmokerConstrictImage") %end.unMountImage(3);
		if(%end.getState() $= "Dead") serverPlay3D("victim_smoked_sound",%end.getHackPosition());

		L4B_SpecialsPinCheck(%smoker,%end);
	}
}

datablock ProjectileData(SmokerTongueProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";
	directDamage        = 0;
	directDamageType    = $DamageType::Direct;
	particleEmitter     = "";
	explosion           = "";

	muzzleVelocity      = 100;
	velInheritFactor    = 0;

	armingDelay         = 0;
	lifetime            = 1000;
	fadeDelay           = 750;
	gravityMod 			= 1;
	isBallistic         = true;
	explodeOnDeath = true;

	uiName = "";
};

function SmokerTongueProjectile::onCollision(%this,%proj,%col,%fade,%pos,%normal)
{
	%obj = %proj.sourceObject;
	if(!isObject(%obj.tongue)) return;
	serverPlay3D("smoker_tongue_hit_sound",%pos);
	
	if(%col.getType() & $Typemasks::PlayerObjectType)
	{
		%col.dismount();
		%obj.tongue.end = %col;
		%obj.SpecialPinAttack(%col);
		%col.setTransform(getWords(%col.getPosition(),0,2) SPC getWords(%obj.getTransform(),3,6));
		return;
	}
	else %obj.tongue.delete();

	Parent::onCollision(%this,%proj,%col,%fade,%pos,%normal);
}

function Projectile::SmokerExplosionCoughEffect(%obj)
{
	if(!isObject(%obj)) return;

	%pos = %obj.getPosition();
	%radius = 5;
	%searchMasks = $TypeMasks::PlayerObjectType;
	InitContainerRadiusSearch(%pos, %radius, %searchMasks);
	while((%target = containerSearchNext()) != 0 )
	{
		if(!%target.hIsInfected && %target.getstate() !$= "Dead")
		{
			%randomtime = getRandom(400,800);
			%target.schedule(%randomtime,playaudio,2,"norm_cough" @ getrandom(1,3) @ "_sound");
			%target.schedule(%randomtime,playthread,3,"plant");
		}
	}
	%obj.schedule(750,SmokerExplosionCoughEffect);
}