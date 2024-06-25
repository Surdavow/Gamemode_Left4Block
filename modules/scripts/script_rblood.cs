function RBloodLargeImage::onDone(%this, %obj)
{
	if(isObject(%obj)) %obj.schedule(100,delete);
}

function serverCmdClearDecals(%client)
{	
	if(!%client.isAdmin) 
	{
		messageClient(%client, '', "\c3You must be an admin to use this command.");
		return;
	}
	else
	{
		if(isObject(DecalGroup)) %decals = DecalGroup.getCount();
		messageAll('MsgClearBricks', "\c3" @ %client.getPlayerName() @ "\c2 cleared all decals on the server." SPC "(" @ %decals @ " decals)");
		if(isObject(DecalGroup)) DecalGroup.deleteAll();
	}
}

function BloodDripProjectile::onCollision(%this, %obj, %col, %pos, %fade, %normal) { %obj.explode(); }

function ceilingBloodLoop(%obj) 
{
	if(!isObject(%obj)) return;	
	if(!%obj.delay) %obj.delay = 500;

	%projectile = new Projectile() 
	{
		dataBlock = BloodDripProjectile;
		initialPosition = %obj.getPosition();
		initialVelocity = "0 0 -2";
	};
	
	cancel(%obj.ceilingBloodSchedule);
	%obj.ceilingBloodSchedule = schedule(%obj.delay+=25, 0, ceilingBloodLoop, %obj);
}

function vectorToAxis(%vector) 
{
	%y = mRadToDeg(mACos(getWord(%vector, 2) / vectorLen(%vector))) % 360;
	%z = mRadToDeg(mATan(getWord(%vector, 1), getWord(%vector, 0)));
	%euler = vectorScale(0 SPC %y SPC %z, $pi / 180);
	return getWords(matrixCreateFromEuler(%euler), 3, 6);
}

function spawnDecal(%dataBlock, %position,%vector,%scale) 
{
	if(!isObject(MissionCleanup) || !isObject(%dataBlock) || %dataBlock.getClassName() !$= StaticShapeData) return;
	if(!isObject(DecalGroup)) MissionCleanup.add(new SimGroup(DecalGroup));	
	else if(DecalGroup.getCount() >= $Pref::L4B::Blood::BloodDecalsLimit) return; 

	%obj = new StaticShape() { dataBlock = %dataBlock; };

	%obj.setTransform(%position SPC vectorToAxis(%vector));
	%obj.setScale(getRandom(10,%scale)*0.1 SPC getRandom(10,%scale)*0.1 SPC 1);
	DecalGroup.add(%obj);
	if($Pref::L4B::Blood::BloodDecalsTimeout >= 0) %obj.schedule($Pref::L4B::Blood::BloodDecalsTimeout, delete);
	return %obj;
}

function Armor::doSplatterBlood(%this,%obj,%amount) 
{	
	if(!isObject(%obj) || !$Pref::L4B::Blood::BloodDecalsLimit) return;
	%pos = %obj.getHackPosition();

	for(%i = 0; %i < getRandom(%amount); %i++) 
	{
		%negativevectorrandom = getRandom(-5,5) SPC getRandom(-5,5) SPC getRandom(-5,5);
		%cross = vectorScale(%negativevectorrandom,5);
		%stop = vectorAdd(%pos, %cross);
		%ray = containerRayCast(%pos, %stop, $TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType);
		
		if(isObject(%ray) && vectorDist(posFromRaycast(%ray),%obj.getHackPosition()) < 5)
		{			
			if(getRandom(1,4) == 1) serverPlay3d("blood_spill_sound", getWords(%ray, 1, 3));
			spawnDecal(BloodDecal @ getRandom(1, 2),posFromRaycast(%ray),getWords(%ray, 4, 6),%amount*5);
		}
	}
}

