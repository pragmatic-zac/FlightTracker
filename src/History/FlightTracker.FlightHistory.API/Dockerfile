#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["History/FlightTracker.FlightHistory.API/FlightTracker.FlightHistory.API.csproj", "History/FlightTracker.FlightHistory.API/"]
RUN dotnet restore "History/FlightTracker.FlightHistory.API/FlightTracker.FlightHistory.API.csproj"
COPY . .
WORKDIR "/src/History/FlightTracker.FlightHistory.API"
RUN dotnet build "FlightTracker.FlightHistory.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlightTracker.FlightHistory.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightTracker.FlightHistory.API.dll"]
