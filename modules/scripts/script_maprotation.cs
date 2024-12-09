return;

function BuildMapLists()
{
	%mapdir = "Add-Ons/MapCycle_*/*.bls";
	
	$maprot::numMap = 0;
	
	%file = findFirstFile(%mapdir);
	while(%file !$= "")
	{
		$maprot::map[$maprot::numMap] = %file;
		$maprot::numMap++;

		%file = findNextFile(%mapdir);
	}
	messageAll('', "\c3" @ ($maprot::numMap) SPC "\c6maps loaded.");
}

function serverCmdvoteNext(%client) {
	if(!$Pref::L4B:MapRotation::Enabled) { return; }
	if(%client.isAdmin+%client.isSuperAdmin+%client.isHost<$Pref::L4B:MapRotation::RequiredVotes) { return; }
	%diff = $maprot::cooldown - ($Sim::Time - %client.lastCommandTime);
	if($Sim::Time - %client.lastCommandTime < $maprot::cooldown) { return messageClient(%client, '', "\c6You need to wait\c3" SPC %diff SPC "\c6seconds to use another command."); }
	if(%client.hasVoted) {		
		messageClient(%client, '', "\c6You have cancelled your vote for the next map.");
		%client.hasVoted = false;
		%client.lastCommandTime = $Sim::Time;
		return;
	}
	%client.hasVoted = true;
	
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		%users = ClientGroup.getObject(%a);
		if(%users.hasVoted) { %votecount++; }
	}
	%needed = $maprot::votemin-%votecount;
	if(%votecount >= $maprot::votemin) {
		%msg = "<font:arial:26><color:FFFF00>Map Rotator\c6 - Next map vote has passed. Loading next map...";
		nextMap(%msg);
	} else {
		messageClient(%client, '', "\c6You have voted for the next map. We need\c3" SPC %needed SPC "\c6more votes to load the next map.");
	}
	%client.lastCommandTime = $Sim::Time;
}

function serverCmdnextMap(%client) {
	if(!$Pref::L4B:MapRotation::Enabled) { return; }
	if(%client.isAdmin+%client.isSuperAdmin+%client.isHost<$Pref::L4B:MapRotation::RequiredVotes) { return; }
	%diff = $maprot::cooldown - ($Sim::Time - %client.lastCommandTime);
	if($Sim::Time - %client.lastCommandTime < $maprot::cooldown) { return messageClient(%client, '', "\c6You need to wait\c3" SPC %diff SPC "\c6seconds to use another command."); }
	%client.lastCommandTime = $Sim::Time;
	%msg = "<font:arial:26><color:FFFF00>Map Rotator\c6 -\c3" SPC %client.name SPC "\c6has called the next map. Loading now...";
	nextMap(%msg);
}

function serverCmdSetMap(%client, %i)
{
   if(%client.isAdmin+%client.isSuperAdmin+%client.isHost<$Pref::L4B:MapRotation::RequiredVotes)
      return;

   if(mFloor(%i) !$= %i)
   {
      messageClient(%client, '', "Usage: /setMap <number>");
      return;
   }

   if(%i < 0)
   {
      messageClient(%client, '', "serverCmdSetMap() - out of range index");
      return;
   }

   messageAll( 'MsgAdminForce', '\c3%1\c2 changed the map', %client.getPlayerName());
   
   $maprot::current = %i;
   nextMap(%msg);
}

function serverCmdmapList(%client) {
	if(!$Pref::L4B:MapRotation::Enabled) { return; }
	messageClient(%client, '', "\c3BI0Hazzard's \c6Map \c3Rotation - Map List");
	for(%a = 0; %a < $maprot::numMap; %a++)
	{
		%mapname = $maprot::map[%a];
		%mapname = strReplace(%mapname, "Add-Ons/MapCycle_", "");
		%mapname = strReplace(%mapname, "/save.bls", "");
		%mapname = strReplace(%mapname, "_", " ");
		messageClient(%client, '', " \c3" @ (%a+1) SPC "\c6-" SPC %mapname);
	}
}

function serverCmdmapHelp(%client) {
	if(!$Pref::L4B:MapRotation::Enabled) { return; }
	%cmd = 0;
	messageClient(%client, '', "\c3BI0Hazzard's \c6Map \c3Rotation.");
		messageClient(%client, '', " \c6- \c3/mapList \c6- Shows a list of maps that the rotator uses.");
	if(%client.isAdmin+%client.isSuperAdmin+%client.isHost>=$Pref::L4B:MapRotation::RequiredVotes) {
		messageClient(%client, '', " \c6- \c3/voteNext \c6- Vote for the next map, if you've already voted it will cancel your vote if called again.");
	}
	if(%client.isAdmin+%client.isSuperAdmin+%client.isHost>=$Pref::L4B:MapRotation::RequiredNext) {
		messageClient(%client, '', " \c6- \c3/nextMap \c6- Loads the next map.");
	}
	if(%client.isAdmin+%client.isSuperAdmin+%client.isHost>=$Pref::L4B:MapRotation::RequiredReload) {
		messageClient(%client, '', " \c6- \c3/reloadMaps \c6- Reloads the servers collection of maps.");
	}
}

