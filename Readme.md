# Introduction

This is a simple demo shows how to develop Web API using .NET Core 3.1 and Web app using Angular 12,
and deploy both of them to Azure using ARM and Azure CLI.

# Run locally

## Backend Api

Go to *Api\Api* folder and execute

`dotnet run`

Visit http://localhost:5000/temperature/convert/0/Celsius

Change temperature and the "from" unit (Celsius / Fahrenheit / Kelvin) to execute a GET request.

## Client Web app

Go to *ClientApp* folder and execute

`ng serve`

Visit http://localhost:4200


# Build and deploy to Azure

## Prerequisites

- [PowerShell](https://docs.microsoft.com/en-us/powershell/)
- [AzureRM PowerShell Module](https://www.powershellgallery.com/packages/AzureRM)
- [AZ CLI](https://docs.microsoft.com/en-us/cli/azure/)

## Setup infrastructure

Open *Infrastructure\deploy-local.ps1* and change **$SubscriptionId** and **$TenantId** variables.

This will push an ARM template to Azure to setup whole stack.

Below instructions are using default configurations in *deploy-local.ps1*. If resources are renamed, please update the parameters when execute below commands.

The paths are in Windows format. So they need to be converted to forward slash for other environments.

Execute

`.\deploy-local.ps1`

to set up new Resource Group, Storage account, App Service plan and App Service. This process can take a few minutes.

(You will be prompted to sign in when you execute it for the first time. A *profile.json* file will be created so you don't need to sign in again.)

![Setup infrastructure](ReadmePictures/SetupInfrastructure.png?raw=true)

## Backend API

Go to *Api\Api* folder

### Build

Execute

`dotnet publish --configuration Release`

to build the project in release mode.

### Package

Execute

`Compress-Archive -Force -Path .\bin\Release\netcoreapp3.1\publish\* -DestinationPath .\bin\Release\netcoreapp3.1\publish.zip`

to compress built binary files into a single zip file.

### Deploy

`az webapp deployment source config-zip --resource-group test-TemperatureConverter --name test-TemperatureConverter --src .\bin\Release\netcoreapp3.1\publish.zip`

![BackendAPI](ReadmePictures/BackendAPI.png?raw=true)

### Check

The API should be up and running by now. You can check it by visiting

https://test-temperatureconverter.azurewebsites.net/temperature/convert/0/Celsius

or execute

`curl https://test-temperatureconverter.azurewebsites.net/temperature/convert/0/Celsius`

![BackendAPI2](ReadmePictures/BackendAPI2.png?raw=true)

## Client Web app

Go to *ClientApp* folder

If you have modified the *$AppName* or *$AppEnvironment* variables, you need to update *apiUrl* value in *src\environments\environment.prod.ts*'.

### Build

Execute

`ng build`

to build the project in production mode.

### Configure (just once)

Execute `az login` to sign in.

Execute

`az storage blob service-properties update --account-name testtemperatureconverter --static-website  --index-document index.html`

to enable Static website feature for the storage account.

### Deploy

Execute

`az storage blob upload-batch -s dist\ClientApp -d '$web' --account-name testtemperatureconverter`

to push built binaries to Storage Account.

![WebApp](ReadmePictures/WebApp.png?raw=true)

### Check

Execute

`az storage account show -g test-TemperatureConverter -n testtemperatureconverter`

to get Primary endpoint's Web URL (the last entry).

![WebApp2](ReadmePictures/WebApp2.png?raw=true)

Visit the Web url. The site should be up and running. 