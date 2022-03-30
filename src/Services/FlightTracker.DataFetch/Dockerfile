#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/FlightTracker.DataFetch/FlightTracker.DataFetch.csproj", "Services/FlightTracker.DataFetch/"]
RUN dotnet restore "Services/FlightTracker.DataFetch/FlightTracker.DataFetch.csproj"
COPY . .
WORKDIR "/src/Services/FlightTracker.DataFetch"
RUN dotnet build "FlightTracker.DataFetch.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlightTracker.DataFetch.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightTracker.DataFetch.dll"]