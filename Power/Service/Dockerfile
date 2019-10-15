FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-alpine AS build
WORKDIR /src
COPY ["./Service.csproj", "./"]
RUN dotnet restore "Service.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet publish "Service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENTRYPOINT ["dotnet", "ChrisKaczor.HomeMonitor.Power.Service.dll"]