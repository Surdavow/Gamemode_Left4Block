function Armor::FootstepLoop(%this,%obj)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead" || !%obj.getDatablock().isSurvivor) return;
	
	cancel(%obj.FootstepLoop);
	%obj.FootstepLoop = %this.schedule(320,FootstepLoop,%obj);

	%pos = %obj.getPosition();
	%vel = %obj.getVelocity();
	%velz = getword(%vel,2);

	%isground = footplacecheck(%obj); //check for solid ground
	%localforwardspeed = mCeil(vectorDot(%obj.getVelocity(), %obj.getForwardVector()));

	if(mAbs(getWord(%vel,0)) < 0.5 && mAbs(getWord(%vel,1)) < 0.5 || isObject(%obj.getObjectMount()) || %obj.isStepProning || !%isGround || %obj.isSwimming || %obj.isStepJetting) //if hasn't moved, or is crouching, or is midair, or is swimming, or is jetting
	{
		cancel(%obj.FootstepLoop);
		%obj.FootstepLoop = %this.schedule(320,FootstepLoop,%obj);
	}
	else if(mAbs(getWord(%vel,0)) > 3 || mAbs(getWord(%vel,1)) > 3) serverplay3d("movestep" @ getRandom(1,4) @ "_sound",%pos);
	else serverplay3d("movequietstep" @ getRandom(1,4) @ "_sound",%pos);

	if(%localforwardspeed < 0)
	{
		cancel(%obj.FootstepLoop);
		%obj.FootstepLoop = %this.schedule(240,FootstepLoop,%obj);
	}

	if(%velz < 0) %obj.addvelocity("0 0 -0.75");	

	if(%obj.lastStepTime < getSimTime())
	{
		%obj.lastStepTime = getSimTime()+1250;

		if(%obj.getdataBlock().isSurvivor && isObject(getMiniGameFromObject(%obj)))
		{
			if(%velz < -15)
			{				
				%obj.playthread(2,"side");
				L4B_SpazzZombie(%obj,0);
				if(!%obj.isFalling)
				{
					%obj.playaudio(0,"survivor_painhigh1_sound");
					%obj.isFalling = 1;
				}
			}
			else if(%obj.isFalling)
			{
				L4B_SpazzZombie(%obj,15);
				%obj.playthread(2,"root");
				%obj.isFalling = 0;
			}

			%obj.AreaZoneNum = 0;
			%survivorsfound = 1;
			%obj.survivorAllyCount = %survivorsfound;

			InitContainerRadiusSearch(%obj.getPosition(), 25, $TypeMasks::PlayerObjectType );
			while(%scan = containerSearchNext())
			{
				if(%scan == %obj || VectorDist(getWord(%obj.getPosition(),2), getWord(%scan.getPosition(),2)) > 5) continue;
				if(%scan.getType() & $TypeMasks::PlayerObjectType && %scan.getdataBlock().isSurvivor)
				{
					%survivorsfound++;
					%obj.survivorAllyCount = %survivorsfound;
				} 
			}
		}	
	}
}

function footplacecheck(%obj)
{
	%pos0 = %obj.getPosition();
	%pos1 = %obj.getHackPosition();
	for(%a=0;%a<2;%a++) //ensure he's not stuck in the ground, with 2 checks
	{
		initContainerBoxSearch(%pos[%a], "1.25 1.25 0.1", $TypeMasks::All);
		%col[%a] = containerSearchNext();
	}
	return isObject(%col0) && %col0 != %col1; //if on the ground, and ground is not overlapping him
}

package Footsteps
{
	function Armor::onEnterLiquid(%this,%obj,%coverage,%type)
	{
		Parent::onEnterLiquid(%this,%obj,%coverage,%type);
		%obj.isSwimming = true; //note when underwater
	}
	function Armor::onLeaveLiquid(%this,%obj,%coverage,%type)
	{
		Parent::onLeaveLiquid(%this,%obj,%coverage,%type);
		%obj.isSwimming = false; //note when out of water
	}

	function Armor::onAdd(%this,%obj)
	{
		Parent::onAdd(%this,%obj);
		%obj.FootstepLoop = %this.schedule(320,FootstepLoop,%obj);
	}
	function Armor::onTrigger(%this,%obj,%slot,%val)
	{
		Parent::onTrigger(%this,%obj,%slot,%val);
		if(%slot == 3) %obj.isStepProning = %val;
		if(%slot == 4 && %this.canJet) %obj.isStepJetting = %val;
	}
};
activatepackage(Footsteps);












return;

if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
	if(!$Fluidity::PrefsLoaded)
	{
		if(!$RTB::RTBR_ServerControl_Hook)
			exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
		RTB_registerPref("Shallow Fall","Fluidity","$Pref::Fluidity::ShallowFall","bool","Player_Fluidity",true);
		RTB_registerPref("Crouch Mitigate (SF)","Fluidity","$Pref::Fluidity::CrouchMitigate","bool","Player_Fluidity",false);
		RTB_registerPref("Wind Shake","Fluidity","$Pref::Fluidity::FluidBlur","bool","Player_Fluidity",true);
		RTB_registerPref("Smart Drop","Fluidity","$Pref::Fluidity::SmartDrop","bool","Player_Fluidity",true);
		RTB_registerPref("Quick Grab","Fluidity","$Pref::Fluidity::QuickGrab","bool","Player_Fluidity",true);
		RTB_registerPref("Smart Step","Fluidity","$Pref::Fluidity::SmartStep","bool","Player_Fluidity",true);
		$Fluidity::PrefsLoaded = true;
	}
}
else
{
	$Pref::Fluidity::ShallowFall = true;
	$Pref::Fluidity::CrouchMitigate = false;
	$Pref::Fluidity::FluidBlur = true;
	$Pref::Fluidity::SmartDrop = true;
	$Pref::Fluidity::QuickGrab = true;
	$Pref::Fluidity::SmartStep = true;
}

registerInputEvent("fxDtsBrick","onLedgeGrab","Self fxDTSBrick" TAB "Client GameConnection");
registerInputEvent("fxDtsBrick","onLedgeMove","Self fxDTSBrick" TAB "Client GameConnection");
function fxDTSBrick::onLedgeGrab(%brick,%client)
{
	if(%brick == %client.oldFluidBrick)
		return;
	$inputTarget_["Self"] = %brick;
	$inputTarget_["Player"] = %client.player;
	$inputTarget_["Bot"] = %brick.vehicle;
	$inputTarget_["Client"] = %client;

	if($Server::LAN)
		$inputTarget_["Minigame"] = getMinigameFromObject(%client);
	else
	{
		if(getMinigameFromObject(%brick) == getMinigameFromObject(%client))
			$inputTarget_["Minigame"] = getMinigameFromObject(%client);
		else
			$inputTarget_["Minigame"] = 0;
	}
	%brick.processInputEvent("onLedgeGrab",%client);
	%client.oldFluidBrick = %brick;
}
function fxDTSBrick::onLedgeMove(%brick,%client)
{
	if(%brick == %client.oldFluidBrick)
		return;
	$inputTarget_["Self"] = %brick;
	$inputTarget_["Player"] = %client.player;
	$inputTarget_["Bot"] = %brick.vehicle;
	$inputTarget_["Client"] = %client;

	if($Server::LAN)
		$inputTarget_["Minigame"] = getMinigameFromObject(%client);
	else
	{
		if(getMinigameFromObject(%brick) == getMinigameFromObject(%client))
			$inputTarget_["Minigame"] = getMinigameFromObject(%client);
		else
			$inputTarget_["Minigame"] = 0;
	}
	%brick.processInputEvent("onLedgeMove",%client);
	%client.oldFluidBrick = %brick;
}

