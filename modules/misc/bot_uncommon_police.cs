datablock PlayerData(ZombiePoliceHoleBot : CommonZombieHoleBot)
{
	uiName = "";
	maxdamage = 100;//Health
	hName = "Infected" SPC "Police";//cannot contain spaces//except it can lmao

	hIsInfected = 1;
	hZombieL4BType = "Normal";
	hPinCI = "";
	hBigMeleeSound = "";

	hShootTimes = 4;
	hMaxShootRange = 60;
};

function ZombiePoliceHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

function ZombiePoliceHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
}

function ZombiePoliceHoleBot::onBotLoop(%this,%obj)
{
	CommonZombieHoleBot::onBotLoop(%this,%obj);
}

function ZombiePoliceHoleBot::onBotFollow( %this, %obj, %targ )
{
	CommonZombieHoleBot::onBotFollow(%this,%obj,%targ);
}

function ZombiePoliceHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
}

function ZombiePoliceHoleBot::onDamage(%this,%obj)
{
	CommonZombieHoleBot::OnDamage(%this,%obj);
}

function ZombiePoliceHoleBot::onDisabled(%this,%obj)
{
	CommonZombieHoleBot::OnDisabled(%this,%obj);
}

function ZombiePoliceHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{	
	%clothesrandmultiplier = getrandom(5,8)*0.25;
	%obj.suitColor = 0.075 SPC 0.125*%clothesrandmultiplier SPC 0.1875*%clothesrandmultiplier SPC 1;
	%uniformColor = %obj.suitColor;
	%uniformColor2 = "0.15 0.15 0.15 1";
	%larmColor = getRandom(0,1);
	if(%larmColor)
	%larmColor = %uniformColor;
	else %larmColor = %skinColor;
	%rarmColor = getRandom(0,1);
	if(%rarmColor)
	%rarmColor = %uniformColor;
	else %rarmColor = %skinColor;
	%rLegColor = getRandom(0,1);
	if(%rLegColor)
	%rLegColor = %uniformColor2;
	else %rLegColor = %skinColor;
	%lLegColor = getRandom(0,1);
	if(%lLegColor)
	%lLegColor = %uniformColor2;
	else %lLegColor = %skinColor;
	%handColor = %skinColor;

	%obj.llegColor =  %lLegColor;
	%obj.secondPackColor =  "1 1 1 1";
	%obj.lhand =  "0";
	%obj.hip =  "0";
	%obj.faceName =  %face;
	%obj.rarmColor =  %rArmColor;
	%obj.hatColor =  %uniformColor;
	%obj.hipColor =  %uniformColor2;
	%obj.chest =  "0";
	%obj.rarm =  "0";
	%obj.packColor =  "1 1 1 1";
	%obj.pack =  "0";
	%obj.decalName =  "Mod-Police";
	%obj.larmColor =  %lArmColor;
	%obj.secondPack =  "0";
	%obj.larm =  "0";
	%obj.chestColor =  %uniformColor;
	%obj.accentColor =  "1 1 1 1";
	%obj.rhandColor =  %handColor;
	%obj.rleg =  "0";
	%obj.rlegColor =  %rLegColor;
	%obj.accent =  "0";
	%obj.headColor =  %skinColor;
	%obj.rhand =  "0";
	%obj.lleg =  "0";
	%obj.lhandColor =  %handColor;
	%obj.hat =  "6";

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}