# InkHouse 图书馆管理系统

InkHouse 是一个基于 C#、Avalonia 和 MySQL 的跨平台图书馆管理系统，支持 Windows 和 Linux，界面优雅简约，易于使用。

## ✨ 功能特性

### 🔐 身份认证与授权
- **基于角色的访问控制**：管理员和普通用户角色，拥有不同权限
- **安全登录**：用户名/密码认证，使用 BCrypt 密码哈希加密
- **用户注册**：新用户注册功能，带有验证和安全特性
- **会话管理**：自动登出功能，支持窗口切换

### 📊 仪表板与统计
- **系统概览**：总藏书量、可借图书、借出图书、注册用户
- **实时统计**：动态仪表板，显示关键指标
- **可视化指示器**：图书状态和座位使用情况指示器

### 📚 图书管理
- **图书CRUD操作**：添加、编辑、删除、查看图书
- **图书状态跟踪**：可借、已借出、逾期、维护中状态
- **搜索与过滤**：高级搜索和过滤功能
- **图书类型分类**：按类型对图书进行分类，便于组织管理

### 👥 用户管理
- **用户CRUD操作**：添加、编辑、删除、查看用户
- **角色管理**：支持管理员和普通用户角色
- **用户认证**：基于角色的安全登录
- **用户资料**：个人统计和活动跟踪

### 📖 借阅管理
- **借阅记录**：跟踪所有图书借阅活动
- **借书/还书操作**：完整的借阅和归还工作流程
- **逾期跟踪**：监控逾期图书和通知
- **用户借阅历史**：查看个人借阅历史和统计数据

### 🪑 座位预约系统
- **座位管理**：查看和管理图书馆座位
- **座位预约**：用户可以预约可用座位
- **签到/签退**：通过签到和签退功能跟踪座位使用情况
- **预约历史**：查看个人座位预约历史和统计数据

### � 搜报表与分析
- **统计报表**：全面的图书馆统计数据
- **用户活动**：跟踪用户借阅和座位预约模式
- **分析仪表板**：可视化数据展示

### ⚙️ 系统设置
- **配置管理**：集中式系统配置
- **数据库设置**：简单的数据库连接管理
- **系统偏好**：可自定义的系统设置

### 🔍 搜索与导航
- **全局搜索**：跨图书、用户、记录的搜索
- **高级过滤**：多条件过滤，包括图书类型过滤
- **导航菜单**：直观的侧边栏导航，适用于管理员和用户界面

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

### 🚀 当前实现状态

#### ✅ 已完成功能
- **身份认证系统**：基于角色的登录，支持管理员和普通用户访问
- **用户注册**：新用户注册功能，带有验证和安全特性
- **窗口管理**：不同角色的登录窗口 ↔ 主窗口切换
- **登出功能**：安全的登出，正确的窗口管理
- **UI框架**：完整的现代化UI，包含仪表板、导航和响应式设计
- **数据库集成**：MySQL数据库与Entity Framework Core
- **服务架构**：集中式服务管理，依赖注入
- **错误处理**：所有层的统一错误处理
- **事件系统**：登录成功/失败事件，正确的窗口切换
- **图书浏览与借阅**：支持分页浏览图书，借阅/归还后列表即时更新，无需整体刷新
- **图书类型分类**：图书可按类型分类并进行过滤
- **搜索与分页**：使用 EF Core `AsNoTracking`、`Skip/Take` 优化查询，并提供 "加载更多" UI
- **性能优化**：连接池、延迟加载及优化查询，大幅提升速度，即使在大数据量下也能流畅运行
- **座位预约系统**：完整的座位管理和预约功能
- **用户个人资料**：借阅历史和座位预约的个人统计

#### 🔄 可进一步增强的功能
- **图书管理**：基本功能已实现，可增强批量操作功能
- **用户管理**：基本功能已实现，可增强更多用户详情
- **仪表板统计**：基本统计已实现，可增强更多可视化效果
- **搜索功能**：基本搜索已实现，可增强高级过滤功能
- **系统设置**：基本设置已实现，可增强更多配置选项

---

## 🛠️ 开发架构与工作流程

### 🎯 架构优势
- ✅ **统一配置管理**：所有配置都在 `AppConfig.cs` 中
- ✅ **服务管理器**：通过 `ServiceManager` 统一获取服务
- ✅ **自动错误处理**：`ViewModelBase` 提供统一的错误处理
- ✅ **简化开发**：无需手动创建数据库连接和服务
- ✅ **框架化设计**：只提供架构框架，具体业务逻辑由团队成员实现

### 🏗️ 架构概览