//Blur-Wind Shake FX
datablock ExplosionData(FluidBlurExplosion)
{
   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "0.05 0.05 0.05";
   camShakeDuration = 2.0;
   camShakeRadius = 2.0;
};
datablock ProjectileData(FluidBlurProjectile)
{
   explosion = FluidBlurExplosion;
};


//FluidTool
datablock StaticShapeData(FluidEmptyData)
{
	category = "Static Shapes";
	shapeFile = "./cube_col.dts";
};


//Playertypes
datablock PlayerData(PlayerFluidity : PlayerStandardArmor)
{
	canJet = false;
	canFluid = true;
	canPhysRoll = true;
	cameraMaxDist = 5;
	jumpForce = 800;
	minImpactSpeed = 22;

	uiName = "Fluidity Player";
	showEnergyBar = false;
};
datablock PlayerData(PlayerFluidNoJump : PlayerFluidity)
{
	canNoJump = true;
	jumpForce = 0;
	uiName = "";
};
datablock PlayerData(PlayerFluidFall : PlayerFluidNoJump)
{
	airControl = 0;
};
datablock PlayerData(PlayerFluidHang : PlayerFluidity)
{
	jumpForce = 0;
	airControl = 0;
	canFluidHang = true;
	maxLookAngle = 0;
	minLookAngle = 0;
	uiName = "";
};


//onAdd, onRemove
package FluidAddRemove
{
	function Armor::onNewDatablock(%data,%obj)
	{
		if(%data.getName() $= "PlayerFluidity")
		{
			cancel(%obj.FSL);
			%obj.FSL = schedule(5,0,FluidSilentLoop,%obj);
		}
		if(!%data.canFluid && %obj.lastData.canFluid)
			stopFluidHang(%obj,true,true);
		Parent::onNewDatablock(%data,%obj);
		%obj.lastData = %data;
	}
	function Armor::onDisabled(%data,%obj,%enabled)
	{
		if(%data.canFluid)
		{
			%obj.client.camera.setMode("Corpse",%obj);
			bottomPrint(%obj.client,"",1);
			stopFluidHang(%obj,true);
		}
		Parent::onDisabled(%data,%obj,%enabled);
	}
	function GameConnection::OnClientLeaveGame(%client)
	{
		FluidShapeRemove(%client);
		Parent::OnClientLeaveGame(%client);
	}
	function Armor::onAdd(%data,%obj)
	{
		Parent::onAdd(%data,%obj);
		%camera = %obj.client.camera;
		schedule(50,0,FluidSetCamLocation,%obj,%camera);
	}
	//Camera F8/F7 Fixes
	function serverCmdDropPlayerAtCamera(%client)
	{
		%obj = %client.player;
		%camera = %client.camera;
	//	echo("F7:  camera mode =" SPC %camera.mode SPC "  cam transform =" SPC %camera.getTransform());
		stopFluidHang(%obj,false,false,true);
		Parent::serverCmdDropPlayerAtCamera(%client);
		%camera.oldTra = %camera.getTransform();
		%obj.isCamF8 = false;
	}
	function serverCmdDropCameraAtPlayer(%client)
	{
		%obj = %client.player;
		if(%obj.isFluidClimb)
			stopFluidHang(%obj,false,true,true);
		Parent::serverCmdDropCameraAtPlayer(%client);
	//	echo("F8:  cam mode =" SPC %client.camera.mode SPC "  cam transform =" SPC %client.camera.getTransform());
		%obj.isCamF8 = true;
	}
	//Tool Anims
	function serverCmdUseTool(%client,%slot)
	{
		%obj = %client.player;
		Parent::serverCmdUseTool(%client,%slot);
		%obj.hasFluidTool = true;
	}
	function serverCmdUnuseTool(%client)
	{
		%obj = %client.player;
		Parent::serverCmdUnuseTool(%client);
		if(isObject(%obj))
			%obj.hasFluidTool = false;
	}
};activatePackage(FluidAddRemove);


//onTriggers
package FluidTriggers
{
	function servercmdSit(%client) //No sitting while hanging
	{
		%data = %client.player.getDatablock();
		if(!%data.canFluidHang)
			Parent::servercmdSit(%client);
	}
	function Observer::onTrigger(%data,%camera,%slot,%val)
	{
		Parent::onTrigger(%data,%camera,%slot,%val);
		%obj = %camera.player;
		if(isObject(%obj))
			%data = %obj.getDatablock();
		if(%data.canFluidHang)
		{
			if(%slot == 2 && %val)
				fluidWallJump(%obj);
			if(%slot == 3 && %val && !%obj.isCamF8)
				stopFluidHang(%obj,false,true);
			if(%slot == 4)
			{
				if(%val)
					%obj.isFluidJet = true;
				else
					%obj.isFluidJet = false;
			}
		}
	}
	function Armor::onTrigger(%data,%obj,%slot,%val)
	{
		Parent::onTrigger(%data,%obj,%slot,%val);
		if(!%data.canFluid)
			return;
		if(%slot == 3)
			%obj.isCrouching = %val;
		if(%slot == 4)
		{
			if(%val)
				%obj.isFluidJet = true;
			else
				%obj.isFluidJet = false;
		}
	}
	function Armor::onImpact(%data,%obj,%col,%pos,%speed)
	{
		if(%data.canFluid && %obj.client.minigame.FallingDamage)
		{
		//	echo(%obj.getShapeName() SPC " On Impact  speed =" SPC %speed);
			%maxDmg = %data.maxDamage;
			%mit = $Pref::Fluidity::CrouchMitigate && %obj.isCrouching;
			if(!$Pref::Fluidity::ShallowFall)
			{
				if(%speed < PlayerStandardArmor.minImpactSpeed)
					return;
			}
			else if((!%mit && %speed >= %data.minImpactSpeed+2) || (%speed >= %data.minImpactSpeed+4))
				%obj.setDamageLevel(%maxDmg-1);
		}
		Parent::onImpact(%data,%obj,%col,%pos,%speed);
	}
};activatePackage(FluidTriggers);

