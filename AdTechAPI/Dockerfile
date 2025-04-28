# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything inside AdTechAPI/
COPY . .

# Restore ONLY the API project
RUN dotnet restore "AdTechAPI.csproj"

# Publish ONLY the API project
RUN dotnet publish "AdTechAPI.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AdTechAPI.dll"]
