# 🏥 Clinics Backend

This is the backend for the Clinics project, built with ASP.NET Core (.NET 8) following Clean Architecture pattern.

## 📁 Structure
- **API/** — Main Web API project
- **Domain/** — Domain models and logic
- **Application/** — Application services and business logic
- **Infrastructure/** — Infrastructure services (e.g., notifications)
- **Persistence/** — Database context and migrations
- **Presentation/** — API presentation layer
- **EmployeesAPI/**, **MedicinesAPI/** — Additional API modules

## 🛠️ Main Tech Stack
- ASP.NET Core (.NET 8)
- Entity Framework Core (SQL Server)
- MediatR
- FluentValidation
- Swagger (Swashbuckle)

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server (or change connection string for your DB)

### Build & Run
```bash
# From this directory
# Build the solution
 dotnet build
# Run the main API project
 dotnet run --project API/API.csproj
```

### Database Migrations
```bash
# Add a migration
 dotnet ef migrations add <MigrationName> --project Persistence --startup-project API
# Update the database
 dotnet ef database update --project Persistence --startup-project API
```

## 📚 API Documentation
Once running, Swagger UI is available at `/swagger` (e.g., http://localhost:5000/swagger).

## 🧪 Testing
You can add and run tests in the `ArchitectureTests/` folder or other test projects.

## 🏗️ Clean Architecture
This project follows Clean Architecture principles, separating concerns into distinct layers:
- **Domain Layer**: Contains enterprise business rules and entities.
- **Application Layer**: Orchestrates the flow of data and implements use cases.
- **Infrastructure Layer**: Provides implementations for external services and frameworks.
- **Presentation Layer**: Handles user interface and API endpoints.

This architecture ensures that the business logic remains independent of external frameworks and databases, making the system more maintainable and testable. 