//Spawn Camera Reset
function FluidSetCamLocation(%obj,%camera)
{
	if(!isObject(%obj))
		return;
	%hack = %obj.getHackPosition();
	%rot = getWords(%obj.getTransform(),3,6);
	%camera.oldTra = %hack SPC %rot;
}

//Silent Loop
function FluidSilentLoop(%obj)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead")
		return;
	%data = %obj.getDatablock();
	if(%data.canFluid && %obj.client !$= "")
	{
		cancel(%obj.FSL);
		cancel(%obj.FSL2);
		%obj.FSL2 = scheduleNoQuota(5,0,FluidSilentLoop,%obj);
		%groundA = FluidBoxCheck(%obj); //on the ground
		%groundB = FluidBoxCheck(%obj,0.8,"","Low"); //high enough for hang
		%groundC = FluidBoxCheck(%obj,2.5,"","Low"); //high enough for descent
		%groundD = FluidBoxCheck(%obj,0.6,"","Low"); //high enough for Step
		if(!isObject(%groundB) && !%obj.isFluidCanceling)
			FluidLedgeGrab(%obj);
		else if((%data.canFluidHang && isObject(%groundB)) || (%data.canNoJump && isObject(%groundA)))
			stopFluidHang(%obj,true);
		if(%obj.lastGroundAExist && !isObject(%groundC))
			FluidDescent(%obj);
		else if(!isObject(%groundD) && !%obj.isFluidHup)
			FluidStep(%obj);
		%obj.lastGroundAExist = isObject(%groundA);
	}
}

//Box Search
function FluidBoxCheck(%obj,%up,%start,%type)
{
	if(%up $= "")
		%up = 0.1;
	%pos = %obj.getPosition();
	%half = "0 0" SPC -%up/2;
	if(%type $= "Low")
		%start = vectorAdd(%half,%pos);
	else if(%start $= "")
		%start = %pos;
	initContainerBoxSearch(%start, "1.235 1.235" SPC %up, $TypeMasks::fxBrickObjectType | $Typemasks::TerrainObjectType);
	%col = containerSearchNext();
	return %col;
}

//Raycast
function FluidBeamCheck(%obj,%pos,%len,%masks,%vec)
{
	%beam = vectorScale(%vec,%len);
	%end = vectorAdd(%pos,%beam);
	%ray = containerRayCast(%pos,%end,%masks,%obj);
	return isObject(firstWord(%ray));
}

//Ledge Find
function FluidLedgeSearch(%obj,%posA,%len,%vec)
{
	if(%len $= "A")
		%len = 0.7; //Wall pressed ledge length
	if(%len $= "")
		%len = 0.8; //Ideal ledge length
	if(%vec $= "")
		%vec = %obj.getForwardVector();
	%startA = vectorAdd(%posA,"0 0 0.23"); //hackPosition add
	%startB = vectorAdd(%posA,"0 0 0.33"); //eyePoint sub
	%beam = vectorScale(%vec,%len);
	%endA = vectorAdd(%startA,%beam);
	%endB = vectorAdd(%startB,%beam);
	%rayA = containerRayCast(%startA,%endA,$TypeMasks::fxBrickObjectType | $TypeMasks::TerrainObjectType,%obj);
	%rayB = containerRayCast(%startB,%endB,$TypeMasks::fxBrickObjectType | $TypeMasks::TerrainObjectType,%obj);
	%colA = firstWord(%rayA); //Ledge
	%colB = firstWord(%rayB); //Space above ledge
	if(isObject(%colA))
		%obj.hangCol = %colA;
	return isObject(%colA) && !isObject(%colB);
}
function FluidMultiLedgeCheck(%obj,%posA,%vec,%lim,%up)
{
	%off = 0.24; //distance between brick and hack
	%psZ = getWord(%posA,2); //isolate hack Z val
	%brick = %psZ+%off; //convert to the supposed brick Z
	%flr = mFloor(%brick*10); //multiply by 10 before rounding to save 1st decimal
	%clZ = %flr*0.1; //restore decimal point
	%nwZ = %clZ-%off; //convert back to hack Z val
	%nPosA = getWords(%posA,0,1) SPC %nwZ; //create new position
//	bottomPrint(%obj.client,"<font:impact:30><color:FFF200><just:center>Multi<br><color:CCCCCC>" @ %posA @ "<br><color:FFFFFF>" @ %nPosA,0.2);

	if(%lim $= "")
		%lim = 3;
	%half = mFloor(%lim/2); //rounds half the lim to match brick grid
	for(%h=0;%h<%lim;%h++)
	{
		%add = "0 0" SPC -%h*0.1; //scale limit to valid height
		%sub = "0 0" SPC %half*0.1; //scale half offset
		%fix = vectorAdd(%add,%sub); //center the loop
		%movA = vectorAdd(%nPosA,%fix);
		%movB = vectorAdd(%movA,"0 0" SPC %up);
		%isLedgeA = FluidLedgeSearch(%obj,%movA,"A",%vec);
		if(!%isLedgeA) //if ledge A exists, don't waste a check
			%isLedgeB = FluidLedgeSearch(%obj,%movA,"",%vec);
		if(%isLedgeA || %isLedgeB)
			break;
	}
	if(!%isLedgeA && !%isLedgeB)
		%movA = "";
	return %movA;
}
function FluidLedgeGrab(%obj)
{
	%pos = %obj.getPosition();
	%vel = %obj.getVelocity();
	%vec = %obj.getForwardVector();
	%dir = FluidDirCheck(%obj,%vec);
	%rot = FluidDirFix(%obj,%dir);

	%camera = %obj.client.camera;
	%hack = %obj.getHackPosition();
	%mount = isObject(%obj.getObjectMount());
	if(getWord(%hack,2)-getWord(%pos,2) > 0.7) //Make sure player is not fully crouched
		%posA = vectorAdd(%pos,"0 0 1.325"); //Hack Position
	else
		return;
	%isLedgeA = FluidLedgeSearch(%obj,%posA,"A",%dir);
	if(!%isLedgeA) //if ledge A exists, don't waste a check
		%isLedgeB = FluidLedgeSearch(%obj,%posA,"",%dir);

	%upv = getWord(%vel,2);
	%shake = 200; //time between cam shakes
	%speed = PlayerFluidity.minImpactSpeed; //fall damage min speed
	if(-%upv > %speed-5)
	{
	//	bottomPrint(%obj.client,"<font:impact:30><color:FFF200><just:center>Blur at up vel of <color:CCCCCC>" @ %upv,0.2);
		if(!%obj.isFluidBlurring && $Pref::Fluidity::FluidBlur)
		{
			FluidFX(%obj,FluidBlurProjectile);
			%obj.isFluidBlurring = true;
			schedule(%shake,0,eval,%obj@".isFluidBlurring = false;");
		}
		if(-%upv > %speed)
			return;
	}
	if(%obj.isCrouching || %obj.isFluidJumping || %obj.isFluidClimb || %obj.isFluidHup)
		return;
	if(!%isLedgeA && !%isLedgeB)
	{
		%found = FluidMultiLedgeCheck(%obj,%posA,%dir,5);
		if(%found !$= "")
			%snap = FluidVerticalSnap(%obj,%found,%pos);
		if(%snap !$= "")
		{
			%pos = %snap; //update to Snap position
			%posA = vectorAdd(%pos,"0 0 1.325"); //Snapped Hack Position
		}
		if(%camera.mode $= "Climb" && ((%snap $= "" && !%obj.isFluidClimbTired && !%obj.isFluidJumpTired) || %mount))
			stopFluidHang(%obj,false,false,false,%mount);
	}
	if(%isLedgeA || %isLedgeB || %snap !$= "")
		FluidLedgeHang(%obj,%posA,%pos,%rot,%dir,%upv,%speed);
}
function FluidVerticalSnap(%obj,%posA,%pos)
{
	%dif = vectorAdd(%posA,"0 0 -1.325"); //Position from Hack Position
	%len = vectorDist(getWord(%dif,2),getWord(%pos,2));
//	echo("z snap len by" SPC %obj.getShapeName() SPC "=" SPC %len);
	if(%obj.isFluidReaching || %len < 0.2)
		%snap = %dif;
	return %snap;
}

