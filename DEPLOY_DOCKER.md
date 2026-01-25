# WebCodeCli Docker 部署文档

## 文档定位与优先级（必读）

本文档是 **默认/推荐** 的部署入口：

- 默认模式：**无需任何 .env 或环境变量配置**，首次访问通过 Web 设置向导完成管理员与 Claude/Codex 等配置。
- 高级模式：如果你需要 **无人值守部署（预置密钥/模型/代理地址）**，或想了解 **容器内置 Claude/Codex CLI 的配置生成与验证**，请参考 `docs/Docker-CLI-集成部署指南.md`。

> 命令说明：下文示例以 `docker compose`（Compose v2）为主；若你的环境仍是 `docker-compose`，可等价替换。

## 🚀 快速开始（推荐）

**WebCodeCli 支持一键部署，无需任何配置文件！** 首次访问时，系统会自动引导您完成所有配置。

### 30 秒部署

```bash
# 1. 克隆代码
git clone https://github.com/xuzeyu91/WebCode.git
cd WebCode

# 2. 一键启动
docker compose up -d

# 3. 访问 http://localhost:5000
#    首次访问会自动进入设置向导
```

就这么简单！🎉

---

## 概述

WebCodeCli 采用 **Web 界面配置** 模式，所有配置都可以在首次访问时通过设置向导完成：

| 配置项 | 需要手动配置？ | 说明 |
|-------|---------------|------|
| 管理员账户 | ❌ 不需要 | 首次访问时在页面设置 |
| Claude Code API | ❌ 不需要 | 首次访问时在页面设置 |
| Codex API | ❌ 不需要 | 首次访问时在页面设置 |
| 数据库 | ❌ 不需要 | 自动使用 SQLite |
| 工作区路径 | ❌ 不需要 | 自动检测 `/app/workspaces` |
| 端口 | ❌ 不需要 | 默认 5000，可通过环境变量修改 |

---

## 一、环境准备

### 1.1 系统要求
- Docker 已安装
- Docker Compose 已安装（推荐）
- 端口 5000 可用

### 1.2 检查环境
```bash
# 检查 Docker
docker --version

# 检查 Docker Compose
docker compose version
```

---

## 二、部署方式

### 方式一：Docker Compose（推荐）

```bash
# 克隆代码
git clone https://github.com/xuzeyu91/WebCode.git
cd WebCode

# 一键启动
docker compose up -d

# 查看状态
docker compose ps
```

**自定义端口：**
```bash
# 使用环境变量指定端口
APP_PORT=8080 docker compose up -d
```

### 方式二：Docker Run（高级配置）

```bash
# 构建镜像
docker build -t webcodecli:latest .

# 启动容器
docker run -d \
  --name webcodecli \
  --restart unless-stopped \
  --network=host \
  -v webcodecli-data:/app/data \
  -v webcodecli-workspaces:/app/workspaces \
  -v webcodecli-logs:/app/logs \
  webcodecli:latest
```

### 方式三：Docker Run（完整挂载，包含技能文件）

```bash
# 启动容器（挂载 appsettings.json 和技能文件）
docker run -d \
  --name webcodecli \
  --restart unless-stopped \
  --network=host \
  --env-file .env \
  -v /data/webcode/WebCode/appsettings.json:/app/appsettings.json \
  -v /data/webcode/workspace:/webcode/workspace \
  -v /data/webcode/WebCode/skills/codex:/root/.codex/skills \
  -v /data/webcode/WebCode/skills/claude:/root/.claude/skills \
  -v webcodecli-data:/app/data \
  -v webcodecli-workspaces:/app/workspaces \
  -v webcodecli-logs:/app/logs \
  webcodecli:latest
```

### 挂载说明

| 宿主机路径 | 容器路径 | 说明 | 必需 |
|------------|----------|------|------|
| `webcodecli-data` | `/app/data` | 数据库和配置 | ✅ |
| `webcodecli-workspaces` | `/app/workspaces` | 工作区文件 | ✅ |
| `webcodecli-logs` | `/app/logs` | 应用日志 | ✅ |
| `/path/to/appsettings.json` | `/app/appsettings.json` | 配置文件（高级） | ❌ |
| `/path/to/skills/codex` | `/root/.codex/skills` | Codex 技能（高级） | ❌ |
| `/path/to/skills/claude` | `/root/.claude/skills` | Claude 技能（高级） | ❌ |

