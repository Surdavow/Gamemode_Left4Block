datablock fxDTSBrickData (BrickAmmoCrateData : brick2x4fData)
{
	ShapeDatablock = AmmoCrateShape;
	category = "Special";
	subCategory = "Interactive";
	uiName = "Ammo Crate";
	iconName = "add-ons/gamemode_left4block/modules/misc/models/icons/icon_ammocrate";
	indestructable = true;
	alwaysShowWireFrame = false;
};

datablock StaticShapeData(AmmoCrateShape)
{
	density = "1";
	drag = "0";
	dynamicType = "0";
	emap = "0";
	mass = "1";
	isInvincible = true;
	shapeFile = "./models/shape_ammocrate.dts";
	shapeBrickPos = "0 0 -0.1";
	isInteractiveShape = true;
};

function BrickAmmoCrateData::onPlant(%this, %obj)
{	
	Parent::onAdd(%this,%obj);
	
	%obj.setName("_Ammocrate");
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

function BrickAmmoCrateData::onloadPlant(%this, %obj) 
{ 
	BrickAmmoCrateData::onPlant(%this, %obj); 
}

function BrickAmmoCrateData::onRemove(%data, %brick)
{
	if(isObject(%brick.interactiveshape)) %brick.interactiveshape.delete();
	Parent::OnRemove(%data,%brick);
}

function BrickAmmoCrateData::onDeath(%data, %brick)
{
   BrickAmmoCrateData::onRemove(%data, %brick);
}

function AmmoCrateShape::onAdd(%this, %obj)
{
	Parent::onAdd(%this, %obj);
	%obj.schedule(100,setnodecolor,"ALL",%obj.color);
}

function InteractiveBrickAnim(%obj,%oc)
{
	if(!isObject(%obj)) return;
	
	if(%oc)
	{
		if(!%obj.isopen)
		{
			%obj.playaudio(2,"lockeropen_sound");
			%obj.playthread(1,"open");
			%obj.isopen = 1;
		}
	}
	else
	{
		%obj.playthread(1,"close");
		cancel(%obj.slowd);
		%obj.isopen = 0;
		%obj.isshutting = 1;
		%obj.slowd = schedule(500,0,%obj.isshutting = 0,%obj);
		%obj.playaudio(2,"lockerclose_sound");
	}
}

function AmmoCrateShape::Interact(%this,%col,%obj)
{
	cancel(%col.closedoor);
	%col.closedoor = schedule(2000,0,InteractiveBrickAnim,%col,0);

	InteractiveBrickAnim(%col,1);
	
	%image = %obj.getmountedimage(0);
	%item = %image.item;
	%ammoType = %item.AEType;
	%ammo = %item.AEAmmo;
	
	%max = %item.AEType.aeMax;
	%ammoused = %ammo-%obj.aeAmmo[%obj.currTool, %image.AEUseAmmo, 0];

	%obj.playthread(3, leftrecoil);
	serverPlay3D("pickupAmmo" @ getRandom(1,2) @ "Sound",%col.getPosition());

	%obj.AEReserve[%ammoType] = %max+%ammoused;
  	%obj.AEReserve[%ammoType] = mClamp(%obj.AEReserve[%ammoType], 0, %max + %ammoused);
	%obj.baadDisplayAmmo(%image);
	%obj.AENotifyAmmo(1);
}

function AmmoCrateShape::CheckConditions(%this,%col,%obj)
{
	if(%obj.getstate() $= "Dead" || %obj.hIsInfected) return false;

	%image = %obj.getmountedimage(0);
	%item = %image.item;
	%ammoType = %item.AEType;
	%ammo = %item.AEAmmo;
	%max = %item.AEType.aeMax;
	%ammoused = %ammo-%obj.aeAmmo[%obj.currTool, %image.AEUseAmmo, 0];
	%ammoneeded = %obj.AEReserve[%ammoType]-%ammoused;
	%ammocompensate = mFloatLength(%max/%ammoneeded * 0.5, 0);
	%brick = %col.spawnbrick;
	%obj.lasttouch = getsimtime();

	if(%col.lasttouch+250 < getsimtime() && !%col.isshutting)
	{
		%col.lasttouch = getsimtime();

		if(%ammoneeded < %max)
		{
			%this.Interact(%col,%obj);
			%col.unhideNode(ammobox);
		}
	}
}