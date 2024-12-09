datablock DebrisData(PillsHereDebris)
{
		shapeFile = "./models/cap.2.dts";
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

datablock ItemData(PillsHereItem)
{
		category = "Weapon";
		className = "Weapon";
		
		shapeFile = "./models/pills.4.dts";
		rotate = false;
		mass = 1;
		density = 0.2;
		elasticity = 0.2;
		friction = 0.6;
		emap = true;
		
		uiName = "Pills";
		iconName = "./icons/Icon_Pills";
		doColorShift = true;
		colorShiftColor = "0.8 0.8 0.8 1";
		
		image = PillsHereImage;
		canDrop = true;
};

datablock ShapeBaseImageData(PillsHereImage)
{
		shapeFile = "./models/pills.4.dts";
		emap = true;
		mountPoint = 0;
		offset = "0 0 0";
		eyeOffset = 0;
		rotation = eulerToMatrix( "0 0 0" );
		
		className = "WeaponImage";
		item = PillsHereItem;
		isHealing = 1;
		
		armReady = true;
		
		doColorShift = PillsHereItem.doColorShift;
		colorShiftColor = PillsHereItem.colorShiftColor;
		
		casing = PillsHereDebris;
		shellExitDir		= "1.0 1.0 1.0";
		shellExitOffset		= "0 0 0";
		shellExitVariance	= 5;	
		shellVelocity		= 10;
		
		stateName[0]					= "Activate";
		stateScript[0]					= "onActivate";
		stateTimeoutValue[0]			= 0.15;
		stateTransitionOnTimeout[0]		= "Ready";

		stateName[1]					= "Ready";
		stateAllowImageChange[1]		= true;
		stateSequence[1]				= "Ready";
		stateTransitionOnTriggerDown[1]	= "Use";
		
		stateName[2]					= "Use";
		stateScript[2]					= "onUse";
		stateTransitionOnTriggerUp[2]	= "Done";
		
		stateName[3]					= "Done";
		stateTransitionOnAmmo[3]		= "Ready";
		stateTransitionOnNoAmmo[3]		= "UnUse";
		
		stateName[4]					= "UnUse";
		stateEjectShell[4]				= true;
		stateScript[4]					= "onUnUse";
		stateSequence[4]				= "Open";
		stateTimeoutValue[4]			= 0.5;
		stateTransitionOnTimeout[4]		= "Hack";
		stateWaitForTimeout[4]			= true;
		
		stateName[5]					= "Hack";
		stateScript[5]					= "onHack";
};

function PillsHereImage::onActivate(%this,%obj,%db) { serverplay3d("heal_pills_deploy" @ getRandom(1,3) @ "_sound", %obj.getPosition()); }

function PillsHereImage::onHack(%this, %obj, %slot)
{
	%obj.playThread(1, "root");
	%obj.schedule(32, "unMountImage", %slot);
	
	if(isObject(%client = %obj.client))
	serverCmdUnUseTool(%client);
}

function PillsHereImage::onUnUse(%this, %obj, %slot)
{
	%tool = %obj.tool[%obj.currTool];
	
	if(isObject(%tool) && %tool.getID() == %this.item.getID())
	{
		%armor = %obj.getDatablock();
		%damage = %obj.getDamageLevel();
		%maxDamage = %armor.maxDamage;
		%heal = %maxDamage / 2;

		%obj.playAudio(1,"heal_pills_pop_sound");
    	%obj.setenergylevel(%obj.getenergylevel()/0.65);

		%obj.setDamageLevel(%obj.getDamageLevel()/1.3);
		%obj.emote(HealImage, 1);
		
		%obj.playThread(2, "shiftAway");
		%obj.setWhiteOut((%maxDamage - %damage) / %maxDamage);
		
		if(isObject(%client = %obj.client))
		messageClient(%client, 'MsgItemPickup', '', %obj.currTool, 0);
		
		%obj.tool[%obj.currTool] = 0;
		%obj.weaponCount--;
		%obj.currTool = -1;
	}
}

function PillsHereImage::onUse(%this, %obj, %slot)
{
	if(%obj.getDamageLevel() > 5 || %obj.getDatablock().getName() $= "DownPlayerSurvivorArmor")
	%obj.setImageAmmo(%slot, false);
	else
	{
		%obj.setImageAmmo(%slot, true);
		
		%client = %obj.client;
		if(isObject(%client))
		commandToClient(%client, 'centerPrint', "\c5You are not injured.", 2);
	}
}