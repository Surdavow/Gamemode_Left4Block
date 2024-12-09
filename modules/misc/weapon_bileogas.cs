datablock ItemData(BileOGasItem : PropaneTankItem)
{
	shapeFile = "./models/Jug.dts";
	uiName = "Gasbile Bottle";
	iconName = "./icons/Icon_BileOGas";
	doColorShift = false;
	colorShiftColor = "0.4 0.071 0.071 1.000";
	image = BileOGasImage;
};

datablock ShapeBaseImageData(BileOGasImage : PropaneTankImage)
{
	shapeFile = "./models/Jug.dts";
	offset = "-0.25 0 0";
	rotation = "0 0 1 90";
	vehicle = BileOGasCol;
	item = BileOGasItem;
	doColorShift = false;
	colorShiftColor = BileOGasItem.colorShiftColor;
};

datablock WheeledVehicleData(BileOGasCol : PropaneTankCol)
{
	shapeFile = "./models/JugCol.dts";
	image = BileOGasImage;
	DistractionRadius = 100;
};

function BileOGasItem::onPickup(%this, %obj, %player)
{  
	PropaneTankItem::onPickup(%this, %obj, %player);
}

function BileOGasImage::onFire(%this, %obj, %slot)
{
	PropaneTankImage::onFire(%this, %obj, %slot);
}

function BileOGasCol::onAdd(%this,%obj)
{
	Parent::onAdd(%this,%obj);
	
	%obj.ContinueSearch = %obj.getDatablock().schedule(500,Distract,%obj);
	%obj.setScale("1 1 1");
}

function BileOGasCol::onDamage(%this,%obj)
{
	createFireCircle(%obj.getPosition(),30,70,%obj.sourceobject.client,%obj,$DamageType::Molotov);

	%c = new projectile()
	{
		datablock = BoomerProjectile;
		initialPosition = %obj.getPosition();
		client = %obj.sourceObject.client;
		sourceObject = %obj.sourceObject;
		damageType = $DamageType::Boomer;
	};
}

function BileOGasCol::oncollision(%this, %obj, %col, %fade, %pos, %norm)
{
	propaneTankCol::oncollision(%this, %obj, %col, %fade, %pos, %norm);
}

function BileOGasImage::onMount(%this,%obj,%slot)
{
	Parent::onMount(%this,%obj,%slot);
	%obj.playThread(1, armReadyRight);
	%obj.playaudio(3,weaponSwitchSound);
}

function BileOGasCol::Distract(%this, %obj)
{	
	if(!isObject(%obj))
	return;

	cancel(%obj.ContinueSearch);
	%obj.ContinueSearch = %obj.getDatablock().schedule(1000,Distract,%obj);

	%pos = %obj.getPosition();
	%radius = %this.DistractionRadius;

	%searchMasks = $TypeMasks::PlayerObjectType;
	InitContainerRadiusSearch(%pos, %radius, %searchMasks);

	while((%targetid = containerSearchNext()) != 0 )
	{
		if(!%targetid.getState() !$= "Dead" && %targetid.getClassName() $= "AIPlayer" && %targetid.hZombieL4BType $= "Normal" && !%targetid.isBurning)
		{
			if(!%targetid.Distraction)
			{
				%targetid.Distraction = %obj.getID();
				%targetid.hSearch = 0;
			}
			else if(%targetid.Distraction == %obj.getID())
			{
				%targetid.setaimobject(%obj);
				%targetid.setmoveobject(%obj);
				%time1 = getRandom(1000,4000);
				%targetid.getDataBlock().schedule(%time1,onBotFollow,%targetid,%obj);
			}
		}
	}
}

function BileOGasImage::onUnMount(%this,%obj,%slot)
{
	PropaneTankImage::onUnMount(%this,%obj,%slot);
}