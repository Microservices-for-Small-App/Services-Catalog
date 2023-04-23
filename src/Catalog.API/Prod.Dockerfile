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
RUN dotnet restore "src/Catalog.API/Catalog.API.csproj"
COPY ./src ./src
WORKDIR "/src/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
