FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY TradingCardGame.Impl.InProcessConsoleClient/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish TradingCardGame.Impl.InProcessConsoleClient -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /app
COPY --from=build-env /app/TradingCardGame.Impl.InProcessConsoleClient/out .
ENTRYPOINT ["dotnet", "TradingCardGame.Impl.InProcessConsoleClient.dll"]