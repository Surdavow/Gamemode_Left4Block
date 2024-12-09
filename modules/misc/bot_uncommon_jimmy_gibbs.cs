datablock PlayerData(ZombieJimmyHoleBot : CommonZombieHoleBot)
{
	uiName = "";
	maxdamage = 250;//Health
	hName = "Infected" SPC "Jimmy" SPC "Gibbs";//cannot contain spaces

	hIsInfected = 1;
	hZombieL4BType = "Normal";
	hPinCI = "";
	hBigMeleeSound = "";
};
	
function ZombieJimmyHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

function ZombieJimmyHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
}

function ZombieJimmyHoleBot::OnBotFollow(%this,%obj,%targ)
{
	CommonZombieHoleBot::OnBotFollow(%this,%obj,%targ);
}

function ZombieJimmyHoleBot::onBotLoop(%this,%obj)
{
	CommonZombieHoleBot::onBotLoop(%this,%obj);
}

function ZombieJimmyHoleBot::onDamage(%this,%obj)
{
	CommonZombieHoleBot::OnDamage(%this,%obj);
}

function ZombieJimmyHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
}

function ZombieJimmyHoleBot::onDisabled(%this,%obj)
{
	CommonZombieHoleBot::onDisabled(%this,%obj);
}

function ZombieJimmyHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{
	%color = "0.105882 0.458824 0.768627 1";
	%color2 = "0.750 0.750 0.750 1.000";

	%hatColor = %color2;
	%packColor = %color2;
	%shirtColor = %color2;
	%pantsColor = %color2;
	%shoeColor = %color;
	%accentColor = getRandomBotColor();

	%larmColor = getRandom(0,1);
	if(%larmColor)
	%larmColor = %color;
	else %larmColor = %skinColor;
	%rarmColor = getRandom(0,1);
	if(%rarmColor)
	%rarmColor = %color;
	else %rarmColor = %skinColor;
	%rLegColor = getRandom(0,1);
	if(%rLegColor)
	%rLegColor = %shoeColor;
	else %rLegColor = %skinColor;
	%lLegColor = getRandom(0,1);
	if(%lLegColor)
	%lLegColor = %shoeColor;
	else %lLegColor = %skinColor;

	%handColor = %skinColor;
	%decal = "Mod-DareDevil";	
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