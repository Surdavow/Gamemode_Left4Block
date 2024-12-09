datablock fxDTSBrickData (BrickLockerData : brick1x4fData)
{
	ShapeDatablock = HealthLockerShape;
	uiName = "Health Locker";
	iconName = "add-ons/gamemode_left4block/modules/misc/models/icons/icon_healthlocker";
	category = "Special";
	subCategory = "Interactive";	
	indestructable = true;
	alwaysShowWireFrame = false;	
};

datablock StaticShapeData(HealthLockerShape)
{
	density = "1";
	drag = "0";
	dynamicType = "0";
	emap = "0";
	mass = "1";
	isInvincible = true;
	shapeFile = "./models/shape_healthlocker.dts";
	shapeBrickPos = "0 0 -0.1";
	isInteractiveShape = true;
};

function BrickLockerData::onPlant(%this, %obj)
{
	Parent::onPlant(%this,%obj);

	%obj.setName("_HealthLocker");

	%interactiveshape = new StaticShape()
	{
		datablock = %this.ShapeDatablock;
		spawnbrick = %obj;
		color = getColorIdTable(%obj.colorid);
	};
	%obj.setrendering(0);
	%obj.setcolliding(0);
	%obj.setraycasting(0);
	%obj.interactiveshape = %interactiveshape;
	%interactiveshape.settransform(vectoradd(%obj.gettransform(),%interactiveshape.getdatablock().shapeBrickPos) SPC getwords(%obj.gettransform(),3,6));
}

function BrickLockerData::onloadPlant(%this, %obj)
{
	BrickLockerData::OnPlant(%this, %obj);
}

function BrickLockerData::onRemove(%this, %obj)
{
	Parent::OnRemove(%this,%obj);
	if(isObject(%obj.interactiveshape)) %obj.interactiveshape.delete();
}

function BrickLockerData::onDeath(%this, %obj)
{
   BrickLockerData::onRemove(%this, %obj);
}

function HealthLockerShape::onAdd(%this, %obj)
{
	Parent::onAdd(%this,%obj);
	%obj.schedule(100,setnodecolor,"ALL",%obj.color);
}

function HealthLockerShape::Interact(%this,%col,%obj)
{
	cancel(%col.closedoor);
	%col.closedoor = schedule(2000,0,InteractiveBrickAnim,%col,0);

	InteractiveBrickAnim(%col,1);
	%obj.addhealth(%obj.getDatablock().maxDamage);	
	%obj.mountImage(HealImage, 3);
	serverPlay3D("locker_stockheal_sound",%obj.getPosition());
	%obj.playthread(3, plant);
}

function HealthLockerShape::CheckConditions(%this,%col,%obj)
{
	if(%obj.getdamagelevel() < 5 || %obj.getstate() $= "Dead" || %obj.hIsInfected) return false;		

	%brick = %col.spawnbrick;
	if(%col.lasttouch+250 < getsimtime() && !%col.isshutting)
	{
		%col.lasttouch = getsimtime();
		%col.getdatablock().Interact(%col,%obj);
	}
}