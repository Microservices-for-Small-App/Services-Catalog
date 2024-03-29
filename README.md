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