//Grid Snap
function FluidDirCheck(%obj,%vec)
{
	%xa = "1 0 0";
	%ya = "0 1 0";
	%xb = "-1 0 0";
	%yb = "0 -1 0";
	%xad = vectorDist(%vec,%xa);
	%yad = vectorDist(%vec,%ya);
	%xbd = vectorDist(%vec,%xb);
	%ybd = vectorDist(%vec,%yb);
	%len = 0.75; //closest dir
	if(%yad < %len)
		%dir = %ya;
	if(%ybd < %len)
		%dir = %yb;
	if(%xad < %len)
		%dir = %xa;
	if(%xbd < %len)
		%dir = %xb;
	return %dir;
}
function FluidDirFix(%obj,%dir)
{
	%yaw = mATan(getWord(%dir,1),getWord(%dir,0));
	%fix = "0 0 -1" SPC %yaw;
	%eul = axisToEuler(%fix);
	%eulA = vectorAdd(%eul,"0 0 -90");
	%grid = eulerToAxis(%eulA);
	return %grid;
}

//Step Onto Ledge
function FluidStep(%obj)
{
	%vel = %obj.getVelocity();
	%upv = getWord(%vel,2);
	%pos = %obj.getPosition();
	%vec = %obj.getForwardVector();

	%mov = vectorAdd(%pos,vectorScale(%vec,0.3)); //Must be Ideal length - 0.4
	%mov = vectorAdd(%mov,"0 0 2.1");
	%block = FluidBoxCheck(%obj,2.6,%mov); //2.6 is the height of 5 bricks
//	echo("Step Up   up vel =" SPC %upv);

	%posC = vectorAdd(%pos,"0 0 0.5"); //Waist Position
	%found = FluidMultiLedgeCheck(%obj,%posC,%vec,5);
	if(%found $= "" || %upv < 4 || isObject(%block) || %obj.isCrouching || !$Pref::Fluidity::SmartStep)
		return;

	%fwd = vectorScale(%vec,3);
	%obj.setTransform(vectorAdd(%found,"0 0 -0.5"));
	%obj.schedule(100,setVelocity,%fwd);
	%obj.setVelocity("0 0 13");
	%obj.playthread(0,walk);
	%obj.playthread(2,plant);
	%obj.schedule(100,playthread,0,root);
	%obj.isFluidHup = true;
	schedule(250,0,eval,%obj@".isFluidHup = false;");
	%obj.changedatablock(PlayerFluidFall);
	%obj.sFHF = schedule(400,0,stopFluidHangFinish,%obj);
}

//Slow the Fall
function FluidQuickGrab(%obj,%upv)
{
//	echo("ledge hang   up vel =" SPC %upv);
	%scl = %upv*0.5;
	%vel = %obj.getVelocity();
	%dv = getWords(%vel,0,1);
	%obj.setVelocity(%dv SPC %scl);
	%obj.playthread(2,activate);
	%obj.playthread(0,shiftup);
	%obj.isFluidJumping = true;
	schedule(1000,0,eval,%obj@".isFluidJumping = false;");
}

