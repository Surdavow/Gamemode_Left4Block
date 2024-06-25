AddDamageType("Crowbar",'<bitmap:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci/ci_crowbar> %1','%2 <bitmap:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci/ci_crowbar> %1',0.2,1);

datablock ParticleData(meleeTrailParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 1.5;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1500;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	animateTexture		= false;

	textureName		= "base/data/particles/dot";
	colors[0]	= "2 2 2 0.0025";
	colors[1]	= "2 2 2 0.0";
	sizes[0]	= 0.4;
	sizes[1]	= 0.1;
	times[0]	= 0.5;
	times[1]	= 0.1;
};

datablock ParticleEmitterData(meleeTrailEmitter)
{
   ejectionPeriodMS = 2;
   periodVarianceMS = 0;
   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;
   ejectionOffset = 0;
   thetaMin         = 0.0;
   thetaMax         = 90.0;  
   particles = meleeTrailParticle;
   useEmitterColors = true;
   uiName = "";
};

datablock ItemData(crowbarItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system
	shapeFile = "./models/model_crowbar.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	uiName = "Crowbar";
	iconName = "./icons/icon_crowbar";
	doColorShift = true;
	colorShiftColor = "0.5 0.5 0.5 1";
	image = crowbarImage;
	canDrop = true;
};

datablock ShapeBaseImageData(crowbarImage)
{
	shapeFile = "./models/model_crowbar.dts";
	emap = true;
	mountPoint = 0;
	offset = "0 0 0";
	correctMuzzleVector = false;
	eyeOffset = "0 0 0";
	className = "WeaponImage";
	item = crowbarItem;
	ammo = " ";
	projectile = "";
	projectileType = Projectile;
	melee = true;
	hasLunge = true;
	doRetraction = false;
	armReady = false;
	doColorShift = crowbarItem.doColorShift;
	colorShiftColor = crowbarItem.colorShiftColor;

	meleeDamageDivisor = 2;
	meleeDamageType = $DamageType::Crowbar;
	meleeHitEnvSound = "crowbar";
	meleeHitPlSound = "crowbar";

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = WeaponSwitchsound;

	stateName[1]                     = "Ready";
	stateScript[1]                  = "onReady";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]					= "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.05;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateFire[3]                    = false;
	stateScript[3]                  = "onFire";
	stateTimeoutValue[3]            = 0.1;
	stateEmitter[3]					= "meleeTrailEmitter";
	stateEmitterNode[3]             = "muzzlePoint";
	stateEmitterTime[3]             = "0.225";

	stateName[4]			= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "StopFire";

	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Break";
	stateTimeoutValue[5]            = 0.1925;
	stateScript[5]                  = "onStopFire";
	stateEmitter[5]					= "";
	stateEmitterNode[5]             = "muzzlePoint";
	stateEmitterTime[5]             = "0.1";

	stateName[6]                    = "Break";
	stateTransitionOnTimeout[6]     = "Ready";
	stateTimeoutValue[6]            = 0.3;
};

function crowbarImage::onReady(%this, %obj, %slot)
{
	if(%obj.getstate() $= "Dead") return;
	%obj.playthread(1, "root");
}

function crowbarImage::onFire(%this, %obj, %slot)
{
	LuaCall(Melee_SwingCheck,%obj,%this,%slot);
}

function crowbarImage::onPreFire(%this, %obj, %slot)
{
	if(%obj.getstate() $= "Dead") return;
	serverPlay3D("melee_swing_sound",%obj.gethackposition());
	%obj.playthread(1, "meleeRaise");
	%obj.playthread(2, "meleeSwing" @ getRandom(1,3));
}

//Don't edit anything, the function in weapon_melee.cs edits the stuff here

