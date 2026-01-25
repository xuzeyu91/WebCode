# WebCode

<p align="center">
  <a href="README.md">简体中文</a> | <a href="README_EN.md">English</a>
</p>

<p align="center">
  <strong>🚀 随时随地，AI 工作助手 | Your AI Workspace, Anywhere</strong>
</p>

<p align="center">
  <em>远程驱动 AI 助手，支持编程、文档处理、需求分析、报告撰写等全方位工作场景</em>
</p>

---

## 🌐 在线试用

想要快速体验？无需安装，直接访问在线演示版：

| 试用地址 | 账号 | 密码 |
|----------|------|------|
| **[https://webcode.tree456.com/](https://webcode.tree456.com/)** | `treechat` | `treechat@123` |

> ⚠️ **注意**：试用环境为演示用途，请勿存储敏感信息

---

## 💬 交流群

扫码加入微信交流群，获取最新动态、反馈问题、交流使用心得：

<p align="center">
  <img src="images/qrcode.jpg" alt="微信群二维码" width="200" />
</p>

---

## ✨ 核心特色

WebCode 是一个**在线 AI 全能工作平台**，让你可以通过 Web 浏览器远程控制各种 AI CLI 助手，实现真正的**随时随地智能办公**——无论你在地铁上、咖啡馆里，还是躺在沙发上，只要有浏览器就能完成编程、文档处理、需求分析等各种工作！

### 🎯 主要功能

#### 💻 编程开发
- **🤖 多 AI 助手支持** - 集成 Claude Code CLI、Codex CLI、GitHub Copilot CLI 等主流 AI 编程工具
- **⚡ 实时流式输出** - 即时看到 AI 的思考和编码过程，打字机效果展示
- **🎨 代码高亮预览** - Monaco Editor 代码高亮，支持多种编程语言

#### 📄 文档处理
- **📝 文档撰写** - 支持 Markdown、Word、PDF 等格式的文档生成与编辑
- **🔄 格式转换** - 文档格式互转，满足不同场景需求
- **📊 数据可视化** - 表格数据处理与图表生成

#### 🎯 需求分析
- **📋 需求文档生成** - 自动生成 PRD、用户故事、功能规格说明
- **🔍 需求澄清** - AI 辅助需求分析与优化建议
- **📈 优先级评估** - 智能评估需求优先级与工作量

#### 📊 报告撰写
- **📈 项目报告** - 项目进度、总结报告自动生成
- **📉 数据分析报告** - 数据洞察与可视化报告
- **💼 商务文档** - 商业计划书、提案文档等

#### 🛠️ 通用特性
- **📱 全平台支持** - 完整的移动端适配，手机、平板、电脑无缝切换
- **📂 会话工作区** - 每个会话独立工作目录，文件隔离，安全可靠
- **🔐 安全执行** - 沙箱环境，命令白名单，防注入保护

## 🖥️ 支持的 AI CLI 工具

### ✅ 已完整支持（流式JSON解析）

| 工具 | 命令 | 特点 | 状态 |
|------|------|------|------|
| **Claude Code CLI** | `claude` | MCP 服务器、会话恢复、stream-json 输出、代理系统 | 🟢 已启用 |
| **Codex CLI** | `codex` | 沙箱执行、网络搜索、Git 集成、JSONL 输出 | 🟢 已启用 |
| **OpenCode CLI** | `opencode` | GitHub Models 集成、多模型支持、流式输出 | 🟢 已启用 |

### 🔧 待扩展支持

| 工具 | 命令 | 特点 | 状态 |
|------|------|------|------|
| **GitHub Copilot CLI** | `copilot` | GitHub 集成、细粒度权限 | 🟡 已配置，待适配 |
| **Qwen CLI** | `qwen` | YOLO 模式、检查点、扩展系统 | 🟡 已配置，待适配 |
| **Gemini CLI** | `gemini` | Google AI、简洁配置 | 🟡 已配置，待适配 |

> 📚 详细的 CLI 工具使用说明请查看 [cli/README.md](./cli/README.md)
> 
> 💡 **扩展支持**：如需添加新的 CLI 工具适配器，请参考 `WebCode.Domain/Domain/Service/Adapters/` 目录下的现有实现

## 📱 移动端支持

WebCode 针对移动设备进行了全面优化：

- **响应式布局** - 自适应手机、平板、桌面各种屏幕
- **触摸优化** - 44px 触摸目标，手势支持，按压反馈
- **iOS 适配** - 解决 Safari 100vh 问题，适配刘海屏
- **横竖屏切换** - 无缝切换，内容不丢失
- **虚拟键盘适配** - 输入时自动调整视口

### 📱 移动端兼容界面

- **顶部导航与工具入口** - 小屏优先布局，常用功能一键触达
- **对话区气泡样式** - 阅读清晰、滚动顺滑
- **底部输入栏与快捷操作** - 触摸友好，减少误触
- **底部导航栏** - 对话/输出/文件/预览/设置快速切换

![移动端界面](images/mobile.png)

### 测试设备支持

- ✅ iPhone SE / iPhone 12-14 / iPhone Pro Max
- ✅ iPad Mini / iPad Pro
- ✅ Android 手机（各尺寸）
- ✅ Chrome / Safari / Firefox / Edge 移动版

## 🧭 首次安装设置向导

首次安装会进入设置界面（/setup），按步骤完成初始化配置：

![设置向导 - 第一步](images/setup1.png)
![设置向导 - 第二步](images/setup2.png)
![设置向导 - 第三步](images/setup3.png)

## 🖼️ 产品截图

> 以下截图来自项目内置演示素材，实际界面以当前版本为准。

![代码编程助手](images/coding.png)
![PPT/文档辅助](images/ppt.png)
![Skills/工作流](images/skill.png)
![游戏/创意示例](images/games.png)

## 🚀 快速开始

### 方式一：Docker 一键部署（推荐）

**无需任何配置，30 秒启动！** 首次访问时，系统会自动引导您完成所有配置。

```bash
# 克隆项目
git clone https://github.com/xuzeyu91/WebCode.git
cd WebCode

# 一键启动
docker compose up -d

# 访问 http://localhost:5000
# 首次访问会自动进入设置向导
```

> 📖 详细部署文档请参考 [DEPLOY_DOCKER.md](./DEPLOY_DOCKER.md)
>
> 🔧 需要预置环境变量/无人值守部署与内置 CLI 验证：参考 [docs/Docker-CLI-集成部署指南.md](./docs/Docker-CLI-集成部署指南.md)

### 方式二：本地开发运行

#### 环境要求

- .NET 10.0 SDK
- 已安装的 AI CLI 工具（如 Claude Code CLI、Codex CLI）

#### 安装运行

```bash
# 克隆项目
git clone https://github.com/xuzeyu91/WebCode.git
cd WebCode

# 恢复依赖
dotnet restore

# 运行应用
dotnet run --project WebCodeCli
```

应用将在 `http://localhost:5000` 启动，访问 `/code-assistant` 开始编程！

### 配置 Claude/Codex 等 CLI（推荐界面配置）

默认情况下无需编辑 appsettings.json：首次启动会进入设置向导（/setup），在 Web 界面完成初始化；之后可在“系统设置”中随时调整 Claude/Codex/OpenCode 等参数。

仅在以下场景建议使用 appsettings.json / 环境变量进行预置：

- 需要无人值守部署（CI/CD）
- 需要把配置写死在镜像/配置文件中
- 进行本地开发调试且希望用文件快速切换配置

示例（高级用法）：

```json
{
  "CliTools": {
    "Tools": [
      {
        "Id": "claude-code",
        "Name": "Claude Code",
        "Command": "claude",
        "ArgumentTemplate": "-p \"{prompt}\"",
        "Enabled": true
      },
      {
        "Id": "codex",
        "Name": "OpenAI Codex",
        "Command": "codex",
        "ArgumentTemplate": "exec \"{prompt}\"",
        "Enabled": true
      }
    ]
  }
}
```

## 🏗️ 技术架构

```
WebCode/
├── WebCode/              # 主项目 (Blazor Server)
│   ├── Components/          # Blazor 组件
│   ├── Pages/               # 页面
│   │   └── CodeAssistant/   # 编程助手页面
│   ├── wwwroot/             # 静态资源
│   └── Program.cs           # 应用入口
├── WebCode.Domain/       # 领域层 (DDD)
│   ├── Domain/
│   │   ├── Model/           # 领域模型
│   │   └── Service/         # 领域服务
│   │       └── Adapters/    # CLI 适配器
│   └── Repositories/        # 数据仓储
└── cli/                     # CLI 工具文档
```

### 技术栈

| 类别 | 技术 |
|------|------|
| **前端框架** | Blazor Server + Tailwind CSS |
| **代码编辑器** | Monaco Editor |
| **AI 功能** | Microsoft Semantic Kernel |
| **数据访问** | SqlSugar ORM (Sqlite/PostgreSQL) |
| **实时通信** | Server-Sent Events (SSE) |
| **进程管理** | System.Diagnostics.Process |

## 📋 功能特性

### 聊天与交互
- ✅ 左右分栏布局（移动端上下布局）
- ✅ 消息历史记录
- ✅ 流式输出（打字机效果）
- ✅ 快捷键发送 (Ctrl+Enter)
- ✅ 清空会话

### 预览与展示
- ✅ 代码高亮预览 (Monaco Editor)
- ✅ Markdown 渲染
- ✅ HTML 实时预览
- ✅ 原始输出查看
- ✅ 多 Tab 切换

### 工作区管理
- ✅ 会话隔离工作区
- ✅ 文件上传/下载
- ✅ 文件树浏览
- ✅ 自动清理过期工作区

### 安全特性
- ✅ 命令白名单验证
- ✅ 输入转义（防注入）
- ✅ 并发限制
- ✅ 超时控制

## 📚 文档

- [快速启动指南](./docs/QUICKSTART_CodeAssistant.md)
- [编程助手使用说明](./docs/README_CodeAssistant.md)
- [CLI 工具配置说明](./docs/CLI工具配置说明.md)
- [移动端兼容性说明](./docs/移动端兼容性优化说明.md)
- [Codex 配置说明](./docs/Codex配置说明.md)
- [环境变量配置](./docs/环境变量配置功能说明.md)

## 💡 推荐 Skills

提升 AI 编程助手能力的优秀 Skills 资源：

- [**planning-with-files**](https://github.com/OthmanAdi/planning-with-files) - 基于文件的项目规划与任务管理技能
- [**Anthropic Skills**](https://github.com/anthropics/skills) - Anthropic 官方 Skills 集合，提供多种 Claude 增强能力
- [**UI/UX Pro Max Skill**](https://github.com/nextlevelbuilder/ui-ux-pro-max-skill) - 专业的 UI/UX 设计与开发技能

> 💡 **提示**：这些 Skills 可以与 Claude Code CLI 等 AI 助手配合使用，增强代码生成、项目规划、UI 设计等能力。

## 🛠️ 使用场景

### 💻 编程开发
- **移动编程** - 手机上随时处理代码任务，紧急 bug 修复不再受限
- **远程开发** - 通过浏览器远程驱动 AI 助手，无需本地开发环境
- **代码审查** - AI 辅助代码审查、测试用例生成、代码重构
- **学习编程** - 初学者通过 AI 互动学习，获得即时反馈

### 📄 文档工作
- **技术文档** - API 文档、技术规范、系统设计文档撰写
- **项目文档** - 项目计划、进度报告、总结文档生成
- **用户手册** - 产品使用手册、操作指南、FAQ 文档
- **内部文档** - 会议纪要、工作日志、知识库整理

### 🎯 产品管理
- **需求分析** - PRD 撰写、用户故事拆分、需求优先级评估
- **功能设计** - 功能规格说明、交互设计文档、原型说明
- **项目规划** - 里程碑规划、任务分解、资源评估
- **数据分析** - 用户反馈分析、数据报告生成、趋势洞察

### 💼 商务办公
- **商业文档** - 商业计划书、项目提案、合作方案
- **报告撰写** - 工作总结、分析报告、述职报告
- **沟通协作** - 邮件撰写、通知公告、培训材料
- **创意策划** - 营销方案、活动策划、内容创作

## 🛠️ 高级配置

### 工作区配置

```json
"CliTools": {
  "TempWorkspaceRoot": "D:\\Temp\\WebCode\\Workspaces",
  "WorkspaceExpirationHours": 24,
  "NpmGlobalPath": "",
  "MaxConcurrentExecutions": 3,
  "DefaultTimeoutSeconds": 300
}
```

| 配置项 | 说明 | 示例值 |
|--------|------|--------|
| `TempWorkspaceRoot` | 临时工作区根目录，用于存放会话隔离的工作文件 | `D:\\Temp\\WebCode\\Workspaces` |
| `WorkspaceExpirationHours` | 工作区过期时间（小时），过期后自动清理 | `24` |
| `NpmGlobalPath` | NPM 全局安装路径（可选，留空则自动检测） | `C:\\Users\\YourUsername\\AppData\\Roaming\\npm\\` 或留空 `""` |
| `MaxConcurrentExecutions` | 最大并发执行数 | `3` |
| `DefaultTimeoutSeconds` | 默认超时时间（秒） | `300` |

> 💡 **提示**：
> - **Windows 用户**：NPM 全局路径通常为 `C:\Users\{用户名}\AppData\Roaming\npm\`
> - **Linux/Mac 用户**：NPM 全局路径通常为 `/usr/local/bin/` 或 `~/.npm-global/bin/`
> - 工作区目录建议使用绝对路径，确保有足够的磁盘空间


## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

## 📄 许可证

本项目采用 **AGPLv3** 开源许可证。

- 开源使用：遵循 [AGPLv3](https://www.gnu.org/licenses/agpl-3.0.html) 协议
- 商业授权：如需商业授权，请联系 **antskpro@qq.com**

详细信息请查看 [LICENSE](LICENSE) 文件。

---

<p align="center">
  <strong>🌟 让 AI 成为你的编程伙伴，随时随地，代码随行 🌟</strong>
</p>
