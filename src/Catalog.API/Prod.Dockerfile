FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Catalog.API/Catalog.API.csproj", "src/Catalog.API/"]
COPY ["src/Catalog.Contracts/Catalog.Contracts.csproj", "src/Catalog.Contracts/"]
COPY ["src/Catalog.Data/Catalog.Data.csproj", "src/Catalog.Data/"]

RUN --mount=type=secret,id=GH_OWNER,dst=/GH_OWNER --mount=type=secret,id=GH_PAT,dst=/GH_PAT \
    dotnet nuget add source --username USERNAME --password `cat /GH_PAT` --store-password-in-clear-text --name github "https://nuget.pkg.github.com/`cat /GH_OWNER`/index.json"

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
