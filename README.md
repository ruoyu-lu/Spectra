# 🌟 Spectra - Cloud-Native Image Social Platform

A modern, cloud-native image social platform built with .NET 8, ASP.NET Core, and .NET Aspire for local development orchestration.

## 🚀 Quick Start with .NET Aspire

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or VS Code with C# extension
- Docker Desktop (required for .NET Aspire container orchestration)
- PostgreSQL (for production) or use Aspire's automatic PostgreSQL container

### Running Locally
```bash
# Clone the repository
git clone <your-repo-url>
cd Spectra

# Run with .NET Aspire orchestration
dotnet run --project Spectra.AppHost
```

This will start:
- Spectra API Service
- Spectra Web Frontend
- PostgreSQL database (containerized via Aspire)
- Aspire Dashboard for monitoring

### Access Points
- **Aspire Dashboard**: http://localhost:15888 (monitoring and logs)
- **API Service**: http://localhost:5000 (with Swagger UI)
- **Web Frontend**: http://localhost:5001
- **API Health Check**: http://localhost:5000/health

## 🏗️ Architecture

Spectra follows a clean architecture pattern with:

- **Spectra.ApiService**: ASP.NET Core Web API with JWT authentication
- **Spectra.Web**: Blazor Server frontend
- **Spectra.Application**: Business logic and services
- **Spectra.Domain**: Core entities and domain models
- **Spectra.Infrastructure**: Data access and external services
- **Spectra.ServiceDefaults**: Shared service configurations
- **Spectra.AppHost**: .NET Aspire orchestration host

## 🔧 Development

### .NET Aspire Benefits
- **Integrated Development**: Seamless .NET project orchestration
- **Built-in Monitoring**: Real-time dashboard for all services
- **Service Discovery**: Automatic service-to-service communication
- **Container Management**: Automatic PostgreSQL container setup
- **Hot Reload**: Automatic recompilation and restart on code changes

### Database Migrations
```bash
# Add new migration
dotnet ef migrations add <MigrationName> --project Spectra.Infrastructure --startup-project Spectra.ApiService

# Update database
dotnet ef database update --project Spectra.Infrastructure --startup-project Spectra.ApiService
```

### Testing
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test Spectra.Tests
```

## 🔐 Authentication

The platform uses JWT-based authentication with:
- User registration and login
- Role-based authorization
- Secure password hashing
- Token refresh capabilities

See [Authentication Documentation](README-Authentication.md) for detailed API usage.

## 📚 Documentation

- [Authentication Guide](README-Authentication.md)
- [PostgreSQL Setup](docs/PostgreSQL-Setup.md)
- [Project Architecture](docs/02_architecture_and_design/)

## 🛠️ Technology Stack

- **.NET 8**: Latest .NET framework
- **ASP.NET Core**: Web API framework
- **Blazor Server**: Interactive web UI
- **Entity Framework Core**: ORM for data access
- **PostgreSQL**: Primary database
- **.NET Aspire**: Local development orchestration
- **JWT**: Authentication and authorization
- **Azure**: Cloud deployment target

## 📊 Status

✅ **Local Development**: Ready with .NET Aspire orchestration
✅ **Authentication**: JWT-based auth system implemented
✅ **Database**: PostgreSQL with EF Core migrations
✅ **API**: RESTful API with Swagger documentation
🚧 **Frontend**: Blazor Server UI in development
🚧 **Cloud Deployment**: Azure deployment configuration in progress

---

**Built with ❤️ using .NET 8 and .NET Aspire**