---

## 三、首次配置向导

启动容器后，访问 `http://localhost:5000`，系统会自动跳转到设置向导：

### 步骤 1：设置管理员账户
- 输入用户名和密码
- 此账户用于登录系统

### 步骤 2：配置 Claude Code（可选）
- `ANTHROPIC_BASE_URL`: API 基础地址
- `ANTHROPIC_AUTH_TOKEN`: API 令牌
- `ANTHROPIC_MODEL`: 模型名称
- 可以跳过，稍后在系统中配置

### 步骤 3：配置 Codex（可选）
- `NEW_API_KEY`: API 密钥
- `CODEX_BASE_URL`: API 基础地址
- `CODEX_MODEL`: 模型名称
- 可以跳过，稍后在系统中配置

完成向导后，系统会自动跳转到登录页面。

---

## 四、数据持久化

Docker Compose 自动创建以下数据卷：

| 数据卷 | 容器路径 | 说明 |
|--------|----------|------|
| `webcodecli-data` | `/app/data` | 数据库和配置 |
| `webcodecli-workspaces` | `/app/workspaces` | 工作区文件 |
| `webcodecli-logs` | `/app/logs` | 应用日志 |

**数据不会丢失**：即使删除容器，只要不删除数据卷，所有配置和数据都会保留。

---

## 五、高级配置（技能文件挂载）

### 5.1 准备技能文件

技能文件是 Claude 和 Codex CLI 的扩展功能，包含各种预定义的工作流和任务模板。

#### 5.1.1 创建技能目录
```bash
mkdir -p /data/webcode/WebCode/skills/codex
mkdir -p /data/webcode/WebCode/skills/claude
```

#### 5.1.2 复制技能文件
```bash
# 从现有服务复制技能文件
cp -r /data/www/.codex/skills/* /data/webcode/WebCode/skills/codex/
cp -r /data/www/.claude/skills/* /data/webcode/WebCode/skills/claude/
```

#### 5.1.3 技能列表

**Codex 技能** (20个):
```
algorithmic-art           # 算法艺术生成
brand-guidelines         # 品牌指南处理
canvas-design           # Canvas 设计工具
distributed-task-orchestrator  # 分布式任务编排
doc-coauthoring         # 文档协作
docx                    # DOCX 文件处理
frontend-design         # 前端设计
internal-comms          # 内部通信
mcp-builder             # MCP 构建器
ms-agent-framework-rag  # MS Agent Framework RAG
office-to-md            # Office 转 Markdown
pdf                     # PDF 处理
planning-with-files     # 文件规划
pptx                    # PPTX 处理
skill-creator           # 技能创建器
slack-gif-creator       # Slack GIF 创建器
theme-factory           # 主题工厂
web-artifacts-builder   # Web 构建器
webapp-testing          # Web 应用测试
xlsx                    # Excel 处理
```

**Claude 技能** (18个):
```
algorithmic-art         # 算法艺术生成
brand-guidelines       # 品牌指南处理
canvas-design         # Canvas 设计工具
doc-coauthoring       # 文档协作
docx                  # DOCX 文件处理
frontend-design       # 前端设计
internal-comms        # 内部通信
mcp-builder           # MCP 构建器
office-to-md          # Office 转 Markdown
pdf                   # PDF 处理
planning-with-files   # 文件规划
pptx                  # PPTX 处理
skill-creator         # 技能创建器
slack-gif-creator     # Slack GIF 创建器
theme-factory         # 主题工厂
web-artifacts-builder # Web 构建器
webapp-testing        # Web 应用测试
xlsx                  # Excel 处理
```

### 5.2 管理技能文件

#### 添加新技能
```bash
# 1. 将新技能复制到相应目录
cp -r /path/to/new-skill /data/webcode/WebCode/skills/codex/

# 2. 重启容器
docker restart webcodecli

# 3. 验证技能已加载
docker exec webcodecli ls /root/.codex/skills/ | grep new-skill
```

#### 验证技能挂载
```bash
# 检查 Codex 技能数量
docker exec webcodecli ls /root/.codex/skills/ | wc -l

# 检查 Claude 技能数量
docker exec webcodecli ls /root/.claude/skills/ | wc -l
```

---

