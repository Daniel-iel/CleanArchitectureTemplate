# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY RideSharingApp.Api.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image for run
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose port 80 and 443
EXPOSE 80
EXPOSE 443

# Set environment variables if needed
# ENV ASPNETCORE_URLS="http://+:80"

ENTRYPOINT ["dotnet", "RideSharingApp.Api.dll"]
