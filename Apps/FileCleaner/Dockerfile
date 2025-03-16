FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Apps/FileCleaner/ConsoleApplication/ConsoleApplication.csproj", "Apps/FileCleaner/ConsoleApplication/"]
COPY ["Apps/FileCleaner/Service/Service.csproj", "Apps/FileCleaner/Service/"]
COPY ["Packages/Logger/Logger.csproj", "Packages/Logger/"]
COPY ["Packages/Events/Events.csproj", "Packages/Events/"]
RUN dotnet restore "Apps/FileCleaner/ConsoleApplication/ConsoleApplication.csproj"

COPY Apps/FileCleaner/ConsoleApplication/ Apps/FileCleaner/ConsoleApplication/
COPY Apps/FileCleaner/Service/ Apps/FileCleaner/Service/
COPY Packages/Events/ Packages/Events/
COPY Packages/Logger/ Packages/Logger/

WORKDIR "Apps/FileCleaner/ConsoleApplication"
RUN dotnet build "ConsoleApplication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ConsoleApplication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApplication.dll"]
