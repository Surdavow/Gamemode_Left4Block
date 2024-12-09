$colorNames = "red burgundy darkorange trueorange lightpink pink magenta violet purple darkpurple blue darkblue lightblue cyan chartreusegreen limegreen mint truegreen darkgreen yellow brown chocolate gray lightgray darkgray lightergray white black pitchblack";
$colorValues["red"] = "1 0 0 1";
$colorValues["burgundy"] = "0.59 0 0.14 1";
$colorValues["darkorange"] = "0.9 0.34 0.08 1";
$colorValues["trueorange"] = "1 0.5 0 1";
$colorValues["lightpink"] = "1 0.75 0.8 1";
$colorValues["pink"] = "1 0.5 1 1";
$colorValues["magenta"] = "1 0 1 1";
$colorValues["violet"] = "0.5 0 1 1";
$colorValues["purple"] = "0.5 0 0.5 1";
$colorValues["darkpurple"] = "0.243 0.093 1 1";
$colorValues["blue"] = "0 0 1 1";
$colorValues["darkblue"] = "0.0 0.14 0.33 1";
$colorValues["lightblue"] = "0.11 0.46 0.77 1";
$colorValues["cyan"] = "0 1 1 1";
$colorValues["chartreusegreen"] = "0.5 1 0 1";
$colorValues["limegreen"] = "0.75 1 0 1";
$colorValues["mint"] = "0.5 1 0.65 1";
$colorValues["truegreen"] = "0 1 0 1";
$colorValues["darkgreen"] = "0 0.5 0.25 1";
$colorValues["yellow"] = "1 1 0 1";
$colorValues["brown"] = "0.39 0.2 0 1";
$colorValues["chocolate"] = "0.22 0.07 0 1";
$colorValues["gray"] = "0.5 0.5 0.5 1";
$colorValues["lightgray"] = "0.75 0.75 0.75 1";
$colorValues["darkgray"] = "0.2 0.2 0.2 1";
$colorValues["lightergray"] = "0.89 0.89 0.89 1";
$colorValues["white"] = "1 1 1 1";
$colorValues["black"] = "0.078 0.078 0.078 1";
$colorValues["pitchblack"] = "0 0 0 1";

function getColorName(%RGBA)
{
    %closestColor = "";
    %minDistance = 99999;

    for(%i = 0; %i < getWordCount($colorNames); %i++)
    {
        %currentColor = getWord($colorNames, %i);
        %currentColorRGBA = $colorValues[%currentColor];
        %distance = VectorDist(%RGBA, %currentColorRGBA);

        if(%distance <= 0.1) return %currentColor;
		else if(%distance < %minDistance)
        {
            %minDistance = %distance;
            %closestColor = %currentColor;
        }
    }

    return %closestColor;
}

function L4B_isInFOV(%viewer, %target)
{    
    if(isObject(%viewer) && isObject(%target)) return vectorDot(%viewer.getEyeVector(), vectorNormalize(vectorSub(%target.getPosition(), %viewer.getPosition()))) >= 0.7;
}

function miniGameFriendlyFire(%objA,%objB)//Return true to indicate if we are firing on a friendly
{    
    if(%objA.getClassName() $= "GameConnection") %TargetA = %objA.player;
    else %TargetA = %objA;
    
    if(%objB.getClassName() $= "GameConnection") %TargetB = %objB.player;
    else %TargetB = %objB;
    
    if(%TargetA !$= %TargetB && %TargetA.hType $= %TargetB.hType) return true;
    else return false;
}

function GunImages_GenerateSideImages()
{
	for(%i = 0; %i < DatablockGroup.getCount(); %i++)	
	if(isObject(%item = DatablockGroup.getObject(%i)))
	{
		if(%item.L4Bitemslot $= "Secondary")
		{
			if(%item.image.meleeDamageDivisor) %mount = 2;
			else %mount = 1;

			%eval = %eval @ "datablock ShapeBaseImageData(" @ %item.image.getName() @ "GunImagesSIDE)";
			%eval = %eval @ "{";
				%eval = %eval @ "shapeFile = \"" @ %item.image.shapeFile @ "\";";
				%eval = %eval @ "doColorShift = " @ (%item.image.doColorShift && 1) @ ";";
				%eval = %eval @ "colorShiftColor = \"" @ %item.image.colorShiftColor @ "\";";
				%eval = %eval @ "offset = \"0 0 0\";";
				%eval = %eval @ "mountPoint = " @ %mount @ ";";
			%eval = %eval @ "};";
			eval(%eval);
		}

		if(%item.L4Bitemslot $= "Grenade")
		{			
			%eval = %eval @ "datablock ShapeBaseImageData(" @ %item.image.getName() @ "GunImagesSIDE)";
			%eval = %eval @ "{";
				%eval = %eval @ "shapeFile = \"" @ %item.image.shapeFile @ "\";";
				%eval = %eval @ "doColorShift = " @ (%item.image.doColorShift && 1) @ ";";
				%eval = %eval @ "colorShiftColor = \"" @ %item.image.colorShiftColor @ "\";";
				%eval = %eval @ "offset = \"0 0 0\";";
				%eval = %eval @ "mountPoint = 3;";
			%eval = %eval @ "};";
			eval(%eval);
		}
	}	

}

