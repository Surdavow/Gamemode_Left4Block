datablock WheeledVehicleData(OxygenTankCol : PropaneTankCol)
{
	shapeFile = "./models/OxygenTankCol.dts";
	maxDamage = 50;
	image = OxygenTankImage;
	DistractionRadius = 50;
};

datablock ItemData(OxygenTankItem : PropaneTankItem)
{
	shapeFile = "./models/OxygenTank.dts";
	uiName = "Oxygen Tank";
	iconName = "./icons/Icon_OxygenTank";
	colorShiftColor = "0.4 0.071 0.071 1.000";
	image = OxygenTankImage;
};

datablock ShapeBaseImageData(OxygenTankImage : PropaneTankItem)
{
	shapeFile = "./models/OxygenTank.dts";
	offset = "-0.53 0.05 -0.7";
	rotation = "0 0 1 90";
	vehicle = OxygenTankCol;
	item = OxygenTankItem;
	colorShiftColor = OxygenTankItem.colorShiftColor;
};

function OxygenTankItem::onPickup(%this, %obj, %player)
{  
	PropaneTankItem::onPickup(%this, %obj, %player);
}

function OxygenTankImage::onFire(%this, %obj, %slot)
{
	PropaneTankImage::onFire(%this, %obj, %slot);
}

function OxygenTankCol::onAdd(%this,%obj)
{
	PropaneTankCol::onAdd(%this,%obj);
}

function OxygenTankCol::oncollision(%this, %obj, %col, %fade, %pos, %norm)
{
	PropaneTankCol::oncollision(%this, %obj, %col, %fade, %pos, %norm);
}

function OxygenTankCol::onDamage(%this,%obj)
{	
	if(!isEventPending(%obj.AboutToExplode))
	{
		%obj.AboutToExplode = %obj.schedule(1900,Damage,%obj,%obj.getPosition(),%this.maxDamage,$DamageType::Suicide);
		%obj.playaudio(3,oxygentank_leak_sound);
		%obj.ContinueSearch = %obj.getDatablock().schedule(250,Distract,%obj);
	}

	if(%obj.getDamageLevel() >= %this.maxDamage)
	PropaneTankCol::onDamage(%this,%obj);

	Parent::onDamage(%this,%obj);
}

function OxygenTankCol::Distract(%this, %obj)
{	
	if(!isObject(%obj))
	return;

	cancel(%obj.ContinueSearch);
	%obj.ContinueSearch = %obj.getDatablock().schedule(500,Distract,%obj);

	%pos = %obj.getPosition();
	%radius = %this.DistractionRadius;

	%searchMasks = $TypeMasks::PlayerObjectType;
	InitContainerRadiusSearch(%pos, %radius, %searchMasks);

	while((%targetid = containerSearchNext()) != 0 )
	{
		if(!%targetid.getState() !$= "Dead" && %targetid.getClassName() $= "AIPlayer" && %targetid.hZombieL4BType && %targetid.hZombieL4BType < 5 && !%targetid.isBurning)
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
function OxygenTankImage::onMount(%this,%obj,%slot)
{
	PropaneTankImage::onMount(%this,%obj,%slot);
}

function OxygenTankImage::onUnMount(%this,%obj,%slot)
{
	PropaneTankImage::onUnMount(%this,%obj,%slot);
}