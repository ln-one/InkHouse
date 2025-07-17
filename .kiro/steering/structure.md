# InkHouse Project Structure

## Architecture Overview

InkHouse follows a layered architecture with clear separation of concerns:

1. **Data Layer**: Models and database context
2. **Service Layer**: Business logic and data access
3. **Presentation Layer**: ViewModels and Views (MVVM pattern)

## Key Components

### Models
Entity classes that map to database tables:
- `Book.cs`: Book entity with properties like Title, Author, ISBN
- `User.cs`: User entity with authentication properties
- `BorrowRecord.cs`: Records of book borrowing transactions
- `Seat.cs`: Library seating information
- `SeatReservation.cs`: Seat reservation records
- `InkHouseContext.cs`: Entity Framework DbContext

### Services
Business logic implementation:
- `ServiceManager.cs`: Central service locator and dependency injection
- `DbContextFactory.cs`: Creates database contexts with connection pooling
- `AppConfig.cs`: Application configuration (connection strings, etc.)
- `UserService.cs`: User authentication and management
- `BookService.cs`: Book inventory management
- `BorrowRecordService.cs`: Borrowing workflow management
- `SeatService.cs`: Seat reservation management

### ViewModels
MVVM pattern implementation:
- `ViewModelBase.cs`: Base class with common functionality
- `LoginViewModel.cs`: Login screen logic
- `MainWindowViewModel.cs`: Main application window logic
- `BookEditViewModel.cs`: Book editing form logic
- `UserEditViewModel.cs`: User editing form logic
- `BorrowEditViewModel.cs`: Borrow record editing logic

### Views
Avalonia UI interface:
- XAML files defining the UI layout
- Code-behind files handling UI events

## Development Workflow

1. **Models**: Define or update entity classes
2. **Migrations**: Create database migrations when models change
3. **Services**: Implement business logic in service classes
4. **ViewModels**: Create ViewModels that use services
5. **Views**: Design UI that binds to ViewModels

## Best Practices

1. **Service Access**: Always access services through ServiceManager
2. **Error Handling**: Use ViewModelBase.ExecuteAsync for operations
3. **Data Binding**: Use SetProperty for observable properties
4. **Async Operations**: Use async/await for database operations
5. **Dependency Injection**: Register services in ServiceManager.Initialize

## Common Patterns

### Service Method Pattern
```csharp
public List<Book> GetAllBooks()
{
    try
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.Books.AsNoTracking().ToList();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error getting books: {ex.Message}");
        return new List<Book>();
    }
}
```

### ViewModel Command Pattern
```csharp
public ICommand SaveCommand { get; }

public MyViewModel()
{
    SaveCommand = new AsyncRelayCommand(SaveAsync);
}

private async Task SaveAsync()
{
    await ExecuteAsync(async () =>
    {
        var service = ServiceManager.GetService<MyService>();
        await service.SaveDataAsync(MyData);
        ShowSuccess("Data saved successfully");
    });
}
```