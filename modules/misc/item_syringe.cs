datablock ItemData(SyringeAntidoteItem)
{
	uiName = "Syringe Antidote";
	iconName = "./icons/icon_syringered";
	image = SyringeAntidoteImage;
	category = Weapon;
	className = Weapon;
	shapeFile = "./models/syringered.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0;
	friction = 0.6;
	emap = true;
	doColorShift = true;
	colorShiftColor = "1 1 1 1";
	canDrop = true;
	gc_syringe = 1;
};

datablock shapeBaseImageData(SyringeAntidoteImage)
{
	shapeFile = "./models/syringered.dts";
	emap = true;
	correctMuzzleVector = false;
	isHealing = 1;
	className = "WeaponImage";
	item = SyringeAntidoteItem;
	ammo = "";
	projectile = "";
	projectileType = Projectile;
	melee = false;
	doReaction = false;
	armReady = true;
	doColorShift = true;
	colorShiftColor = "1 1 1 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.2;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = "heal_syringe_pickup_sound";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;

	stateName[2] = "Fire";
	stateTransitionOnTimeout[2] = "Ready";
	stateTimeoutValue[2] = 0.2;
	stateFire[2] = true;
	stateScript[2] = "onFire";
	stateWaitForTimeout[2] = true;
	stateAllowImageChange[2] = false;
};

function SyringeAntidoteImage::onFire(%this,%obj,%slot)
{
	if(isObject(%client = %obj.client) && %obj.getDamageLevel() < 5) commandToClient(%client, 'centerPrint', "\c5You are not injured.", 1);
  	else
  	{
  	    %obj.playThread(3, plant);
  	    cancel(%obj.gc_poisoning2);
  	    %obj.setDamageFlash(0);
  	    %obj.setDamageLevel(%obj.getDamageLevel()/1.7);
  	    %obj.emote(HealImage, 1);
  	    %obj.playaudio(2, "heal_syringe_stab_sound");
  	    %currSlot = %obj.currTool;
  	    %obj.tool[%currSlot] = 0;
  	    %obj.weaponCount--;
  	    messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
  	    serverCmdUnUseTool(%obj.client);
	
		if(%obj.getenergylevel() < 100 && %obj.getDatablock().isDowned) %obj.setenergylevel(%obj.getenergylevel()/0.65);
  	}
}