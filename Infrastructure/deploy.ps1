# This script will create Azure resources using below parameters.
# This script is intended to be used by a deployment server.
Param(
  [string]
  [Parameter(ParameterSetName = "SpecifyServicePrincipal", Mandatory = $true)]
  $ServicePrincipalId,

  [string]
  [Parameter(ParameterSetName = "SpecifyServicePrincipal", Mandatory = $true)]
  $ServicePrincipalPassword,

  [switch]
  [Parameter(ParameterSetName = "AlreadyLoggedIn", Mandatory = $true)]
  $AlreadyLoggedIn,

  [string] [Parameter(Mandatory = $true)] $SubscriptionId,
  [string] [Parameter(Mandatory = $true)] $TenantId,
  [string] [Parameter(Mandatory = $true)] $Location,
  [string] [Parameter(Mandatory = $true)] $AppName,
  [string] [Parameter(Mandatory = $true)] $AppEnvironment,
  [string] [Parameter(Mandatory = $true)] $AppServicePlanResourceGroup,
  [string] [Parameter(Mandatory = $true)] $AppServicePlanName,
  [string] $StorageType = "Standard_LRS",
  [string] $ResourceGroupName = "$AppEnvironment-$AppName"
)

function Get-Parameters() {
  return @{
    "appServicePlanName"               = $AppServicePlanName;
    "webAppName"                       = $ResourceGroupName;
    "storageName"                      = ($ResourceGroupName -replace "-", "").ToLower();
    "storageType"                      = $StorageType;
  }
}

try {
  Set-StrictMode -Version "Latest"
  $ErrorActionPreference = "Stop"

  if (-not $AlreadyLoggedIn.IsPresent) {
    Write-Output "Authenticating to ARM as service principal $ServicePrincipalId"
    $securePassword = ConvertTo-SecureString $ServicePrincipalPassword -AsPlainText -Force
    $servicePrincipalCredentials = New-Object System.Management.Automation.PSCredential ($ServicePrincipalId, $securePassword)
    Login-AzureRmAccount -ServicePrincipal -TenantId $TenantId -Credential $servicePrincipalCredentials | Out-Null
  }

  Write-Output "Selecting subscription $SubscriptionId"
  Select-AzureRmSubscription -SubscriptionId $SubscriptionId -TenantId $TenantId | Out-Null

  Write-Output "Ensuring resource group $ResourceGroupName exists"
  New-AzureRmResourceGroup -Location $Location -Name $ResourceGroupName -Force | Out-Null

  Write-Output "Creating parameters for ARM deployment"
  $Parameters = Get-Parameters
  $Parameters.GetEnumerator() | Sort-Object Name | ForEach-Object { ForEach-Object { "{0}`t{1}" -f $_.Name, ($_.Value -join ", ") } | Write-Verbose }

  Write-Output "Deploying resources to ARM"
  $result = New-AzureRmResourceGroupDeployment -ResourceGroupName $ResourceGroupName -TemplateFile "$PSScriptRoot\azuredeploy.json" -TemplateParameterObject $Parameters -Name ("$AppName-$AppEnvironment-" + (Get-Date -Format "yyyy-MM-dd-HH-mm-ss")) -ErrorAction Continue -Verbose
  Write-Output $result
  if ((-not $result) -or ($result.ProvisioningState -ne "Succeeded")) {
    throw "Deployment failed"
  }
}
catch {
  Write-Error $_ -ErrorAction Continue
  exit 1
}