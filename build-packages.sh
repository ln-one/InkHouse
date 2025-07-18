#!/bin/bash

# InkHouse 跨平台打包脚本
# 此脚本用于为Windows、Linux和macOS生成安装包

# 设置版本号
VERSION="1.0.0"
PROJECT_PATH="./InkHouse/InkHouse.csproj"
OUTPUT_DIR="./dist"

# 创建输出目录
mkdir -p $OUTPUT_DIR

echo "===== InkHouse 跨平台打包开始 ====="
echo "版本: $VERSION"
echo "输出目录: $OUTPUT_DIR"
echo "====================================="

# 确保安装了必要的工具
echo "检查必要工具..."
if ! command -v dotnet &> /dev/null; then
    echo "错误: 未找到dotnet命令。请安装.NET SDK 9.0或更高版本。"
    exit 1
fi

# 恢复项目依赖
echo "恢复项目依赖..."
dotnet restore $PROJECT_PATH

# 为Windows打包
echo "===== 为Windows打包 ====="
# Windows x64
echo "构建Windows x64版本..."
dotnet publish $PROJECT_PATH -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true /p:IncludeNativeLibrariesForSelfExtract=true -o "$OUTPUT_DIR/win-x64"
# 创建ZIP包
echo "创建Windows x64 ZIP包..."
cd "$OUTPUT_DIR/win-x64"
zip -r "../InkHouse-$VERSION-win-x64.zip" .
cd -

# Windows ARM64
echo "构建Windows ARM64版本..."
dotnet publish $PROJECT_PATH -c Release -r win-arm64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true /p:IncludeNativeLibrariesForSelfExtract=true -o "$OUTPUT_DIR/win-arm64"
# 创建ZIP包
echo "创建Windows ARM64 ZIP包..."
cd "$OUTPUT_DIR/win-arm64"
zip -r "../InkHouse-$VERSION-win-arm64.zip" .
cd -

# 为Linux打包
echo "===== 为Linux打包 ====="
# Linux x64
echo "构建Linux x64版本..."
dotnet publish $PROJECT_PATH -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true -o "$OUTPUT_DIR/linux-x64"
# 创建tar.gz包
echo "创建Linux x64 tar.gz包..."
cd "$OUTPUT_DIR/linux-x64"
tar -czf "../InkHouse-$VERSION-linux-x64.tar.gz" .
cd -

# Linux ARM64
echo "构建Linux ARM64版本..."
dotnet publish $PROJECT_PATH -c Release -r linux-arm64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true -o "$OUTPUT_DIR/linux-arm64"
# 创建tar.gz包
echo "创建Linux ARM64 tar.gz包..."
cd "$OUTPUT_DIR/linux-arm64"
tar -czf "../InkHouse-$VERSION-linux-arm64.tar.gz" .
cd -

# 为macOS打包
echo "===== 为macOS打包 ====="
# macOS x64
echo "构建macOS x64版本..."
dotnet publish $PROJECT_PATH -c Release -r osx-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true -o "$OUTPUT_DIR/osx-x64"
# 创建tar.gz包
echo "创建macOS x64 tar.gz包..."
cd "$OUTPUT_DIR/osx-x64"
tar -czf "../InkHouse-$VERSION-osx-x64.tar.gz" .
cd -

# macOS ARM64 (Apple Silicon)
echo "构建macOS ARM64版本..."
dotnet publish $PROJECT_PATH -c Release -r osx-arm64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true -o "$OUTPUT_DIR/osx-arm64"
# 创建tar.gz包
echo "创建macOS ARM64 tar.gz包..."
cd "$OUTPUT_DIR/osx-arm64"
tar -czf "../InkHouse-$VERSION-osx-arm64.tar.gz" .
cd -

echo "===== 打包完成 ====="
echo "所有安装包已生成到: $OUTPUT_DIR"
echo "Windows x64: InkHouse-$VERSION-win-x64.zip"
echo "Windows ARM64: InkHouse-$VERSION-win-arm64.zip"
echo "Linux x64: InkHouse-$VERSION-linux-x64.tar.gz"
echo "Linux ARM64: InkHouse-$VERSION-linux-arm64.tar.gz"
echo "macOS x64: InkHouse-$VERSION-osx-x64.tar.gz"
echo "macOS ARM64: InkHouse-$VERSION-osx-arm64.tar.gz"
echo "====================================="