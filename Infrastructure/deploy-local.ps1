# This script will create Azure resources in the subscription defined below.
# Make sure to add your own Azure SubscriptionId below to test in your Visual Studio subscription.
# This script is intended to be execued from local development environment.
$SubscriptionId = "YOUR_AZURE_SUBSCRIPTION_ID"
$TenantId = "YOUR_AZURE_TENANT_ID"
$AppName = "TemperatureConverter"
$Location = "Australia Southeast"
$AppEnvironment = "test"
$AppServicePlanResourceGroup = "test-temperature-converter"
$AppServicePlanName = "DefaultAppServicePlan"

$ErrorActionPreference = "Stop"

$profile = Import-AzureRmContext -Path "$PSScriptRoot\profile.json" -ErrorAction SilentlyContinue
if (-not $profile.Context) {
    Login-AzureRmAccount -TenantId $TenantId | Out-Null
    Save-AzureRmContext -Path "$PSScriptRoot\profile.json"
}

. $PSScriptRoot\Deploy.ps1 `
    -AlreadyLoggedIn `
    -TenantId $TenantId `
    -SubscriptionId $SubscriptionId `
    -Location $Location `
    -AppName $AppName `
    -AppEnvironment $AppEnvironment `
    -AppServicePlanResourceGroup $AppServicePlanResourceGroup `
    -AppServicePlanName $AppServicePlanName