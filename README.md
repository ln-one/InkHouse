# InkHouse Library Management System

[中文版说明](./README.zh.md)

InkHouse is a cross-platform library management system built with C#, Avalonia, and MySQL. It supports both Windows and Linux environments, providing a modern, elegant, and easy-to-use interface.

## ✨ Features

### 🔐 Authentication & Authorization
- **Role-based Access Control**: Only Admin users can access the system
- **Secure Login**: Username/password authentication with role validation
- **Session Management**: Automatic logout functionality with window switching

### 📊 Dashboard & Statistics
- **System Overview**: Total books, available books, borrowed books, registered users
- **Real-time Statistics**: Dynamic dashboard with key metrics
- **Visual Indicators**: Status indicators for book availability

### 📚 Book Management
- **Book CRUD Operations**: Add, edit, delete, and view books
- **Book Status Tracking**: Available, borrowed, overdue, maintenance status
- **Search & Filter**: Advanced search and filtering capabilities

### 👥 User Management
- **User CRUD Operations**: Add, edit, delete, and view users
- **Role Management**: Admin and User role support
- **User Authentication**: Secure login with role-based access

### 📖 Borrow Management
- **Borrow Records**: Track all book borrowing activities
- **Borrow/Return Operations**: Complete borrow and return workflow
- **Overdue Tracking**: Monitor overdue books and notifications

### 📈 Reports & Analytics
- **Statistical Reports**: Comprehensive library statistics
- **Data Export**: Export reports in various formats
- **Analytics Dashboard**: Visual data representation

### ⚙️ System Settings
- **Configuration Management**: Centralized system configuration
- **Database Settings**: Easy database connection management
- **System Preferences**: Customizable system settings

### 🔍 Search & Navigation
- **Global Search**: Search across books, users, and records
- **Advanced Filters**: Multi-criteria filtering
- **Navigation Menu**: Intuitive sidebar navigation

---

## 🤝 Team Collaboration & Project Structure

### Team Collaboration
- **Version Control**: Everyone uses Git, with a remote repo (GitHub/GitLab) for collaboration.
- **Branch Management**: The main branch (main) holds only stable, runnable code. Create a new feature/bugfix branch for each task, merge to main after completion.
- **Commit Message Convention**: Clearly describe each change, preferably in English (e.g., `feat: add book borrow feature`).
- **Code Review**: Team members should review each other's code before merging to improve quality.
- **Task Assignment**: Use issues or a todo list to assign tasks and sync progress regularly.

### Project Structure
| Path                | Purpose                                         |
|---------------------|-------------------------------------------------|
| `App.axaml`         | App entry XAML, defines global styles/resources |
| `App.axaml.cs`      | App entry C# code, initializes the application  |
| `Assets/`           | Stores images, icons, and other static assets   |
| `Models/`           | Data models, define DB table structures         |
| `Services/`         | Service layer, encapsulates DB/business logic   |
| `ViewModels/`       | ViewModel layer, connects UI and data logic     |
| `Views/`            | View layer, XAML UI files and code-behind       |
| `InkHouse.csproj`   | Project config, dependency management           |
| `Program.cs`        | Main program entry point                        |
| `app.manifest`      | App manifest, configures permissions, etc.      |
| `bin/`, `obj/`      | Build output and intermediate files (auto-gen)  |

### Layered Architecture
- **Models**: Define C# classes mapping to DB tables (e.g., Book.cs, User.cs), for data access and entity mapping.
- **Services**: Encapsulate all DB interaction logic (e.g., UserService), such as CRUD and business rules.
- **ViewModels**: Handle UI logic and data binding, bridge between Views and Services (e.g., LoginViewModel).
- **Views**: XAML files define UI layout, .cs files handle UI events (e.g., button clicks).

### Quick Start & Dev Tips
- **Read the README and structure guide first to understand each layer's responsibility.**
- **For new features, create corresponding files in Models/Services/ViewModels/Views.**
- **Use breakpoints for debugging, and leverage Rider's XAML preview and DB tools.**
- **Communicate frequently; ask questions when in doubt.**

### 🚀 Current Implementation Status

