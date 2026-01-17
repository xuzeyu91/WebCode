# OpenCode CLI 使用指南

## 简介

OpenCode 是一个功能强大的 AI 编程助手，支持多种 AI 模型（通过 models.dev 集成），提供 TUI（终端用户界面）和非交互式命令行模式，集成 MCP 服务器、Agent 系统等高级功能。

## 特性

- ✅ **多模型支持** - 通过 models.dev 集成，支持 Anthropic、OpenAI、Google 等多家提供商的模型
- ✅ **TUI 终端界面** - 功能完整的交互式终端用户界面
- ✅ **非交互模式** - 适合脚本和自动化的 `run` 命令
- ✅ **会话管理** - 支持会话继续 (`--continue`, `--session`)、导出导入
- ✅ **MCP 服务器** - 支持添加和管理 Model Context Protocol 服务器
- ✅ **Agent 系统** - 创建和管理自定义 Agent
- ✅ **JSON 输出** - 适合程序化处理的 `--format json` 输出
- ✅ **文件附加** - 支持通过 `-f` 附加文件到消息

## 安装

### 方式一：通过 curl 安装（推荐）

```bash
curl -fsSL https://opencode.ai/install.sh | bash
```

### 方式二：通过 npm 安装

```bash
npm install -g @opencode-ai/opencode
```

### 方式三：其他包管理器

```bash
# pnpm
pnpm add -g @opencode-ai/opencode

# bun
bun add -g @opencode-ai/opencode

# brew (macOS)
brew install opencode
```

## 配置

### 认证配置

OpenCode 使用 models.dev 提供的模型，需要配置相应提供商的 API Key：

```bash
# 登录配置 API Key
opencode auth login
```

这会将认证信息存储在 `~/.local/share/opencode/auth.json`

### 查看已认证的提供商

```bash
opencode auth list
# 或简写
opencode auth ls
```

### 退出登录

```bash
opencode auth logout
```

## 基本使用

### 查看模型
https://models.dev/?search=GLM-4.7

### 启动 TUI（交互模式）

```bash
# 启动终端用户界面
opencode

# 在指定项目目录启动
opencode /path/to/project

# 继续上次会话
opencode --continue
# 或简写
opencode -c

# 继续指定会话
opencode --session <session_id>
# 或简写
opencode -s <session_id>

# 使用指定模型
opencode --model anthropic/claude-3-5-sonnet-20241022
# 或简写
opencode -m anthropic/claude-3-5-sonnet-20241022

# 使用指定 Agent
opencode --agent build
```

### 非交互模式（run 命令）

```bash
# 单次执行
opencode run "创建一个简单的 HTTP 服务器"

# 指定模型
opencode run "解释这段代码" --model anthropic/claude-3-5-sonnet-20241022
# 或简写
opencode run "解释这段代码" -m anthropic/claude-3-5-sonnet-20241022

# 继续上次会话
opencode run --continue "继续之前的任务"
# 或简写
opencode run -c "继续之前的任务"

# 继续指定会话
opencode run --session <session_id> "继续任务"
# 或简写
opencode run -s <session_id> "继续任务"

# JSON 格式输出（适合程序化处理）
opencode run "生成测试用例" --format json

# 附加文件
opencode run "分析这个文件" --file src/main.js
# 或简写
opencode run "分析这个文件" -f src/main.js

# 附加多个文件
opencode run "重构这些文件" -f src/a.js -f src/b.js

# 使用自定义命令
opencode run --command test "运行所有测试"
```

## 高级功能

### 模型管理

```bash
# 列出所有可用模型
opencode models

# 列出指定提供商的模型
opencode models anthropic
opencode models openai

# 刷新模型缓存
opencode models --refresh

# 显示详细信息（包括价格）
opencode models --verbose
```

### 会话管理

```bash
# 列出所有会话
opencode session list

# 限制显示最近 N 个会话
opencode session list --max-count 10
# 或简写
opencode session list -n 10

# JSON 格式输出
opencode session list --format json

# 导出会话
opencode export <session_id>

# 导入会话
opencode import session.json
opencode import https://opncd.ai/s/abc123
```

### 统计信息

```bash
# 查看 token 使用和成本统计
opencode stats

# 查看最近 N 天的统计
opencode stats --days 7

# 显示工具使用情况
opencode stats --tools

# 显示模型使用情况（显示前 N 个）
opencode stats --models 5

# 按项目过滤
opencode stats --project /path/to/project
```

### Agent 管理

```bash
# 创建新的 Agent
opencode agent create

# 列出所有 Agent
opencode agent list

# 使用 Agent
opencode --agent <agent_name>
opencode run --agent <agent_name> "任务描述"
```

### MCP 服务器

```bash
# 添加 MCP 服务器
opencode mcp add

# 列出所有 MCP 服务器
opencode mcp list
# 或简写
opencode mcp ls

# OAuth 认证
opencode mcp auth <server_name>

# 列出 OAuth 服务器
opencode mcp auth list

# 退出 OAuth
opencode mcp logout <server_name>

# 调试 OAuth 连接
opencode mcp debug <server_name>
```

### 服务器模式

```bash
# 启动无头服务器（API 访问）
opencode serve
opencode serve --port 4096 --hostname 0.0.0.0

# 启动 Web 界面服务器
opencode web
opencode web --port 4096

# 附加到运行中的服务器
opencode attach http://localhost:4096

# 使用 run 命令附加到服务器（避免 MCP 冷启动）
opencode run --attach http://localhost:4096 "任务描述"
```