#### 1. 配置管理 (`AppConfig.cs`)
```csharp
// 修改数据库连接信息
AppConfig.SetDatabaseConnection("服务器", 3306, "数据库", "用户名", "密码");

// 或者直接修改连接字符串
AppConfig.DatabaseConnectionString = "你的连接字符串";
```

#### 2. 服务管理器 (`ServiceManager.cs`)
```csharp
// 获取各种服务（无需手动创建）
var userService = ServiceManager.GetService<UserService>();
var bookService = ServiceManager.GetService<BookService>();
var borrowService = ServiceManager.GetService<BorrowRecordService>();
var seatService = ServiceManager.GetService<SeatService>();
```

#### 3. 基础ViewModel (`ViewModelBase.cs`)
```csharp
public class MyFeatureViewModel : ViewModelBase
{
    // 自动获得以下功能：
    // - IsLoading: 加载状态
    // - ErrorMessage: 错误消息
    // - SuccessMessage: 成功消息
    // - ShowError(): 显示错误
    // - ShowSuccess(): 显示成功
    // - ExecuteAsync(): 安全执行异步操作
}
```

### 🚀 快速开始指南

#### 步骤1：配置数据库
**⚠️ 重要提示**：出于安全考虑，实际的 `AppConfig.cs` 文件不包含在仓库中。

1. **复制模板文件**：
   ```bash
   cp InkHouse/Services/AppConfig.template.cs InkHouse/Services/AppConfig.cs
   ```

2. **在 `Services/AppConfig.cs` 中修改数据库连接信息**：
   ```csharp
   public static string DatabaseConnectionString { get; set; } = 
       "server=你的服务器;port=3306;database=InternShip;user=你的用户名;password=你的密码;";
   ```

3. **将占位符替换为你的实际数据库信息**：
   - `你的服务器`：数据库服务器地址
   - `你的用户名`：数据库用户名
   - `你的密码`：数据库密码

📖 **查看详细设置说明**：`InkHouse/Services/README.md`

#### 步骤2：创建新的ViewModel
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
            // 获取服务
            var service = ServiceManager.GetService<UserService>();
            
            // 执行业务逻辑
            var result = service.SomeMethod();
            
            // 显示结果
            ShowSuccess("操作成功！");
        });
    }
}
```

#### 步骤3：创建View
```csharp
public partial class MyFeatureView : UserControl
{
    public MyFeatureView()
    {
        InitializeComponent();
        DataContext = new MyFeatureViewModel(); // 无需传入服务
    }
}

// 或者创建Window
public partial class MyFeatureWindow : Window
{
    public MyFeatureWindow()
    {
        InitializeComponent();
        DataContext = new MyFeatureViewModel();
    }
}
```

### 📚 服务使用示例

#### 用户服务 (UserService)
```csharp
var userService = ServiceManager.GetService<UserService>();

// 用户登录（已实现）
var user = userService.Login("用户名", "密码");

// 用户注册（已实现）
var registerResult = await userService.RegisterAsync(newUser);

// 获取所有用户
var users = await userService.GetAllUsersAsync();

// 搜索用户
var searchResults = await userService.SearchUsersAsync("搜索词");
```

#### 图书服务 (BookService)
```csharp
var bookService = ServiceManager.GetService<BookService>();

// 获取所有图书
var books = await bookService.GetAllBooksAsync();

// 按类型获取图书
var fictionBooks = await bookService.GetBooksByTypeAsync("小说");

// 搜索图书
var searchResults = await bookService.SearchBooksAsync("关键词");

// 获取图书统计信息
var (totalBooks, availableBooks, borrowedBooks) = await bookService.GetBookStatisticsAsync();
```

#### 借阅记录服务 (BorrowRecordService)
```csharp
var borrowService = ServiceManager.GetService<BorrowRecordService>();

// 获取所有借阅记录
var records = await borrowService.GetAllBorrowRecordsAsync();

// 获取用户的借阅记录
var userRecords = await borrowService.GetBorrowRecordsByUserIdAsync(userId);

// 借书
var borrowRecord = await borrowService.BorrowBookAsync(bookId, userId);

// 还书
var success = await borrowService.ReturnBookAsync(borrowRecordId);
```

#### 座位服务 (SeatService)
```csharp
var seatService = ServiceManager.GetService<SeatService>();

// 获取所有座位
var seats = await seatService.GetAllSeatsAsync();

// 预约座位
var reservation = await seatService.ReserveSeatAsync(seatId, userId);

// 签到
var updatedReservation = await seatService.CheckInAsync(reservationId);

// 签退
var completedReservation = await seatService.CheckOutAsync(reservationId);
```

### 🎨 UI开发技巧

#### 1. 数据绑定
```xml
<!-- 绑定到ViewModel的属性 -->
<TextBox Text="{Binding UserName}" />
<TextBox Classes="password" Text="{Binding Password}" />
<Button Command="{Binding LoginCommand}" Content="登录" />

