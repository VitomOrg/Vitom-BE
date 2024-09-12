# Use the official Microsoft ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official Microsoft .NET SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0.401-1-alpine3.20-amd64 AS build
WORKDIR /app/src

ENV ConnectionStrings__DB="Host=postgres_db;Port=5432;User ID=postgres;Password=postgres;Database=VitomDB;TrustServerCertificate=true"
ENV ConnectionStrings__Cache="redis:6379"

# Copy the solution file and restore dependencies
COPY ["src/API/API.csproj", "API/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/Persistence/Persistence.csproj", "Persistence/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]

RUN dotnet restore "API/API.csproj"

# Copy the rest of the project files and build the project
COPY . /app

WORKDIR /app
RUN dotnet build

FROM build AS publish
WORKDIR /app/src/API
RUN dotnet publish API.csproj -c Release --no-restore -o /app/publish

# Create a new image from the ASP.NET Core runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define the entry point for the container
ENTRYPOINT ["dotnet", "API.dll"]