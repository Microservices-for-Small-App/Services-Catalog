#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Catalog.API/Catalog.API.csproj", "src/Catalog.API/"]
COPY ["src/Catalog.Contracts/Catalog.Contracts.csproj", "src/Catalog.Contracts/"]
COPY ["src/Catalog.Data/Catalog.Data.csproj", "src/Catalog.Data/"]
RUN dotnet restore "src/Catalog.API/Catalog.API.csproj"
COPY ./src ./src
WORKDIR "/src/src/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]