#### ✅ Completed Features
- **Authentication System**: Role-based login with admin-only access
- **Window Management**: Login window ↔ Main window switching
- **Logout Functionality**: Secure logout with proper window management
- **UI Framework**: Complete modern UI with dashboard, navigation, and responsive design
- **Database Integration**: MySQL database with Entity Framework Core
- **Service Architecture**: Centralized service management with dependency injection
- **Error Handling**: Unified error handling across all layers
- **Event System**: Login success/failure events with proper window switching
- **Book Browsing & Borrowing**: Users can browse available books with pagination and borrow/return without reloading the entire list
- **Search & Pagination**: Optimized search using EF Core `AsNoTracking`, `Skip/Take`, and a "Load More" UI for smooth paging
- **Performance Optimizations**: Connection pooling, lazy loading, and optimized queries for noticeably faster response times, even with large datasets

#### 🔄 Ready for Implementation
- **Book Management**: UI interface ready, service methods need implementation
- **User Management**: UI interface ready, service methods need implementation  
- **Borrow Management**: UI interface ready, service methods need implementation
- **Dashboard Statistics**: UI ready, data binding needs implementation
- **Search Functionality**: UI ready, search logic needs implementation
- **System Settings**: UI ready, configuration logic needs implementation

---

## 🛠️ Development Architecture & Workflow

### 🎯 Architecture Advantages
- ✅ **Unified Configuration Management**: All configurations are centralized in `AppConfig.cs`
- ✅ **Service Manager**: Get services through `ServiceManager` without manual creation
- ✅ **Automatic Error Handling**: `ViewModelBase` provides unified error handling
- ✅ **Simplified Development**: No need to manually create database connections and services
- ✅ **Framework Design**: Only provides architecture framework, specific business logic is implemented by team members

### 🏗️ Architecture Overview

#### 1. Configuration Management (`AppConfig.cs`)
```csharp
// Modify database connection information
AppConfig.SetDatabaseConnection("server", 3306, "database", "username", "password");

// Or directly modify the connection string
AppConfig.DatabaseConnectionString = "your connection string";
```

#### 2. Service Manager (`ServiceManager.cs`)
```csharp
// Get various services (no manual creation needed)
var userService = ServiceManager.Instance.UserService;
var bookService = ServiceManager.Instance.BookService;
var borrowService = ServiceManager.Instance.BorrowRecordService;
```

#### 3. Base ViewModel (`ViewModelBase.cs`)
```csharp
public class MyFeatureViewModel : ViewModelBase
{
    // Automatically get the following features:
    // - IsLoading: Loading state
    // - ErrorMessage: Error message
    // - SuccessMessage: Success message
    // - ShowError(): Display error
    // - ShowSuccess(): Display success
    // - ExecuteAsync(): Safely execute async operations
}
```

### 🚀 Quick Start Guide

#### Step 1: Configure Database
**⚠️ Important**: For security reasons, the actual `AppConfig.cs` file is not included in the repository.

1. **Copy the template file**:
   ```bash
   cp InkHouse/Services/AppConfig.template.cs InkHouse/Services/AppConfig.cs
   ```

2. **Modify database connection information** in `Services/AppConfig.cs`:
   ```csharp
   public static string DatabaseConnectionString { get; set; } = 
       "server=your_server;port=3306;database=InternShip;user=your_username;password=your_password;";
   ```

3. **Replace the placeholders** with your actual database information:
   - `your_server`: Database server address
   - `your_username`: Database username  
   - `your_password`: Database password

📖 **See detailed setup instructions**: `InkHouse/Services/README.md`

#### Step 2: Create New ViewModel
```csharp
public class MyFeatureViewModel : ViewModelBase
{
    private string _myProperty;
    
    public string MyProperty
    {
        get => _myProperty;
        set => SetProperty(ref _myProperty, value);
    }
    
    public ICommand MyCommand { get; }
    
    public MyFeatureViewModel()
    {
        MyCommand = new AsyncRelayCommand(MyMethodAsync);
    }
    
    private async Task MyMethodAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Get service
            var service = ServiceManager.Instance.UserService;
            
            // Execute business logic
            var result = service.SomeMethod();
            
            // Show result
            ShowSuccess("Operation successful!");
        });
    }
}
```

