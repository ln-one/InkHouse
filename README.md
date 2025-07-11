# InkHouse Library Management System

[中文版说明](./README.zh.md)

InkHouse is a cross-platform library management system built with C#, Avalonia, and MySQL. It supports both Windows and Linux environments, providing a modern, elegant, and easy-to-use interface.

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

---

## 🛠️ Complete Development Workflow

### 1. 🚀 Project Setup & Cloning
```bash
# Clone the repository
git clone <your-repo-url>
cd InkHouse

# Create and switch to a new feature branch
git checkout -b feature/add-book-management
```

### 2. 📚 Understanding Entity Framework Core
Entity Framework Core (EF Core) is an ORM (Object-Relational Mapping) that lets you work with databases using C# objects instead of writing SQL directly.

**Key Concepts:**
- **DbContext**: Represents a session with the database (like `InkHouseContext.cs`)
- **Entities**: C# classes that map to database tables (like `Book.cs`, `User.cs`)
- **Migrations**: Track database schema changes over time

**Example: Adding a new Book entity**
```csharp
// In Models/Book.cs
public class Book
{
    public int Id { get; set; }           // Primary key
    public string Title { get; set; }     // Book title
    public string Author { get; set; }    // Author name
    public bool IsAvailable { get; set; } // Availability status
}
```

### 3. 🏗️ Development Process Example: Adding Book Search Feature

#### Step 1: Create/Update Model
```csharp
// In Models/Book.cs (if not exists)
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsAvailable { get; set; }
}
```

#### Step 2: Add to DbContext
```csharp
// In Models/InkHouseContext.cs
public class InkHouseContext : DbContext
{
    public DbSet<Book> Books { get; set; }  // This creates the Books table
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
```

#### Step 3: Create Service
```csharp
// In Services/BookService.cs
public class BookService
{
    private readonly InkHouseContext _context;
    
    public BookService(InkHouseContext context)
    {
        _context = context;
    }
    
    public List<Book> SearchBooks(string searchTerm)
    {
        return _context.Books
            .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
            .ToList();
    }
}
```

#### Step 4: Create ViewModel
```csharp
// In ViewModels/BookSearchViewModel.cs
public class BookSearchViewModel : ViewModelBase
{
    private readonly BookService _bookService;
    private string _searchTerm;
    private List<Book> _searchResults;
    
    public string SearchTerm
    {
        get => _searchTerm;
        set => SetProperty(ref _searchTerm, value);
    }
    
    public List<Book> SearchResults
    {
        get => _searchResults;
        set => SetProperty(ref _searchResults, value);
    }
    
    public void Search()
    {
        SearchResults = _bookService.SearchBooks(SearchTerm);
    }
}
```

#### Step 5: Create View
```xml
<!-- In Views/BookSearchView.axaml -->
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <StackPanel Margin="20">
        <TextBox Text="{Binding SearchTerm}" 
                 Watermark="Enter book title or author"/>
        <Button Content="Search" 
                Command="{Binding SearchCommand}"/>
        <ListBox ItemsSource="{Binding SearchResults}">
            <ListBox.ItemTemplate>
                <DataTemplate>
                    <TextBlock Text="{Binding Title}"/>
                </DataTemplate>
            </ListBox.ItemTemplate>
        </ListBox>
    </StackPanel>
</Window>
```

### 4. 📝 Code Standards & Best Practices

#### Naming Conventions
- **Classes**: PascalCase (e.g., `BookService`, `LoginViewModel`)
- **Methods**: PascalCase (e.g., `SearchBooks()`, `ValidateUser()`)
- **Properties**: PascalCase (e.g., `SearchTerm`, `IsAvailable`)
- **Private fields**: camelCase with underscore (e.g., `_bookService`, `_searchTerm`)

#### File Organization
```
Models/
├── Book.cs              # Book entity
├── User.cs              # User entity
└── InkHouseContext.cs   # Database context

Services/
├── BookService.cs       # Book-related operations
└── UserService.cs       # User-related operations

ViewModels/
├── BookSearchViewModel.cs
└── LoginViewModel.cs

Views/
├── BookSearchView.axaml
└── LoginView.axaml
```

#### Database Operations Pattern
```csharp
// Always use using statement for database operations
using (var context = new InkHouseContext())
{
    var books = context.Books
        .Where(b => b.IsAvailable)
        .ToList();
}
```

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
- **"Connection failed"**: Check connection string in `LoginView.axaml.cs`
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
- In the Solution Explorer, navigate to `InkHouse/Views/LoginView.axaml.cs`.
- Find the line:
  ```csharp
  string connectionString = "server=YOUR_SERVER;port=3306;database=InternShip;user=YOUR_USER;password=YOUR_PASSWORD;";
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
- 👤 User login (Admin & Normal User)
- 📚 Book management (CRUD)
- 🔄 Borrow and return records
- 🛡️ Role-based access control

## 🛠️ Tech Stack
- Avalonia UI (cross-platform desktop)
- Entity Framework Core (ORM)
- MySQL (cloud database)

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
This project is for educational and internship purposes only. Feel free to use and modify.