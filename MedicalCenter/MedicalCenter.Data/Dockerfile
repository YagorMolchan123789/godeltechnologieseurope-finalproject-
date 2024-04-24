# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS Migrations
WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
COPY MedicalCenter.Api/MedicalCenter.Api.csproj MedicalCenter.Api/MedicalCenter.Api.csproj
COPY MedicalCenter.Business/MedicalCenter.Business.csproj MedicalCenter.Business/MedicalCenter.Business.csproj
COPY MedicalCenter.Data/MedicalCenter.Data.csproj MedicalCenter.Data/MedicalCenter.Data.csproj
COPY MedicalCenter.Domain/MedicalCenter.Domain.csproj MedicalCenter.Domain/MedicalCenter.Domain.csproj
RUN dotnet restore MedicalCenter.Api/MedicalCenter.Api.csproj

# Copy the rest of the application code
COPY . ./

#Migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet-ef database update --project MedicalCenter.Data/ --startup-project MedicalCenter.Api/