#### Step 3: Create View
```csharp
public partial class MyFeatureView : UserControl
{
    public MyFeatureView()
    {
        InitializeComponent();
        DataContext = new MyFeatureViewModel(); // No need to pass services
    }
}

// Or create a Window
public partial class MyFeatureWindow : Window
{
    public MyFeatureWindow()
    {
        InitializeComponent();
        DataContext = new MyFeatureViewModel();
    }
}
```

### 📚 Service Usage Examples

#### User Service (UserService)
```csharp
var userService = ServiceManager.Instance.UserService;

// User login (already implemented)
var user = userService.Login("username", "password");

// TODO: Team members can add other user-related methods here
// For example: GetAllUsers(), AddUser(), UpdateUser(), DeleteUser(), etc.
```

### 🏗️ Main Window Features Interface

The main window (`MainWindowViewModel`) provides a complete interface for all library management features:

#### Dashboard Statistics
```csharp
public int TotalBooks { get; set; }
public int AvailableBooks { get; set; }
public int BorrowedBooks { get; set; }
public int RegisteredUsers { get; set; }
```

#### Book Management Interface
```csharp
public ObservableCollection<Book> Books { get; set; }
public Task LoadBooksAsync();
public Task AddBookAsync(Book book);
public Task EditBookAsync(Book book);
public Task DeleteBookAsync(Book book);
```

#### User Management Interface
```csharp
public ObservableCollection<User> Users { get; set; }
public Task LoadUsersAsync();
public Task AddUserAsync(User user);
public Task EditUserAsync(User user);
public Task DeleteUserAsync(User user);
```

#### Borrow Management Interface
```csharp
public ObservableCollection<BorrowRecord> BorrowRecords { get; set; }
public Task LoadBorrowRecordsAsync();
public Task BorrowBookAsync(BorrowRecord record);
public Task ReturnBookAsync(BorrowRecord record);
```

#### Authentication & Session Management
```csharp
public ICommand LogoutCommand { get; }
public void Logout(); // Handles window switching and logout
```

#### Book Service (BookService)
```csharp
var bookService = ServiceManager.Instance.BookService;

// TODO: Team members can add book-related methods here
// For example: GetAllBooks(), AddBook(), UpdateBook(), DeleteBook(), etc.
```

#### Borrow Record Service (BorrowRecordService)
```csharp
var borrowService = ServiceManager.Instance.BorrowRecordService;

// TODO: Team members can add borrow record-related methods here
// For example: GetAllBorrowRecords(), BorrowBook(), ReturnBook(), etc.
```

### 🎨 UI Development Tips

#### 1. Data Binding
```xml
<!-- Bind to ViewModel properties -->
<TextBox Text="{Binding UserName}" />
<TextBox Classes="password" Text="{Binding Password}" />
<Button Command="{Binding LoginCommand}" Content="Login" />

<!-- Bind to lists -->
<ListBox ItemsSource="{Binding Books}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Title}"/>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>

<!-- Bind to window events -->
<Window x:Class="InkHouse.Views.LoginWindow">
    <!-- Window content -->
</Window>
```

#### 2. Display Loading State
```xml
<StackPanel>
    <!-- Loading indicator -->
    <ProgressBar IsIndeterminate="{Binding IsLoading}" 
                 IsVisible="{Binding IsLoading}" />
    
    <!-- Error message -->
    <TextBlock Text="{Binding ErrorMessage}" 
               Foreground="Red" 
               IsVisible="{Binding ErrorMessage, Converter={x:Static StringConverters.IsNotNullOrEmpty}}" />
    
    <!-- Success message -->
    <TextBlock Text="{Binding SuccessMessage}" 
               Foreground="Green" 
               IsVisible="{Binding SuccessMessage, Converter={x:Static StringConverters.IsNotNullOrEmpty}}" />
</StackPanel>
```

#### 3. Command Binding
```xml
<Button Command="{Binding LoginCommand}" Content="Login" />
<Button Command="{Binding LogoutCommand}" Content="Logout" />
<Button Command="{Binding SearchCommand}" Content="Search" />
```

