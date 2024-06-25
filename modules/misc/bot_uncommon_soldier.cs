return;

datablock PlayerData(ZombieSoldierHoleBot : CommonZombieHoleBot)
{
	shapeFile = UncommonMDts.baseShape;
	uiName = "";
	hName = "Infected" SPC "Soldier";//cannot contain spaces//except it can lmao

	hIsInfected = 1;
	hZombieL4BType = "Normal";
	hPinCI = "";
	hBigMeleeSound = "";
	maxdamage = 250;//Health

	hShootTimes = 4;
	hMaxShootRange = 60;
};

function ZombieSoldierHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

function ZombieSoldierHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
}

function ZombieSoldierHoleBot::onBotLoop(%this,%obj)
{
	CommonZombieHoleBot::onBotLoop(%this,%obj);
}

function ZombieSoldierHoleBot::onBotFollow( %this, %obj, %targ )
{
	CommonZombieHoleBot::onBotFollow(%this,%obj,%targ);
}

function ZombieSoldierHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
}

function ZombieSoldierHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	%limb = %obj.rgetDamageLocation(%position);
	if(%damageType !$= $DamageType::FallDamage || %damageType !$= $DamageType::Impact)
	if(%limb == 1 || %limb == 6 || %limb == 0) %damage = %damage/8;
	
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);
}

function ZombieSoldierHoleBot::onDamage(%this,%obj)
{
	%obj.playaudio(2,"kevlarhit" @ getrandom(1,3) @ "_sound");
	CommonZombieHoleBot::OnDamage(%this,%obj);
}

function ZombieSoldierHoleBot::onDisabled(%this,%obj)
{
	CommonZombieHoleBot::OnDisabled(%this,%obj);

	L4B_ZombieDropLoot(%obj,$L4B_Ammo[getRandom(1,$L4B_AmmoAmount)],25);
	L4B_ZombieDropLoot(%obj,$L4B_Ammo[getRandom(1,$L4B_AmmoAmount)],25);
	L4B_ZombieDropLoot(%obj,$L4B_PistolT1[getRandom(1,$L4B_PistolT2Amount)],25);
	L4B_ZombieDropLoot(%obj,$L4B_Medical[getRandom(1,$L4B_MedicalAmount)],5);
}

function ZombieSoldierHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{	
	%obj.suitColor = getRandomBotPantsColor();
	%uniformColor = %obj.suitColor;
	%larmColor = %uniformColor;
	%rarmColor = %uniformColor;
	%rLegColor = %uniformColor;
	%lLegColor = %uniformColor;
	%handColor = %skinColor;
	
	if(getRandom(1,3) == 1)
	{
		if(getRandom(1,2) == 1) %larmColor = %skinColor;
		if(getRandom(1,2) == 1) %rarmColor = %skinColor;
		if(getRandom(1,2) == 1) %rLegColor = %skinColor;
		if(getRandom(1,2) == 1) %lLegColor = %skinColor;
	}

	%obj.llegColor =  %lLegColor;
	%obj.secondPackColor =  "1 1 1 1";
	%obj.lhand =  "0";
	%obj.hip =  "0";
	%obj.faceName =  %face;
	%obj.rarmColor =  %rArmColor;
	%obj.hatColor =  "1 1 1 1";
	%obj.hipColor =  %uniformColor;
	%obj.chest =  "0";
	%obj.rarm =  "0";
	%obj.packColor =  "1 1 1 1";
	%obj.pack =  "0";
	%obj.decalName =  "AAA-None";
	%obj.larmColor =  %lArmColor;
	%obj.secondPack =  "0";
	%obj.larm =  "0";
	%obj.chestColor =  %uniformColor;
	%obj.accentColor =  "1 1 1 1";
	%obj.rhandColor =  %handColor;
	%obj.rleg =  "0";
	%obj.rlegColor =  %rLegColor;
	%obj.accent =  "1";
	%obj.headColor =  %skinColor;
	%obj.rhand =  "0";
	%obj.lleg =  "0";
	%obj.lhandColor =  %handColor;
	%obj.hat =  "0";

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

function ZombieSoldierHoleBot::L4BAppearance(%this,%client,%obj)
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
	%obj.unhidenode("BallisticHelmet");
	%obj.unhidenode("BallisticVest");
	
	%obj.setnodecolor("BallisticHelmet",%client.suitColor);	
	%obj.setnodecolor("BallisticVest",%client.suitColor);
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