//Hang Actual
function FluidLedgeHang(%obj,%posA,%pos,%rot,%dir,%upv,%speed)
{
	%client = %obj.client;
	%camera = %client.camera;
	%dist = PlayerFluidHang.cameraMaxDist;
	%rvec = getRelativeX(%obj,%dir);
	%lvec = vectorScale(%rvec,-1);

	if(-%upv > %speed-5) //Quickly grab and release a ledge to slow down
	{
		if($Pref::Fluidity::QuickGrab)
			FluidQuickGrab(%obj,%upv);
		return;
	}

	if(%obj.isFluidAnim) //Only use camera's viewpoint if already climbing
		%camVec = %camera.getEyeVector();
	else
		%camVec = %obj.getEyeVector();

	%camXY = getWords(%camVec,0,1) SPC 0;
	%dirB = vectorScale(%dir,-1); //-dir
	%dl = vectorDist(%lvec,%camXY); //left
	%dr = vectorDist(%rvec,%camXY); //right
	%df = vectorDist(%dir,%camXY); //forward
	%db = vectorDist(%dirB,%camXY); //back
	%z = getWord(%camVec,2); //z axis (up/down)
	%zO = %z;

	%hack = %obj.getHackPosition(); //Free Look Cam
	%back = vectorScale(%dir,-1); //
	%pivt = vectorAdd(%hack,%back); //
	%tra = %camera.getTransform(); //

	%xA = 0; //zero out x add so it's never undefined
	%zM = 0.35; //default z min
	%minLR = 1.2; //minimum angle to start registering left/right look angle
	%groundB = FluidBoxCheck(%obj,0.8,%pos,"Low"); //Must match ground B in Silent Loop function
	if(isObject(%groundB) || (%obj.isCamF8 && !%obj.isFluidAnim) || !%obj.isFluidAnim && (%z == 1 || %z == -1))
		return;

	%edge = FluidEdgeDetect(%obj,%dir,%lvec,%rvec,%posA);
	%canL = %dl < %minLR && %edge !$= "left" && %edge !$= "both"; //Edge detection result left
	%canR = %dr < %minLR && %edge !$= "right" && %edge !$= "both"; //Edge detection result right

	%pos = FluidYShift(%obj,%posA,%dir,%pos);
	%obj.setTransform(%pos SPC %rot); //combines all adjustments
	FluidShape(%obj,%pos);
	if(!%obj.isFluidFirstHang)
	{
		cancel(%obj.iFH);
		%obj.isFluidFirstHang = true;
		%obj.isFluidPreHang = true;
		if(%camera.mode !$= "Climb")
			%obj.iFH = schedule(200,0,eval,%obj@".isFluidPreHang = false;");
		else
			%obj.iFH = schedule(50,0,eval,%obj@".isFluidPreHang = false;");
	}
	if(!%obj.isFluidAnim)
	{
	//	echo(%obj.getShapeName() @ "'s first Fluid Anim");
		%obj.isFluidAnim = true;
		%obj.setVelocity("0 0 0");
		schedule(50,0,FluidAnimStart,%obj);
		%obj.hangCol.onLedgeGrab(%client); //use brick on first hang
		%obj.changedatablock(PlayerFluidHang);
		if(%camera.mode !$= "Climb")
		{
		//	echo("Fluid Cam start");
			%tra = %obj.getEyeTransform();
			%camera.player = %obj;
			%canCamModeSet = true;
		}
	}
	if(%obj.hasFluidTool && !%obj.isFluidToolAnim && !%obj.getMountedImage(0).isActionMelee) 
	{
		%obj.playthread(2,armreadyleft);
		%obj.playthread(1,root);
		%obj.isFluidToolAnim = true;
	}
	else if(!%obj.hasFluidTool && %obj.isFluidToolAnim)
	{
		%obj.playthread(2,armreadyboth);
		%obj.isFluidToolAnim = false;
	}
	if((%obj.isFluidJet || %df < 1.5) && !%obj.hasCamSetUp) //swap to back cam at this df
	{
		%obj.hasCamSetup = true;
		%camera.setOrbitMode(%obj,%tra,0,%dist,%dist,true); //Normal climb cam
		%obj.isCamPointMode = false;
		%camera.setTransform(%tra);
		bottomPrint(%client,"",1);
	}
	else if(!%obj.isFluidJet && %df > 1.5 && !%obj.isCamPointMode && %obj.hasCamSetup)
	{
		%obj.hasCamSetup = false;
		%camera.setOrbitPointMode(%pivt,0); //Backwards facing climb cam
		%obj.isCamPointMode = true;
	}
	if(%camera.getControlObject() != %camera)
		%camera.setControlObject(%camera); //confirm camera is controlling itself
	if(%client.getControlObject() != %camera)
	{
		%client.setControlObject(%camera); //confirm client is controlling camera
		if(%canCamModeSet) //if very first grab onto ledge, change camera to climb mode
			%camera.mode = "Climb";
	}
	if(!isObject(%camera) || !%obj.isFluidJet || %obj.isFluidPreHang || %obj.isFluidJumpTired)
		return;
	%obj.hangCol.onLedgeMove(%client); //use currently grabbed brick
	if((%dl < %minLR || %dr < %minLR) && mAbs(%z) < 0.8) //define horizontal movement
	{
		%a = %df*7; //scale side velocity to left/right look angle
		if(%df > 1)
			%a = %db*7; //if looking backwards fix the scale
		if(%a > 5) //limit left/right movement speed
			%a = 5;
		if(%dl < %minLR)
		{
			%svec = %lvec;
			%isL = true; //is looking left
		}
		if(%dr < %minLR)
		{
			%svec = %rvec;
			%isR = true; //is looking right
		}
		if(%a-mAbs(%z) > 2) //set min angle to allow diagonal climbing
			%xA = vectorScale(%svec,%a/2); //apply scale to left/right vector (divide by 2 to make it more manageable)
		if(mAbs(%z) > 0.2)
			%a = %a-mAbs(%z*2); //scale A down the higher Z is
		if(%a < 2.3) //set left/right movement dead zone
			%a = 2.3;
	}
	if(%z > %zM || %z < -%zM) //define/apply vertical movement
	{
		%zP = mAbs(%z)-%zM; //subtract the minimum look angle
		if(%z > %zM)
			%z = %zP; //restore z as positive
		else
			%z = -%zP; //restore z as negative
		%time = 300-mAbs(%z*150); //scale time to match climbing speed
		cancel(%obj.FluidCl); //cancel climb down anim
	//	bottomPrint(%obj.client,"<font:impact:30><color:FFF200><just:center>Hang   time =<color:CCCCCC>" SPC %time,2);
		if(%z > 0)
			FluidUp(%obj,%z,%time,%xA);
		if(%z < 0)
			FluidDown(%obj,%z,%time,%xA);
	}
	%xM = vectorScale(%svec,%a);
	if((%isL && %canL || %isR && %canR) && %a !$= "" && !%obj.isFluidClimb && !%obj.isFluidHup) //apply horizontal movement, if not climbing up/down already
		%obj.setVelocity(%xM);
//	bottomPrint(%obj.client,"<font:impact:30><color:FFF200><just:center>Hang   <color:CCCCCC>a =" SPC %a SPC "  z =" SPC %z SPC "  df =" SPC %df SPC "  z old =" SPC %zO @ "<color:666666><br>x add =" SPC %xA SPC "  x move =" SPC %xM @ "<br><color:FF4900>dl =" SPC %dl SPC "  dr =" SPC %dr SPC "  Edge =" SPC %edge,5);
}

//Hang Animation
function FluidAnimStart(%obj)
{
	if(%obj.isFluidAnim && !%obj.isFluidClimb && !%obj.isFluidHup)
	{
		%obj.playthread(2,armreadyboth);
		%obj.playthread(0,fall);
		%obj.setActionThread(root);
		%obj.isFluidToolAnim = false;
	}
}

//Hang Break
function stopFluidHang(%obj,%silent,%force,%cam,%mount)
{
	if(!isObject(%obj) || %obj.getState() $= "Dead")
		return;

//	echo("fluid hang stop called by " SPC %obj.getShapeName() SPC "  silent =" SPC %silent SPC "  force =" SPC %force SPC "  cam =" SPC %cam SPC "  mount =" SPC %mount);
	%camera = %obj.client.camera;
	%data = %obj.getDatablock();

	//If hanging, don't let hang cam overwrite previous f7
	if(%cam && !%obj.isCamF8 && %camera.mode !$= "Corpse" && %camera.oldTra !$= %camera.getTransform())
		%camera.setTransform(%camera.oldTra);
	if((%cam && !%obj.isFluidAnim) || %obj.hasStoppedFluidHang)
		return;

	cancel(%obj.iFH);
	cancel(%obj.iFA);
	cancel(%obj.iFC);
	%obj.hasCamSetup = false;
	%obj.isFluidAnim = false;
	%obj.isFluidClimb = false;
	%obj.isFluidFirstHang = false;

	if(!%cam)
	{
		%obj.client.setControlObject(%obj);
		%camera.mode = "Observer";
		%camera.setFlyMode();
	}
	if(%force)
	{
		%obj.isFluidCanceling = true;
		schedule(200,0,eval,%obj@".isFluidCanceling = false;");
	}
	if(%data.canFluidHang && (!%cam || %obj.isCamF8))
	{
		%obj.changedatablock(PlayerFluidNoJump);
		%obj.sFHF = schedule(400,0,stopFluidHangFinish,%obj);
	}
	else if(%silent)
		stopFluidHangFinish(%obj);
	stopFluidHangAnim(%obj,%mount);
	FluidShapeRemove(%obj.client);
	%obj.hasStoppedFluidHang = true;
	schedule(500,0,eval,%obj@".hasStoppedFluidHang = false;");
//	echo("Fluid Hang Stop end");
}
function stopFluidHangFinish(%obj)
{
	cancel(%obj.sFHF);
	if(%obj.getState() !$= "Dead" && %obj.getDatablock().canNoJump)
		%obj.changedatablock(PlayerFluidity);
}

