datablock StaticShapeData(elevatorStaticShape)
{
	density = "1";
	drag = "0";
	dynamicType = "0";
	emap = "0";
	mass = "1";
	isInvincible = true;

	shapeFile = "./models/shape_elevator.dts";
};

datablock fxDTSBrickData(ElevatorBrick8x8)
{
    brickFile = "base/data/bricks/flats/8x8F.blb";
    category = "Special";
    subCategory = "Interactive";
    uiName = "Elevator";
	iconName = "add-ons/gamemode_left4block/modules/misc/models/icons/icon_elevator";
    isElevator = true;
	indestructable = true;
	alwaysShowWireFrame = false;
};

function fxDTSBrick::elevatorCreate(%this)
{
    if(!isObject(%this.elevatorGlass) && %this.getDatablock().brickSizeZ == 1)
    {
		%this.elevatorGlass = new StaticShape()
		{
		    scale = "2 2 1";
		    position = %this.getPosition();
		    canSetIFLs = false;
		    datablock = "elevatorStaticShape";
		    elevatorBrick = %this;
		    indestructable = true;
		};

		%this.setRendering(0);
		%this.setColliding(0);
		%this.setRaycasting(0);
		%this.isElevator = true;
    }
}

function fxDTSBrick::elevatorReset(%this)
{
    if(isObject(%this.elevatorGlass))
    {
		cancel(%this.elevatorGlass.schedule);
		%this.elevatorGlass.setTransform(%this.getPosition());
    }
}

function fxDTSBrick::elevatorMove(%this, %movement)
{
    if(%this.isElevator)
    {
		%this.elevatorGlass.moveTotal = mAbs(%movement);
		%this.elevatorGlass.moveDone = 0;

		if(%movement > 0) %dir = 1;
		if(%movement < 0) %dir = -1;

		%this.elevatorGlass.moveDir = %dir;
		%this.moveElevatorGlass();
    }
}

function fxDTSBrick::elevatorGotoBrick(%this, %brickname, %client)
{
    if(%this.isElevator)
    {
		%group = getBrickGroupFromObject(%this);
		%dest = eval("return "@ %group @".NTObject_"@ %brickname @"_0;");
		if(isObject(%dest))
		{
		    if(%dest.getDatablock().brickSizeZ == 1)
		    {
			%zDest = getWord(%dest.getPosition(), 2);
			%z = getWord(%this.elevatorGlass.getPosition(), 2);
			%this.elevatorDestination = %dest;
			%this.elevatorMove((%zDest - %z)*5);
			%this.elevatorTrigClient = %client;
		    }
		}
    }
}

function fxDTSBrick::moveElevatorGlass(%this)
{
    cancel(%this.elevatorGlass.schedule);
    %g = %this.elevatorGlass;
    %mt = %g.moveTotal;
    %md = %g.moveDone;
    %mdir = %g.moveDir / 5;
    if(%md < %mt)
    {
		if(!%g.elevatorSound)
		{
			%g.playaudio(0,"elev1_sound");
			%g.elevatorSound = true;
		}

		%g.moveDone += 1;
		%t = %g.getTransform();
		%tx = getWord(%t, 0);
		%ty = getWord(%t, 1);
		%tz = getWord(%t, 2);

		%objCount = 0;
		%searchdb["Player"] = 1;
		%searchdb["AIPlayer"] = 1;
		%searchdb["WheeledVehicle"] = 1;
		%searchdb["Vehicle"] = 1;
		%brickX = %this.getDatablock().brickSizeX;
		%brickY = %this.getDatablock().brickSizeY;
		%radius = getMax(%brickX, %brickY) / 4;
		InitContainerRadiusSearch(%t, mSqrt(%radius*%radius+%radius*%radius)+0.1, $TypeMasks::ShapeBaseObjectType);
		while((%obj = containerSearchNext()) !$= 0)
		{
		    if(%searchdb[%obj.getClassName()] != 1) continue;

		    %x = getWord(%obj.getTransform(), 0);
		    %y = getWord(%obj.getTransform(), 1);
		    %z = getWord(%obj.getTransform(), 2);
		    %trialA = (%z - %tz) > 0;
		    %trialB = mAbs(%x - %tx) < %radius + 0.1;
		    %trialC = mAbs(%y - %ty) < %radius + 0.1;
		    if(%trialA && %trialB && %trialC)
		    {
				if(isObject(%obj.getObjectMount())) continue;
				%objMove[%objCount] = %obj;
				%objCount++;
		    }
		}

		for(%a = 0; %a < %objCount; %a++)
		{
		    %obj = %objMove[%a];
		    %transform = setWord(%obj.getTransform(), 2, getWord(%obj.getTransform(), 2) + %mdir);

		    if(getWord(%transform, 2) > 0) %obj.setTransform(%transform);
		}

		%t = setWord(%g.getTransform(), 2, getWord(%g.getTransform(), 2)+%mdir);

		if(getWord(%t, 2) > 0)
		{
			%g.setTransform(%t);
			%this.elevatorGlass.schedule = %this.schedule(75, moveElevatorGlass);
		}
    }

    else
    {
		%g.stopaudio(0);
		%g.playaudio(0,"elev1_sound");
		%g.elevatorSound = true;
		%this.elevatorDestination.processInputEvent("onElevatorArrive", %this.elevatorTrigClient);
		%this.elevatorDestination = "";
		%this.elevatorTrigClient = "";
    }
}

package ElevatorPackage
{
    function fxDTSBrick::onPlant(%this)
    {
		Parent::onPlant(%this);
		if(%this.getDatablock().isElevator) %this.elevatorCreate();
    }
    function fxDTSBrick::onLoadPlant(%this)
    {
		Parent::onPlant(%this);
		if(%this.getDatablock().isElevator) %this.elevatorCreate();
    }
    function fxDTSBrick::onRemove(%this)
    {
		Parent::onRemove(%this);
			if(%this.elevatorGlass)
			{
			    MissionCleanup.remove(%this.elevatorGlass);
			    %this.elevatorGlass.delete();
			}
    }
};
activatePackage(ElevatorPackage);

registerInputEvent("fxDTSBrick", "onElevatorArrive", "Self fxDTSBrick", 1);

registerOutputEvent("fxDTSBrick", "elevatorGotoBrick", "String 25 150");
registerOutputEvent("fxDTSBrick", "elevatorCreate");
registerOutputEvent("fxDTSBrick", "elevatorMove", "int -1000 1000 0");
registerOutputEvent("fxDTSBrick", "elevatorReset");