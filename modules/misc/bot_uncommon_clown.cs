datablock PlayerData(ZombieClownHoleBot : CommonZombieHoleBot)
{
	uiName = "";
	hName = "Infected" SPC "Clown";//cannot contain spaces
	hIsInfected = 1;
	hZombieL4BType = "Normal";
	hPinCI = "";
	hBigMeleeSound = "";
};

function ZombieClownHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);
}

function ZombieClownHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
}

function ZombieClownHoleBot::onBotLoop(%this,%obj)
{
	CommonZombieHoleBot::onBotLoop(%this,%obj);
	%obj.playaudio(1,"clown_horn2_sound");
	L4B_CommonZombDistraction(%obj);
}

function ZombieClownHoleBot::onBotFollow( %this, %obj, %targ )
{
	CommonZombieHoleBot::onBotFollow(%this,%obj,%targ);
}

function ZombieClownHoleBot::onBotMelee(%this,%obj,%col)
{
	CommonZombieHoleBot::onBotMelee(%this,%obj,%col);
	%obj.playaudio(2,"clown_hit" @ getRandom(1,2) @ "_sound");
}

function ZombieClownHoleBot::onDisabled(%this,%obj)
{
	CommonZombieHoleBot::onDisabled(%this,%obj);
	if(isObject(%obj.light))
	%obj.light.delete();
	%obj.playaudio(1,"clown_horn2_sound");
}

function ZombieClownHoleBot::onDamage(%this,%obj)
{
	CommonZombieHoleBot::OnDamage(%this,%obj);
	%obj.playaudio(1,"clown_horn1_sound");
}

function L4B_CommonZombDistraction(%obj)
{
	%pos = %obj.getPosition();
	%radius = 1000;

	%searchMasks = $TypeMasks::PlayerObjectType;
	InitContainerRadiusSearch(%pos, %radius, %searchMasks);

	while((%targetid = containerSearchNext()) != 0 )
	{
		if(!%targetid.hFlashBanged && isObject(%targetid) && %targetid.getDatablock().getName() !$= "ZombieClownHoleBot" && %targetid.getClassName() $= "AIPlayer" && %targetid.hcanDistract $= "1" && !%targetid.isBurning)
		{
			if(!%targetid.Distraction && %targetid.hState $= "Following" || %targetid.hState $= "Wandering")
			{	
				if(isObject(%obj.hFollowing))
				{
					%targetid.setmoveobject(%obj.hFollowing);
					%targetid.setaimobject(%obj.hFollowing);
					%targetid.hFollowing = %obj.hFollowing;
				}
				else
				{
					%targetid.setmoveobject(%obj);
					%targetid.setaimobject(%obj);
				}
					
			}
		}
	}
}

function ZombieClownHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{
	%clothesrandmultiplier = getrandom(2,10)*0.15;
	%uniformColor = 0.481818*%clothesrandmultiplier SPC 0.481818*%clothesrandmultiplier SPC 0.481818*%clothesrandmultiplier SPC 1;
	%uniformColor2 = 0.581818*%clothesrandmultiplier SPC 0.481818*%clothesrandmultiplier SPC 0.381818*%clothesrandmultiplier SPC 1;

	%larmColor = (getRandom(1) ? %uniformColor : %skinColor);
	%rarmColor = (getRandom(1) ? %uniformColor : %skinColor);
	%rLegColor = (getRandom(1) ? %uniformColor2 : %skinColor);
	%lLegColor = (getRandom(1) ? %uniformColor2 : %skinColor);	

	%handColor = %skinColor;
	%decal = "AAA-None";
	%hat = 0;
	%pack = 0;
	%pack2 = 0;
	%accent = 0;

	//holeAppearance zombie
	%obj.llegColor =  %lLegColor;
	%obj.secondPackColor =  "0 0.435 0.831 1";
	%obj.lhand =  "0";
	%obj.hip =  "0";
	%obj.faceName =  %face;
	%obj.rarmColor =  %rArmColor;
	%obj.hatColor =  "1 1 1 1";
	%obj.hipColor =  %uniformColor2;
	%obj.chest =  "0";
	%obj.rarm =  "0";
	%obj.packColor =  "0.2 0 0.8 1";
	%obj.pack =  "0";
	%obj.decalName =  %decal;
	%obj.larmColor =  %lArmColor;
	%obj.secondPack =  "0";
	%obj.larm =  "0";
	%obj.chestColor =  %uniformColor;
	%obj.accentColor =  "0.990 0.960 0 0.700";
	%obj.rhandColor =  %handColor;
	%obj.rleg =  "0";
	%obj.rlegColor =  %rLegColor;
	%obj.accent =  "1";
	%obj.headColor =  %skinColor;
	%obj.rhand =  "0";
	%obj.lleg =  "0";
	%obj.lhandColor =  %handColor;
	%obj.hat =  "0";
	%obj.ClownFace =  getRandom(0,1);

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}