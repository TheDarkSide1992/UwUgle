FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

#Sets Dotnet Envirement
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

#Copy Files From porjects
COPY ["Apps/FileReader/ConsoleApplication/ConsoleApplication.csproj", "Apps/FileReader/ConsoleApplication/"]
COPY ["Apps/FileReader/Infrastructure/Infrastructure.csproj", "Apps/FileReader/Infrastructure/"]
COPY ["Apps/FileReader/Service/Service.csproj", "Apps/FileReader/Service/"]
COPY ["Packages/Logger/Logger.csproj", "Packages/Logger/"]
COPY ["Packages/Events/Events.csproj", "Packages/Events/"]
RUN dotnet restore "Apps/FileReader/ConsoleApplication/ConsoleApplication.csproj"

#Copy Files to Docker
COPY Apps/FileReader/ConsoleApplication/ Apps/FileReader/ConsoleApplication/
COPY Apps/FileReader/Infrastructure/ Apps/FileReader/Infrastructure/
COPY Apps/FileReader/Service/ Apps/FileReader/Service/
COPY Packages/Logger/ Packages/Logger/
COPY Packages/Events/ Packages/Events/

WORKDIR "Apps/FileReader/ConsoleApplication"
RUN dotnet build "ConsoleApplication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ConsoleApplication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApplication.dll"]
