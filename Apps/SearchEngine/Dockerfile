FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
#Sets Dotnet Envirement
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

#COpy Files From porjects
COPY ["Apps/SearchEngine/API/API.csproj", "Apps/SearchEngine/API/"]
COPY ["Apps/SearchEngine/Service/Service.csproj", "Apps/SearchEngine/Service/"]
COPY ["Apps/SearchEngine/Infrastructure/Infrastructure.csproj", "Apps/SearchEngine/Infrastructure/"]
RUN dotnet restore "Apps/SearchEngine/API/API.csproj"

#COpy Files to Docker
COPY Apps/SearchEngine/API/ Apps/SearchEngine/API/
COPY Apps/SearchEngine/Service/ Apps/SearchEngine/Service/
COPY Apps/SearchEngine/Infrastructure/ Apps/SearchEngine/Infrastructure/

WORKDIR "Apps/SearchEngine/API"
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
