# RideSharingApp

This project is a Clean Architecture .NET solution for a ride-sharing business domain (Uber-like) with:
- Subscription, login, JWT authentication/authorization
- SQL Server persistence using Dapper
- Scrutor for dependency injection (no MediatR)
- Domain events with Azure ServiceBus
- RESTful endpoints
- FluentValidation and Result Pattern
- Unit tests with MOQ, XUnit
- Architecture tests with TngTech.ArchUnitNET
- Performance best practices

## Projects
- **RideSharingApp.Api**: ASP.NET Core Web API
- **RideSharingApp.Application**: Application logic, use cases, validators
- **RideSharingApp.Domain**: Domain entities, events, aggregates
- **RideSharingApp.Infrastructure**: Data access, Dapper, ServiceBus, SQL Server
- **RideSharingApp.Tests**: Unit and architecture tests

## Getting Started
1. Configure your SQL Server connection string in `appsettings.json`.
2. Run database migrations (if any).
3. Build and run the solution:
   ```powershell
   dotnet build
   dotnet run --project RideSharingApp.Api
   ```
4. The API will be available at `https://localhost:5001` (default).

## Testing
Run all tests:
```powershell
dotnet test
```

## Notes
- This project uses Scrutor for DI scanning instead of MediatR.
- Domain events are published to Azure ServiceBus as an example.
- Follows best practices for performance and maintainability.
