# InkHouse 图书馆管理系统

InkHouse 是一个基于 C#、Avalonia 和 MySQL 的跨平台图书馆管理系统，支持 Windows 和 Linux，界面优雅简约，易于使用。

---

## 🤝 团队协作与项目结构

### 团队协作
- **版本控制**：全员使用 Git，建议采用 GitHub/GitLab 远程仓库。
- **分支管理**：主分支（main）只存放稳定可运行代码。每个功能/修复新建 feature/bugfix 分支，开发完成后合并到 main。
- **代码提交规范**：每次提交写明本次更改内容，建议英文（如：`feat: add book borrow feature`）。
- **代码评审**：合并前建议团队成员互相 Code Review，提升代码质量。
- **任务分工**：可用 issue 或 todo list 分配任务，定期同步进度。

### 项目结构
| 目录/文件              | 作用                                         |
|------------------------|----------------------------------------------|
| `App.axaml`            | 应用入口的 XAML 配置，定义全局样式和资源         |
| `App.axaml.cs`         | 应用入口 C# 代码，初始化应用                    |
| `Assets/`              | 存放图片、图标等静态资源                        |
| `Models/`              | 数据模型层，定义数据库表结构（如 Book、User）    |
| `Services/`            | 服务层，封装数据库操作、业务逻辑                 |
| `ViewModels/`          | 视图模型层，连接 UI 和数据逻辑                   |
| `Views/`               | 视图层，XAML UI 文件及其后台代码                 |
| `InkHouse.csproj`      | 项目配置文件，依赖管理                          |
| `Program.cs`           | 程序主入口                                      |
| `app.manifest`         | 应用清单，配置权限等                            |
| `bin/`, `obj/`         | 编译输出和中间文件，自动生成                    |

### 分层架构
- **Models**：定义与数据库表对应的 C# 类（如 Book.cs、User.cs），用于数据存取和实体映射。
- **Services**：封装所有与数据库交互的逻辑（如 UserService），如增删改查、业务规则等。
- **ViewModels**：负责处理 UI 逻辑、数据绑定，桥接视图和服务层（如 LoginViewModel）。
- **Views**：XAML 文件定义界面布局，.cs 文件处理界面事件（如按钮点击）。

### 快速上手与开发建议
- **先读 README 和代码结构说明，了解每层职责。**
- **每次开发新功能，先在 Models/Services/ViewModels/Views 里各自新建对应文件。**
- **遇到问题多用断点调试，善用 Rider 的 XAML 预览和数据库工具。**
- **团队成员多沟通，遇到不懂的地方及时提问。**

---

## 🛠️ 完整开发流程指南

### 1. 🚀 项目设置与克隆
```bash
# 克隆仓库
git clone <你的仓库地址>
cd InkHouse

# 创建并切换到新功能分支
git checkout -b feature/add-book-management
```

### 2. 📚 理解 Entity Framework Core
Entity Framework Core (EF Core) 是一个 ORM（对象关系映射）框架，让你可以用 C# 对象操作数据库，而不需要直接写 SQL。

**核心概念：**
- **DbContext**：代表与数据库的会话（如 `InkHouseContext.cs`）
- **Entities**：映射到数据库表的 C# 类（如 `Book.cs`、`User.cs`）
- **Migrations**：跟踪数据库架构随时间的变化

**示例：添加新的 Book 实体**
```csharp
// 在 Models/Book.cs 中
public class Book
{
    public int Id { get; set; }           // 主键
    public string Title { get; set; }     // 书名
    public string Author { get; set; }    // 作者
    public bool IsAvailable { get; set; } // 是否可借
}
```

### 3. 🏗️ 开发流程示例：添加图书搜索功能

#### 第一步：创建/更新模型
```csharp
// 在 Models/Book.cs 中（如果不存在）
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsAvailable { get; set; }
}
```

#### 第二步：添加到 DbContext
```csharp
// 在 Models/InkHouseContext.cs 中
public class InkHouseContext : DbContext
{
    public DbSet<Book> Books { get; set; }  // 这会创建 Books 表
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
```

#### 第三步：创建服务
```csharp
// 在 Services/BookService.cs 中
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

#### 第四步：创建视图模型
```csharp
// 在 ViewModels/BookSearchViewModel.cs 中
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

#### 第五步：创建视图
```xml
<!-- 在 Views/BookSearchView.axaml 中 -->
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <StackPanel Margin="20">
        <TextBox Text="{Binding SearchTerm}" 
                 Watermark="输入书名或作者"/>
        <Button Content="搜索" 
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

### 4. 📝 代码规范与最佳实践

#### 命名规范
- **类名**：PascalCase（如 `BookService`、`LoginViewModel`）
- **方法名**：PascalCase（如 `SearchBooks()`、`ValidateUser()`）
- **属性名**：PascalCase（如 `SearchTerm`、`IsAvailable`）
- **私有字段**：camelCase 加下划线（如 `_bookService`、`_searchTerm`）

#### 文件组织
```
Models/
├── Book.cs              # 图书实体
├── User.cs              # 用户实体
└── InkHouseContext.cs   # 数据库上下文