## 在 WebCode 中配置

### 基本配置

```json
{
  "CliTools": {
    "Tools": [
      {
        "Id": "opencode",
        "Name": "OpenCode",
        "Command": "opencode",
        "ArgumentTemplate": "run \"{prompt}\" --format json",
        "Enabled": true
      }
    ]
  }
}
```

### 带模型选择的配置

```json
{
  "Id": "opencode-claude",
  "Name": "OpenCode (Claude Sonnet)",
  "Command": "opencode",
  "ArgumentTemplate": "run \"{prompt}\" --format json --model anthropic/claude-3-5-sonnet-20241022",
  "Enabled": true
}
```

### 带 Agent 的配置

```json
{
  "Id": "opencode-build",
  "Name": "OpenCode (Build Agent)",
  "Command": "opencode",
  "ArgumentTemplate": "run \"{prompt}\" --format json --agent build",
  "Enabled": true
}
```

## JSON 输出格式

OpenCode 的 `--format json` 输出包含以下事件类型：

```json
{
  "type": "session_start",
  "timestamp": 1234567890,
  "sessionID": "sess_xxx"
}

{
  "type": "tool_use",
  "timestamp": 1234567890,
  "sessionID": "sess_xxx",
  "part": {
    "type": "tool",
    "tool": "bash",
    "state": {
      "title": "运行命令",
      "input": { "command": "ls -la" }
    }
  }
}

{
  "type": "tool_result",
  "timestamp": 1234567890,
  "sessionID": "sess_xxx",
  "part": {
    "state": {
      "status": "completed",
      "output": "文件列表..."
    }
  }
}

{
  "type": "message",
  "timestamp": 1234567890,
  "sessionID": "sess_xxx",
  "content": "AI 的回复内容"
}

{
  "type": "complete",
  "timestamp": 1234567890,
  "sessionID": "sess_xxx"
}
```

## 环境变量

OpenCode 支持以下环境变量：

| 变量 | 说明 |
|------|------|
| `OPENCODE_AUTO_SHARE` | 自动分享会话 |
| `OPENCODE_CONFIG` | 配置文件路径 |
| `OPENCODE_CONFIG_DIR` | 配置目录路径 |
| `OPENCODE_DISABLE_AUTOUPDATE` | 禁用自动更新检查 |
| `OPENCODE_SERVER_PASSWORD` | 服务器模式的密码（启用基本认证） |
| `OPENCODE_SERVER_USERNAME` | 服务器模式的用户名（默认 `opencode`） |
| `OPENCODE_CLIENT` | 客户端标识符（默认 `cli`） |

## 最佳实践

### 1. 使用服务器模式避免冷启动

```bash
# 在一个终端启动服务器
opencode serve --port 4096

# 在另一个终端使用 run 命令附加
opencode run --attach http://localhost:4096 "任务"
```

### 2. 选择合适的模型

```bash
# 查看可用模型和价格
opencode models --verbose

# 复杂任务使用高级模型
opencode run -m anthropic/claude-3-5-sonnet-20241022 "复杂任务"

# 简单任务使用经济模型
opencode run -m anthropic/claude-3-haiku-20250107 "简单任务"
```

### 3. 利用会话上下文

```bash
# 第一轮
opencode run "创建一个 React 组件" -m anthropic/claude-3-5-sonnet-20241022

# 第二轮（基于上下文）
opencode run --continue "添加 TypeScript 类型"

# 第三轮（继续优化）
opencode run -c "添加单元测试"
```

## 常见问题

### Q: 如何查看所有命令？

```bash
opencode --help
```

### Q: 如何查看特定命令的帮助？

```bash
opencode run --help
opencode session --help
```

### Q: 会话存储在哪里？

会话数据存储在 `~/.local/share/opencode/` 目录。

### Q: 如何更新 OpenCode？

```bash
opencode upgrade

# 升级到特定版本
opencode upgrade v0.1.48

# 指定安装方法
opencode upgrade --method npm
```

### Q: 如何卸载 OpenCode？

```bash
opencode uninstall

# 保留配置文件
opencode uninstall --keep-config

# 保留会话数据
opencode uninstall --keep-data

# 查看将删除什么（不实际删除）
opencode uninstall --dry-run
```

## 故障排查

### 认证问题

```bash
# 重新登录
opencode auth logout
opencode auth login

# 检查已认证的提供商
opencode auth list
```

### 模型不可用

```bash
# 刷新模型列表
opencode models --refresh

# 查看详细模型信息
opencode models --verbose
```

### 查看日志

```bash
# 打印日志到 stderr
opencode --print-logs

# 设置日志级别
opencode --log-level DEBUG
```

## 相关资源

- [OpenCode 官方网站](https://opencode.ai/)
- [OpenCode 文档](https://opencode.ai/docs/)
- [OpenCode GitHub 仓库](https://github.com/anomalyco/opencode)
- [Models.dev](https://models.dev/) - 模型集成平台
- [WebCode 项目主页](https://github.com/xuzeyu91/WebCode)

## 支持与反馈

如有问题或建议，欢迎：

- 加入 Discord 社区: https://opencode.ai/discord
- 提交 Issue: https://github.com/anomalyco/opencode/issues
- 访问文档: https://opencode.ai/docs/

---

**最后更新**: 2026年1月17日  
**OpenCode 版本**: 基于最新文档 (2026年1月16日)
