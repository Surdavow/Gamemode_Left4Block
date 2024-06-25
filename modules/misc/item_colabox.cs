datablock ItemData(ColaBoxItem)
{
	category = "Weapon";
	className = "Weapon";
	shapeFile = "./models/ColaBox.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	uiName = "Cola Box";
	iconName = "./icons/icon_ColaBox";
	doColorShift = false;
	image = ColaBoxImage;
	canDrop = true;
};
datablock ShapeBaseImageData(ColaBoxImage)
{
   	className = "WeaponImage";
   	shapeFile = "./models/ColaBox.dts";
   	emap = true;
   	mountPoint = 0;
   	item = ColaBoxItem;
   	armReady = true;
   	melee = false;
   	doRetraction = false;
   	doColorShift = false;
	stateName[0]                     = "Activate";
	stateScript[0]                  = "onActivate";
	stateTimeoutValue[0]             = 0;
};

function ColaBoxImage::onActivate(%this, %obj, %slot)
{
	%obj.playaudio(1,"colaboxequip" @ getrandom(1,4) @ "_sound");
	%obj.playthread(2, "armReadyBoth");
	%obj.playthread(2, "plant");
}
