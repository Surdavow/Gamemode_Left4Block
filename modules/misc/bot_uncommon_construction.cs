return;

datablock shapeBaseImageData(ConstructionConeSpeakerImage)
{
	shapefile = "./models/conespeaker.dts";

	mountPoint = 1;
	offset = "0 0.18 0.25";
	doColorShift = false;
	className = "WeaponImage";
	armReady = false;
};

datablock PlayerData(ZombieConstructionHoleBot : CommonZombieHoleBot)
{
	shapeFile = UncommonMDts.baseShape;
	uiName = "";
	hName =  "Infected" SPC "Construction";//cannot contain spaces
	hIsInfected = 1;
	hZombieL4BType = "Normal";
	hPinCI = "";
	hBigMeleeSound = "";
};

function ZombieConstructionHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);

	if(getRandom(0,100) <= 20)
	{
		switch(getRandom(1,2))
		{
			case 1: %obj.mountImage(ConstructionConeSpeakerImage, 0);
			case 2: serverCmdUseSprayCan(%obj,getRandom(0,27));
		}
	}
}

function ZombieConstructionHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
}

function ZombieConstructionHoleBot::onDamage(%this,%obj)
{
	CommonZombieHoleBot::OnDamage(%this,%obj);
}

function ZombieConstructionHoleBot::onDisabled(%this,%obj)
{
	CommonZombieHoleBot::OnDisabled(%this,%obj);
	if(isObject(%obj.light))
	%obj.light.delete();
}

function ZombieConstructionHoleBot::onBotLoop(%this,%obj)
{
	CommonZombieHoleBot::onBotLoop(%this,%obj);	
}

function ZombieConstructionHoleBot::onBotFollow( %this, %obj, %targ )
{
	CommonZombieHoleBot::onBotFollow( %this, %obj, %targ );

	if(isObject(%obj.getmountedImage(0)) && %obj.getmountedImage(0).getName() $= "ConstructionConeSpeakerImage")
	{
		%obj.playThread(3, leftrecoil);
		%obj.playAudio(0, "zombiemale_attackcone" @ getRandom(1,2) @ "_sound");
		L4B_CommonZombDistraction(%obj);
	}
}

function ZombieConstructionHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
}

function ZombieConstructionHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{	
	%clothesrandmultiplier = getrandom(75,200)*0.01;
	%hatColor = getRandomBotRGBColor();
	%packColor = getRandomBotRGBColor();
	%pack2Color = getRandomBotRGBColor();
	%accentColor = getRandomBotRGBColor();
	%shirtColor = 0.108411*%clothesrandmultiplier SPC 0.258824*%clothesrandmultiplier SPC 0.556075*%clothesrandmultiplier SPC 1;
	%pantsColor = 0 SPC 0.141176*%clothesrandmultiplier SPC 0.333333*%clothesrandmultiplier SPC 1;
	%shoeColor = getRandomBotPantsColor();

	%larmColor = %shirtColor;
	%rarmColor = %shirtColor;
	%rLegColor = %shoeColor;
	%lLegColor = %shoeColor;
	%handColor = %skinColor;
	
	if(getRandom(1,3) == 1)
	{
		if(getRandom(1,2) == 1) %larmColor = %skinColor;
		if(getRandom(1,2) == 1) %rarmColor = %skinColor;
		if(getRandom(1,2) == 1) %rLegColor = %skinColor;
		if(getRandom(1,2) == 1) %lLegColor = %skinColor;
	}

	%decal = "worm_engineer";
	%hat = 0;
	%pack = 0;
	%pack2 = 0;
	%accent = 0;
	
	// accent
	%obj.accentColor = %accentColor;
	%obj.accent =  %accent;
	
	// hat
	%obj.hatColor = %hatColor;
	%obj.hat = %hat;
	
	// head
	%obj.headColor = %skinColor;
	%obj.faceName = %face;
	
	// chest
	%obj.chest =  "0";

	%obj.decalName = %decal;
	%obj.chestColor = %shirtColor;
		
	// packs
	%obj.pack =  %pack;
	%obj.packColor =  %packColor;

	%obj.secondPack =  %pack2;
	%obj.secondPackColor =  %packColor;
		
	// left arm
	%obj.larm =  "0";
	%obj.larmColor = %larmColor;
	
	%obj.lhand =  "0";
	%obj.lhandColor = %handColor;
	
	// right arm
	%obj.rarm =  "0";
	%obj.rarmColor = %rarmColor;
	
	%obj.rhandColor = %handColor;
	%obj.rhand =  "0";
	
	// hip
	%obj.hip =  "0";
	%obj.hipColor = %pantsColor;
	
	// left leg
	%obj.lleg =  "0";
	%obj.llegColor = %llegColor;
	
	// right leg
	%obj.rleg =  "0";
	%obj.rlegColor = %rlegColor;

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

function ZombieConstructionHoleBot::L4BAppearance(%this,%client,%obj)
{
	%obj.hideNode("ALL");
	%obj.setHeadUp(0);	

	%obj.unHideNode("chest");
	%obj.unHideNode("rhand");
	%obj.unHideNode("lhand");
	%obj.unHideNode(("rarm"));
	%obj.unHideNode(("larm"));
	%obj.unHideNode("headskin");
	%obj.unHideNode("pants");
	%obj.unHideNode("rshoe");
	%obj.unHideNode("lshoe");
	%obj.unhidenode("gloweyes");
	%obj.unhidenode("ConstructionHelmetBuds");
	%obj.unhidenode("ConstructionHelmet");
	%obj.unhidenode("ConstructionVest");
	
	%obj.setnodecolor("ConstructionHelmet","0.8 0.8 0.1 1");	
	%obj.setnodecolor("ConstructionVest","0.8 0.8 0.1 1");
	%obj.setNodeColor("chest",%client.chestColor);
	%obj.setnodeColor("gloweyes","1 1 0 1");
	%obj.setFaceName("asciiTerror");
	%obj.setDecalName(%client.decalName);
	%obj.setNodeColor("headskin",%client.headColor);
	%obj.setNodeColor("pants",%client.hipColor);
	%obj.setNodeColor("rarm",%client.rarmColor);
	%obj.setNodeColor("larm",%client.larmColor);
	%obj.setNodeColor("rhand",%client.rhandColor);
	%obj.setNodeColor("lhand",%client.lhandColor);
	%obj.setNodeColor("rshoe",%client.rlegColor);
	%obj.setNodeColor("lshoe",%client.llegColor);
	%obj.setNodeColor("pants",%client.hipColor);
}