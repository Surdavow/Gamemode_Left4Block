datablock PlayerData(ZombiePirateHoleBot : CommonZombieHoleBot)
{
	uiName = "";
	hName = "Infected" SPC "Pirate";
	hIsInfected = 1;
	hZombieL4BType = "Normal";
	hPinCI = "";
	hBigMeleeSound = "";
};

function ZombiePirateHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

function ZombiePirateHoleBot::onBotLoop(%this,%obj)
{
	CommonZombieHoleBot::onBotLoop(%this,%obj);
}

function ZombiePirateHoleBot::onBotFollow( %this, %obj, %targ )
{
	CommonZombieHoleBot::onBotFollow(%this,%obj,%targ);
}

function ZombiePirateHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
}

function ZombiePirateHoleBot::onDisabled(%this,%obj)
{
	CommonZombieHoleBot::onDisabled(%this,%obj);
	if(isObject(%obj.light))
	%obj.light.delete();
}

function ZombiePirateHoleBot::onDamage(%this,%obj)
{
	CommonZombieHoleBot::OnDamage(%this,%obj);
}

function ZombiePirateHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{
	%obj.hidenode(Rshoe_blood);
	%obj.bloody["rshoe"] = false;
	%obj.hidenode(Lhand_blood);
	%obj.bloody["lhand"] = false;
	
	%randmultiplier = getRandom(200,1500)*0.001;
	%uniformColor = 0.241818*%randmultiplier SPC 0.201818*%randmultiplier SPC 0.121818*%randmultiplier SPC 1;
	%uniformColor2 = 0.181818*%randmultiplier SPC 0.081818*%randmultiplier SPC 0.0081818*%randmultiplier SPC 1;

	if(%larmColor)
	%larmColor = %uniformColor;
	else %larmColor = %skinColor;
	%rarmColor = getRandom(0,1);
	if(%rarmColor)
	%rarmColor = %uniformColor2;
	else %rarmColor = %skinColor;
	%lLegColor = getRandom(0,1);
	if(%lLegColor)
	%lLegColor = %uniformColor2;
	else %lLegColor = %skinColor;
	%handColor = %skinColor;
	%handColor = %skinColor;
	%decal = "Archer";

	%rLegColor = %uniformColor2;

	//holeAppearance zombie
	%obj.llegColor =  %lLegColor;
	%obj.secondPackColor =  "0 0.435 0.831 1";
	%obj.lhand =  "1";
	%obj.hip =  "0";
	%obj.faceName =  %face;
	%obj.rarmColor =  %rArmColor;
	%obj.hatColor =  getRandomBotPantsColor();
	%obj.hipColor =  %uniformColor2;
	%obj.chest =  getRandom(0,1);
	%obj.rarm =  "0";
	%obj.packColor =  "0 0 0 1";
	%obj.pack =  "0";
	%obj.decalName =  %decal;
	%obj.larmColor =  %lArmColor;
	%obj.secondPack =  "0";
	%obj.larm =  "1";
	%obj.chestColor =  %uniformColor;
	%obj.accentColor =  "0 0 0 1";
	%obj.rhandColor =  %handColor;
	%obj.rleg =  "1";
	%obj.rlegColor =  %rLegColor;
	%obj.accent =  "0";
	%obj.headColor =  %skinColor;
	%obj.rhand =  "0";
	%obj.lleg =  "0";
	%obj.lhandColor =  "0.5 0.5 0.5 1";
	%obj.hat =  "5";

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}