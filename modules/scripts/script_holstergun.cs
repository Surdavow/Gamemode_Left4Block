//Function to check if an item is currently being holstered, if it is then visibly holster it on the player
//Edited by Davow (46492) but credits go to aebaadcode and MonoBlaster... I think?

//This will create new datablocks, there will be a server preference whether or not to enable or disable this (Blockland sucks monkey nuts)

function ItemHolsterPlayer::UpdateImages(%this,%obj,%player)
{	
	if(!isObject(%obj) || !isObject(%player))
	{
		if(isObject(%obj)) %obj.delete();
		return;
	}
	
	for(%i = 0; %i < %player.getDataBlock().maxTools; %i++)//Primary
	{ 	
		if(%player.tool[%i].L4Bitemslot $= "Primary")
		{			
			if(%player.CurrTool == %i) 
			{
				%obj.unmountImage(0);
				break;
			}

			%obj.mountImage(%player.tool[%i].image,0);
			break;
		}
	}

	for(%i = 0; %i < %player.getDataBlock().maxTools; %i++)//Secondary
	{
		if(%player.tool[%i].L4Bitemslot $= "Secondary")
		{
			if(%player.CurrTool == %i) 
			{
				%obj.unmountImage(1);
				break;
			}			
			
			%obj.mountImage(%player.tool[%i].image.getName() @ "GunImagesSIDE",1);
			break;			
		}		
	}

	for(%i = 0; %i < %player.getDataBlock().maxTools; %i++)//Grenade
	{
		if(%player.tool[%i].L4Bitemslot $= "Grenade")
		{
			if(%player.CurrTool == %i) 
			{
				%obj.unmountImage(2);
				break;
			}			
			
			%obj.mountImage(%player.tool[%i].image.getName() @ "GunImagesSIDE",2);
			break;			
		}		
	}	
}

function ItemHolsterPlayer::onAdd(%this,%obj)
{	
	if(!isObject(%mount = %obj.mount))
	{
		%obj.delete();
		return;
	}
	
	%mount.mountObject(%obj,7);
	%obj.setDamageLevel(%this.maxDamage);
	%this.UpdateImages(%obj,%mount);
}

function ItemHolsterPlayer::doDismount(%this, %obj)//Sometimes the game does some weird mumbo jumbo so I'm adding this just in case
{
	return;
}
function ItemHolsterPlayer::onDisabled(%this, %obj) 
{
	return;
}

package L4B_HolsterItems
{
	function Armor::onNewDatablock(%db,%obj)
	{
		Parent::onNewDatablock(%db,%obj);
		
		if(%obj.getdataBlock().isSurvivor && !isObject(%obj.HolsterMountBot))//Only if the player is a survivor, I don't want there to be so many unnecessary bots, and it works better with the survivor player
		%obj.HolsterMountBot = new AiPlayer()
		{
			dataBlock = ItemHolsterPlayer;
			mount = %obj;
		};
	}

	function Armor::OnRemove(%db,%obj) //Kill the bot if the object gets removed
	{		
		Parent::OnRemove(%db,%obj);
		if(isObject(%obj.HolsterMountBot)) %obj.HolsterMountBot.delete();
	}	
	
//The rest of these are generally the same thing, they constantly check the inventory of the player based on each action, which is what the function above is about

	function Player::Pickup(%obj,%item)
	{		
		Parent::Pickup(%obj,%item);

		if(isObject(%holstermountbot = %obj.HolsterMountBot)) %holstermountbot.getDataBlock().UpdateImages(%holstermountbot,%obj);
	}
	
	function ServerCmdUseTool(%client,%slot)
	{
		Parent::ServerCmdUseTool(%client, %slot);

		if(isObject(%player = %client.player) && isObject(%holstermountbot = %player.HolsterMountBot)) 
		%holstermountbot.getDataBlock().UpdateImages(%holstermountbot,%player);
	}

	function ServerCmdUnUseTool(%client)
	{		
		Parent::ServerCmdUnUseTool(%client);

		if(isObject(%player = %client.player) && isObject(%holstermountbot = %player.HolsterMountBot)) 
		%holstermountbot.getDataBlock().UpdateImages(%holstermountbot,%player);
	}

	function ServerCmdDropTool(%client,%position)
	{
		Parent::ServerCmdDropTool(%client, %position);

		if(isObject(%player = %client.player) && isObject(%holstermountbot = %player.HolsterMountBot)) 
		%holstermountbot.getDataBlock().UpdateImages(%holstermountbot,%player);
	}
};
activatePackage("L4B_HolsterItems");