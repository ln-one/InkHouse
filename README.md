# InkHouse Library Management System

[中文版说明](./README.zh.md)

InkHouse is a cross-platform library management system built with C#, Avalonia, and MySQL. It supports both Windows and Linux environments, providing a modern, elegant, and easy-to-use interface.

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