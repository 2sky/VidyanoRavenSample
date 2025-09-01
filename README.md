# VidyanoRavenSample

A sample application demonstrating the integration of [Vidyano](https://www.vidyano.com) application framework with [RavenDB](https://ravendb.net/) document database, running on ASP.NET Core 9.0 with Docker support.

## Overview

This project serves as a reference implementation for building modern web applications using:
- **Vidyano Framework** - A complete application framework for data-driven applications
- **RavenDB** - A NoSQL document database with ACID guarantees
- **ASP.NET Core 9.0** - Modern, cross-platform web framework
- **Docker** - Containerized deployment support

The sample includes a complete Northwind-style demo application with companies, orders, products, and employees to demonstrate real-world patterns and best practices.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) (17.8+) or [Visual Studio Code](https://code.visualstudio.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for containerized development)
- [RavenDB](https://ravendb.net/download) (or use Docker: `docker run -p 8080:8080 ravendb/ravendb`)

## Getting Started

### Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/VidyanoRavenSample.git
   cd VidyanoRavenSample
   ```

2. **Run RavenDB** (if not already running)
   ```bash
   docker run -d -p 8080:8080 --name ravendb ravendb/ravendb
   ```

3. **Build and run the application**
   ```bash
   dotnet run --project VidyanoRavenSample/VidyanoRavenSample.csproj
   ```

4. **Access the application**
   - Navigate to `https://localhost:5001` or `http://localhost:5000`
   - Login with default credentials: **admin/admin**
   - You'll be prompted to change the password on first login

### Development

```bash
# Build the project
dotnet build

# Run with hot reload
dotnet watch run --project VidyanoRavenSample/VidyanoRavenSample.csproj

# Run in Docker
docker build -f VidyanoRavenSample/Dockerfile -t vidyanoravensample .
docker run -p 5000:80 vidyanoravensample
```

## Project Structure

```
VidyanoRavenSample/
├── Service/                       # Core business logic
│   ├── VidyanoRavenSampleContext.cs    # RavenDB context and data access
│   ├── Models.cs                        # Domain models (Company, Order, Product, etc.)
│   ├── ModelIndexes.cs                  # RavenDB indexes and projections
│   ├── GettingStartedActions.cs         # Vidyano UI actions
│   ├── VidyanoRavenSampleBusinessRules.cs # Business rules (extensible)
│   └── VidyanoRavenSampleWeb.cs        # Custom API endpoints (extensible)
├── App_Data/                      # Application configuration
│   ├── model.json                       # Vidyano application model
│   ├── security.json                    # Security configuration
│   └── culture.json                     # Localization data
├── Program.cs                     # Application entry point
├── Startup.cs                     # Service configuration
└── CreateSampleDataOperation.cs   # Sample data generation

```

## Features

### Domain Model
- **Companies** - Customer and supplier management
- **Orders** - Complete order processing with order lines
- **Products** - Product catalog with categories
- **Employees** - Employee management with territories
- **Regions** - Geographic organization

### Technical Features
- Document database with ACID transactions
- Full-text search capabilities via RavenDB indexes
- Map-Reduce aggregations for reporting
- Value objects for domain modeling (Address, Contact, etc.)
- Automatic sample data generation
- Docker container support
- Hot reload for development

## Configuration

### Database Connection

Edit `appsettings.json` to configure your RavenDB connection:

```json
{
  "RavenSettings": {
    "Urls": ["http://localhost:8080"],
    "DatabaseName": "VidyanoRavenSample"
  }
}
```

### Security

The application uses Vidyano's built-in security features. Default configuration is in `App_Data/security.json`. 

⚠️ **Important**: Change the default admin password immediately after first login.

## Architecture

### Key Components

1. **Vidyano Framework Integration**
   - Configured via `AddVidyanoRavenDB()` in Startup.cs
   - Provides UI generation, security, and data operations

2. **RavenDB Document Store**
   - Session-per-request pattern
   - Automatic change tracking
   - LINQ query support

3. **Domain-Driven Design**
   - Value Objects for complex types
   - Aggregate roots with proper boundaries
   - Entity references using document IDs

### Data Access Pattern

```csharp
// Access data through the context
public class VidyanoRavenSampleContext : RequestScopeProvider<VidyanoRavenSampleContext>
{
    public IQueryable<Company> Companies => Query<Company>();
    public IQueryable<Order> Orders => Query<Order>();
    // ... other entities
}
```

## Development Tips

- The database and sample data are created automatically on first run
- Use RavenDB Studio (http://localhost:8080) to inspect and manage data
- Vidyano actions in `GettingStartedActions.cs` handle UI operations
- Custom business rules can be added to `VidyanoRavenSampleBusinessRules.cs`
- API endpoints can be extended in `VidyanoRavenSampleWeb.cs`

## Troubleshooting

### RavenDB Connection Issues
- Ensure RavenDB is running on the configured port (default: 8080)
- Check firewall settings if using remote RavenDB server
- Verify the database name in appsettings.json

### Docker Issues
- For Docker Desktop on Windows, use `host.docker.internal` instead of `localhost` in connection strings
- Ensure Docker Desktop is running before building containers

## License

[Your License Here]

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

For Vidyano framework support: https://www.vidyano.com
For RavenDB support: https://ravendb.net/support