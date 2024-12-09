datablock ItemData(GauzeItem)
{
		category = "Weapon";
		className = "Weapon";
		
		shapeFile = "./models/gauze.dts";
		rotate = false;
		mass = 1;
		density = 0.2;
		elasticity = 0.2;
		friction = 0.6;
		emap = true;
		
		uiName = "Gauze";
		iconName = "./icons/Icon_Gauze";
		doColorShift = false;
		
		image = GauzeImage;
		canDrop = true;
		l4ditemtype = "heal_full";
};


datablock ShapeBaseImageData(GauzeImage)
{
		shapeFile = "./models/gauze.dts";
		emap = true;
		mountPoint = 0;
		offset = "0 0 0";
		eyeOffset = 0;
		rotation = eulerToMatrix("0 0 0");
		
		className = "WeaponImage";
		item = GauzeItem;
		isHealing = 1;
		
		armReady = true;
		doColorShift = false;
		
		stateName[0]					= "Activate";
		stateSound[0]					= "heal_stop_sound";
		stateTimeoutValue[0]			= 0.15;
		stateSequence[0]				= "Ready";
		stateTransitionOnTimeout[0]		= "Ready";

		stateName[1]					= "Ready";
		stateAllowImageChange[1]		= true;
		stateScript[1]					= "onReady";
		stateTransitionOnTriggerDown[1]	= "Use";
		
		stateName[2]					= "Use";
		stateScript[2]					= "onUse";
		stateTransitionOnTriggerUp[2]	= "Ready";
};

function GauzeImage::onActivate(%this,%obj)
{
	%obj.playThread(0,plant);
}
function GauzeImage::healLoop(%this, %obj)
{
	%bandageSlot = %obj.currTool;
	%client = %obj.client;
	%tool = %obj.tool[%bandageSlot];
	
	if(isObject(%tool) && %tool.getID() == %this.item.getID())
	{
		%time = 2.4;
		%obj.GauzeUse += 0.1;
		
		if(%obj.GauzeUse >= %time)
		{
				%obj.pseudoHealth = 0;
				%obj.setDamageLevel(%obj.getDamageLevel()/3.25);
				%obj.emote(HealImage, 1);
				%obj.tool[%bandageSlot] = 0;
				%obj.weaponCount--;
				%obj.setMaxForwardSpeed(%obj.getDatablock().maxForwardSpeed);
				
				if(isObject(%client))
				{
					messageClient(%client, 'MsgItemPickup', '', %bandageSlot, 0);
					%client.setControlObject(%obj);
				}
				
				%obj.unMountImage(%slot);
				%obj.playThread(0, "root");
				%obj.playThread(1, "root");
				
				%client.Gauzing = false;
				%obj.GauzeUse = false;
				cancel(%obj.GauzeSched);
		}
		else
		{
			if((%obj.GauzeUse * 10) % 10 == 0)
			{
				if(getRandom(0, 1)) %obj.playThread(0, "activate");
				
				else %obj.playThread(3, "activate2");
			}
			
			if(isObject(%client))
			{
				%bars = "<color:ffaaaa>";
				%div = 20;
				%tens = mFloor((%obj.GauzeUse / %time) * %div);
				
				for(%a = 0; %a < %div; %a++)
				{
					if(%a == (%div - %tens)) %bars = %bars @ "<color:aaffaa>";
					
					%bars = %bars @ "|";
				}
				
				commandToClient(%client, 'centerPrint', %bars, 0.25);
			}
			cancel(%obj.GauzeSched);
			%obj.GauzeSched = %this.schedule(100, "healLoop", %obj);
			
		}
	}
	else
	{
		if(isObject(%client))
		{
			commandToClient(%client, 'centerPrint', "<color:ffaaaa>Heal Aborted!", 1);
			%client.setControlObject(%obj);
			%client.player.playAudio(1,"heal_stop_sound");
			%obj.setMaxForwardSpeed(%obj.getDatablock().maxForwardSpeed);
		}
		cancel(%obj.GauzeSched);
	}
}

function GauzeImage::onMount(%this, %obj, %slot)
{
	parent::onMount(%this, %obj, %slot);
	
	%obj.GauzeUse = 0;
}

function GauzeImage::onReady(%this, %obj, %slot)
{
	%obj.GauzeUse = 0;
}

function GauzeImage::onUnMount(%this, %obj, %slot)
{
	%obj.playThread(0, "root");
	parent::onUnMount(%this, %obj, %slot);
}

function GauzeImage::onUse(%this, %obj, %slot)
{
	%client = %obj.client;
	
	if(%obj.getDamageLevel() < 5 || %obj.getDatablock().getName() $= "DownPlayerSurvivorArmor")
	{
		if(isObject(%client))
		{
			if(%obj.getDatablock().getName() $= "DownPlayerSurvivorArmor")
			commandToClient(%client, 'centerPrint', "\c5Cannot use while down.", 1);
			else commandToClient(%client, 'centerPrint', "\c5You are not injured.", 1);
		}
	}
	else
	{
		if(isObject(%client))
		{
			%client.Gauzing = true;
			%client.zombieMedpackHelpTime = $sim::time;
		}

		%client.player.playAudio(1,"heal_gauze_bandaging_sound");
		%this.healLoop(%obj);
	}
}

function GauzeImage::onTrigger(%this, %obj, %trigger, %state)
{
	if(isObject(%client = %obj.getControllingClient()) && %client.Gauzing)
	{
		if(%trigger == 0 && !%state)
		{
			%client.player.GauzeUse = 0;
			%client.Gauzing = false;
			cancel(%client.player.GauzeSched);
			%client.setControlObject(%client.player);
			commandToClient(%client, 'centerPrint', "<color:ffaaaa>Heal Aborted!", 1);
			%obj.setMaxForwardSpeed(%obj.getDatablock().maxForwardSpeed);
			%client.player.playAudio(1,"heal_stop_sound");
		}
	}
	
	return parent::onTrigger(%this, %obj, %trigger, %state);
}