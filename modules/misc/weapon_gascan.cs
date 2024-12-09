datablock ItemData(GasCanItem : PropaneTankItem)
{
	shapeFile = "./models/GasCan.dts";
	uiName = "Gas Can";
	iconName = "./icons/Icon_GasCan";
	colorShiftColor = "0.4 0.071 0.071 1.000";
	image = GasCanImage;
};

datablock ShapeBaseImageData(GasCanImage : PropaneTankImage)
{
	shapeFile = "./models/GasCan.dts";
	offset = "-0.53 0.05 -0.7";
	rotation = "0 0 1 90";
	vehicle = GasCanCol;
	item = GasCanItem;
	colorShiftColor = GasCanItem.colorShiftColor;
};

datablock WheeledVehicleData(GasCanCol : PropaneTankCol)
{
	shapeFile = "./models/GasCanCol.dts";
	image = GasCanImage;
};

function GasCanItem::onPickup(%this, %obj, %player)
{  
	PropaneTankItem::onPickup(%this, %obj, %player);
}

function GasCanImage::onFire(%this, %obj, %slot)
{
	PropaneTankImage::onFire(%this, %obj, %slot);
}

function GasCanCol::onAdd(%this,%obj)
{
	PropaneTankCol::onAdd(%this,%obj);
}
function GasCanCol::oncollision(%this, %obj, %col, %fade, %pos, %norm)
{
	propaneTankCol::oncollision(%this, %obj, %col, %fade, %pos, %norm);
}
function GasCanCol::onDamage(%this,%obj)
{
	createFireCircle(%obj.getPosition(),30,70,%obj.sourceobject.client,%obj,$DamageType::Molotov);
}
function GasCanImage::onMount(%this,%obj,%slot)
{
	PropaneTankImage::onMount(%this,%obj,%slot);
}

function GasCanImage::onUnMount(%this,%obj,%slot)
{
	PropaneTankImage::onUnMount(%this,%obj,%slot);
}