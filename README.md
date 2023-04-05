# Services Catalog

.NET 7 Web API for Small Microservices Solution

## Dotnet Commands

```dotnetcli
dotnet new webapi -n Catalog.API

dotnet pack -o C:\LordKrishna\Packages\

jdbc:sqlserver://YourServer.database.windows.net:1433;database=YourDataBase;user=YourUser@YourServer.database.windows.net;password=Sample@123$;encrypt=true;hostNameInCertificate=*.database.windows.net;loginTimeout=30;
```

## Docker Commands

```dockerfile
docker run -d --rm --name store-mongo -p 27017:27017 -v mongo-db-persistance-store:/data/db -v mongo-db-config-store:/data/configdb mongo
```

## Physical Path

`\\wsl$\docker-desktop-data\data\docker\volumes`

![Docker Volumne Path](./documentation/images/DockerVolumesPath.PNG)

## Create and publish package to GitHub using PowerShell

```powershell
$version="1.0.5"
$owner="Microservices-for-Small-App"
$username="vishipayyallore"
$repo="Services-Catalog"
$libname="Catalog.Contracts"
$gh_pat="ghp_Your_GitHib_Classic_PAT"

dotnet clean
dotnet build -c Release
dotnet pack --configuration Release -o C:\LordKrishna\SSP\Packages

dotnet nuget push C:\LordKrishna\SSP\Packages\$libname.$version.nupkg --source "gHmicroservices" --api-key $gh_pat
```
