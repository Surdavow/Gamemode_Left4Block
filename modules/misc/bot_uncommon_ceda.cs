return;

datablock TSShapeConstructor(UncommonMDts) 
{
	baseShape = "./models/zombie_uncommon.dts";
	sequence0 = "./models/default_old.dsq";
	sequence1 = "./models/zombie.dsq";
};

datablock PlayerData(ZombieCedaHoleBot : CommonZombieHoleBot)
{
	uiName = "";
	maxdamage = 35;//Health
	hName = "Infected" SPC "CEDA";//cannot contain spaces
	noBurning = 1;
	hIsInfected = 2;
	hZombieL4BType = "Normal";
	hPinCI = "";
	hBigMeleeSound = "";
};

function ZombieCedaHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);

	if(getRandom(0,100) <= 20)
	{
		%obj.mountImage(BileBombImage, 0);
		%obj.playthread(1,"armReady");
	}
}

function ZombieCedaHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
}

function ZombieCedaHoleBot::onDamage(%this,%obj)
{
	if(%obj.getstate() !$= "Dead" && %obj.lastdamage+500 < getsimtime())//Check if the chest is the female variant and add a 1 second cooldown
	{
		if(%obj.raisearms)
		{
			%obj.raisearms = 0;
			%obj.playthread(1,"root");
			%obj.playthread(2,"plant");
		}

		switch(%obj.chest)
		{
			case 0: %obj.playaudio(0,"zombiemale_ceda_pain" @ getrandom(1,3) @ "_sound");
			case 1: %obj.playaudio(0,"zombiefemale_ceda_pain" @ getrandom(1,3) @ "_sound");
		}

		%obj.lastdamage = getsimtime();	
	}

	Parent::OnDamage(%this,%obj);
}

function ZombieCedaHoleBot::onDisabled(%this,%obj)
{
	if(%obj.getstate() !$= "Dead")
	return;
	
	if(isObject(%weapon = %obj.getMountedImage(0)))
	{
		L4B_ZombieDropLoot(%obj,%weapon.item,100);
		%obj.unMountImage(0);
	}

    switch(%obj.chest)
	{
		case 0: %obj.playaudio(0,"zombiemale_ceda_death" @ getrandom(1,3) @ "_sound");
		case 1: %obj.playaudio(0,"zombiefemale_ceda_death" @ getrandom(1,3) @ "_sound");
	}

	%obj.playaudio(1,"ceda_suit_deflate" @ getrandom(1,3) @ "_sound");

	Parent::OnDisabled(%this,%obj);
}

function ZombieCedaHoleBot::onBotLoop(%this,%obj)
{
	CommonZombieHoleBot::onBotLoop(%this,%obj);	
}

function ZombieCedaHoleBot::onBotFollow( %this, %obj, %targ )
{
	if(!isObject(%obj) || %obj.getstate() $= "Dead")
	return;
	
	if(obj.lastsaw+getRandom(1000,4000) < getsimtime())
	{
		if(!%obj.raisearms)
		{	
			%obj.playthread(1,"armReadyboth");
			%obj.raisearms = 1;
			%obj.setMaxForwardSpeed(10);
		}	

		%obj.lastsaw = getsimtime();
		%obj.playthread(2,plant);

    	switch(%obj.chest)
		{
			case 0: %obj.playaudio(0,"zombiemale_ceda_attack" @ getrandom(1,4) @ "_sound");
			case 1: %obj.playaudio(0,"zombiefemale_ceda_attack" @ getrandom(1,4) @ "_sound");
		}
	}
}

function ZombieCedaHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
}

function ZombieCedaHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{
	//Apppearance Zombie
	%clothesrandmultiplier = getrandom(3,6)*0.15;
	%suitcolor = 0.9*%clothesrandmultiplier SPC 0.9*%clothesrandmultiplier SPC 0.454545*%clothesrandmultiplier SPC 1;
	%suitcolor3 = "0.0784314 0.0784314 0.0784314 1";
	%accentColor =  "0.1 0.1 0.1 0.95";
	%decal = "AAA-None";
	%handColor = %suitcolor3;
	%hatColor = %suitcolor;
	%packColor = %suitcolor;
	%shirtColor = %suitcolor;
	%pantsColor = %suitcolor;
	%shoeColor = %suitcolor3;
	%hat = 1;
	%pack = 6;
	%pack2 = 0;
	%accent = 1;

	// accent
	%obj.accentColor = %accentColor;
	%obj.accent =  %accent;
	
	// hat
	%obj.hatColor = %hatColor;
	%obj.hat = %hat;
	
	// head
	%obj.headColor = %suitcolor3;
	%obj.faceName = %face;
	
	// chest
	%obj.chest =  %chest;

	%obj.decalName = %decal;
	%obj.chestColor = %shirtColor;
		
	// packs
	%obj.pack =  %pack;
	%obj.packColor =  %packColor;

	%obj.secondPack =  %pack2;
	%obj.secondPackColor =  %packColor;
		
	// left arm
	%obj.larm =  "0";
	%obj.larmColor = %shirtColor;
	
	%obj.lhand =  "0";
	%obj.lhandColor = %handColor;
	
	// right arm
	%obj.rarm =  "0";
	%obj.rarmColor = %shirtColor;
	
	%obj.rhandColor = %handColor;
	%obj.rhand =  "0";
	
	// hip
	%obj.hip =  "0";
	%obj.hipColor = %pantsColor;
	
	// left leg
	%obj.lleg =  "0";
	%obj.llegColor = %shoeColor;
	
	// right leg
	%obj.rleg =  "0";
	%obj.rlegColor = %shoeColor;

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}