# 配置文件设置说明

## 🔐 安全配置

为了保护敏感信息（如数据库密码），实际的配置文件 `AppConfig.cs` 已被添加到 `.gitignore` 中，不会被提交到版本控制系统。

## 📋 设置步骤

### 1. 复制模板文件
```bash
# 将模板文件复制为实际配置文件
cp AppConfig.template.cs AppConfig.cs
```

### 2. 修改数据库连接信息
打开 `AppConfig.cs` 文件，修改以下参数：

```csharp
public static string DatabaseConnectionString { get; set; } = 
    "server=YOUR_SERVER;port=3306;database=InternShip;user=YOUR_USERNAME;password=YOUR_PASSWORD;";
```

将以下占位符替换为你的实际信息：
- `YOUR_SERVER`: 数据库服务器地址
- `YOUR_USERNAME`: 数据库用户名
- `YOUR_PASSWORD`: 数据库密码

### 3. 验证配置
确保 `AppConfig.cs` 文件已添加到 `.gitignore` 中（通常已经配置好了）。

## 🔒 安全提醒

- ✅ **不要**将包含真实密码的 `AppConfig.cs` 提交到Git
- ✅ **不要**在代码中硬编码密码
- ✅ **使用**模板文件作为起点
- ✅ **确保** `.gitignore` 包含 `AppConfig.cs`

## 📝 示例配置

```csharp
// 本地开发环境
"server=localhost;port=3306;database=InternShip;user=root;password=your_password;"

// 云数据库
"server=your-server.com;port=3306;database=InternShip;user=your_user;password=your_password;"
```

## 🚨 注意事项

如果你意外提交了包含密码的文件：
1. 立即更改数据库密码
2. 从Git历史中删除敏感文件
3. 通知团队成员更新他们的配置 