<!-- 绑定到列表 -->
<ListBox ItemsSource="{Binding Books}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Title}"/>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>

<!-- 绑定到窗口事件 -->
<Window x:Class="InkHouse.Views.LoginWindow">
    <!-- 窗口内容 -->
</Window>
```

#### 2. 显示加载状态
```xml
<StackPanel>
    <!-- 加载指示器 -->
    <ProgressBar IsIndeterminate="{Binding IsLoading}" 
                 IsVisible="{Binding IsLoading}" />
    
    <!-- 错误消息 -->
    <TextBlock Text="{Binding ErrorMessage}" 
               Foreground="Red" 
               IsVisible="{Binding ErrorMessage, Converter={x:Static StringConverters.IsNotNullOrEmpty}}" />
    
    <!-- 成功消息 -->
    <TextBlock Text="{Binding SuccessMessage}" 
               Foreground="Green" 
               IsVisible="{Binding SuccessMessage, Converter={x:Static StringConverters.IsNotNullOrEmpty}}" />
</StackPanel>
```

#### 3. 命令绑定
```xml
<Button Command="{Binding LoginCommand}" Content="登录" />
<Button Command="{Binding LogoutCommand}" Content="登出" />
<Button Command="{Binding SearchCommand}" Content="搜索" />
```

### 🔧 调试技巧

#### 1. 查看错误信息
所有服务方法都有异常处理，错误信息会输出到控制台：
```csharp
Console.WriteLine($"操作失败: {ex.Message}");
```

#### 2. 使用ViewModel的调试功能
```csharp
// 在ViewModel中显示调试信息
ShowError("调试信息");
ShowSuccess("操作成功");
```

#### 3. 检查数据库连接
```csharp
// 在AppConfig.cs中设置调试模式
AppConfig.IsDebugMode = true;
```

### 📝 开发规范

#### 1. 命名规范
- **类名**：PascalCase（如 `BookService`、`LoginViewModel`）
- **方法名**：PascalCase（如 `GetAllBooks()`、`ValidateUser()`）
- **属性名**：PascalCase（如 `BookTitle`、`IsAvailable`）
- **私有字段**：camelCase + 下划线（如 `_bookService`、`_searchTerm`）

#### 2. 文件组织
```
Models/
├── Book.cs              # 图书实体
├── User.cs              # 用户实体
├── BorrowRecord.cs      # 借阅记录实体
├── Seat.cs              # 座位实体
├── SeatReservation.cs   # 座位预约实体
├── UserRoles.cs         # 用户角色常量
└── InkHouseContext.cs   # Entity Framework上下文

Services/
├── AppConfig.cs         # 配置管理
├── ServiceManager.cs    # 服务管理器
├── DbContextFactory.cs  # 数据库上下文工厂
├── UserService.cs       # 用户服务
├── BookService.cs       # 图书服务
├── BorrowRecordService.cs # 借阅记录服务
└── SeatService.cs       # 座位服务

ViewModels/
├── ViewModelBase.cs     # 基础ViewModel
├── LoginViewModel.cs    # 登录ViewModel
├── RegisterViewModel.cs # 注册ViewModel
├── MainWindowViewModel.cs # 管理员主窗口ViewModel
├── UserMainWindowViewModel.cs # 用户主窗口ViewModel
└── SeatReservationViewModel.cs # 座位预约ViewModel

