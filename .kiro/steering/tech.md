# InkHouse Technical Stack

## Core Technologies
- **Framework**: .NET 9.0
- **UI Framework**: Avalonia UI 11.3.2 (cross-platform desktop framework)
- **Architecture Pattern**: MVVM (Model-View-ViewModel)
- **ORM**: Entity Framework Core 8.0.13 with MySQL provider
- **Database**: MySQL
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection 9.0.7
- **MVVM Toolkit**: CommunityToolkit.Mvvm 8.2.1
- **Reactive UI**: ReactiveUI 20.4.1
- **Password Hashing**: BCrypt.Net-Next 4.0.3

## Project Structure
- **Models**: Entity Framework Core models representing database tables
- **Services**: Business logic and data access layer
- **ViewModels**: MVVM pattern implementation connecting UI with services
- **Views**: Avalonia UI XAML files and code-behind

## Build System
The project uses the standard .NET SDK build system with MSBuild.

## Common Commands

### Build and Run
```bash
# Build the project
dotnet build

# Run the application
dotnet run --project InkHouse/InkHouse.csproj

# Run with auto-test mode
dotnet run --project InkHouse/InkHouse.csproj -- --auto-test
```

### Database Migrations
```bash
# Add a new migration
dotnet ef migrations add MigrationName --project InkHouse

# Update database with latest migrations
dotnet ef database update --project InkHouse

# Generate SQL script for migrations
dotnet ef migrations script --project InkHouse
```

### Publish
```bash
# Publish for current platform
dotnet publish -c Release --self-contained

# Publish for specific platform (e.g., Windows)
dotnet publish -c Release -r win-x64 --self-contained

# Publish for Linux
dotnet publish -c Release -r linux-x64 --self-contained
```

## Development Environment
- **IDE**: JetBrains Rider (recommended) or Visual Studio
- **Database Tools**: MySQL Workbench, DBeaver, or Rider's built-in Database tool