### 🔧 Debugging Tips

#### 1. View Error Information
All service methods have exception handling, error messages will be output to console:
```csharp
Console.WriteLine($"Operation failed: {ex.Message}");
```

#### 2. Use ViewModel's Debug Features
```csharp
// Display debug information in ViewModel
ShowError("Debug information");
ShowSuccess("Operation successful");
```

#### 3. Check Database Connection
```csharp
// Set debug mode in AppConfig.cs
AppConfig.IsDebugMode = true;
```

### 📝 Development Standards

#### 1. Naming Conventions
- **Class names**: PascalCase (e.g., `BookService`, `LoginViewModel`)
- **Method names**: PascalCase (e.g., `GetAllBooks()`, `ValidateUser()`)
- **Property names**: PascalCase (e.g., `BookTitle`, `IsAvailable`)
- **Private fields**: camelCase with underscore (e.g., `_bookService`, `_searchTerm`)

#### 2. File Organization
```
Models/
├── Book.cs              # Book entity
├── User.cs              # User entity
├── BorrowRecord.cs      # Borrow record entity
├── UserRoles.cs         # User role constants
└── InkHouseContext.cs   # Entity Framework context

Services/
├── AppConfig.cs         # Configuration management
├── ServiceManager.cs    # Service manager
├── DbContextFactory.cs  # Database context factory
├── UserService.cs       # User service
├── BookService.cs       # Book service
└── BorrowRecordService.cs # Borrow record service

ViewModels/
├── ViewModelBase.cs     # Base ViewModel
├── LoginViewModel.cs    # Login ViewModel
└── MainWindowViewModel.cs # Main window ViewModel

Views/
├── LoginView.axaml      # Login user control
├── LoginView.axaml.cs   # Login user control code-behind
├── LoginWindow.axaml    # Login window
├── LoginWindow.axaml.cs # Login window code-behind
├── MainWindow.axaml     # Main window
└── MainWindow.axaml.cs  # Main window code-behind
```

#### 3. Code Comments
All public methods and properties should have comments:
```csharp
/// <summary>
/// Get all books
/// </summary>
/// <returns>List of books</returns>
public List<Book> GetAllBooks()
{
    // Implementation code
}
```

### 🚨 Common Issues

#### Q1: Database connection failed?
A1: Check the connection string in `AppConfig.cs` is correct, ensure the database server is accessible.

#### Q2: Why is my ViewModel not responding?
A2: Make sure the ViewModel inherits from `ViewModelBase` and properties use the `SetProperty` method.

#### Q3: How to add new services?
A3: Create new service classes following the same pattern, then add corresponding properties in `ServiceManager`.

#### Q4: How to handle complex business logic?
A4: Implement complex business logic in the service layer, ViewModel only handles UI interaction and data binding.

### 🎉 Summary

The new architecture makes development simpler and more unified:

1. **Centralized Configuration Management**: All configurations in one place
2. **Unified Service Access**: Get all services through ServiceManager
3. **Automatic Error Handling**: ViewModelBase provides unified error handling
4. **Cleaner Code**: No need to repeat database connection and service creation
5. **Complete Comments**: All code has detailed comments
6. **Framework Design**: Only provides architecture framework, specific business logic implemented by team members

Now you can focus on implementing business logic instead of repeating database connection code!

### 📝 Message to Team Members

This architecture has already set up the basic framework for you, including:
- ✅ Database connection management
- ✅ Service manager
- ✅ Unified error handling
- ✅ Role-based authentication system
- ✅ Window management (Login ↔ Main)
- ✅ Logout functionality
- ✅ Complete UI framework with modern design

You only need to:
1. Add specific business logic methods in service classes
2. Call these methods in ViewModels
3. Display results in Views

All database connections, error handling, configuration management, and authentication have been handled for you!



### 5. 🔄 Git Workflow

#### Before Starting Development
```bash
# Always pull latest changes
git pull origin main

# Create feature branch
git checkout -b feature/your-feature-name
```

#### During Development
```bash
# Check status
git status

# Add changes
git add .

# Commit with descriptive message
git commit -m "feat: add book search functionality

- Add Book entity with EF Core mapping
- Create BookService for database operations
- Implement BookSearchViewModel with MVVM pattern
- Add BookSearchView with search interface"
```