function configLoadL4BTXT(%file,%svartype)//Set up custom variables
{
	%read = new FileObject();
	if(!isFile("config/server/Left4Block/" @ %file @ ".txt"))
	{
		%read.openForRead("add-ons/gamemode_left4block/config/" @ %file @ ".txt");

		%write = new FileObject();
		%write.openForWrite("config/server/Left4Block/" @ %file @ ".txt");
	
		while(!%read.isEOF())
		{
			%line = %read.readLine();
			%write.writeLine(%line);
		}

		%write.close();
		%write.delete();
	}

	%read.openForRead("config/server/Left4Block/" @ %file @ ".txt");

	while(!%read.isEOF())
	{
		%i++;
		%line = %read.readLine(); 
		eval("$" @ %svartype @"[%i] = \"" @ %line @ "\";");
		eval("$" @ %svartype @"Amount = %i;");
	}
	
	%read.close();
	%read.delete();
}

function configLoadL4BItemScavenge()//Set up items
{
	%read = new FileObject();
	if(!isFile("config/server/Left4Block/itemscavenge.txt"))
	{
		%read.openForRead("add-ons/gamemode_left4block/config/itemscavenge.txt");

		%write = new FileObject();
		%write.openForWrite("config/server/Left4Block/itemscavenge.txt");
	
		while(!%read.isEOF())
		{
			%line = %read.readLine();
			%write.writeLine(%line);
		}

		%write.close();
		%write.delete();
	}

	%read.openForRead("config/server/Left4Block/itemscavenge.txt");

	while(!%read.isEOF())
	{
		%i++;
		%line = %read.readLine(); 

		%itemremoveword = strreplace(%line, getWord(%line,0) @ " ", "");
		%previousline[%i] = getWord(%line,0);

		if(%previousline[%i] $= %previousline[mClamp(%i-1, 1, %i)])
		{
			%j++;
			eval("$" @ getWord(%line,0) @"[%j] = \"" @ %itemremoveword @ "\";");
			eval("$" @ getWord(%line,0) @"Amount = %j;");
		}
		else 
		{
			eval("$" @ getWord(%line,0) @"[1] = \"" @ %itemremoveword @ "\";");
			%j = 1;
		}

		for (%d = 0; %d < DatablockGroup.getCount(); %d++) 
		{
			%datablock = DatablockGroup.getObject(%d);

			if(%datablock.getClassName() $= "ItemData")
			if(%datablock.uiName $= %itemremoveword)
			{	
				%item = %datablock;
				eval("$" @ getWord(%line,0) @"[%j] = \"" @ %item.getName() @ "\";");
			}
		}
	}
	%read.close();
	%read.delete();
}

function configLoadL4BItemSlots()
{
	%read = new FileObject();
	if(!isFile("config/server/Left4Block/itemslots.txt"))
	{
		%read.openForRead("Add-Ons/Gamemode_Left4Block/config/itemslots.txt");
		%write = new FileObject();
		%write.openForWrite("config/server/Left4Block/itemslots.txt");
		
		while(!%read.isEOF())
		{
			%line = %read.readLine();
			%write.writeLine(%line);
		}
		
		%read.close();
		%write.close();
		%write.delete();
	}
	
	%read.openForRead("config/server/Left4Block/itemslots.txt");
	
	while(!%read.isEOF())
	{
		%i++;
		%line = %read.readLine(); 

		%firstword = getWord(%line,0);
		%itemremoveword = strreplace(%line, %firstword @ " ", "");

		for (%d = 0; %d < DatablockGroup.getCount(); %d++) 
		{
			%datablock = DatablockGroup.getObject(%d);
			if(%datablock.getClassName() $= "ItemData" && %datablock.uiName $= %itemremoveword) %datablock.L4Bitemslot = %firstword;
		}
	}
	%read.close();
	%read.delete();
}