Services/
├── BookService.cs       # 图书相关操作
└── UserService.cs       # 用户相关操作

ViewModels/
├── BookSearchViewModel.cs
└── LoginViewModel.cs

Views/
├── BookSearchView.axaml
└── LoginView.axaml
```

#### 数据库操作模式
```csharp
// 始终使用 using 语句进行数据库操作
using (var context = new InkHouseContext())
{
    var books = context.Books
        .Where(b => b.IsAvailable)
        .ToList();
}
```

### 5. 🔄 Git 工作流程

#### 开始开发前
```bash
# 始终拉取最新更改
git pull origin main

# 创建功能分支
git checkout -b feature/你的功能名称
```

#### 开发过程中
```bash
# 检查状态
git status

# 添加更改
git add .

# 提交并描述更改
git commit -m "feat: 添加图书搜索功能

- 使用 EF Core 映射添加 Book 实体
- 创建 BookService 进行数据库操作
- 使用 MVVM 模式实现 BookSearchViewModel
- 添加带搜索界面的 BookSearchView"
```

#### 合并前
```bash
# 推送你的分支
git push origin feature/你的功能名称

# 在 GitHub/GitLab 上创建 Pull Request (PR)
# 请求团队成员进行代码审查
# 如有审查意见，及时修改
# 获得批准后合并到 main
```

### 6. 🐛 常见问题与解决方案

#### Entity Framework 问题
- **"表不存在"**：运行数据库迁移
- **"连接失败"**：检查 `LoginView.axaml.cs` 中的连接字符串
- **"实体未找到"**：确保 DbSet 已添加到 DbContext

#### Avalonia UI 问题
- **"绑定错误"**：检查 View 和 ViewModel 中的属性名是否匹配
- **"XAML 预览不工作"**：重启 Rider 或清理/重建项目
- **"控件不显示"**：验证 XAML 语法和命名空间声明

#### Git 问题
- **"合并冲突"**：手动解决冲突，然后提交
- **"分支落后于 main"**：变基你的分支：`git rebase main`

---

## 🚀 使用 Rider IDE 快速上手

### 1. 🖥️ 用 Rider 打开项目
- 启动 JetBrains Rider。
- 点击 `Open`，选择项目根目录（包含 `InkHouse.sln` 的文件夹）。

### 2. 📦 还原依赖
- Rider 会自动检测解决方案并提示还原 NuGet 包。
- 如未自动还原，可在解决方案资源管理器中右键解决方案，选择“还原 NuGet 包”。

### 3. ⚙️ 配置数据库连接
- 在解决方案资源管理器中，定位到 `InkHouse/Views/LoginView.axaml.cs`。
- 找到如下代码：
  ```csharp
  string connectionString = "server=你的服务器;port=3306;database=InternShip;user=你的用户名;password=你的密码;";
  ```
- 将 `你的服务器`、`你的用户名`、`你的密码` 替换为实际的 MySQL 信息。

### 4. 🗄️ 初始化数据库
- 使用你喜欢的 MySQL 客户端（如 MySQL Workbench、DBeaver 或 Rider 内置数据库工具）新建名为 `InternShip` 的数据库。
- 示例 SQL：
  ```sql
  CREATE DATABASE InternShip;
  ```

### 5. 🛠️ 构建并运行项目
- 在 Rider 中，将 `InkHouse` 设为启动项目。
- 点击绿色 `运行` 按钮或按 `Shift+F10` 构建并运行。
- 应用窗口会自动弹出。

### 6. 🖼️ 可视化界面设计
- 可以用 Rider 内置的 Avalonia XAML 预览器编辑 `.axaml` 文件。
- 打开任意 `.axaml` 文件（如 `LoginView.axaml`），使用分屏模式同时查看代码和界面预览。
- 可拖拽控件或直接编辑 XAML 代码实现自定义布局。

### 7. 🐞 调试
- 在 C# 代码中设置断点。
- 点击 `调试` 按钮或按 `Shift+F9` 启动调试。
- 可查看变量、单步执行、查看调用堆栈等。

---

## 🧩 功能模块
- 👤 用户登录（管理员/普通用户）
- 📚 图书管理（增删改查）
- 🔄 借阅归还记录
- 🛡️ 权限管理

## 🛠️ 技术栈
- Avalonia UI（跨平台桌面）
- Entity Framework Core（ORM）
- MySQL（云数据库）

## 👥 贡献者
- 组员A
- 组员B
- 组员C

## ❓ 常见问题与小贴士
- 如果提示缺少SDK，请确认已安装 .NET 8.0+ 并重启 Rider
- 如果无法连接MySQL，请检查防火墙、IP、端口、用户名和密码
- Linux下可能需要额外安装 Avalonia UI 依赖（见 [Avalonia官方文档](https://docs.avaloniaui.net/)）
- 更多 Rider 使用技巧见 [Rider 官方文档](https://www.jetbrains.com/help/rider/)

## 📄 许可
本项目仅供学习和实习使用，欢迎自由使用和修改。 