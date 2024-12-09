return;

$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount = 0] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0.000 0.200 0.640 0.700";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0.9 0.9 0.9 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "AAA-None";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "smiley";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "1 1 0 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "1 0.878 0.611 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0 0 1 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0.9 0 0 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "1 0.878 0.611 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0 0 1 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0 0.435 0.831 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0.9 0 0 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "1 0.878 0.611 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0 0 1 1";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0";
$L4B_ReferenceAppearance[$L4B_ReferenceAppearanceCount++] = "0 1 0 1";
$L4B_ReferenceAppearanceTags = "accent accentColor chest chestColor decalName faceName hat hatColor headColor hip hipColor larm larmColor lhand lhandColor lleg llegColor pack packColor rarm rarmColor rhand rhandColor rleg rlegColor secondPack secondPackColor";
$L4B_ClientStatsTags = "name score";
$L4B_AppearanceZombieTags = "chestColor rArmColor lArmColor rhandColor lhandColor hipColor rlegColor llegColor";

function L4B_storeClientData(%client)
{
	if(!isObject(%client) || %client.getClassName() !$= "GameConnection") return;
	
	%appearanceTagCount = getWordCount($L4B_ReferenceAppearanceTags);
	%statsTagCount = getWordCount($L4B_ClientStatsTags);

	for(%i = 0; %i < %appearanceTagCount; %i++) 
	if(%client.getFieldValue(getWord($L4B_ReferenceAppearanceTags,%i)) $= $L4B_ReferenceAppearance[%i]) 
	{
		%defaultAppearanceCount++;
		if(%defaultAppearanceCount == $L4B_ReferenceAppearanceCount)
		{
			%noAppearanceLogging = true;
			break;
		}
	}

	echo("Storing" SPC %client.name @ "'s stats...");	
	%file = new fileObject();
	%file.openForWrite("config/server/Left4Block/loggedplayers/" @ %client.BL_ID @ ".txt");

	for(%i = 0; %i < %statsTagCount; %i++)
	{
		%statsReference = getWord($L4B_ClientStatsTags,%i);
		%file.writeLine(%statsReference SPC %client.getFieldValue(%statsReference));
	}

	if(!%noAppearanceLogging)
	for(%i = 0; %i < %appearanceTagCount; %i++)
	{
		%appearanceReference = getWord($L4B_ReferenceAppearanceTags,%i);
		%file.writeLine(%appearanceReference SPC %client.getFieldValue(%appearanceReference));
	}

	%file.close();
	%file.delete();		
	//L4B_loadClientSnapshots();
}

function L4B_loadClientData(%client)
{		
	//if(isObject($L4B_loggedClients)) $L4B_loggedClients.delete();
	//$L4B_loggedClients = new ScriptGroup();
//
	//%appearancepath = "config/server/Left4Block/loggedplayers/*.txt";	
	//for(%appearancefile = findFirstFile(%appearancepath); isFile(%appearancefile); %appearancefile = findNextFile(%appearancepath))
	//{
	//	%fileid = strreplace(filename(%appearancefile), ".txt", "");		
	//	%file = new fileObject();
	//	%file.openForRead(%appearancefile);
	//	%loggedclient = new ScriptObject();	
//
	//	for(%apc = 0; %apc < getWordCount($L4B_ReferenceAppearanceTags); %apc++)
	//	%loggedclient.setField(getWord($L4B_ReferenceAppearanceTags,%apc),%file.readLine());
//
	//	$L4B_loggedClients.add(%loggedclient);
	//	%file.close();
	//	%file.delete();
	//}
//
	//if($L4B_loggedClients.getCount()) echo("Loading" SPC $L4B_loggedClients.getCount() SPC "client's appearances...");
}

function L4B_pushClientSnapshot(%obj,%sourceClient,%zombify)
{
	if(!isObject(%sourceClient) || %sourceClient.getClassName() !$= "GameConnection")
	{
		if(isObject(ClientGroup) && ClientGroup.getCount()) for(%i = 0; %i < ClientGroup.getCount(); %i++) 
		if(isObject(%client = ClientGroup.getObject(%i))) %clientlist[%cl++] = %client;
		
		if(isObject($L4B_loggedClients) && $L4B_loggedClients.getCount()) for(%i = 0; %i < $L4B_loggedClients.getCount(); %i++) 
		if(isObject(%loggedclient = $L4B_loggedClients.getObject(%i))) %clientlist[%cl++] = %loggedclient;

		if(!%cl) return false;
		%sourceClient = %clientlist[(getRandom(1,%cl))];
	}

	%skin = %sourceClient.headColor;
	%zskin = getWord(%skin,0)/2.75 SPC getWord(%skin,1)/1.5 SPC getWord(%skin,2)/2.75 SPC 1;

	%obj.headColor = %zskin;
	%obj.chestColor = %sourceClient.chestColor;
	%obj.hipColor = %sourceClient.hipColor;
	%obj.rArmColor = %sourceClient.rarmColor;
	%obj.lArmColor = %sourceClient.larmColor;
	%obj.rhandColor = %sourceClient.rhandColor;
	%obj.lhandColor = %sourceClient.lhandColor;
	%obj.rlegColor = %sourceClient.rlegColor;
	%obj.llegColor = %sourceClient.llegColor;
	%obj.stolenname = %sourceClient.name;
	
	if(%zombify)
	{
		if(%sourceClient.chestColor $= %skin) %obj.chestColor = %zskin;
		if(%sourceClient.rArmColor $= %skin) %obj.rArmColor = %zskin;
		if(%sourceClient.lArmColor $= %skin) %obj.lArmColor = %zskin;
		if(%sourceClient.rhandColor $= %skin) %obj.rhandColor = %zskin;
		if(%sourceClient.lhandColor $= %skin) %obj.lhandColor = %zskin;
		if(%sourceClient.hipColor $= %skin)	%obj.hipColor = %zskin;
		if(%sourceClient.rLegColor $= %skin) %obj.rlegColor = %zskin;
		if(%sourceClient.lLegColor $= %skin) %obj.llegColor = %zskin;
	}
    
	%obj.secondPackColor = %sourceClient.secondPackColor;
	%obj.lhand = %sourceClient.lhand;
	%obj.hip = %sourceClient.hip;
	%obj.faceName = "asciiterror";
	%obj.hatColor = %sourceClient.hatColor;
	%obj.chest = %sourceClient.chest;
	%obj.rarm = %sourceClient.rarm;
	%obj.packColor = %sourceClient.packColor;
	%obj.pack = %sourceClient.pack;
	%obj.decalName = %sourceClient.decalName;
	%obj.secondPack = %sourceClient.secondPack;
	%obj.larm = %sourceClient.larm;
	%obj.accentColor = %sourceClient.accentColor;
	%obj.rleg = %sourceClient.rleg;
	%obj.accent = %sourceClient.accent;
	%obj.rhand = %sourceClient.rhand;
	%obj.lleg = %sourceClient.lleg;
	%obj.hat = %sourceClient.hat;

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

package L4B_loggedClientsger
{
	function GameConnection::onClientEnterGame(%client)
	{
		parent::onClientEnterGame(%client);
		L4B_loadClientData(%client);
		L4B_storeClientData(%client);
	}

	function GameConnection::onClientLeaveGame(%client)
	{
		parent::onClientLeaveGame(%client);
		L4B_storeClientData(%client);		
	}
};
activatePackage(L4B_loggedClientsger);