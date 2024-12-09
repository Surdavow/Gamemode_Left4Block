$Defibrillator::ChargeTime = 3.4; //Requires restart.
addDamageType("Defibrillator", '<bitmap:Add-Ons/Gamemode_left4block/modules/misc/icons/ci/CI_Defib> %1', '%2 <bitmap:Add-Ons/Gamemode_left4block/modules/misc/icons/ci/CI_Defib> %1', 0.5, 1);

datablock ProjectileData(DefibrillatorProjectile : radioWaveProjectile)
{
	directDamage = 125;
	directDamageType = $DamageType::Defibrillator;
	radiusDamageType = $DamageType::Defibrillator;

	collideWithPlayers = true;

	lifetime = 100;

	uiName = "";
};

datablock ItemData(DefibrillatorItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./models/Defib_Right.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Defibrillator";
	iconName = "./icons/icon_Defib";
	doColorShift = false;

	image = DefibrillatorImage;
	canDrop = true;
};

datablock ShapeBaseImageData(DefibrillatorImage)
{
	shapeFile = "./models/Defib_Right.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0.05 0.05";
	eyeOffset = 0;
	rotation = "1 0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";

	item = DefibrillatorItem;
	ammo = " ";
	projectile = DefibrillatorProjectile;
	projectileType = Projectile;

	melee = false;
	armReady = true;

	doColorShift = false;

	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.5;
	stateTransitionOnTimeout[0]	= "Ready";
	stateSequence[0]		= "ready";
	stateSound[0]			= "weaponSwitchSound";

	stateName[1]			= "Ready";
	stateTransitionOnTriggerDown[1]	= "Charge";
	stateAllowImageChange[1]	= true;
	
	stateName[2]                    = "Charge";
	stateTransitionOnTimeout[2]	= "Charge2";
	stateTimeoutValue[2]            = $Defibrillator::ChargeTime / 2;
	stateWaitForTimeout[2]		= false;
	stateTransitionOnTriggerUp[2]	= "AbortCharge";
	//stateScript[2]                  = "onCharge";
	stateAllowImageChange[2]        = false;
	stateSound[2]			= "defib_ready_sound";
	
	stateName[3]			= "AbortCharge";
	stateTransitionOnTimeout[3]	= "Ready";
	stateTimeoutValue[3]		= 0.5;
	stateWaitForTimeout[3]		= true;
	//stateScript[3]			= "onAbortedCharge";
	stateAllowImageChange[3]	= false;

	stateName[4]			= "Charge2";
	stateTimeoutValue[4]		= $Defibrillator::ChargeTime / 2;
	stateTransitionOnTimeout[4]	= "Fire";
	stateTransitionOnTriggerUp[4]	= "AbortCharge";
	//stateScript[4]		= "onCharge2";
	stateAllowImageChange[4]	= false;
	stateSound[4]			= "defib_charge_sound";

	stateName[5]			= "Fire";
	stateTransitionOnTimeout[5]	= "Ready";
	stateTimeoutValue[5]		= 0.5;
	stateFire[5]			= true;
	stateSequence[5]		= "fire";
	stateScript[5]			= "onFire";
	stateWaitForTimeout[5]		= true;
	stateAllowImageChange[5]	= false;
	stateSound[5]			= "defib_fire_sound";
};

datablock ShapeBaseImageData(LeftHandDefibrillatorImage : DefibrillatorImage)
{
	shapeFile = "./models/Defib_Left.dts";

	mountPoint = 1;

	armReady = false;

	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= DefibrillatorImage.stateTimeoutValue[0];
	stateTransitionOnTimeout[0]	= "Ready";
	stateSequence[0]		= "ready";

	stateName[1]			= "Ready";
	stateTransitionOnTriggerDown[1]	= "Charge";
	stateAllowImageChange[1]	= true;
	
	stateName[2]                    = "Charge";
	stateTransitionOnTimeout[2]	= "Charge2";
	stateTimeoutValue[2]            = DefibrillatorImage.stateTimeoutValue[2];
	stateWaitForTimeout[2]		= false;
	stateTransitionOnTriggerUp[2]	= "AbortCharge";
	stateScript[2]                  = "onCharge";
	stateAllowImageChange[2]        = false;
	
	stateName[3]			= "AbortCharge";
	stateTransitionOnTimeout[3]	= "Ready";
	stateTimeoutValue[3]		= DefibrillatorImage.stateTimeoutValue[3];
	stateWaitForTimeout[3]		= true;
	stateScript[3]			= "onAbortedCharge";
	stateAllowImageChange[3]	= false;

	stateName[4]			= "Charge2";
	stateTimeoutValue[4]		= DefibrillatorImage.stateTimeoutValue[4];
	stateTransitionOnTimeout[4]	= "Fire";
	stateTransitionOnTriggerUp[4]	= "AbortCharge";
	stateScript[4]			= "onCharge2";
	stateAllowImageChange[4]	= false;

	stateName[5]			= "Fire";
	stateTransitionOnTimeout[5]	= "Ready";
	stateTimeoutValue[5]		= 1.25;
	stateFire[5]			= true;
	stateSequence[5]		= "fire";
	stateScript[5]			= "onFire";
	stateWaitForTimeout[5]		= true;
	stateAllowImageChange[5]	= false;
};


function DefibrillatorImage::onFire(%data, %obj, %slot)
{
	parent::onFire(%data, %obj, %slot);

	%obj.playThread(0, plant);
	%obj.playThread(2, shiftDown);
}

function LeftHandDefibrillatorImage::onFire(%data, %obj, %slot)
{
	parent::onFire(%data, %obj, %slot);
}

