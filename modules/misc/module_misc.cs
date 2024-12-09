exec("./datablocks_misc.cs");
exec("./player_survivor.cs");
exec("./bot_common.cs");
exec("./bot_hunter.cs");

%path = "./*.cs";
for(%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
	if(strstr(filename(%file),"module_misc") != -1) continue;
	if(strstr(filename(%file),"datablocks_misc") != -1) continue;
	if(strstr(filename(%file),"bot_common") != -1) continue;
	if(strstr(filename(%file),"bot_hunter") != -1) continue;
	if(strstr(filename(%file),"player_survivor") != -1) continue;
	
	exec(%file);
}