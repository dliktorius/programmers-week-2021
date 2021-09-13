$region = "Central US"
$resourceGroupName = "PW2021Demos"
$registryName = "pw2021acr"
$containerImage = "containerdemo"
$containerRegistryTag = "pw2021acr.azurecr.io/containerdemo:latest"

if ((Get-AzADUser -ErrorAction SilentlyContinue).Count -lt 1) {
	Write-Output "No Azure session or expired. Prompting for login..."
	Connect-AzAccount
}

Write-Output "Creating resource group (if missing)..."
New-AzResourceGroup -Name $resourceGroupName -Location $region -Force

Write-Output "Creating/Updating Azure Container Registry..."
New-AzResourceGroupDeployment -Name ContainerRegistryDeployment -ResourceGroupName $resourceGroupName `
  -TemplateFile .\container-registry-template.json `
  -TemplateParameterFile .\container-registry-parameters.json

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

Write-Output "Getting Azure Container Registry credentials..."
$acrCreds = Get-AzContainerRegistryCredential -ResourceGroupName "PW2021Demos" -Name "pw2021acr"

Write-Output "Creating/Updating Azure Container Instance Group..."
New-AzResourceGroupDeployment -Name ContainerInstanceDeployment -ResourceGroupName PW2021Demos `
  -TemplateFile .\container-instance-template.json `
  -TemplateParameterFile .\container-instance-parameters.json `
  -imagePassword (ConvertTo-SecureString $acrCreds.Password -AsPlainText -Force)
