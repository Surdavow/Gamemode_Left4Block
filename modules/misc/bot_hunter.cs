datablock fxDTSBrickData (BrickHunter_HoleSpawnData : BrickCommonZombie_HoleSpawnData)
{
	uiName = "Hunter Zombie Hole";
	iconName = "Add-Ons/Gamemode_Left4Block/modules/misc/icons/icon_hunter";

	holeBot = "ZombieHunterHoleBot";
};

datablock TSShapeConstructor(ClawsMDts : ZombieMDts) 
{
	baseShape = "./models/mhunterwitch.dts";
};

datablock PlayerData(ZombieHunterHoleBot : CommonZombieHoleBot)
{
	shapeFile = ClawsMDts.baseShape;
	uiName = "Hunter Infected";
	speedDamageScale = 0;
	jumpForce = 90*8;

	cameramaxdist = 4;
    cameraVerticalOffset = 1;
    cameraHorizontalOffset = 0.6;
    cameratilt = 0.1;
    maxfreelookangle = 2;

    maxForwardSpeed = 8;
    maxBackwardSpeed = 7;
    maxSideSpeed = 6;

 	maxForwardCrouchSpeed = 6;
    maxBackwardCrouchSpeed = 5;
    maxSideCrouchSpeed = 4;

	hIsInfected = 2;
	hZombieL4BType = "Special";
	hPinCI = "<bitmapk:Add-Ons/Gamemode_Left4Block/modules/misc/icons/ci_hunter2>";
	SpecialCPMessage = "Hold shift, then press space to leap <br>\c6Pounce to pin non-infected";
	hBigMeleeSound = "";

	maxdamage = 100;//Health
	hTickRate = 4000;

	hName = "Hunter";//cannot contain spaces
	hStrafe = 1;//Randomly strafe while following player
	hAttackDamage = $Pref::L4B::Zombies::SpecialsDamage/2;

	rechargeRate = 1.75;
	maxenergy = 100;
	showEnergyBar = true;
};

function ZombieHunterHoleBot::onNewDataBlock(%this,%obj)
{
	Parent::onNewDataBlock(%this,%obj);
	CommonZombieHoleBot::onNewDataBlock(%this,%obj);
}

function ZombieHunterHoleBot::onAdd(%this,%obj,%style)
{
	Parent::onAdd(%this,%obj);
	CommonZombieHoleBot::onAdd(%this,%obj);	
}

function ZombieHunterHoleBot::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc)
{
	%limb = %obj.rgetDamageLocation(%position);
	if(%damageType !$= $DamageType::FallDamage || %damageType !$= $DamageType::Impact)
	if(%limb) %damage = %damage/5;
	else %damage = %damage*2.5;
	
	Parent::Damage(%this,%obj,%sourceObject,%position,%damage,%damageType,%damageLoc);

	if(%obj.lastDamaged < getSimTime())
	{
		for(%i = 0; %i < getRandom(1,5); %i++)
		{			
			doBloodExplosion(%position, getWord(%obj.getScale(), 2));
			%this.doSplatterBlood(%obj,5);
		}
		serverPlay3D("blood_impact" @ getRandom(1,4) @ "_sound", %position);
		%obj.lastDamaged = getSimTime()+50;
	}
}

function ZombieHunterHoleBot::onDamage(%this,%obj,%delta)
{
	Parent::onDamage(%this,%obj,%delta);

    if(%obj.lastdamage+1000 < getsimtime())
	{			
		if(%obj.getstate() !$= "Dead") %obj.playaudio(0,"hunter_pain" @ getrandom(1,3) @ "_sound");
		else %obj.playaudio(0,"hunter_death" @ getrandom(1,3) @ "_sound");

		%obj.playthread(2,"plant");
		%obj.lastdamage = getsimtime();

		if(%obj.raisearms)
		{
			%obj.raisearms = false;	
			%obj.playthread(1,plant);
		}

		if(isObject(%obj.hEating))
		{
			%obj.hEating.isBeingStrangled = 0;
			L4B_SpecialsPinCheck(%obj,%obj.hEating);
		}
	}
}

function ZombieHunterHoleBot::onPinLoop(%this,%obj,%col)
{
	if(L4B_SpecialsPinCheck(%obj,%col))
	{
		%obj.setenergylevel(0);
		%obj.unmount();
		%obj.playthread(2,"zAttack" @ getRandom(1,3));		
		%col.playthread(2,"activate2");
		%col.playthread(3,"plant");
		%obj.playaudio(2,"hunter_hit" @ getrandom(1,3) @ "_sound");
		%this.RBloodSimulate(%col, %col.getMuzzlePoint(2), 1, 25);
		%col.damage(%obj, %col.gethackposition(), $Pref::L4B::Zombies::SpecialsDamage/4, $DamageType::Hunter);
		%this.schedule(250,onPinLoop,%obj,%col);				
	}
}