//Stop Hang Animation
function stopFluidHangAnim(%obj,%mount)
{
	%obj.playthread(2,root);
	%obj.setActionThread(root);
	if(!%obj.isFluidJumping && !%mount)
	{
		cancel(%obj.FluidCl); //cancel climb down anim
		%obj.playthread(3,root);
		%obj.playthread(0,root);
	}
	fixArmReady(%obj);
}

//Shift backwards to ideal length
function FluidYShift(%obj,%posA,%dir,%pos)
{
	%len = 0.7; //Must match Wall-pressed length
	%bck = -0.08;
	%isHit = FluidBeamCheck(%obj,%posA,%len,$TypeMasks::fxBrickObjectType | $TypeMasks::TerrainObjectType,%dir);
	if(%isHit)
		%pos = vectorAdd(%pos,vectorScale(%dir,%bck));
	return %pos;
}

//Corner/Edge Check
function FluidEdgeDetect(%obj,%dir,%lvec,%rvec,%posA)
{
	%lPosA = vectorAdd(%posA,vectorScale(%lvec,0.25)); //Shift left
	%rPosA = vectorAdd(%posA,vectorScale(%rvec,0.25)); //Shift right
	%isLedgeL = FluidLedgeSearch(%obj,%lPosA); //Edge detection
	%isLedgeR = FluidLedgeSearch(%obj,%rPosA); //
	%isHitL = FluidBeamCheck(%obj,%posA,0.7,$TypeMasks::fxBrickObjectType | $TypeMasks::TerrainObjectType,%lvec); //check for perpendicular ledge/wall
	%isHitR = FluidBeamCheck(%obj,%posA,0.7,$TypeMasks::fxBrickObjectType | $TypeMasks::TerrainObjectType,%rvec); //
	%endA = !%isLedgeL || %isHitL; //true if wall or no ledge
	%endB = !%isLedgeR || %isHitR; //
	%obj.canTurn = %isHitL || %isHitR; //true if corner is found
	if(%endA && %endB)
		%end = "both";
	else if(%endA)
		%end = "left";
	else if(%endB)
		%end = "right";
	return %end;
}

//Climb Alternate
function FluidClLeft(%obj,%dir,%time)
{
	%obj.playthread(2,armreadyleft);
	if(%dir $= "Down")
		%obj.FluidCl = %obj.schedule(%time/2,playthread,3,leftrecoil);
	else
		%obj.playthread(3,leftrecoil);
	%obj.isFluidClLeft = false; //alternate climbing hands
}
function FluidClRight(%obj,%dir,%time)
{
	%obj.playthread(2,armreadyright);
	if(%dir $= "Down")
		%obj.FluidCl = %obj.schedule(%time/2,playthread,3,shiftaway);
	else
		%obj.playthread(3,shiftaway);
	%obj.isFluidClLeft = true; //alternate climbing hands
}
function FluidClimbRandom(%obj)
{
	%rand = getRandom(1);
	if(%rand == 1)
		%obj.playthread(0,plant);
	else
		%obj.playthread(0,jump);
}

//Climb Up
function FluidUp(%obj,%z,%time,%xA)
{
	if(%xA !$= 0)
		%scl = vectorScale(%xA,0.2); //scale the x add for predicting next ledge
	%zA = mAbs(%z); //vertical velocity add
	%up = 6.5; //base vertical velocity

	%hack = %obj.getHackPosition();
	%posA = vectorAdd(%scl,%hack);
	%posB = vectorAdd(%posA,"0 0" SPC 1.2);
	%found = FluidMultiLedgeCheck(%obj,%posB) !$= "";
	if(%found) //don't waste a safety check
		%isHit = FluidBeamCheck(%obj,%posA,1.2+1.2,$TypeMasks::fxBrickObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType,"0 0 1"); //1.2 is an extra brick

	%posA2 = vectorAdd(%scl,%hack);
	%posB2 = vectorAdd(%posA2,"0 0" SPC 1.8);
	%found2 = FluidMultiLedgeCheck(%obj,%posB2) !$= "";
	if(%found2) //don't waste a safety check
		%isHit2 = FluidBeamCheck(%obj,%posA2,1.8+1.2,$TypeMasks::fxBrickObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType,"0 0 1"); //

	%sVel = vectorScale(vectorSub(%posB,%hack),%up+%zA);
	%sAdd = vectorScale(%scl,-2); //offset left/right velocity to actually match the found position
	%vel = vectorAdd(%sVel,%sAdd);
//	bottomPrint(%obj.client,"<font:impact:30><color:FFF200><just:center>Climb Up   <color:FFFFFF>time =" SPC %time SPC "  z =" SPC %z @ "<color:CCCCCC><br>side Vel =" SPC %sVel SPC "  vel =" SPC %vel SPC "<br><color:666666>  z Add =" SPC  %zA SPC "  scale =" SPC %scl,10);
	if(%found && !%isHit)
	{
	//	echo("Climbed up   z =" SPC %z);
		cancel(%obj.iFT);
		%obj.isFluidClimb = true;
		%obj.isFluidFirstHang = false;
		%obj.isFluidClimbTired = true;
		%obj.iFT = schedule(%time*1.6,0,eval,%obj@".isFluidClimbTired = false;");
		%obj.iFC = schedule(%time,0,eval,%obj@".isFluidClimb = false;");
		%obj.iFA = schedule(%time,0,eval,%obj@".isFluidAnim = false;");
		%obj.setVelocity(%vel);
		if(%obj.isFluidClLeft)
			FluidClLeft(%obj);
		else
			FluidClRight(%obj);
		FluidClimbRandom(%obj);
	}
	else if(%found2 && !%isHit2)
		FluidHangJump(%obj,%zA,%time,"Up",%scl,%hack,%posB2);
	else if(%z > 0.1) //should be 0.1 above min Fluid Up call speed
		FluidHup(%obj);
}

