%path = "./*.cs";
for(%file = findFirstFile(%path); %file !$= ""; %file = findNextFile(%path))
{
	if(strstr(filename(%file),"module_scripts") != -1) continue;
	exec(%file);
}