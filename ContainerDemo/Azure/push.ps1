$region = "Central US"
$resourceGroupName = "PW2021Demos"
$registryName = "pw2021acr"
$containerImage = "containerdemo"
$containerRegistryTag = "pw2021acr.azurecr.io/containerdemo:latest"

if ((Get-AzADUser -ErrorAction SilentlyContinue).Count -lt 1) {
	Write-Output "No Azure session or expired. Prompting for login..."
	Connect-AzAccount
}

Write-Output "Connecting to Azure Container Registry..."
while ($false -eq (Connect-AzContainerRegistry -Name $registryName))
{
  Write-Output "Container Registry initializing, Waiting 5 seconds before retrying..."
  Start-Sleep -Seconds 5
}

Write-Output "Applying registry tag [$($containerRegistryTag)] to local container '$($containerImage)'..."
docker tag $containerImage $containerRegistryTag

Write-Output "Pushing Docker container '$($containerImage)' to Azure Container Registry..."
docker push $containerRegistryTag

Write-Output "Restarting Container Instance Group..."
$cg = Get-AzContainerGroup -ResourceGroupName $resourceGroupName -Name $containerImage
Invoke-AzResourceAction -ResourceId $cg.Id -Action Restart -Force
