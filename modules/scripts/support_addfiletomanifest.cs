function addFileToManifest(%fileName)
{
	if(!ServerGroup.addedExtraResource[%fileName])
	{
		if(ServerGroup.extraResourceCount $= "")ServerGroup.extraResourceCount = 0;

		ServerGroup.extraResource[ServerGroup.extraResourceCount] = %fileName;
		ServerGroup.extraResourceCount++;
		ServerGroup.addedExtraResource[%fileName] = true;
	}
}

package L4B_AddFileToManifest
{
	function EnvGuiServer::PopulateEnvResourceList()
	{
		Parent::PopulateEnvResourceList();

		for(%i = 0; %i < ServerGroup.extraResourceCount; %i++)
		{
			$EnvGuiServer::Resource[$EnvGuiServer::ResourceCount] = ServerGroup.extraResource[%i];
			$EnvGuiServer::ResourceCount++;
		}
	}
};
activatePackage(L4B_AddFileToManifest);