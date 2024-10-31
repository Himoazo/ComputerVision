# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy solution file and restore dependencies
COPY *.sln .
COPY ComputerVision.API/*.csproj ./ComputerVision.API/
COPY ComputerVision.Data/*.csproj ./ComputerVision.Data/
RUN dotnet restore

# Copy all project files
COPY ComputerVision.API/. ./ComputerVision.API/
COPY ComputerVision.Data/. ./ComputerVision.Data/

# Build and publish the main API project
WORKDIR /app/ComputerVision.API
RUN dotnet publish -c Release -o /app/out

# Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port the app runs on
EXPOSE 8080

# Entry point for the application
ENTRYPOINT ["dotnet", "ComputerVision.API.dll"]