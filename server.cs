if(ForceRequiredAddOn("Bot_Hole") == $Error::AddOn_NotFound) return error("Bot_Hole is required for Gamemode_Left4Block to work");
if(ForceRequiredAddOn("Projectile_Radio_Wave") == $Error::AddOn_NotFound) return error("Projectile_Radio_Wave is required for Gamemode_Left4Block to work");
if(ForceRequiredAddOn("Weapon_Spear") == $Error::AddOn_NotFound) return error("Weapon_Spear is required for Gamemode_Left4Block to work");
if(ForceRequiredAddOn("Weapon_Push_Broom") == $Error::AddOn_NotFound) return error("Weapon_Push_Broom is required for Gamemode_Left4Block to work");
if(ForceRequiredAddOn("Weapon_Gun") == $Error::AddOn_NotFound) return error("Weapon_Gun is required for Gamemode_Left4Block to work");

exec("./modules/scripts/module_scripts.cs");
exec("./modules/misc/module_misc.cs");

if($RTB::Hooks::ServerControl)//Return to Blockland Preferences
{            
	RTB_registerPref("Blood damage threshold (0 to disable)",	"Left 4 Block - Blood","$Pref::L4B::Blood::BloodDamageThreshold",	"int 0 1000","Gamemode_Left4Block","15","0","0","Gamemode_Left4Block_BloodDamageThreshold");
	RTB_registerPref("Blood dismember threshold (0 to disable)",	"Left 4 Block - Blood","$Pref::L4B::Blood::BloodDismemberThreshold",	"int 0 1000","Gamemode_Left4Block","75","0","0","Gamemode_Left4Block_BloodDismemberThreshold");
	RTB_registerPref("Blood decals limit (0 to disable)",	"Left 4 Block - Blood","$Pref::L4B::Blood::BloodDecalsLimit",	"int 0 1000","Gamemode_Left4Block","100","0","0","Gamemode_Left4Block_BloodDecalsLimit");
	RTB_registerPref("Blood decals timeout (ms)",	"Left 4 Block - Blood","$Pref::L4B::Blood::BloodDecalsTimeout",	"int 1 30000","Gamemode_Left4Block","5000","0","0","Gamemode_Left4Block_BloodDecalsTimeout");
}

if($Pref::L4B::Blood::BloodDismemberThreshold $= "") $Pref::L4B::Blood::BloodDismemberThreshold = 15;
if($Pref::L4B::Blood::BloodDecalsLimit $= "") $Pref::L4B::Blood::BloodDecalsLimit = 100;
if($Pref::L4B::Blood::BloodDecalsTimeout $= "") $Pref::L4B::Blood::BloodDecalsTimeout = 5000;
//Map rotation is not integrated yet
//$Pref::L4B::MapRotation::Enabled = true;
//$Pref::L4B::MapRotation::RequiredVotes = 0;
//$Pref::L4B::MapRotation::RequiredNext = 2;
//$Pref::L4B::MapRotation::RequiredReload = 2;
//$Pref::L4B::MapRotation::VoteMin = 5;
//$Pref::L4B::MapRotation::MinReset = 5;
//$Pref::L4B:MapRotation::CoolDown = 10;
$Pref::L4B::Zombies::NormalDamage = 5;
$Pref::L4B::Zombies::SpecialsDamage = $Pref::L4B::Zombies::NormalDamage*3;

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

$L4B_CurrentMonth = getWord(strreplace(getDateTime(), "/", " "),0);
$RBloodLimbString0 = "headskin helmet bicorn visor shades gloweyes ballistichelmet constructionhelmet constructionhelmetbuds gloweyeL gloweyeR copHat caphat detective fancyhat fedora detectiveHat knitHat scoutHat fedoraHat shades gasmask";
$RBloodLimbDismemberString0 = "headpart1 headpart3 headpart4 headpart5 headpart6 headskullpart1 headskullpart3 headskullpart4 headskullpart5 headskullpart6";
$RBloodLimbString1 = "femchest chest clothstrap armor bucket cape pack quiver tank";
$RBloodLimbDismemberString1 = "chestpart1 chestpart2 chestpart3 chestpart4 chestpart5 chestpart6 chestpart7 chestpart8";
$RBloodLimbDismemberStringF1 = "femchestpart1 femchestpart2 femchestpart3 femchestpart4 femchestpart5 femchestpart6 femchestpart7 femchestpart8";
$RBloodLimbString2 = "rarm";
$RBloodLimbString3 = "larm";
$RBloodLimbString4 = "rhand";
$RBloodLimbString5 = "lhand";
$RBloodLimbString6 = "pants";
$RBloodLimbString7 = "rshoe";
$RBloodLimbString8 = "lshoe";
$L4BHat[$L4BHatAmount++] = "helmet";
$L4BHat[$L4BHatAmount++] = "capHat";
$L4BHat[$L4BHatAmount++] = "detectiveHat";
$L4BHat[$L4BHatAmount++] = "scoutHat";
$L4BHat[$L4BHatAmount++] = "fancyHat";
$L4BHat[$L4BHatAmount++] = "copHat";
$L4BHat[$L4BHatAmount++] = "knitHat";
$L4BHat[$L4BHatAmount++] = "fedoraHat";
$hZombieSkin[$hZombieSkinAmount++] = "0.16 0.25 0.21 1";
$hZombieSpecialType[$hZombieSpecialTypeAmount++] = "ZombieChargerHoleBot";
$hZombieSpecialType[$hZombieSpecialTypeAmount++] = "ZombieBoomerHoleBot";
$hZombieSpecialType[$hZombieSpecialTypeAmount++] = "ZombieSpitterHoleBot";
$hZombieSpecialType[$hZombieSpecialTypeAmount++] = "ZombieHunterHoleBot";
$hZombieSpecialType[$hZombieSpecialTypeAmount++] = "ZombieSmokerHoleBot";
$hZombieSpecialType[$hZombieSpecialTypeAmount++] = "ZombieJockeyHoleBot";
$hZombieUncommonType[$hZombieUncommonTypeAmount++] = "ZombieConstructionHoleBot";
$hZombieUncommonType[$hZombieUncommonTypeAmount++] = "ZombieFallenHoleBot";
$hZombieUncommonType[$hZombieUncommonTypeAmount++] = "ZombieCedaHoleBot";
$hZombieUncommonType[$hZombieUncommonTypeAmount++] = "ZombieSoldierHoleBot";
$hZombieUncommonType[$hZombieUncommonTypeAmount++] = "ZombiePoliceHoleBot";
configLoadL4BItemScavenge();
configLoadL4BItemSlots();
GunImages_GenerateSideImages();