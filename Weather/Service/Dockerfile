FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["./Service.csproj", "./"]
RUN dotnet restore "Service.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet publish "Service.csproj" -c Release -o /app

FROM base AS final
RUN apk add --no-cache tzdata
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ChrisKaczor.HomeMonitor.Weather.Service.dll"]