# 🎓 Quiz System - Examination Management System

A comprehensive, scalable examination and quiz management system built with **ASP.NET Core**, **Entity Framework Core**, **Redis**, and **Hangfire**. This system enables instructors to create courses and exams, students to take exams, and provides automated grading with intelligent caching strategies for optimal performance.

---

## 📋 Table of Contents

- [Features](#-features)
- [Technology Stack](#-technology-stack)
- [System Architecture](#-system-architecture)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [API Documentation](#-api-documentation)
- [Performance Optimizations](#-performance-optimizations)
- [Background Jobs](#-background-jobs)
- [Database Schema](#-database-schema)
- [Project Structure](#-project-structure)
- [Contributing](#-contributing)
- [License](#-license)

---

## ✨ Features

### 👥 User Management
- **Role-based Authentication** (Student, Instructor)
- JWT token-based authorization
- User profile management
- Secure password handling with ASP.NET Identity

### 📚 Course Management
- Create and manage courses
- Assign instructors to courses
- Enroll students in courses
- Track course assignments

### 📝 Exam Management
- Create multiple exam types (Quiz, Midterm, Final)
- Auto-generated or manual question selection
- Set exam duration and deadlines
- Question bank with difficulty levels
- Multiple choice questions support

### ✅ Student Examination
- Take exams within specified time windows
- Submit answers in real-time
- Automatic exam completion on deadline
- Instant score calculation
- View exam results and performance

### 🚀 Performance Features
- **Redis Distributed Caching** for frequently accessed data
- **Hangfire Background Jobs** for async processing
- **Automatic Cache Invalidation** on data changes
- **Benchmark Monitoring** for performance tracking
- **Query Optimization** with EF Core

---

## 🛠 Technology Stack

### Backend
- **ASP.NET Core 10** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - Primary database
- **ASP.NET Identity** - Authentication & authorization
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation

### Caching & Jobs
- **Redis** - Distributed caching
- **Hangfire** - Background job processing
- **MemoryCache** - In-memory caching

### Security
- **JWT (JSON Web Tokens)** - Stateless authentication
- **BCrypt** - Password hashing

### API Documentation
- **OpenAPI/Swagger** - API documentation & testing

---

## 🚀 Getting Started

### Prerequisites

Ensure you have the following installed:

- **.NET 10 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/10.0))
- **SQL Server 2019+** ([Download](https://www.microsoft.com/sql-server/sql-server-downloads))
- **Redis Server** (Local or Cloud):
  - Windows: [Redis for Windows](https://github.com/microsoftarchive/redis/releases)
  - Linux/macOS: `sudo apt-get install redis-server` or `brew install redis`
  - Cloud: [Redis Labs](https://redis.com/), [Azure Cache for Redis](https://azure.microsoft.com/services/cache/)
- **Visual Studio 2022** or **VS Code** with C# extension
- **Git** ([Download](https://git-scm.com/))

### Installation Steps

#### 1. Clone the Repository

#### 2. Configure Database Connection

Edit `appsettings.json` or create `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASSWORD;",
    "Redis": "YOUR_REDIS_CONNECTION_STRING"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE",
    "ExpiresInMinutes": 60
  }
}
```

**For Azure SQL Database:**

````````

#### 3. Configure Redis

**Option A: Local Redis (Development)**

For development, you can use a local instance of Redis. Ensure that Redis is running on your machine. You can start Redis using Docker:

```bash
docker run --name redis-dev -p 6379:6379 -d redis
```

**Option B: Cloud Redis**

For production, use a cloud-based Redis service like [Redis Labs](https://redis.com/) or [Azure Cache for Redis](https://azure.microsoft.com/services/cache/). Update the `appsettings.json` with your Redis connection string.

```json
{
  "ConnectionStrings": {
    "Redis": "YOUR_CLOUD_REDIS_CONNECTION_STRING"
  }
}
```

> **Note:** The repository includes a cloud Redis instance for development. Replace it with your own for production.

#### 4. Configure JWT Settings

Add to `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE",
    "ExpiresInMinutes": 60
  }
}
```

**Security Note:** 
- Use a strong, randomly generated key (minimum 32 characters)
- Store sensitive settings in **User Secrets** (Development) or **Azure Key Vault** (Production)
- Never commit secrets to source control

#### 5. Restore NuGet Packages

#### 6. Apply Database Migrations

If migrations don't exist, create them:

```bash
dotnet ef migrations add InitialCreate
```

```bash
dotnet ef database update
```

#### 7. Build the Project   

#### 8. Run the Application

Or press **F5** in Visual Studio.

#### 9. Access the Application

The API will be available at:
- **HTTPS**: `https://localhost:7000`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:7000/swagger` or `https://localhost:7000` (Development mode)
- **Hangfire Dashboard**: `https://localhost:7000/hangfire`

---

## ⚙️ Configuration

### Database Configuration

The system uses **Code-First** approach with Entity Framework Core.

**Key Settings in `Program.cs`:**

````````

**Cache Strategy:**

````````markdown
#### Cache Expiration Policy

| Data Type | TTL | Auto-Refresh | On-Demand Invalidation |
|-----------|-----|--------------|------------------------|
| All Courses | 10 min | Every 30 min | ✅ On Create/Update/Delete |
| All Exams | 10 min | Every 30 min | ✅ On Create/Update/Delete |
| All Questions | 10 min | Every 30 min | ✅ On Create/Update/Delete |
| Single Course | 30 min | Manual | ✅ On Update/Delete |
| Single Exam | 30 min | Manual | ✅ On Update/Delete |
| Single Question | 30 min | Manual | ✅ On Update/Delete |
| Exam Questions | 2 hours | Manual | ✅ On Question/Exam Change |
| User Profile | 10 min | Manual | ✅ On Profile Update |

### Hangfire Configuration

**Job Storage:**
**Dashboard Access:**
- Development: No authentication required
- Production: Implement `IDashboardAuthorizationFilter`

**Recommended Production Filter:**

````````

### CORS Configuration

**Development:**

```csharp
app.UseCors(builder =>
    builder.WithOrigins("http://localhost:3000") // React app URL
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials());
```

**Production:**

```csharp
app.UseCors(builder =>
    builder.WithOrigins("https://your-production-url.com")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials());
```

**Note:** Configure CORS based on your frontend application URL. Ensure that the appropriate headers and methods are allowed.

### Logging Configuration

**Development:**

- Logs are stored in `Logs` folder in the project directory
- Log level set to `Debug` for detailed output

**Production:**

- Logs are sent to Azure Application Insights
- Configure Instrumentation Key in `appsettings.Production.json`:

```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "YOUR_INSTRUMENTATION_KEY"
  }
}
```

**Note:** 
- Use `ILogger<T>` for logging in your classes
- Avoid logging sensitive data (e.g., passwords, personal information)

### Health Checks

- Endpoint: `/health`
- Checks:
  - Database connectivity
  - Redis availability
  - Disk space

```csharp
app.MapHealthChecks("/health");
```

---

## 🏗 System Architecture

![System Architecture](docs/architecture.png)

- **Client**: React.js application
- **API Gateway**: ASP.NET Core Web API
- **Services**:
  - **Identity Service**: Manages user authentication and profiles
  - **Course Service**: Handles course creation, enrollment, and management
  - **Exam Service**: Manages exam creation, scheduling, and grading
- **Data Storage**:
  - **SQL Server**: Relational data (users, courses, exams, results)
  - **Redis**: Caching frequently accessed data
- **Background Jobs**: Hangfire for processing background tasks (e.g., sending emails, generating reports)
- **Monitoring**: Application Insights for performance monitoring and logging

---

## 📊 API Documentation

- **Swagger UI**: Interactive API documentation
  - Access at `/swagger` endpoint
  - Provides API exploration and testing capabilities

---

## 🚦 Performance Optimizations

- Enabled **Response Compression** for JSON responses