//Climb Down
function FluidDown(%obj,%z,%time,%xA)
{
	if(%xA !$= 0)
		%scl = vectorScale(%xA,0.2); //scale the x add for predicting next ledge
	%zA = mAbs(%z); //vertical velocity add
	%up = 2; //base vertical velocity

	%hack = %obj.getHackPosition();
	%posA = vectorAdd(%scl,%hack);
	%posB = vectorAdd(%posA,"0 0" SPC -1.2);
	%found = FluidMultiLedgeCheck(%obj,%posB) !$= "";
	if(%found) //don't waste a safety check
		%isHit = FluidBeamCheck(%obj,%posA,-1.2-1.2,$TypeMasks::PlayerObjectType,"0 0 1"); //1.2 is an extra brick

	%posA2 = vectorAdd(%scl,%hack);
	%posB2 = vectorAdd(%posA2,"0 0" SPC -1.8);
	%found2 = FluidMultiLedgeCheck(%obj,%posB2) !$= "";
	if(%found2) //don't waste a safety check
		%isHit2 = FluidBeamCheck(%obj,%posA2,-1.8-1.2,$TypeMasks::PlayerObjectType,"0 0 1"); //

	%sVel = vectorScale(vectorSub(%posB,%hack),%up+%zA);
	%sAdd = vectorScale(%scl,2); //offset left/right velocity to actually match the found position
	%vel = vectorAdd(%sVel,%sAdd);
//	bottomPrint(%obj.client,"<font:impact:30><color:FFF200><just:center>Climb Down   <color:FFFFFF>time =" SPC %time SPC "  z =" SPC %z @ "<color:CCCCCC><br>side Vel =" SPC %sVel SPC "  vel =" SPC %vel SPC "<br><color:666666>  z Add =" SPC  %zA SPC "  scale =" SPC %scl,10);
	if(%found && !%isHit)
	{
	//	echo("Climbed down   z =" SPC %z);
		cancel(%obj.iFT);
		%obj.isFluidClimb = true;
		%obj.isFluidFirstHang = false;
		%obj.isFluidClimbTired = true;
		%obj.iFT = schedule(%time*1.6,0,eval,%obj@".isFluidClimbTired = false;");
		%obj.iFC = schedule(%time,0,eval,%obj@".isFluidClimb = false;");
		%obj.iFA = schedule(%time,0,eval,%obj@".isFluidAnim = false;");
		%pos = %obj.getPosition();
		%mov = vectorAdd(%pos,%scl);
		%sub = vectorAdd(%mov,"0 0" SPC -1.2);
		FluidShape(%obj,%sub);
		%obj.setVelocity(%vel);
		if(%obj.isFluidClLeft)
			FluidClLeft(%obj,"Down",%time);
		else
			FluidClRight(%obj,"Down",%time);
		FluidClimbRandom(%obj);
	}
	else if(%found2 && !%isHit2)
		FluidHangJump(%obj,%zA,%time,"Down",%scl,%hack,%posB2);
}

//Climb Onto
function FluidHup(%obj)
{
	%hack = %obj.getHackPosition();
	%vec = %obj.getForwardVector();
	%mov = vectorAdd(%hack,vectorScale(%vec,0.3)); //Must be Ideal length - 0.4
	%mov = vectorAdd(%mov,"0 0 1.6");
	%block = FluidBoxCheck(%obj,2.5,%mov); //2.5 is the exact height of 5 bricks
	%canHup = !isObject(%block);
	if(%canHup)
	{
	//	echo("hupped");
		%vel = %obj.getVelocity();
		%ya = vectorScale(%vec,4);
		%dv = getWords(%vel,0,1);
		%obj.playthread(2,crouch);
		%obj.schedule(200,setThreadDir,2,false);
		%obj.playthread(3,shiftdown);
		%obj.setVelocity(%dv SPC 10.5);
		%obj.schedule(300,setVelocity,%ya);
		schedule(350,0,eval,%obj@".isFluidHup = false;");
		%obj.isFluidHup = true;
	}
}

//Hanging Upward/Downward Jump
function FluidHangJump(%obj,%zA,%time,%dir,%scl,%hack,%posB2)
{
	cancel(%obj.iFJ);
	%obj.iFJ = schedule(%time*1.6,0,eval,%obj@".isFluidJumpTired = false;");
	%obj.iFC = schedule(%time,0,eval,%obj@".isFluidClimb = false;");
	%obj.iFA = schedule(%time,0,eval,%obj@".isFluidAnim = false;");
	%obj.isFluidJumpTired = true;
	%obj.isFluidFirstHang = false;
	%obj.isFluidClimb = true;

	if(%dir $= "Up")
	{
		%obj.playthread(0,jump);
		if(%obj.isFluidClLeft)
			FluidClLeft(%obj);
		else
			FluidClRight(%obj);
		%up = 5.5; //base vertical velocity
		%sVel = vectorScale(vectorSub(%posB2,%hack),%up+%zA);
		%sAdd = vectorScale(%scl,-2/1.5); //offset left/right velocity to actually match the found position
		%vel = vectorAdd(%sVel,%sAdd);
		%obj.setVelocity(%vel);
	}
	if(%dir $= "Down")
	{
		%obj.playthread(0,plant);
		if(%obj.isFluidClLeft)
			FluidClLeft(%obj,"Down",%time);
		else
			FluidClRight(%obj,"Down",%time);
		%up = 1; //base vertical velocity
		%sVel = vectorScale(vectorSub(%posB2,%hack),%up+%zA);
		%sAdd = vectorScale(%scl,2); //offset left/right velocity to actually match the found position
		%vel = vectorAdd(%sVel,%sAdd);
		%obj.setVelocity(%vel);
		%pos = %obj.getPosition();
		%mov = vectorAdd(%pos,%scl);
		%sub = vectorAdd(%mov,"0 0" SPC -1.8);
		FluidShape(%obj,%sub);
	}
//	bottomPrint(%obj.client,"<font:impact:30><color:FFF200><just:center>Hang Jumped" SPC %dir SPC "   <color:FFFFFF>time =" SPC %time @ "  <color:CCCCCC><br>side Vel =" SPC %sVel SPC "  vel =" SPC %vel SPC "<br><color:666666>  z Add =" SPC  %zA SPC "  scale =" SPC %scl,10);
}