## 六、日常维护

### 6.1 查看日志
```bash
# Docker Compose
docker compose logs -f

# Docker Run
docker logs -f webcodecli
```

### 6.2 重启服务
```bash
# Docker Compose
docker compose restart

# Docker Run
docker restart webcodecli
```

### 6.3 更新版本
```bash
# 拉取最新代码
git pull

# 重新构建并启动
docker compose up -d --build
```

### 6.4 停止服务
```bash
# Docker Compose
docker compose down

# Docker Run
docker stop webcodecli
```

---

## 七、高级配置（环境变量）

如果您需要在启动时预置配置，可以使用环境变量：

- 默认不需要：不配置也能启动，首次访问走 Web 设置向导。
- 需要预置/无人值守：建议按 `docs/Docker-CLI-集成部署指南.md` 的环境变量清单准备 `.env`（包含完整变量说明与 Codex 配置生成规则），本文仅给出示例。

### 7.1 通过 .env 文件

```bash
# 创建 .env 文件
cat > .env << EOF
APP_PORT=5000

# Claude Code（可选，也可在页面配置）
ANTHROPIC_BASE_URL=https://api.antsk.cn/
ANTHROPIC_AUTH_TOKEN=your_token
ANTHROPIC_MODEL=glm-4.7
ANTHROPIC_SMALL_FAST_MODEL=glm-4.7

# Codex（可选，也可在页面配置）
NEW_API_KEY=your_api_key
CODEX_MODEL=glm-4.7
CODEX_MODEL_REASONING_EFFORT=medium
CODEX_PROFILE=webcode
CODEX_BASE_URL=https://api.antsk.cn/v1
CODEX_PROVIDER_NAME=azure codex-mini
CODEX_APPROVAL_POLICY=never
CODEX_SANDBOX_MODE=danger-full-access

# 数据库配置（可选）
DB_TYPE=Sqlite
DB_CONNECTION=Data Source=/app/data/webcodecli.db
EOF

# 启动
docker compose up -d
```

### 7.2 通过命令行

```bash
docker run -d \
  --name webcodecli \
  --network=host \
  -e ANTHROPIC_AUTH_TOKEN=your_token \
  -e NEW_API_KEY=your_api_key \
  -v webcodecli-data:/app/data \
  -v webcodecli-workspaces:/app/workspaces \
  webcodecli:latest
```

---

## 八、故障排查

### 8.1 容器无法启动
```bash
# 查看详细日志
docker compose logs

# 检查容器状态
docker compose ps -a
```

### 8.2 端口被占用
```bash
# 检查端口
netstat -tlnp | grep 5000

# 使用其他端口
APP_PORT=8080 docker compose up -d
```

### 8.3 重置系统配置
```bash
# 停止容器
docker compose down

# 删除数据卷（⚠️ 会清除所有数据）
docker volume rm webcodecli-data

# 重新启动
docker compose up -d
```

### 8.4 技能未加载
```bash
# 检查技能目录
ls -la /data/webcode/WebCode/skills/codex/
ls -la /data/webcode/WebCode/skills/claude/

# 检查容器内技能
docker exec webcodecli ls /root/.codex/skills/
docker exec webcodecli ls /root/.claude/skills/

# 确认挂载点
docker inspect webcodecli | grep -A 10 Mounts
```

---

## 九、备份与恢复

### 备份
```bash
# 备份数据卷
docker run --rm \
  -v webcodecli-data:/data \
  -v $(pwd)/backup:/backup \
  alpine tar czf /backup/webcodecli-backup-$(date +%Y%m%d).tar.gz /data

# 备份技能文件（如果使用外部挂载）
tar czf /backup/webcodecli-skills-$(date +%Y%m%d).tar.gz -C /data/webcode/WebCode skills/
```

### 恢复
```bash
# 恢复数据卷
docker run --rm \
  -v webcodecli-data:/data \
  -v $(pwd)/backup:/backup \
  alpine tar xzf /backup/webcodecli-backup-20260114.tar.gz -C /

# 恢复技能文件（如果使用外部挂载）
tar xzf /backup/webcodecli-skills-20260114.tar.gz -C /data/webcode/WebCode

# 重启容器
docker compose restart
```

---

## 架构说明

### Docker 镜像构建过程

