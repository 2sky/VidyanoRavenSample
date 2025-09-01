# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Development Commands

```bash
# Build the project
dotnet build VidyanoRavenSample/VidyanoRavenSample.csproj

# Run the application
dotnet run --project VidyanoRavenSample/VidyanoRavenSample.csproj

# Run with hot reload
dotnet watch run --project VidyanoRavenSample/VidyanoRavenSample.csproj

# Build Docker image
docker build -f VidyanoRavenSample/Dockerfile -t vidyanoravensample .

# Clean build artifacts
dotnet clean VidyanoRavenSample/VidyanoRavenSample.csproj
```

## Architecture Overview

This is a Vidyano framework application integrated with RavenDB, built on ASP.NET Core 9.0. The architecture follows these key patterns:

### Core Integration Points

1. **Startup Configuration** (Startup.cs): Services are configured through `AddVidyanoRavenDB()` which sets up the Vidyano framework, RavenDB document store, and request scoping.

2. **Database Context** (Service/VidyanoRavenSampleContext.cs): Inherits from `Vidyano.RavenDB.RequestScopeProvider` and exposes queryable collections for all entities. This is the primary data access layer.

3. **Domain Models** (Service/Models.cs): Entities use specific attributes:
   - `[ValueObject]` for value types (Address, Contact, OrderLine)
   - `[Reference(typeof(T))]` for entity relationships
   - `[FieldName("name")]` for property naming conventions

4. **RavenDB Indexes** (Service/ModelIndexes.cs): Custom indexes for search and projections:
   - Map indexes for searching (Products_Search)
   - Map-Reduce for aggregations (Products_ByCategory)
   - View projections (Orders_Overview, Company_View)

### Request Flow

1. HTTP requests are handled by the Vidyano middleware pipeline
2. The framework automatically creates a scoped `VidyanoRavenSampleContext` per request
3. Database operations go through RavenDB's session-per-request pattern
4. Vidyano actions (Service/GettingStartedActions.cs) handle UI operations

### Key Dependencies

- **Vidyano.RavenDB**: Core framework providing the integration layer
- **RavenDB**: Document database accessed via IDocumentStore
- **.NET 9.0**: Target framework with nullable reference types and implicit usings

### Data Access Patterns

- Use the context's queryable properties (e.g., `context.Companies`, `context.Orders`)
- Leverage RavenDB indexes for complex queries and aggregations
- Value objects are embedded in parent documents automatically
- References between entities use document IDs (e.g., "companies/1")

### Application Configuration

- **App_Data/model.json**: Defines the Vidyano application model and UI structure
- **App_Data/security.json**: Security configuration and authentication settings
- **appsettings.json**: RavenDB connection and Vidyano framework settings

### Development Notes

- The application auto-creates the database and sample data on first run
- Default admin credentials: admin/admin (requires change on first login)
- RavenDB runs on http://host.docker.internal:8080 in Docker environments
- The CreateSampleDataOperation.cs creates Northwind-style demo data