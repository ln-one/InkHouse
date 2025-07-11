# InkHouse 图书馆管理系统

InkHouse 是一个基于 C#、Avalonia 和 MySQL 的跨平台图书馆管理系统，支持 Windows 和 Linux，界面优雅简约，易于使用。

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