1. **构建阶段** (mcr.azure.cn/dotnet/sdk:10.0)
   - 安装 Node.js 20.x
   - 还原 NuGet 包
   - 构建 TailwindCSS
   - 编译 .NET 应用

2. **运行时镜像** (mcr.azure.cn/dotnet/aspnet:10.0)
   - 安装基础依赖: curl, wget, git, python3 等
   - 安装 Node.js 20.x
   - 安装 Rust (Codex 需要)
   - 安装 Claude Code CLI: `@anthropic-ai/claude-code`
   - 安装 Codex CLI: `@openai/codex`
   - 配置 Codex
   - 复制应用文件

### 端口说明
- `5000`: Web 应用端口
- `8010-9000`: 前端预览服务端口（内部使用）

---

## 常见问题

### Q: 首次访问没有跳转到设置向导？
A: 可能是数据卷中已有旧配置。尝试删除数据卷后重新启动：
```bash
docker compose down -v
docker compose up -d
```

### Q: 如何修改已保存的配置？
A: 登录系统后，进入"系统设置"页面修改。

### Q: 支持哪些数据库？
A: 默认使用 SQLite，无需额外配置。也支持 MySQL、PostgreSQL 等，需要修改配置文件。

### Q: 如何查看系统是否正常运行？
A: 访问 `http://localhost:5000/health` 检查健康状态。

### Q: 如何使用 Host 网络模式？
A: 使用 `--network=host` 参数启动容器，适合生产环境。

---

## 快速部署脚本

### 一键部署脚本
保存为 `deploy-docker.sh`:

```bash
#!/bin/bash
set -e

echo "=========================================="
echo "WebCodeCli Docker 部署脚本"
echo "=========================================="

# 停止旧服务
echo "停止旧服务..."
systemctl stop webcode.service 2>/dev/null || true
systemctl disable webcode.service 2>/dev/null || true
docker rm -f webcodecli 2>/dev/null || true

# 创建目录
echo "创建目录..."
mkdir -p /data/webcode/workspace
mkdir -p /data/webcode/WebCode/skills/codex
mkdir -p /data/webcode/WebCode/skills/claude

# 拉取代码
echo "拉取代码..."
cd /data/webcode
if [ -d "WebCode" ]; then
    cd WebCode
    git pull origin main
else
    git clone https://github.com/xuzeyu91/WebCode.git
    cd WebCode
fi

# 复制技能文件（如果存在）
if [ -d "/data/www/.codex/skills" ]; then
    echo "复制 Codex 技能文件..."
    cp -r /data/www/.codex/skills/* /data/webcode/WebCode/skills/codex/
fi

if [ -d "/data/www/.claude/skills" ]; then
    echo "复制 Claude 技能文件..."
    cp -r /data/www/.claude/skills/* /data/webcode/WebCode/skills/claude/
fi

# 构建镜像
echo "构建镜像..."
docker build --network=host -t webcodecli:latest .

# 启动容器
echo "启动容器..."
docker run -d \
  --name webcodecli \
  --restart unless-stopped \
  --network=host \
  -v webcodecli-data:/app/data \
  -v webcodecli-workspaces:/app/workspaces \
  -v webcodecli-logs:/app/logs \
  webcodecli:latest

echo "=========================================="
echo "部署完成！"
echo "=========================================="
docker ps | grep webcodecli

echo ""
echo "访问 http://localhost:5000 开始配置"
```

### 使用脚本
```bash
chmod +x deploy-docker.sh
./deploy-docker.sh
```

---

## 常用命令速查

```bash
# 查看容器状态
docker ps | grep webcodecli

# 查看容器日志
docker logs -f webcodecli

# 进入容器
docker exec -it webcodecli bash

# 查看技能列表
docker exec webcodecli ls /root/.codex/skills/
docker exec webcodecli ls /root/.claude/skills/

# 重启容器
docker restart webcodecli

# 查看容器详细信息
docker inspect webcodecli

# 查看容器资源使用
docker stats webcodecli
```

---

**文档版本**: 3.0
**更新日期**: 2026-01-14
**维护者**: WebCode Team

### 更新日志
- v3.0 (2026-01-14): 合并 main 分支，添加快速开始和 Web 配置向导
- v2.0 (2026-01-14): 添加技能文件挂载说明，更新部署脚本
- v1.0 (2026-01-14): 初始版本
