FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./SerialReader.csproj", "./"]
RUN dotnet restore -r linux-arm "SerialReader.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet publish -r linux-arm --self-contained=false "SerialReader.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ChrisKaczor.HomeMonitor.Weather.SerialReader.dll"]