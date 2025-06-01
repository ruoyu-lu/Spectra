# Spectra - Cloud-Native Image Social Platform

## 🎯 Project Overview

Spectra is a cloud-native image social platform built with .NET 8, ASP.NET Core, and .NET Aspire. This project implements a modern, scalable architecture with JWT-based authentication and is designed for Azure deployment.

## 🏗️ Architecture

### Project Structure
```
Spectra/
├── Spectra.AppHost/          # .NET Aspire orchestration
├── Spectra.ServiceDefaults/  # Shared service configurations
├── Spectra.ApiService/       # Main API with ASP.NET Core
├── Spectra.Domain/           # Domain models and entities
├── Spectra.Infrastructure/   # Data access and external services
├── Spectra.Application/      # Business logic and services
├── Spectra.Web/             # Frontend (Blazor - to be replaced with React)
└── docs/                    # Project documentation
```

### Technology Stack
- **Backend**: .NET 8, ASP.NET Core, C#
- **Authentication**: ASP.NET Core Identity + JWT tokens
- **Database**: Entity Framework Core with SQL Server
- **Orchestration**: .NET Aspire
- **API Documentation**: Swagger/OpenAPI
- **Frontend**: React (planned)
- **Deployment**: Microsoft Azure (configured)

## ✅ Completed Features

### 1. Project Setup
- [x] .NET Aspire solution structure
- [x] Clean architecture with Domain, Application, Infrastructure layers
- [x] Entity Framework Core with SQL Server
- [x] Dependency injection configuration

### 2. Authentication System
- [x] ASP.NET Core Identity integration
- [x] JWT token generation and validation
- [x] User registration endpoint
- [x] User login endpoint
- [x] Protected user profile endpoint
- [x] Logout functionality
- [x] Password validation and security policies

### 3. Domain Models
- [x] ApplicationUser (extends IdentityUser)
- [x] Image entity
- [x] Follow relationship entity
- [x] Like entity
- [x] Comment entity
- [x] Database relationships and constraints

### 4. API Features
- [x] RESTful API design
- [x] Swagger/OpenAPI documentation
- [x] CORS configuration for React frontend
- [x] Error handling and validation
- [x] Logging configuration

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- .NET Aspire workload
- SQL Server or LocalDB

### Running the Application

1. **Clone and navigate to the project:**
   ```bash
   cd Spectra
   ```

2. **Build the solution:**
   ```bash
   dotnet build
   ```

3. **Run with .NET Aspire:**
   ```bash
   dotnet run --project Spectra.AppHost
   ```

4. **Access the services:**
   - Aspire Dashboard: https://localhost:17255
   - API Swagger UI: https://localhost:7109
   - Web Frontend: https://localhost:7018

### Testing the Authentication API

Use the provided `test-auth.http` file to test the authentication endpoints:

1. Register a new user
2. Login to get JWT token
3. Use token to access protected endpoints

## 📋 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `GET /api/auth/me` - Get current user info (protected)
- `POST /api/auth/logout` - User logout (protected)

### Configuration

The application uses the following configuration in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SpectraDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "Spectra.Api",
    "Audience": "Spectra.Client",
    "ExpirationMinutes": "60"
  }
}
```

## 🔐 Security Features

- Password requirements (6+ chars, uppercase, lowercase, digit)
- JWT token-based authentication
- HTTPS enforcement
- CORS policy for frontend integration
- Account lockout protection
- Secure password hashing with Identity

## 🎯 Next Steps

### Immediate Tasks
1. **Frontend Development**: Replace Blazor with React frontend
2. **Image Management**: Implement image upload and storage
3. **Social Features**: Add follow/unfollow, likes, comments
4. **Azure Integration**: Set up Azure services (Blob Storage, SQL Database)

### Future Enhancements
1. **Real-time Features**: SignalR for notifications
2. **Image Processing**: Thumbnail generation, image optimization
3. **Search**: Elasticsearch integration
4. **Caching**: Redis for performance
5. **Monitoring**: Application Insights integration

## 🛠️ Development Notes

- The project follows Clean Architecture principles
- Entity Framework migrations will be needed for production
- JWT secret key should be stored in Azure Key Vault for production
- Database connection string should use Azure SQL Database for production
- CORS policy should be updated for production frontend URL

## 📚 Documentation

- [Requirements](docs/01_project_overview/REQUIREMENTS.md)
- [Architecture Documentation](docs/02_architecture_and_design/)
- [Development Guide](docs/03_development/)

---

**Status**: ✅ Phase 1 Complete - Authentication system implemented and tested
**Next Phase**: Frontend development with React integration
