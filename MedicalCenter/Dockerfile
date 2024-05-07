# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
COPY MedicalCenter.Api/MedicalCenter.Api.csproj MedicalCenter.Api/MedicalCenter.Api.csproj
COPY MedicalCenter.Business/MedicalCenter.Business.csproj MedicalCenter.Business/MedicalCenter.Business.csproj
COPY MedicalCenter.Data/MedicalCenter.Data.csproj MedicalCenter.Data/MedicalCenter.Data.csproj
RUN dotnet restore MedicalCenter.Api/MedicalCenter.Api.csproj

# Copy the rest of the application code
COPY . ./

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose the port your application will run on
EXPOSE 8080
EXPOSE 8081

RUN apt-get update
RUN apt-get --yes install curl

# Start the application
ENTRYPOINT ["dotnet", "MedicalCenter.Api.dll"]