//Fluid Wall Jump
function fluidWallJump(%obj)
{
	if(%obj.isFluidJumpTired || %obj.isFluidPreHang || !%obj.isFluidAnim || %obj.isFluidJet)
		return;

	%camera = %obj.client.camera;
	%camTra = %camera.getTransform();
	%camRot = getWords(%camTra,3,6);

	%vec = %obj.getForwardVector(); //forward vector pre transform
	%fwd = FluidDirCheck(%obj,%vec); //wall facing vector
	%cvc = %camera.getEyeVector();
	%xy0 = getWords(%cvc,0,1) SPC 0;
	%xyN = vectorNormalize(%xy0);

	%pos = %obj.getPosition(); //Free Look Cam
	%back = vectorScale(%fwd,-1); //
	%pivt = vectorAdd(%pos,%back); //

	%z = getWord(%cvc,2);
	%df = vectorDist(%fwd,%xyN);
	if(%df < 0.8 || (%df < 1 && %z < -0.8) || (%z > 0.8 || %z == -1))
		return;

	if(%obj.isCamPointMode)
		%pos = %pivt;

	stopFluidHang(%obj);
	%obj.setTransform(%pos SPC %camRot);
	serverPlay3D(jumpsound,%pos);
	%obj.playthread(2,shiftdown);
	%obj.playthread(0,jump);

	%obj.isFluidJumping = true;
	%obj.isFluidJumpTired = true;
	schedule(800,0,eval,%obj@".isFluidJumpTired = false;");
	schedule(200,0,eval,%obj@".isFluidJumping = false;");

	%len = 8; //wall jump final speed
	%canDir = $Pref::Fluidity::SmartDrop;
	%vec = %obj.getForwardVector(); //forward vector post transform
	%dir = FluidDirCheck(%obj,%vec); //use nearest side angle if looking at wall
	if(%df > 1.5) //min angle to start using look direction for wall jump
		%dir = %vec;
	if(%z < -0.9 && %canDir)
	{
		%dir = %cvc;
		%len = %len/2;
	}
	%zA = "0 0 2";
	%yA = vectorScale(%dir,3);
	%yzA = vectorAdd(%yA,%zA);
	%nrm = vectorNormalize(%yzA);
	%scl = vectorScale(%nrm,%len);
	%obj.setVelocity(%scl);
}

//Descend From Top of Ledge
function FluidDescent(%obj)
{
	%vel = %obj.getVelocity();
	%eye = %obj.getEyeVector();
	%vec = %obj.getForwardVector();
	%dir = FluidDirCheck(%obj,%vec);
	%fwd = vectorScale(%dir,2);
	%rvec = getRelativeX(%obj,%vec);
	%rvel = vectorDot(%vel,%rvec);
	%upv = getWord(%vel,2);
	%fv = vectorDot(%vel,%vec);
	%dv = vectorDot(%vel,%dir);
	%df = vectorDist(%vec,%dir);
	%eZ = getWord(%eye,2);
	%eAdd = vectorAdd(%eye,"0 0 0.5");
	%eNrm = vectorNormalize(%eAdd);
	%eScl = vectorScale(%eNrm,2);
	%canDir = $Pref::Fluidity::SmartDrop;
	if(%df < 0.4 && %dv < -0.1 && %upv < 1 && (%rvel > -5 && %rvel < 5) && !%obj.isCrouching)
	{
		%obj.setVelocity(vectorAdd(%fwd,"0 0 -0.5"));
		%obj.changedatablock(PlayerFluidFall);
		%obj.sFHF = schedule(500,0,stopFluidHangFinish,%obj);
	}
	else if(%eZ < -0.9 && %fv > 0.1 && %upv < 1 && %canDir)
	{
	//	echo("eye z =" SPC %eZ);
		%obj.playthread(2,shiftdown);
		%obj.playthread(0,plant);
		%obj.setVelocity(%eScl);
		%obj.changedatablock(PlayerFluidNoJump);
		%obj.sFHF = schedule(400,0,stopFluidHangFinish,%obj);
	}
}

//Blur FX
function FluidFX(%obj,%data,%pos)
{
	if(%pos $= "")
		%pos = %obj.getPosition();
	%p = new Projectile()
	{
		dataBlock = %data;
		initialPosition = %pos;
		initialVelocity = "0 0 0";
		sourceObject = %obj;
		client = %obj.client;
	};MissionCleanup.add(%p);
	%p.explode();
}

//Fluid Tool
function FluidShape(%obj,%pos)
{
	%rot = getWords(%obj.getTransform(),3,6);
	%low = vectorAdd(%pos,"0 0 -0.11");
	%vec = %obj.getForwardVector();
	%fwd = vectorScale(%vec,0.4);
	%off = vectorAdd(%fwd,%low);
	%shape = %obj.client.FlTool;
	if(isObject(%shape))
	{
		%old = %shape.getPosition();
		%dst = vectorDist(%old,%off);
		if(%dst < 0.1)
			return;
	}

	//Hang Platform Static Shape
	%shape = new StaticShape()
	{
		datablock = FluidEmptyData;
		position = %off;
		client = %obj.client;
		scale = "0.4 0.2 0.1";
	};missionCleanup.add(%shape);
	%shape.setTransform(%off SPC %rot);
	FluidShapeRemove(%obj.client);
	%obj.client.FlTool = %shape;
}
function FluidShapeRemove(%client)
{
	if(isObject(%client.FlTool))
		%client.FlTool.delete();
}


//Relative Vector
function getRelativeX(%obj,%dir)
{
	return vectorCross(%dir,%obj.getUpVector());
}
//Euler to Axis & Axis to Euler
function eulerToAxis(%euler)
{
	%euler = VectorScale(%euler,$pi / 180);
	%matrix = MatrixCreateFromEuler(%euler);
	return getWords(%matrix,3,6);
}
function axisToEuler(%axis)
{
	%angleOver2 = getWord(%axis,3) * 0.5;
	%angleOver2 = -%angleOver2;
	%sinThetaOver2 = mSin(%angleOver2);
	%cosThetaOver2 = mCos(%angleOver2);
	%q0 = %cosThetaOver2;
	%q1 = getWord(%axis,0) * %sinThetaOver2;
	%q2 = getWord(%axis,1) * %sinThetaOver2;
	%q3 = getWord(%axis,2) * %sinThetaOver2;
	%q0q0 = %q0 * %q0;
	%q1q2 = %q1 * %q2;
	%q0q3 = %q0 * %q3;
	%q1q3 = %q1 * %q3;
	%q0q2 = %q0 * %q2;
	%q2q2 = %q2 * %q2;
	%q2q3 = %q2 * %q3;
	%q0q1 = %q0 * %q1;
	%q3q3 = %q3 * %q3;
	%m13 = 2.0 * (%q1q3 - %q0q2);
	%m21 = 2.0 * (%q1q2 - %q0q3);
	%m22 = 2.0 * %q0q0 - 1.0 + 2.0 * %q2q2;
	%m23 = 2.0 * (%q2q3 + %q0q1);
	%m33 = 2.0 * %q0q0 - 1.0 + 2.0 * %q3q3;
	return mRadToDeg(mAsin(%m23)) SPC mRadToDeg(mAtan(-%m13, %m33)) SPC mRadToDeg(mAtan(-%m21, %m22));
}