#### Before Merging
```bash
# Push your branch
git push origin feature/your-feature-name

# Create Pull Request (PR) on GitHub/GitLab
# Request code review from team members
# Address review comments if any
# Merge to main after approval
```

### 6. 🐛 Common Issues & Solutions

#### Entity Framework Issues
- **"Table doesn't exist"**: Run database migrations
- **"Connection failed"**: Check connection string in `AppConfig.cs`
- **"Entity not found"**: Ensure DbSet is added to DbContext

#### Avalonia UI Issues
- **"Binding errors"**: Check property names match between View and ViewModel
- **"XAML preview not working"**: Restart Rider or clean/rebuild project
- **"Controls not showing"**: Verify XAML syntax and namespace declarations

#### Git Issues
- **"Merge conflicts"**: Resolve conflicts manually, then commit
- **"Branch behind main"**: Rebase your branch: `git rebase main`

---

## 🚀 Quick Start with Rider IDE

### 1. 🖥️ Open the Project in Rider
- Launch JetBrains Rider.
- Click `Open` and select the project root folder (the one containing `InkHouse.sln`).

### 2. 📦 Restore Dependencies
- Rider will automatically detect the solution and prompt to restore NuGet packages.
- If not, right-click the solution in the Solution Explorer and select `Restore NuGet Packages`.

### 3. ⚙️ Configure Database Connection
- In the Solution Explorer, navigate to `InkHouse/Services/AppConfig.cs`.
- Find the line:
  ```csharp
  public static string DatabaseConnectionString { get; set; } = 
      "server=YOUR_SERVER;port=3306;database=InternShip;user=YOUR_USER;password=YOUR_PASSWORD;";
  ```
- Replace `YOUR_SERVER`, `YOUR_USER`, `YOUR_PASSWORD` with your actual MySQL information.

### 4. 🗄️ Initialize the Database
- Use your preferred MySQL client (such as MySQL Workbench, DBeaver, or Rider's built-in Database tool) to create a database named `InternShip`.
- Example SQL:
  ```sql
  CREATE DATABASE InternShip;
  ```

### 5. 🛠️ Build and Run the Project
- In Rider, select `InkHouse` as the startup project.
- Click the green `Run` button or press `Shift+F10` to build and run.
- The application window will appear.

### 6. 🖼️ Visual UI Design
- You can edit `.axaml` files with Rider's built-in XAML previewer for Avalonia.
- Open any `.axaml` file (such as `LoginView.axaml`), and use the split view to see both code and visual preview.
- Drag and drop controls, or edit XAML directly for custom layouts.

### 7. 🐞 Debugging
- Set breakpoints in your C# code.
- Use the `Debug` button or press `Shift+F9` to start debugging.
- Inspect variables, step through code, and view call stacks as needed.

---

## 🧩 Features
- 👤 Role-based authentication (Admin only access)
- 🔐 Secure login with role validation
- 🚪 Logout functionality with window switching
- 📊 Dashboard with system statistics
- 📚 Book management interface (ready for implementation)
- 👥 User management interface (ready for implementation)
- 📖 Borrow management interface (ready for implementation)
- 🔍 Search functionality (ready for implementation)
- ⚙️ System settings (ready for implementation)

## 🛠️ Tech Stack
- **Avalonia UI** (cross-platform desktop framework)
- **Entity Framework Core** (ORM with MySQL provider)
- **MySQL** (database)
- **.NET 8.0** (runtime)
- **MVVM Pattern** (architecture)
- **Dependency Injection** (service management)



## 👥 Contributors
- Member A
- Member B
- Member C

## ❓ FAQ & Tips
- If you see errors about missing SDK, make sure you installed .NET 8.0+ and restarted Rider
- If you cannot connect to MySQL, check your firewall, IP, port, username, and password
- For Linux, you may need to install additional libraries for Avalonia UI (see [Avalonia Docs](https://docs.avaloniaui.net/))
- For more Rider tips, see [Rider Documentation](https://www.jetbrains.com/help/rider/)

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.