function DefibrillatorImage::onMount(%data, %obj, %slot)
{
	parent::onMount(%data, %obj, %slot);

	%obj.mountImage(LeftHandDefibrillatorImage, 1);
	%obj.playThread(1, armReadyBoth);
}

function DefibrillatorImage::onUnMount(%data, %obj, %slot)
{
	parent::onUnMount(%data, %obj, %slot);

	if(isObject(%obj.getMountedImage(LeftHandDefibrillatorImage.mountPoint)) && %obj.getMountedImage(LeftHandDefibrillatorImage.mountPoint).getName() $= "LeftHandDefibrillatorImage")
	%obj.unMountImage(LeftHandDefibrillatorImage.mountPoint);
	
}

function LeftHandDefibrillatorImage::onUnMount(%data, %obj, %slot)
{
	parent::onUnMount(%data, %obj, %slot);
}

function DefibrillatorProjectile::onCollision(%data, %proj, %col, %fade, %pos, %norm)
{
	parent::onCollision(%data, %proj, %col, %fade, %pos, %norm);

	%client = %proj.client;

	if(!isObject(%client) && isObject(%proj.sourceObject))
	{
		if(%proj.sourceObject.getClassName() $= "Player") %client = %proj.sourceObject.client;
		else if(%proj.sourceObject.getClassName() $= "gameConnection") %client = %proj.sourceObject;		
	}

	if(!isObject(%client) || !isObject(%client.player)) return;
	

	initContainerRadiusSearch(%pos, 0.1, $TypeMasks::CorpseObjectType);

	if(!isObject(%body = containerSearchNext()) || %body.getClassName() !$= "Player" || !isObject(%bodyClient = %body.prevClient) || isObject(%client.slyrTeam) || isObject(%bodyClient.slyrTeam) && %client.slyrTeam != %bodyClient.slyrTeam)
	return;	

	%count = %body.getDataBlock().maxTools;
	for(%i = 0; %i < %count; %i++) %tool[%i] = %body.tool[%i];
	

	%bodyCurrTool = %body.currTool;
	%bodyData = %body.getDataBlock();
	%bodyPos = %body.getTransform();
	%bodyClient.spawnPlayer(); //The body will disppear here, so we need to save some info before we do this.
	%bodyClient.player.setDataBlock(%bodyData);
	%bodyClient.player.setTransform(%bodyPos);
	%bodyClient.player.emote(HealImage, 1);

	%bodyClient.player.clearTools();
	%count = %bodyClient.player.getDataBlock().maxTools;

	for(%i = 0; %i < %count; %i++)
	{
		%bodyClient.player.tool[%i] = %tool[%i];
		messageClient(%bodyClient, 'MsgItemPickup', "", %i, %tool[%i], 1);

		if(isObject(%tool[%i]) && %tool[%i].className $= "Weapon") %bodyClient.player.weaponCount++;
	}

	%bodyClient.player.currTool = %bodyCurrTool;
	%bodyClient.player.updateArm(%bodyClient.player.tool[%bodyCurrTool].image);
	%bodyClient.player.mountImage(%bodyClient.player.tool[%bodyCurrTool].image, %bodyClient.player.tool[%bodyCurrTool].image.mountPoint);
	schedule(500, %client.player, eval, %client.player @ ".tool[" @ %client.player.currTool @ "] = 0;");
	schedule(500, %client.player, messageClient, %client, 'MsgItemPickup', "", %client.player.currTool, 0, 1);
	schedule(500, %client.player, serverCmdUnUseTool, %client);
	
}

function DefibrillatorProjectile::damage(%data, %player, %col, %fade, %pos, %norm)
{
	Parent::damage(%data, %player, %col, %fade, %pos, %norm);
}

package Defibrillator
{
	function player::removeBody(%player)
	{
		if(%player.getClassName() $= "AIPlayer" || $ConfirmRemoveBody) //Using a global variable for this because we can't rely on others who package this function to parent an extra variable for no reason.
		{
			$ConfirmRemoveBody = false;
			Parent::removeBody(%player);
		}
	}

	function gameConnection::onDeath(%client, %sourcePlayer, %sourceClient, %damageType, %damageArea)
	{
		%client.corpse = %client.player;
		%client.player.prevClient = %client;

		parent::onDeath(%client, %sourcePlayer, %sourceClient, %damageType, %damageArea);

		if(isObject(%client.minigame))
		{
			if(isObject(%client.slyrTeam) && %client.slyrTeam.respawnTime_player >= 0)
			{
				%time = %client.slyrTeam.respawnTime_player < 1 ? 1 : %client.slyrTeam.respawnTime_player;
				%time *= 1000;
			}

			else
			{
				%time = %client.minigame.respawnTime_player < 1 ? 1 : %client.minigame.respawnTime_player;
				%time *= 1000;
			}
		}

		else
		{
			%time = $Game::MinRespawnTime;
		}
	}

	function MiniGameSO::removeMember(%mini, %client)
	{
		if(isObject(%client) && %mini.isMember(%client) && isObject(%client.corpse))
		{
			$ConfirmRemoveBody = true;
			%client.corpse.removeBody();
		}

		parent::removeMember(%mini, %client);
	}

	function gameConnection::spawnPlayer(%client)
	{
		if(isObject(%client.corpse))
		{
			%client.corpse.delete();
		}

		parent::spawnPlayer(%client);
	}

	function gameConnection::onClientLeaveGame(%client)
	{
		if(isObject(%client.corpse))
		{
			$ConfirmRemoveBody = true;
			%client.corpse.removeBody();
		}

		parent::onClientLeaveGame(%client);
	}
};
activatePackage(Defibrillator);	