function servercmdReloadMaps(%client)
{
	if(%client.isAdmin+%client.isSuperAdmin+%client.isHost<$Pref::L4B:MapRotation::RequiredReload) { return; }
	
	messageAll('', "\c3" @ %client.name SPC "\c6reloaded the server maps.");
	setModPaths(getModPaths());
	BuildMapLists();
}

function nextMap(%msg) {
	$maprot::ResetCount = 0;
	%nextnum = $maprot::current+1;
	%current = $maprot::current;
	if($maprot::map[%nextnum] !$= "") {
		%filename = $maprot::map[%nextnum];
		$maprot::current++;
		messageAll('MsgUploadEnd', %msg);
	} else {
		if($maprot::current != 0) {
			messageAll('MsgUploadEnd', %msg);
		}
		%filename = $maprot::map0;
		$maprot::current = 0;
	}

	//suspend minigame resets
	$maprot::MapChange = true;

	for(%a = 0; %a < ClientGroup.getCount(); %a++)
	{
		%client = ClientGroup.getObject(%a);
		%player = %client.player;
		if(isObject(%player))
			%player.delete();

		%camera = %client.camera;
		%camera.setFlyMode();
		%camera.mode = "Observer";
		%client.setControlObject(%camera);
	}
	
	//clear all bricks 
	// note: this function is deferred, so we'll have to set a callback to be triggered when it's done
	BrickGroup_46492.chaindeletecallback = "LoadLevel(\"" @ %filename @ "\");";
	BrickGroup_46492.chaindeleteall();	
}

function LoadLevel(%filename)
{
   echo("Loading map " @ %filename);

   %displayName = %filename;
   %displayName = strReplace(%displayName, "Add-Ons/MapCycle_", "");
      %displayName = strReplace(%displayName, "_", " ");
   %displayName = strReplace(%displayName, "/save.bls", "");
   
   %loadMsg = "\c5Now loading \c6" @ %displayName;


   //read and display credits file, if it exists
   // limited to one line
   %creditsFilename = filePath(%fileName) @ "/credits.txt";
   if(isFile(%creditsFilename))
   {
      %file = new FileObject();
      %file.openforRead(%creditsFilename);

      %line = %file.readLine();
      %line = stripMLControlChars(%line);
      %loadMsg = %loadMsg @ "\c5, created by \c3" @ %line;

      %file.close();
      %file.delete();
   }

   messageAll('', %loadMsg);

 //load environment if it exists
   %envFile = filePath(%fileName) @ "/environment.txt"; 
   if(isFile(%envFile))
   {  
      //echo("parsing env file " @ %envFile);
      //usage: GameModeGuiServer::ParseGameModeFile(%filename, %append);
      //if %append == 0, all minigame variables will be cleared 
      %res = GameModeGuiServer::ParseGameModeFile(%envFile, 1);

      EnvGuiServer::getIdxFromFilenames();
      EnvGuiServer::SetSimpleMode();

      if(!$EnvGuiServer::SimpleMode)    
      {
         EnvGuiServer::fillAdvancedVarsFromSimple();
         EnvGuiServer::SetAdvancedMode();
      }
   }

	//load save file
	schedule(10, 0, serverDirectSaveFileLoad, %fileName, 1, "", 1, 1);
}
if(isPackage(mapRotatorPackage)) {
	deactivatepackage(mapRotatorPackage);
}

package mapRotatorPackage
{
	
	function gameConnection::spawnPlayer(%client)
	{
		if($maprot::MapChange) { return; }
		%client.hasVoted = false;
		Parent::spawnPlayer(%client);
	}
	function ServerLoadSaveFile_End()
	{
		$maprot::MapChange = false;
		for(%a = 0; %a < ClientGroup.getCount(); %a++)
		{
			%client = ClientGroup.getObject(%a);
			%client.spawnPlayer();
		}
		Parent::ServerLoadSaveFile_End();
	}
	function MiniGameSO::Reset(%obj, %client)
	{
		if($maprot::ResetCount > $maprot::minreset) 
		{
			%msg = "<font:arial:26><color:FFFF00>Map Rotator\c6 -\c3" SPC $maprot::minreset SPC "\c6rounds have passed, time to load another map!";
			nextMap(%msg);
		}
		
		if(ClientGroup.getCount()) $maprot::ResetCount++;
		
		Parent::Reset(%obj, %client);
	}
};
activatePackage(mapRotatorPackage);
BuildMapLists();