Views/
├── LoginView.axaml      # 登录用户控件
├── LoginView.axaml.cs   # 登录用户控件代码后端
├── RegisterView.axaml   # 注册用户控件
├── RegisterView.axaml.cs # 注册用户控件代码后端
├── MainWindow.axaml     # 管理员主窗口
└── UserMainWindow.axaml # 用户主窗口
```

#### 3. 代码注释
所有公共方法和属性都应该有中文注释：
```csharp
/// <summary>
/// 获取所有图书
/// </summary>
/// <returns>图书列表</returns>
public List<Book> GetAllBooks()
{
    // 实现代码
}
```

### 🚨 常见问题

#### Q1: 数据库连接失败怎么办？
A1: 检查 `AppConfig.cs` 中的连接字符串是否正确，确保数据库服务器可访问。

#### Q2: 为什么我的ViewModel没有响应？
A2: 确保ViewModel继承自 `ViewModelBase`，并且属性使用了 `SetProperty` 方法。

#### Q3: 如何添加新的服务？
A3: 创建新的服务类，继承相同的模式，然后在 `ServiceManager` 中添加对应的属性。

#### Q4: 如何处理复杂的业务逻辑？
A4: 在服务层实现复杂的业务逻辑，ViewModel只负责UI交互和数据绑定。

### 🎉 总结

新的架构让开发变得更加简单和统一：

1. **配置集中管理**：所有配置都在一个地方
2. **服务统一获取**：通过ServiceManager获取所有服务
3. **错误自动处理**：ViewModelBase提供统一的错误处理
4. **代码更简洁**：无需重复创建数据库连接和服务
5. **中文注释完整**：所有代码都有详细的中文说明
6. **框架化设计**：只提供架构框架，具体业务逻辑由团队成员实现

现在你可以专注于业务逻辑的实现，而不是重复的数据库连接代码！

### 📝 给团队成员的话

这个架构已经为您搭建好了基础框架，包括：
- ✅ 数据库连接管理
- ✅ 服务管理器
- ✅ 统一的错误处理
- ✅ 基于角色的身份认证系统
- ✅ 窗口管理（登录 ↔ 主界面）
- ✅ 登出功能
- ✅ 完整的UI框架，现代化设计
- ✅ 图书类型分类和过滤
- ✅ 座位预约系统
- ✅ 用户个人资料统计

您只需要：
1. 在服务类中添加具体的业务逻辑方法
2. 在ViewModel中调用这些方法
3. 在View中展示结果

所有的数据库连接、错误处理、配置管理和身份认证都已经为您处理好了！

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
- **"连接失败"**：检查 `AppConfig.cs` 中的连接字符串
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
- 如未自动还原，可在解决方案资源管理器中右键解决方案，选择"还原 NuGet 包"。

### 3. ⚙️ 配置数据库连接
- 在解决方案资源管理器中，定位到 `InkHouse/Services/AppConfig.cs`。
- 找到如下代码：
  ```csharp
  public static string DatabaseConnectionString { get; set; } = 
      "server=你的服务器;port=3306;database=InternShip;user=你的用户名;password=你的密码;";
  ```
- 将 `你的服务器`、`你的用户名`、`你的密码` 替换为实际的 MySQL 信息。

### 4. 🗄️ 初始化数据库
- 使用你喜欢的 MySQL 客户端（如 MySQL Workbench、DBeaver 或 Rider 内置数据库工具）新建名为 `InternShip` 的数据库。
- 示例 SQL：
  ```sql
  CREATE DATABASE InternShip;
  ```

### 5. �的️ 构建并运行项目
- 在 Rider 中，将 `InkHouse` 设为启动项目。
- 点击绿色 `运行` 按钮或按 `Shift+F10` 构建并运行。
- 应用窗口会自动弹出。

### 6. 🖼️ 可视化界面设计
- 可以用 Rider 内置的 Avalonia XAML 预览器编辑 `.axaml` 文件。
- 打开任意 `.axaml` 文件（如 `LoginView.axaml`），使用分屏模式同时查看代码和界面预览。
- 可拖拽控件或直接编辑 XAML 代码实现自定义布局。

### 7. �栈 调试
- 在 C# 代码中设置断点。
- 点击 `调试` 按钮或按 `Shift+F9` 启动调试。
- 可查看变量、单步执行、查看调用堆栈等。

---

## 🧩 功能模块
- � 基于角色的身份认证（管理员和普通用户访问）
- 🔐 安全的登录验证和角色检查，使用 BCrypt 密码哈希
- 📝 用户注册功能，带有验证和安全特性
- 🚪 登出功能，支持窗口切换
- 📊 仪表板，显示系统统计信息
- 📚 图书管理，支持类型分类和过滤
- 👥 用户管理，支持角色分配
- 📖 借阅管理，支持历史跟踪
- 🪑 座位预约系统，支持签到/签退
- 👤 用户个人资料，显示个人统计数据
- 🔍 跨图书和记录的搜索功能
- ⚙️ 系统配置设置

## 🛠️ 技术栈
- **Avalonia UI**（跨平台桌面框架）
- **Entity Framework Core**（ORM，MySQL提供程序）
- **MySQL**（数据库）
- **.NET 8.0**（运行时）
- **MVVM模式**（架构）
- **依赖注入**（服务管理）
- **BCrypt**（密码哈希）

## 👥 贡献者
- 成员A
- 成员B
- 成员C

## ❓ 常见问题与小贴士
- 如果提示缺少SDK，请确认已安装 .NET 8.0+ 并重启 Rider
- 如果无法连接MySQL，请检查防火墙、IP、端口、用户名和密码
- Linux下可能需要额外安装 Avalonia UI 依赖（见 [Avalonia官方文档](https://docs.avaloniaui.net/)）
- 更多 Rider 使用技巧见 [Rider 官方文档](https://www.jetbrains.com/help/rider/)

## 📄 许可
本项目仅供学习和实习使用，欢迎自由使用和修改。