AddDamageType(Machete,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Machete> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Machete> %1',0.2,1);
datablock ItemData(MacheteItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Machete.dts"; uiName = "Machete"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Machete"; colorShiftColor = "0.5 0.5 0.5 1"; image = "MacheteImage";};
datablock ShapeBaseImageData(MacheteImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Machete.dts"; item = "MacheteItem"; doColorShift = MacheteItem.doColorShift; colorShiftColor = MacheteItem.colorShiftColor; meleeDamageDivisor = 1; damageType = $DamageType::Machete; meleeHitEnvSound = "Machete"; meleeHitPlSound = "Machete"; stateTimeoutValue[6] = 0.275;};
function MacheteImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function MacheteImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function MacheteImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(cKnife,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_cKnife> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_cKnife> %1',0.2,1);
datablock ItemData(cKnifeItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_cKnife.dts"; uiName = "cKnife"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_cKnife"; colorShiftColor = "0.75 0.75 0.75 1"; image = "cKnifeImage";};
datablock ShapeBaseImageData(cKnifeImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_cKnife.dts"; item = "cKnifeItem"; doColorShift = cKnifeItem.doColorShift; colorShiftColor = cKnifeItem.colorShiftColor; meleeDamageDivisor = 1; damageType = $DamageType::cKnife; meleeHitEnvSound = "Machete"; meleeHitPlSound = "cKnife"; stateTimeoutValue[6] = 0.125;};
function cKnifeImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function cKnifeImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function cKnifeImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(Hatchet,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Hatchet> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Hatchet> %1',0.2,1);
datablock ItemData(HatchetItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Hatchet.dts"; uiName = "Hatchet"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Hatchet"; colorShiftColor = "0.75 0.75 0.75 1"; image = "HatchetImage";};
datablock ShapeBaseImageData(HatchetImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Hatchet.dts"; item = "HatchetItem"; doColorShift = HatchetItem.doColorShift; colorShiftColor = HatchetItem.colorShiftColor; meleeDamageDivisor = 1; damageType = $DamageType::Hatchet; meleeHitEnvSound = "Crowbar"; meleeHitPlSound = "Machete"; stateTimeoutValue[6] = 0.35;};
function HatchetImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function HatchetImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function HatchetImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(Axe,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Axe> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Axe> %1',0.2,1);
datablock ItemData(AxeItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Axe.dts"; uiName = "Axe"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Axe"; colorShiftColor = "0.75 0.75 0.5 1"; image = "AxeImage";};
datablock ShapeBaseImageData(AxeImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Axe.dts"; item = "AxeItem"; doColorShift = AxeItem.doColorShift; colorShiftColor = AxeItem.colorShiftColor; meleeDamageDivisor = 1; damageType = $DamageType::Axe; meleeHitEnvSound = "Crowbar"; meleeHitPlSound = "Machete"; stateTimeoutValue[6] = 0.4;};
function AxeImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function AxeImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function AxeImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(Katana,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Katana> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Katana> %1',0.2,1);
datablock ItemData(KatanaItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Katana.dts"; uiName = "Katana"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Katana"; colorShiftColor = "0.75 0.75 0.75 1"; image = "KatanaImage";};
datablock ShapeBaseImageData(KatanaImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Katana.dts"; item = "KatanaItem"; doColorShift = KatanaItem.doColorShift; colorShiftColor = KatanaItem.colorShiftColor; meleeDamageDivisor = 1; damageType = $DamageType::Katana; meleeHitEnvSound = "Machete"; meleeHitPlSound = "Machete"; stateTimeoutValue[6] = 0.3;};
function KatanaImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function KatanaImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function KatanaImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(Spikebat,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Spikebat> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Spikebat> %1',0.2,1);
datablock ItemData(SpikebatItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Spikebat.dts"; uiName = "Spikebat"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Spikebat"; colorShiftColor = "0.675 0.45 0.275 1"; image = "SpikebatImage";};
datablock ShapeBaseImageData(SpikebatImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Spikebat.dts"; item = "SpikebatItem"; doColorShift = SpikebatItem.doColorShift; colorShiftColor = SpikebatItem.colorShiftColor; meleeDamageDivisor = 1; damageType = $DamageType::Spikebat; meleeHitEnvSound = "Bat"; meleeHitPlSound = "Spikebat"; stateTimeoutValue[6] = 0.3;};
function SpikebatImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function SpikebatImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function SpikebatImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(Bat,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Bat> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Bat> %1',0.2,1);
datablock ItemData(BatItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Bat.dts"; uiName = "Bat"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Bat"; colorShiftColor = "0.675 0.45 0.275 1"; image = "BatImage";};
datablock ShapeBaseImageData(BatImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Bat.dts"; item = "BatItem"; doColorShift = BatItem.doColorShift; colorShiftColor = BatItem.colorShiftColor; meleeDamageDivisor = 2; damageType = $DamageType::Bat; meleeHitEnvSound = "Bat"; meleeHitPlSound = "Bat"; stateTimeoutValue[6] = 0.325;};
function BatImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function BatImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function BatImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(Baton,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Baton> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Baton> %1',0.2,1);
datablock ItemData(BatonItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Baton.dts"; uiName = "Baton"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Baton"; colorShiftColor = "0.125 0.125 0.125 1"; image = "BatonImage";};
datablock ShapeBaseImageData(BatonImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Baton.dts"; item = "BatonItem"; doColorShift = BatonItem.doColorShift; colorShiftColor = BatonItem.colorShiftColor; meleeDamageDivisor = 2; damageType = $DamageType::Baton; meleeHitEnvSound = "Bat"; meleeHitPlSound = "Bat"; stateTimeoutValue[6] = 0.275;};
function BatonImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function BatonImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function BatonImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(Shovel,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Shovel> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Shovel> %1',0.2,1);
datablock ItemData(ShovelItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Shovel.dts"; uiName = "Shovel"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Shovel"; colorShiftColor = "0.5 0.5 0.5 1"; image = "ShovelImage";};
datablock ShapeBaseImageData(ShovelImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Shovel.dts"; item = "ShovelItem"; doColorShift = ShovelItem.doColorShift; colorShiftColor = ShovelItem.colorShiftColor; meleeDamageDivisor = 1; damageType = $DamageType::Shovel; meleeHitEnvSound = "Crowbar"; meleeHitPlSound = "Crowbar"; stateTimeoutValue[6] = 0.45;};
function ShovelImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function ShovelImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function ShovelImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(Sledgehammer,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Sledgehammer> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Sledgehammer> %1',0.2,1);
datablock ItemData(SledgehammerItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Sledgehammer.dts"; uiName = "Sledgehammer"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Sledgehammer"; colorShiftColor = "0.8 0.8 0.8 1"; image = "SledgehammerImage";};
datablock ShapeBaseImageData(SledgehammerImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Sledgehammer.dts"; item = "SledgehammerItem"; doColorShift = SledgehammerItem.doColorShift; colorShiftColor = SledgehammerItem.colorShiftColor; meleeDamageDivisor = 2; damageType = $DamageType::Sledgehammer; meleeHitEnvSound = "Crowbar"; meleeHitPlSound = "Sledgehammer"; stateTimeoutValue[6] = 0.4;};
function SledgehammerImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function SledgehammerImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function SledgehammerImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

AddDamageType(Pan,'<bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Pan> %1','%2 <bitmap:add-ons/gamemode_left4block/modules/misc/icons/ci/ci_Pan> %1',0.2,1);
datablock ItemData(PanItem : crowbarItem){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Pan.dts"; uiName = "Pan"; iconName = "add-ons/gamemode_left4block/modules/misc/icons/icon_Pan"; colorShiftColor = "0.375 0.375 0.375 1"; image = "PanImage";};
datablock ShapeBaseImageData(PanImage : crowbarImage){shapeFile = "add-ons/gamemode_left4block/modules/misc/models/model_Pan.dts"; item = "PanItem"; doColorShift = PanItem.doColorShift; colorShiftColor = PanItem.colorShiftColor; meleeDamageDivisor = 2; damageType = $DamageType::Pan; meleeHitEnvSound = "Pan"; meleeHitPlSound = "Pan"; stateTimeoutValue[6] = 0.225;};
function PanImage::onReady(%this, %obj, %slot) {crowbarImage::onReady(%this, %obj, %slot);}
function PanImage::onPreFire(%this, %obj, %slot) {crowbarImage::onPreFire(%this, %obj, %slot);}
function PanImage::onFire(%this, %obj, %slot) {crowbarImage::onFire(%this, %obj, %slot);}

//Swing function.

function Melee_SwingCheck(%obj, %this, %slot)
{
	
	//Do not continue if the object is invalid or dead
	%mountedImage = %obj.getMountedImage(0);
	if(!isObject(%obj) || %obj.getState() $= "Dead" || !isObject(%mountedImage)) return;
	
	//Also do not continue if it's not meleeing
	%imageState = %obj.getImageState(0);
	if(%mountedImage.meleeDamageDivisor $= "" || %imageState $= "Ready" || %imageState $= "StopFire") return;
	

	%radius = 5;
	%pos = %obj.getMuzzlePoint(%slot);
	%endPos = VectorAdd(%pos, VectorScale(%obj.getMuzzleVector(%slot), 2.5 * getWord(%obj.getScale(), 2)));
	%mask = ($TypeMasks::FxBrickObjectType & $TypeMasks::TerrainObjectType & $TypeMasks::StaticObjectType & $TypeMasks::VehicleObjectType & $TypeMasks::PlayerObjectType);

	initContainerRadiusSearch(%pos, %radius, %mask, %obj);
	while(%scanner = containerSearchNext())
	{
		if(!%scanned.getID() == %obj.getID())
		{
			%scannedPos = %scanned.getWorldBoxCenter();
			%ray = containerRayCast(%pos, %scannedPos, %mask, %obj);
			%rayPos = posFromRaycast(%ray);

			if(isObject(%ray) && VectorDist(%pos, %rayPos) < 1)
			{
				%class = %ray.getClassName();
				if(%obj.lastmeleeenvhitdelay $= "")
				{
					%obj.lastmeleeenvhitdelay = 0;
				}

				if(%class $= "AIPlayer" || %class $= "Player")
				{
					%hitDelay = 15;
				}
				else if(%class $= "fxDTSBrick" || %class $= "WheeledVehicle" || %class $= "fxPlane")
				{
					%hitDelay = 50;
				}

				if((%obj.lastmeleeenvhitdelay + %hitDelay) < getSimTime())
				{
					%p = new projectile() 
					{
						datablock = "SecondaryMeleeProjectile"; 
						initialposition = %rayPos;
					};  
					missionCleanup.add(%p); 
					%p.explode();

					%obj.lastmeleeenvhitdelay = getSimTime();

					if(%class $= "AIPlayer" || %class $= "Player")
					{
						serverPlay3D(%this.meleeHitPlSound @ "_hitpl" @ getRandom(1, 2) @ "_sound", %rayPos);
						if(!%ray.getState() $= "Dead" && minigameCanDamage(%obj, %ray) $= true)
						{
							%rayData = %ray.getDatablock();
							if(!%rayData.getName() $= "ZombieTankHoleBot")
							{
								if(%class $= "AIPlayer")
								{
									%ray.setMoveY(-0.15);
									%ray.setMoveX(0);
									%ray.setAimObject(%obj);
								}

								%ray.playThread(3, "zstumble" @ getRandom(1, 4));
								%ray.damage(%obj.client, %rayPos, %rayData.maxDamage / %thismeleeDamageDivision, %this.DamageType);
								%ray.applyImpulse(%rayPos, VectorAdd(VectorScale(%obj.getForwardVector(), 600), "0 0 400"));
							}
							else
							{
								%ray.damage(%obj.client, %rayPos, %rayData.maxDamage / 25, %this.DamageType);
							}
							%p = new projectile() 
							{
								datablock = "SecondaryMeleeProjectile"; 
								initialposition = %rayPos;
							};  
						}
					}
					else if(%class $= "fxDTSBrick" || %class $= "WheeledVehicle" || %class $= "fxPlane")
					{
						serverPlay3D(%this.meleeHitEnvSound @ "_hitenv" @ getRandom(1, 2) @ "_sound", %rayPos);
					}
				}
			}
		}
	}
	%obj.schedule(50, Melee_SwingCheck, %obj, %this, %slot);
}