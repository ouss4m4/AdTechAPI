FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY ["AdTechCLI.csproj", "./"]
RUN dotnet restore "./AdTechCLI.csproj"
COPY . .
RUN dotnet publish "./AdTechCLI.csproj" -c Release -o out
WORKDIR /app/out
ENTRYPOINT ["dotnet", "AdTechCLI.dll"]