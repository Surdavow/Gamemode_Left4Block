datablock PlayerData(ToxiCommonZombieHoleBot : CommonZombieHoleBot)
{
	uiName = "";
	hName = "Infected" SPC "Toxic";//cannot contain spaces

	hIsInfected = 1;
	hZombieL4BType = "Normal";
	hPinCI = "";
	hBigMeleeSound = "";
};

function ToxiCommonZombieHoleBot::onAdd(%this,%obj)
{	
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

function ToxiCommonZombieHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);

	if($L4B2Bots::UncommonWarningLight)
	SpecialsWarningLight(%obj);
	%obj.isToxic = 1;
	%obj.mountImage(SpitAcidStatusPlayerImage, 3);
	%obj.setscale("1 1 1");
}

function ToxiCommonZombieHoleBot::onDamage(%this,%obj)
{
	CommonZombieHoleBot::OnDamage(%this,%obj);
	%obj.playaudio(0,"norm_cough" @ getrandom(1,3) @ "_sound");
}

function ToxiCommonZombieHoleBot::onDisabled(%this,%obj)
{
	CommonZombieHoleBot::OnDisabled(%this,%obj);
}

function ToxiCommonZombieHoleBot::onBotLoop(%this,%obj)
{
	CommonZombieHoleBot::onBotLoop(%this,%obj);
	%obj.playaudio(0,"norm_cough" @ getrandom(1,3) @ "_sound");
}

function ToxiCommonZombieHoleBot::onBotFollow( %this, %obj, %targ )
{
	CommonZombieHoleBot::onBotFollow( %this, %obj, %targ );
}

AddDamageType("Toxic", '<bitmap:base/client/ui/CI/skull> %1', '%2 <bitmap:base/client/ui/CI/skull> %1', 0.2, 1);
function Toxicity(%col,%obj)
{
	if(!isObject(%col) || %col.getState() $= "Dead" || !%col.Toxified)
	return;

	if(%col.ToxicityCount < 8)//For bots/players that aren't part of the team the obj is in   
	{
		%col.ToxicityCount++;
		%col.stopaudio(2);
		%col.playaudio(2,"toxic_burn_sound");
		cancel(%col.ToxicSchedule);
		%col.ToxicSchedule = schedule(500,0,Toxicity,%col,%obj);
		%col.mountImage(SpitAcidStatusPlayerImage, 2);

		if(isObject(%obj))
		%col.damage(%obj.hFakeProjectile, %col.getposition(), getrandom(1,8), $DamageType::Toxic);
		else %col.damage(%col, %col.getposition(), getrandom(1,8), $DamageType::Toxic);

		if(%col.hZombieL4BType && %col.hZombieL4BType < 4)
		%col.hRunAwayFromPlayer(%col);
	}
	else
	{
		%col.Toxified = 0;
		%col.ToxicityCount = 0;

		if(isObject(%col.getMountedImage(2)) && %col.getMountedImage(2).getName() $= "SpitAcidStatusPlayerImage")
		%col.unMountImage(2);
		return;
	}
}

function ToxiCommonZombieHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
	%obj.playaudio(1,"toxic_melee_sound");

	if(!%col.isToxic)
	{
		%col.Toxified = 1;
		%col.ToxicityCount = 0;
		Toxicity(%col,%obj);
	}
}

function ToxiCommonZombieHoleBot::onCollision(%this, %obj, %col, %fade, %pos, %norm)
{
	if(%obj.lasttoxified+1000 < getsimtime())
	{
		%obj.lasttoxified = getsimtime();
		
		if(%col.getType() & $TypeMasks::PlayerObjectType && %col.getstate() !$= "Dead" && !%col.isToxic)
		{
			%col.Toxified = 1;
			%col.ToxicityCount = 0;
			cancel(%col.ToxicSchedule);
			Toxicity(%col,%obj);
		}
	}
	Parent::oncollision(%this, %obj, %col, %fade, %pos, %norm);	
}

function ToxiCommonZombieHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{	
	%acidcolor = "0.25 0.85 0.3 1";
	%randmultiplier = getRandom(200,1150)*0.001;
	%skincolor = getWord(%acidcolor,0)*%randmultiplier SPC getWord(%acidcolor,1)*%randmultiplier SPC getWord(%acidcolor,2)*%randmultiplier SPC 1;

	%handColor = %skinColor;
	%shirtColor = %skinColor;
	%pantsColor = %skinColor;
	%shoeColor = %skinColor;

	switch(getRandom(0,1))
	{
		case 0: %decal = "AAA-None";
		case 1: %decal = "HCZombie";
	}

	// accent
	%obj.accentColor = "0 0 0 1";
	%obj.accent =  0;
	
	// hat
	%obj.hatColor = "0 0 0 1";
	%obj.hat = 0;
	
	// head
	%obj.headColor = %skinColor;
	%obj.faceName = %face;
	
	// chest
	%obj.chest =  %chest;

	%obj.decalName = %decal;
	%obj.chestColor = %shirtColor;
		
	// packs
	%obj.pack =  0;
	%obj.packColor =  "0 0 0 1";

	%obj.secondPack =  0;
	%obj.secondPackColor =  "0 0 0 1";
		
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