function vectorClosestPointAngle(%vector, %startPoint, %point)
{
	%vN = vectorNormalize(%vector);
	%v2 = vectorSub(%point, %startPoint);
	%dot = vectorDot(%vN, vectorNormalize(%v2));
	%closestPoint = vectorAdd(%startPoint, vectorScale(%vN, %dot * vectorLen(%v2)));
	%angle = mACos(%dot);
	return %closestPoint SPC %angle;
}

function Player::rgetDamageLocation(%obj, %position)
{	
	%scale = getWord(%obj.getScale(), 2);
	%forwardVector = %obj.getForwardVector();						// direction the bot is facing
	%sideVector = vectorCross(%forwardVector, %obj.getUpVector());  // vector facing directly to the sides of the bot
	%centerPoint = %obj.getWorldBoxCenter();								// middle reference point
	%CPR = vectorClosestPointAngle(%sideVector, %centerPoint, %position);	// where we are on the side-to-side vector
	%closestPoint = getWords(%CPR, 0, 2);									// closest point to where our round hit on the side-to-side vector
	%angle = mRadToDeg(getWord(%CPR, 3));									// angle from the reference point. 90 is center, 0 is left, 180 is right.
	%distanceFromCenter = vectorDist(%closestPoint, %centerPoint);			// distance from the reference point to the side-to-side vector. 0 is center, anything above .4 is an arm.
	%zLocation = getWord(%position, 2);					// Z position of our impact
	%zWorldBox = getword(%obj.getWorldBoxCenter(), 2);	// Where the middle of our player is on the z axis
	
	if(%zLocation > %zWorldBox - 3.3 * %scale) %limb = 0;//head
	else if(%zLocation > %zWorldBox - 4.2 * %scale)
	{
		if(%distanceFromCenter < (0.4 * %scale)) %limb = 1;// Torso	
		else if(%angle < 90)
		{
			if(%zLocation > %zWorldBox - 3.825 * %scale) %limb = 2;// Right Shoulder
			else if(%zLocation > %zWorldBox - 4.2 * %scale) %limb = 4;// Right Hand
		} 
		else
		{
			if(%zLocation > %zWorldBox - 3.825 * %scale) %limb = 3;// Left Shoulder
			else if(%zLocation > %zWorldBox - 4.2 * %scale) %limb = 5;// Left Hand
		}
	}
	else if(%zLocation > %zWorldBox - 4.6 * %scale) %limb = 6;
	else
	{
		if(%angle < 90) %limb = 7;// Right Leg
		else %limb = 8;// Left Leg
	}
	return %limb;
}

// functions for spawning blood effects
function doBloodExplosion(%position, %scale)
{
	%bloodExplosionProjectile = new Projectile()
	{
		datablock = bloodBurstFinalExplosionProjectile;
		initialPosition = %position;
	};
	MissionCleanup.add(%bloodExplosionProjectile);
	%bloodExplosionProjectile.setScale(%scale SPC %scale SPC %scale);
	%bloodExplosionProjectile.explode();
}

function doGibLimbExplosion(%datablock,%position,%scale)
{
	%bloodGibLimbsProjectile = new Projectile()
	{
		datablock = %datablock;
		initialPosition = %position;
	};
	MissionCleanup.add(%bloodGibLimbsProjectile);
	%bloodGibLimbsProjectile.setScale(%scale SPC %scale SPC %scale);
	%bloodGibLimbsProjectile.explode();
}

function Armor::RBloodSimulate(%this, %obj, %position, %damagetype, %damage)
{
	if(%position $= "" || %position $= "0 0 0" || vectorDist(%position, %obj.getHackPosition()) > 1.5*getWord(%obj.getScale(), 2)) %position = %obj.getHackPosition();
	
	%obj.limbShotgunStrike++;
	if(%obj.lastHitTime+5 < getSimTime())
	{
		%obj.limbShotgunStrike = 0;
		%obj.lastHitTime = getSimTime();
	}

	%limb = %obj.rgetDamageLocation(%position);
	%spraydamage = %damage*0.001;

	if(%obj.lastDamaged < getSimTime())
	{
		for(%i = 0; %i < getRandom(1,4); %i++)
		{			
			doBloodExplosion(%position, getWord(%obj.getScale(), 2));
			%this.doSplatterBlood(%obj,4);
		}
		serverPlay3D("blood_impact" @ getRandom(1,4) @ "_sound", %position);
		%obj.lastDamaged = getSimTime()+50;
	}

	if(%obj.getstate() $= "Dead" && %damage > %obj.getdataBlock().maxDamage*5) %obj.markForGibExplosion = true;
	if($Pref::L4B::Blood::BloodDismemberThreshold && (%damage >= $Pref::L4B::Blood::BloodDismemberThreshold || %obj.limbShotgunStrike >= 2)) %this.RbloodDismember(%obj,%limb,true,%position);	
}

