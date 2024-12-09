datablock DebrisData(beancanDebris)
{
	shapeFile = "./beans_can.dts";
	lifetime = 8.0;
	lifetimeVariance = 1.0;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 200.0;
	elasticity = 0.5;
	friction = 0.2;
	numBounces = 3;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
};
datablock ExplosionData(beancanExplosion)
{
	debris 					= beancanDebris;
	debrisNum 				= 1;
	debrisNumVariance 		= 0;
	debrisPhiMin 			= 0;
	debrisPhiMax 			= 360;
	debrisThetaMin 			= 0;
	debrisThetaMax 			= 180;
	debrisVelocity 			= 1;
	debrisVelocityVariance 	= 0;
};
datablock ProjectileData(beancanProjectile)
{
	explosion = beancanExplosion;
};

datablock DebrisData(bakedbeansDebris)
{
	shapeFile = "./beans_lid.dts";
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

datablock ItemData(bakedbeansItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/item_bakedbeans.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Baked Beans";
	iconName = "./icons/icon_bakedbeans";
	doColorShift = true;
	colorShiftColor = "0.2 0.121 0.071 1.000";
	
	image = bakedbeansImage;
	canDrop = true;
};

datablock ShapeBaseImageData(bakedbeansImage)
{
	shapeFile = bakedbeansItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "-0.025 -0.05 0.1";
	eyeOffset = 0;
	rotation = "0 0 15 1";

	className = "WeaponImage";
	item = bakedbeansItem;

	projectile = "";

	casing = bakedbeansDebris;
	shellExitDir		= ".0 0.25 0.25";
	shellExitOffset		= "0 0 5";
	shellExitVariance	= 5;	
	shellVelocity		= 10;

	armReady = true;

	doColorShift = true;
	colorShiftColor = bakedbeansItem.colorShiftColor;

	stateName[0]			= "Ready";
	stateTransitionOnTriggerDown[0]	= "Check";
	stateAllowImageChange[0]	= true;

	stateName[1]					= "Check";
	stateTransitionOnAmmo[1]		= "Open";
	stateTransitionOnNoAmmo[1]		= "Ready";

	stateName[2]					= "Open";
	stateSound[2]			= "can_open_sound";
	stateSequence[2]		= "canLid";
	stateTimeoutValue[2]			= 0.1;
	stateScript[1]					= "onOpen";
	stateTransitionOnTimeout[2] 	= "Pull";

	stateName[3]			= "Pull";
	stateSound[3]			= "can_spoon_sound";
	stateSequence[3]		= "canPull";
	stateTimeoutValue[3]			= 0.8;
	stateTransitionOnTimeout[3] 	= "Remove";
	stateScript[3]			= "onPull";

	
	stateName[4]			= "Remove";
	stateSequence[4]		= "canlidremove";
	stateTimeoutValue[4]			= 0.001;
	stateTransitionOnTimeout[4] 	= "Eat";
	stateEjectShell[4]				= true;

	stateName[5]			= "Eat";
	stateScript[5]			= "onEat";
};

function bakedbeansImage::onOpen(%data, %obj, %slot)
{
	%obj.playthread(2, shiftRight);
	%obj.playthread(3, shiftLeft);
}

function bakedbeansImage::onPull(%data, %obj, %slot)
{
	%obj.playthread(3, shfitForward);
}

function vectorRelativeShift(%forward,%up,%shift)
{
	return vectorAdd(vectorAdd(vectorScale(%forward,getWord(%shift,0)),vectorScale(vectorCross(%forward,%up),getWord(%shift,1))),vectorScale(%up,getWord(%shift,2)));
}
function getUpFromVec(%vec)
{
	%x = getWord(%vec,0);
	%y = getWord(%vec,1);
	%z = getWord(%vec,2);
	%up = vectorNormalize((-%x*%z) SPC (-%z*%y) SPC ((%x*%x)+(%y*%y)));
	return %up;
}
function player::getUpVectorHack(%pl)
{
	return getUpFromVec(%pl.getEyeVector());
}
function player::getEyeVectorHack(%pl)
{
    %forward = %pl.getForwardVector();
    %eye = %pl.getEyeVector();

    %x = getWord(%eye, 0);
    %y = getWord(%eye, 1);

    %yaw = mATan(getWord(%forward,0),getWord(%forward,1));
    %pitch = mATan(getWord(%eye,2),mSqrt(%x*%x+%y*%y));

    return MatrixMulVector(MatrixCreateFromEuler(%pitch SPC 0 SPC -%yaw),"0 1 0");
}

function bakedbeansImage::onEat(%data, %obj, %slot)
{
	%up = %obj.getUpVectorHack();
	%forward = %obj.getEyeVectorHack();
	%p = new projectile()
	{
		datablock = beancanProjectile;
		initialPosition = vectorAdd(%obj.getMuzzlePoint(%slot),vectorRelativeShift(%forward,%up,"-0.2 -0.5 -1.2"));
	};
	%p.explode();

	%client = %obj.client;	
	%obj.setDamageFlash(0);
	%obj.setDamageLevel(%obj.getDamageLevel()/1.8);
	%obj.playthread(3, plant);
	%obj.playaudio(1, "drink_gulp" @ getRandom(1,3) @ "_sound");
	%currSlot = %obj.currTool;
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%obj.client);
	
	if(%obj.getenergylevel() < 100 && %obj.getDatablock().getName() $= "DownPlayerSurvivorArmor") %obj.setenergylevel(%obj.getenergylevel()/0.775);	
}