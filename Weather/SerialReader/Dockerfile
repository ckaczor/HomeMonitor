FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-arm32v7 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
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