function getColorName(%RGBA)
{
    %closestColor = "";
    %minDistance = 99999;

    for(%i = 0; %i < getWordCount($colorNames); %i++)
    {
        %currentColor = getWord($colorNames, %i);
        %currentColorRGBA = $colorValues[%currentColor];
        %distance = VectorDist(%RGBA, %currentColorRGBA);

        if(%distance == 0) return %closestColor;
		else if(%distance < %minDistance)
        {
            %minDistance = %distance;
            %closestColor = %currentColor;
        }
    }
    return %closestColor;
}

// Sorry Robbin, I had to use my old script that probably sucks worse than what you had before, the console was spamming .add errors.
// Now, the single line of code this is needed for: `addTaggedString`.

function Armor::RbloodDismember(%this,%obj,%limb,%doeffects,%position)
{
	if(!isObject(%obj)) return;

	%scale = getWord(%obj.getScale(), 2);
	%dismemberstring = $RBloodLimbDismemberString[%limb];

	if(!%obj.limbDismemberedLevel[%limb])
	{
		%obj.limbDismemberedLevel[%limb] = 1;		
		%dopartdismember = true;
		if(%doeffects) %doeffects = 2;

		switch(%limb)
		{
			case 0: if(%obj.hType $= "Zombie" && %obj.getState() !$= "Dead") %obj.playthread(3,"zstumble" @ getRandom(1,3));
					%obj.unhidenode("brain");
					if(%obj.getdataBlock().noHatRemoval) %dopartdismember = false;
					else if(%doeffects)
					{
						%obj.headbloodbot = new Player() 
						{ 
							dataBlock = "EmptyPlayer";
							source = %obj;
							slotToMountBot = 5;
							imageToMount = "RBloodLargeImage";
						};
	
						if(%obj.hat)
						{
							%obj.hatprop = new Player() 
							{ 
								dataBlock = "EmptyPlayer"; 
								source = %obj;
								slotToMountBot = 6;
								imageToMount = %obj.currentHat @ "image";
								imageColor = addTaggedString(getColorName(%obj.hatColor));
							};

							%obj.unmountimage(2);
							%obj.hatprop.unmount();
							%obj.hatprop.setTransform(vectorAdd(%obj.getMuzzlePoint(2),"0 0 2.5"));
							%objhalfvelocity = getWord(%obj.getVelocity(),0)/2 SPC getWord(%obj.getVelocity(),1)/2 SPC getWord(%obj.getVelocity(),2)/2;
							%obj.hatprop.setvelocity(vectorAdd(%objhalfvelocity,getRandom(-8,8) SPC getRandom(-8,8) SPC getRandom(8,16)));								
						}
					}

					%obj.headstrap = 0;
					%obj.shades = 0;
					%obj.hat = 0;

			case 1: if(%obj.chest) %dismemberstring = $RBloodLimbDismemberStringF[%limb];
					%obj.unHideNode("skeletonchest");
					%obj.unHideNode("skeletonchestpiece1");
					%obj.HideNode("skeletonchestpiece2");
					%obj.unHideNode("organs");

					if(%doeffects)
					{
						%obj.chestbloodbot = new Player() 
						{ 
							dataBlock = "EmptyPlayer";
							source = %obj;
							slotToMountBot = 2;
							imageToMount = "RBloodLargeImage";
						};						
					}

					%obj.cheststrap = 0;
					%obj.pack = 0;

			case 2:	%obj.unhidenode("rarmSlim");
			case 3:	%obj.unhidenode("larmSlim");
			case 4: if(%doeffects) doGibLimbExplosion("bloodHandDebrisProjectile",%position,%scale);
			case 5: if(%doeffects) doGibLimbExplosion("bloodHandDebrisProjectile",%position,%scale);
			case 6:	%obj.unHideNode("skelepants");
					%obj.unHideNode("pantswound");
			case 7:	if(%doeffects) doGibLimbExplosion("bloodFootDebrisProjectile",%position,%scale);
					%obj.unhideNode("legstumpr");
					%obj.nolegs++;
			case 8:	doGibLimbExplosion("bloodFootDebrisProjectile",%position,%scale);
					%obj.unhideNode("legstumpl");
					%obj.nolegs++;
		}					
	}

	if(%obj.getState() $= "Dead" && %obj.limbDismemberedLevel[%limb] != 2)
	{
		%obj.limbDismemberedLevel[%limb] = 2;
		%doeffects = 2;
		%dopartdismember = true;

		switch(%limb)
		{
			case 0:	if(%obj.getdataBlock().noHatRemoval) %dopartdismember = false;
					%obj.hidenode("brain");
					doGibLimbExplosion("RBloodBrainProjectile",%position,%scale);
					%obj.schedule(5,stopAudio,0);
			case 1: if(%obj.chest) %dismemberstring = $RBloodLimbDismemberStringF[%limb];
					doGibLimbExplosion("RBloodOrganProjectile",%position,%scale);
					%obj.HideNode("organs");
					%obj.unHideNode("skeletonchest");
					%obj.HideNode("skeletonchestpiece1");
					%obj.unHideNode("skeletonchestpiece2");
			default:
		}
	}	
	
	if(%dopartdismember)
	{
		for(%i = 0; %i < getWordCount($RBloodLimbString[%limb]); %i++) %obj.hideNode(getWord($RBloodLimbString[%limb], %i));
		
		for(%j = 0; %j < getWordCount(%dismemberstring); %j++)
		{
			%obj.unhideNode(getWord(%dismemberstring,%j));
			if(%obj.limbPartDismembered[%limb,%j] || (!%obj.limbPartDismembered[%limb,%j] && getRandom(1)))
			{
				%obj.hideNode(getWord(%dismemberstring,%j));
				%obj.limbPartDismembered[%limb,%j] = true;
			}
		}
	}

	if(%doeffects == 2)
	{
		%this.doSplatterBlood(%obj,20);
		doBloodExplosion(%position, 1.5);
		serverPlay3D("blood_dismember" @ getRandom(1,4) @ "_sound", %position);		
	}

	if(%obj.nolegs && %obj.getClassName() $= "AIPlayer") %obj.setcrouching(1);

	if(%obj.getstate() $= "Dead" && %obj.markForGibExplosion)
	{
		%obj.hideNode("ALL");
		%obj.schedule(50,delete);
		%this.doSplatterBlood(%obj,30);
		serverPlay3D("blood_explosion" @ getRandom(1,2) @ "_sound", %obj.getHackPosition());

		%datablock = "bloodHeadDebrisProjectile RBloodOrganProjectile 0 0 bloodHandDebrisProjectile bloodHandDebrisProjectile 0 bloodFootDebrisProjectile bloodFootDebrisProjectile";
		for(%i = 0; %i < getWordCount(%datablock); %i++) if(isObject(getWord(%datablock, %i)) && !%obj.limbDismemberedLevel[%i]) doGibLimbExplosion(getWord(%datablock, %i),%obj.getHackPosition(), getWord(%obj.getScale(), 2));
		for (%j = 0; %j < getRandom(2,4); %j++) doGibLimbExplosion("bloodDismemberProjectile",%obj.getHackPosition(), getWord(%obj.getScale(), 2));
		return;
	}
}