function ZombieHunterHoleBot::onBotLoop(%this,%obj)
{	
	switch$(%obj.hState)
	{
		case "Wandering":	%obj.isStrangling = false;
							%obj.hNoSeeIdleTeleport();

							if(getsimtime() >= %obj.lastidle+8000)
							{
								%obj.playaudio(0,"hunter_idle" @ getrandom(1,3) @ "_sound");
								%obj.playthread(3,"plant");
								%obj.lastidle = getSimTime();
							}
		default:
	}
}

function ZombieHunterHoleBot::onBotFollow( %this, %obj, %targ )
{	
	if((!isObject(%obj) || %obj.getState() $= "Dead" || %obj.isStrangling) || (!isObject(%targ)) || %targ.isBeingStrangled) return;

	cancel(%obj.hLastFollowSched);
	%obj.hLastFollowSched = %this.schedule(500,onBotFollow,%obj,%targ);	

	if((%distance = vectordist(%obj.getposition(),%targ.getposition())) < 100)
	{
		if(%obj.GetEnergyLevel() >= %this.maxenergy && !isEventPending(%obj.SpecialSched))
		{
            %cansee = vectorDot(%obj.getEyeVector(),vectorNormalize(vectorSub(%targ.getposition(),%obj.getposition()))) > 0.5;
            %obscure = containerRayCast(%obj.getEyePoint(),%targ.getMuzzlePoint(2),$TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::FxBrickObjectType, %obj);

            if(!isObject(%obscure) && %cansee)
			{

				if(%distance > 15) %time = 750;
				else %time = 500;

				%obj.setJumping(0);
				%obj.hCrouch(%time);
				%obj.schedule(%time-50,hShootAim,%targ);
			
				cancel(%obj.SpecialSched);
				%obj.SpecialSched = %obj.schedule(%time,hJump);
			}
		}
		
		if(%distance < 10)
		{
			%this.onTrigger(%obj,0,true);
			%obj.setMoveX(0);
			%obj.setMoveY(1);
			%obj.setmoveobject(%targ);
		}

		if(!%obj.raisearms)
		{	
			%obj.playthread(1,"armReadyboth");
			%obj.raisearms = true;
		}	
	}
	else if(%obj.raisearms)
	{	
		%obj.playthread(1,"root");
		%obj.raisearms = false;
	}
}

function ZombieHunterHoleBot::onBotMelee(%this,%obj,%col)
{
	%meleeimpulse = mClamp(%obj.hLastMeleeDamage, 1, 10);	
	%obj.playthread(2,"zAttack" @ getRandom(1,3));
	%obj.setaimobject(%col);
	
	if(%col.getType() & $TypeMasks::PlayerObjectType)
	{
		if(%col.getClassName() $= "Player") %col.spawnExplosion("ZombieHitProjectile",%meleeimpulse/2 SPC %meleeimpulse/2 SPC %meleeimpulse/2);
		%col.playthread(3,"plant");		
		%obj.playaudio(2,"hunter_hit" @ getrandom(1,3) @ "_sound");
	}
	else
	{ 
		%col.applyimpulse(%col.getposition(),vectoradd(vectorscale(%obj.getforwardvector(),getrandom(100,100*%meleeimpulse)),"0" SPC "0" SPC getrandom(100,100*%meleeimpulse)));	
		%obj.playaudio(1,"melee_hit" @ getrandom(1,8) @ "_sound");
	}
}

function ZombieHunterHoleBot::onImpact(%this, %obj, %col, %vec, %force)
{
	Parent::onImpact(%this, %obj, %col, %vec, %force);

	if(%oScale = getWord(%obj.getScale(),0) >= 0.9) 
	if(!%obj.SpecialPinAttack(%col,%force/2.5))
	{
		if(%obj.getState() !$= "Dead")
		{
			%obj.playThread(3,"zstumble" @ getrandom(1,3));
			%this.onDamage(%obj,10);
			%obj.setMoveY(-0.375);
			%obj.setMoveX(0);
			%obj.setAimObject(%col);

			if((%col.getType() & $TypeMasks::PlayerObjectType) && %col.getState() !$= "Dead") %col.damage(%obj, %col.gethackposition(), $Pref::L4B::Zombies::SpecialsDamage*2.5, $DamageType::Hunter);
		}		
	}
}

