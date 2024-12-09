exec("./L4B_GUI.gui");

if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
	if(!$RTB::RTBR_InfoTips_Hook) exec("Add-Ons/System_ReturnToBlockland/RTBR_InfoTips_Hook.cs");		
	RTB_addInfoTip("Using distractable items against common zombie hordes can quickly dispose of them, especially pipebombs.", "", "Left4Block");
}

//function RaddToMinigameGui(%target)
//{
//	for (%i = 0; %i < CMG_ScrollBox.getCount(); %i++) 
//	if((%object = CMG_ScrollBox.getObject(%i)).getName() $= "L4B_MinigameGui") 
//	{
//		echo(%object.extent);
//		return;
//	}
//
//	%minigamePosX = getword(CMG_ScrollBox.position,0);
//	%MinigameExtent = CMG_ScrollBox.extent;
//	%minigameExtX = getword(%MinigameExtent,0);
//	%minigameExtY = getword(%MinigameExtent,1);
//	%TargetExtY = getword(%target.extent,1);
//
//	CMG_ScrollBox.extent = %minigameExtX SPC %minigameExtY+%TargetExtY;
//
//	%lastMinigameGui = CMG_ScrollBox.getobject(CMG_ScrollBox.getcount()-1);
//	%lastMinigamePosY = getword(%lastMinigameGui.position,1);
//	%lastMinigameExtent = %lastMinigameGui.extent;
//	%lastMinigameExtentY = getword(%lastMinigameExtent,1);
//
//	CMG_ScrollBox.add(%target);
//	%target.position = %minigamePosX SPC %lastMinigamePosY+%lastMinigameExtentY;
//}
//RaddToMinigameGui(L4B_MinigameGui);
//package MinigameZombieGuiAddPackage
//{
//	function CreateMinigameGUI::onWake(%this,%a,%b,%c)
//	{
//		Parent::onWake(%this,%a,%b,%c);
//		if($SpaceMods::Client::TeamDM::serverHasMod && $zombies::adjustedTDM != 1)
//		{
//			$zombies::adjustedTDM = 1;
//			L4B_MinigameGui.position = vectoradd(ZombieMinigameGui.position,"0 65");
//		}
//	}
//};
//activatePackage(MinigameZombieGuiAddPackage);