# Use the official Microsoft ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official Microsoft .NET SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app/src

# Copy the solution file and restore dependencies
COPY ["src/API/API.csproj", "API/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/Persistence/Persistence.csproj", "Persistence/"]

RUN dotnet restore "API/API.csproj"

# Copy the rest of the project files and build the project
COPY . /app

WORKDIR /app
RUN dotnet build

FROM build AS publish
WORKDIR /app/src/API
RUN dotnet publish -c Release --no-restore -o /app/publish

# Create a new image from the ASP.NET Core runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define the entry point for the container
ENTRYPOINT ["dotnet", "API.dll"]