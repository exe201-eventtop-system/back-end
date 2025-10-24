# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt-get update && apt-get install -y krb5-locales && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
COPY API/ocelot.json .
# COPY ../../.env .env
ENTRYPOINT ["dotnet", "API.dll"] 