function ZombieHunterHoleBot::holeAppearance(%this,%obj,%skinColor,%face,%decal,%hat,%pack,%chest)
{	
	%clothesrandmultiplier = getrandom(2,8)*0.25;
	%shirtColor = 0.075 SPC 0.125*%clothesrandmultiplier SPC 0.1875*%clothesrandmultiplier SPC 1;
	%pantsColor = 0.15 SPC 0.125*%clothesrandmultiplier SPC 0.05*%clothesrandmultiplier SPC 1;
	%hatColor = %shirtColor;
	%shoeColor = getRandomBotPantsColor();
	%packColor = getRandomBotRGBColor();
	%pack2Color = getRandomBotRGBColor();
	%accentColor = getRandomBotRGBColor();		
	%decal = "Hoodie";
	%hat = 1;
	%pack = 0;
	%pack2 = 0;
	%accent = 0;
	%chest = 0; 
	%handColor = %skinColor;
	%larmColor = %shirtColor;
	%rarmColor = %shirtColor;
	%rLegColor = %shoeColor;
	%lLegColor = %shoeColor;

	if(getRandom(1,4) == 1)
	{
		if(getRandom(1,0)) %larmColor = %skinColor;
		if(getRandom(1,0)) %rarmColor = %skinColor;
		if(getRandom(1,0)) %rLegColor = %skinColor;
		if(getRandom(1,0)) %lLegColor = %skinColor;
	}

	%obj.accentColor = %accentColor;
	%obj.accent =  %accent;
	%obj.hatColor = %hatColor;
	%obj.hat = 0;
	%obj.headColor = %skinColor;
	%obj.faceName = %face;
	%obj.chest =  %chest;
	%obj.decalName = %decal;
	%obj.chestColor = %shirtColor;
	%obj.pack =  %pack;
	%obj.packColor =  %packColor;
	%obj.secondPack =  %pack2;
	%obj.secondPackColor =  %packColor;
	%obj.larm =  "0";
	%obj.larmColor = %larmColor;
	%obj.lhand =  0;
	%obj.lhandColor = %handColor;
	%obj.rarm =  "0";
	%obj.rarmColor = %rarmColor;
	%obj.rhandColor = %handColor;
	%obj.rhand = 0;
	%obj.hip =  "0";
	%obj.hipColor = %pantsColor;
	%obj.lleg =  0;
	%obj.llegColor = %lLegColor;
	%obj.rleg =  0;
	%obj.rlegColor = %rLegColor;
	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

function ZombieHunterHoleBot::L4BAppearance(%this,%obj,%client)
{
	Parent::L4BAppearance(%this,%obj,%client);
	%obj.unhideNode("hoodie");
	%obj.setNodeColor("hoodie",%client.hatColor);
}

function ZombieHunterHoleBot::onTrigger (%this, %obj, %triggerNum, %val)
{	
	Parent::onTrigger (%this, %obj, %triggerNum, %val);

	if(!isObject(%obj) || %obj.getState() $= "Dead") return;

	if(isObject(%obj.hFollowing)) %targ = %obj.hFollowing;
	else if(isObject(%obj.lastactivated) && %obj.lastactivated.getType() && $TypeMasks::PlayerObjectType) %targ = %obj.lastactivated;
	else return;	

	if(%val) switch(%triggerNum)
	{			
		case 0: if(!isEventPending(%obj.MeleeSched))
				{
					%obj.playthread(2,"zAttack" @ getRandom(1,3));
					cancel(%obj.MeleeSched);
					%obj.MeleeSched = %this.schedule(350,Melee,%obj,%targ);
				}

		case 2: if(%obj.GetEnergyLevel() >= %this.maxenergy) %this.Special(%obj);

		case 3: if(%obj.GetEnergyLevel() >= %this.maxenergy)
				{
					%obj.playaudio(0,"hunter_recognize" @ getrandom(1,3) @ "_sound");
					%obj.BeginPounce = true;
				}
	}
	else %obj.BeginPounce = false;
}

function ZombieHunterHoleBot::Melee(%this,%obj,%targ)
{
	CommonZombieHoleBot::Melee(%this,%obj,%targ);
}

function ZombieHunterHoleBot::Special(%this,%obj)
{
	if(getWord(%obj.getvelocity(),2) <= 5 && %obj.BeginPounce)
	{
		%obj.BeginPounce = false;
		%obj.setenergylevel(0);
		%obj.playaudio(0,"hunter_attack" @ getrandom(1,3) @ "_sound");
		%obj.playaudio(1,"hunter_lunge_sound");
		%obj.playthread(0,"jump");
		%obj.playthread(1,"activate2");											
		%normVec = VectorNormalize(%obj.getEyeVector());
		%eye = VectorAdd(vectorscale(%normVec,100),"0 0 1.25");
		%obj.setvelocity(%eye);		
	}
}