# WebCode

<p align="center">
  <a href="README.md">简体中文</a> | <a href="README_EN.md">English</a>
</p>

<p align="center">
  <strong>🚀 Your AI Workspace, Anywhere | 随时随地，AI 工作助手</strong>
</p>

<p align="center">
  <em>Remote AI Assistant supporting coding, document processing, requirement analysis, report writing, and more across all platforms</em>
</p>

---

## 🌐 Online Demo

Want to try it quickly? No installation required, just visit the online demo:

| Demo URL | Username | Password |
|----------|----------|----------|
| **[https://webcode.tree456.com/](https://webcode.tree456.com/)** | `treechat` | `treechat@123` |

> ⚠️ **Note**: This demo environment is for demonstration purposes only. Please do not store sensitive information.

---

## 💬 Community

Join our WeChat group to get the latest updates, report issues, and share your experiences:

<p align="center">
  <img src="images/qrcode.jpg" alt="WeChat Group QR Code" width="200" />
</p>

---

## ✨ Core Features

WebCodeCli is an **online AI-powered workspace platform** that allows you to remotely control various AI CLI assistants through a web browser, enabling **true work anywhere, anytime** - whether you're on the subway, in a coffee shop, or lounging on the sofa, you can code, write documents, analyze requirements, and more with just a browser!

### 🎯 Main Features

#### 💻 Programming & Development
- **🤖 Multiple AI Assistant Support** - Integration with mainstream AI programming tools like Claude Code CLI, Codex CLI, GitHub Copilot CLI, etc.
- **⚡ Real-time Streaming Output** - Instantly see AI's thinking and coding process with typewriter effect
- **🎨 Code Highlighting Preview** - Monaco Editor with syntax highlighting for multiple languages

#### 📄 Document Processing
- **📝 Document Creation** - Generate and edit documents in Markdown, Word, PDF, and more
- **🔄 Format Conversion** - Convert between different document formats seamlessly
- **📊 Data Visualization** - Process tabular data and generate charts

#### 🎯 Requirement Analysis
- **📋 Requirements Documentation** - Auto-generate PRDs, user stories, and feature specifications
- **🔍 Requirement Clarification** - AI-assisted requirement analysis and optimization suggestions
- **📈 Priority Assessment** - Smart evaluation of requirement priorities and workload

#### 📊 Report Writing
- **📈 Project Reports** - Auto-generate project progress and summary reports
- **📉 Data Analysis Reports** - Data insights and visualization reports
- **💼 Business Documents** - Business plans, proposals, and more

#### 🛠️ Universal Features
- **📱 Cross-Platform Support** - Full mobile optimization, seamless experience across phones, tablets, and computers
- **📂 Session Workspace** - Isolated working directories for each session, secure and reliable
- **🔐 Secure Execution** - Sandbox environment, command whitelist, injection protection

## 🖥️ Supported AI CLI Tools

### ✅ Fully Supported (Streaming JSON Parsing)

| Tool | Command | Features | Status |
|------|---------|----------|--------|
| **Claude Code CLI** | `claude` | MCP server, session recovery, stream-json output, proxy system | 🟢 Enabled |
| **Codex CLI** | `codex` | Sandbox execution, web search, Git integration, JSONL output | 🟢 Enabled |
| **OpenCode CLI** | `opencode` | GitHub Models integration, multi-model support, streaming output | 🟢 Enabled |

### 🔧 To Be Extended

| Tool | Command | Features | Status |
|------|---------|----------|--------|
| **GitHub Copilot CLI** | `copilot` | GitHub integration, fine-grained permissions | 🟡 Configured, pending adaptation |
| **Qwen CLI** | `qwen` | YOLO mode, checkpoints, extension system | 🟡 Configured, pending adaptation |
| **Gemini CLI** | `gemini` | Google AI, simple configuration | 🟡 Configured, pending adaptation |

> 📚 For detailed CLI tool usage instructions, please refer to [cli/README.md](./cli/README.md)
> 
> 💡 **Extension Support**: To add new CLI tool adapters, please refer to the existing implementations in the `WebCodeCli.Domain/Domain/Service/Adapters/` directory

## 📱 Mobile Support

WebCodeCli is fully optimized for mobile devices:

- **Responsive Layout** - Adapts to phones, tablets, and desktop screens
- **Touch Optimization** - 44px touch targets, gesture support, press feedback
- **iOS Adaptation** - Solves Safari 100vh issue, adapts to notch screens
- **Portrait/Landscape Switching** - Seamless switching without content loss
- **Virtual Keyboard Adaptation** - Auto-adjusts viewport during input

### 📱 Mobile-Compatible UI

- **Top navigation & quick actions** - Small-screen-first layout with fast access to core tools
- **Chat bubble layout** - Clear reading and smooth scrolling
- **Bottom input & shortcuts** - Touch-friendly controls that reduce mis-taps
- **Bottom tab bar** - Quick access to Chat / Output / Files / Preview / Settings

![Mobile UI](images/mobile.png)

### Tested Device Support

- ✅ iPhone SE / iPhone 12-14 / iPhone Pro Max
- ✅ iPad Mini / iPad Pro
- ✅ Android phones (various sizes)
- ✅ Chrome / Safari / Firefox / Edge mobile versions

## 🧭 First-Run Setup Wizard

On first install, you will be guided through the setup page (/setup) to complete initialization:

![Setup wizard - Step 1](images/setup1.png)
![Setup wizard - Step 2](images/setup2.png)
![Setup wizard - Step 3](images/setup3.png)

## 🖼️ Screenshots

> These images are demo assets included in the repo; the actual UI may vary by version.

![Coding assistant](images/coding.png)
![PPT / document helper](images/ppt.png)
![Skills / workflows](images/skill.png)
![Games / creative examples](images/games.png)

## 🚀 Quick Start

### Option 1: Docker One-Click Deployment (Recommended)

**No configuration required, start in 30 seconds!** The system will automatically guide you through all configuration on first visit.

```bash
# Clone the project
git clone https://github.com/shuyu-labs/WebCode.git
cd WebCode

# One-click start
docker compose up -d

# Visit http://localhost:5000
# First visit will automatically enter the setup wizard
```

> 📖 For detailed deployment documentation, see [DEPLOY_DOCKER.md](./DEPLOY_DOCKER.md)
>
> 🔧 For pre-provisioning (env vars / unattended deploy) and built-in CLI verification, see [docs/Docker-CLI-集成部署指南.md](./docs/Docker-CLI-集成部署指南.md)

### Option 2: Local Development

#### Requirements

- .NET 10.0 SDK
- Installed AI CLI tools (such as Claude Code CLI, Codex CLI)

#### Installation and Running

```bash
# Clone the project
git clone https://github.com/shuyu-labs/WebCode.git
cd WebCode

# Restore dependencies
dotnet restore

# Run the application
dotnet run --project WebCodeCli
```

The application will start at `http://localhost:5000`, visit `/code-assistant` to start coding!

### Configure CLI Tools

By default, you do not need to edit appsettings.json. On first visit, you will enter the setup wizard (/setup) to initialize settings in the Web UI; later you can adjust Claude/Codex/OpenCode in System Settings.

Use appsettings.json / environment variables only if you want pre-provisioning (CI/CD, unattended deployment, or fast local switching).

Example (advanced):

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

## 🏗️ Technical Architecture

```
WebCodeCli/
├── WebCodeCli/              # Main project (Blazor Server)
│   ├── Components/          # Blazor components
│   ├── Pages/               # Pages
│   │   └── CodeAssistant/   # Programming assistant page
│   ├── wwwroot/             # Static resources
│   └── Program.cs           # Application entry
├── WebCodeCli.Domain/       # Domain layer (DDD)
│   ├── Domain/
│   │   ├── Model/           # Domain models
│   │   └── Service/         # Domain services
│   │       └── Adapters/    # CLI adapters
│   └── Repositories/        # Data repositories
└── cli/                     # CLI tools documentation
```

### Tech Stack

| Category | Technology |
|----------|------------|
| **Frontend Framework** | Blazor Server + Tailwind CSS |
| **Code Editor** | Monaco Editor |
| **AI Features** | Microsoft Semantic Kernel |
| **Data Access** | SqlSugar ORM (Sqlite/PostgreSQL) |
| **Real-time Communication** | Server-Sent Events (SSE) |
| **Process Management** | System.Diagnostics.Process |

## 📋 Features

### Chat & Interaction
- ✅ Left-right split layout (top-bottom on mobile)
- ✅ Message history
- ✅ Streaming output (typewriter effect)
- ✅ Shortcut send (Ctrl+Enter)
- ✅ Clear session

### Preview & Display
- ✅ Code highlighting preview (Monaco Editor)
- ✅ Markdown rendering
- ✅ HTML live preview
- ✅ Raw output view
- ✅ Multi-tab switching

### Workspace Management
- ✅ Session-isolated workspace
- ✅ File upload/download
- ✅ File tree browsing
- ✅ Auto-cleanup of expired workspaces

### Security Features
- ✅ Command whitelist validation
- ✅ Input escaping (injection prevention)
- ✅ Concurrency limits
- ✅ Timeout control

## 📚 Documentation

- [Quick Start Guide](./docs/QUICKSTART_CodeAssistant.md)
- [Code Assistant Usage Guide](./docs/README_CodeAssistant.md)
- [CLI Tool Configuration Guide](./docs/CLI工具配置说明.md)
- [Mobile Compatibility Guide](./docs/移动端兼容性优化说明.md)
- [Codex Configuration Guide](./docs/Codex配置说明.md)
- [Environment Variables Configuration](./docs/环境变量配置功能说明.md)

## 💡 Recommended Skills

Excellent Skills resources to enhance AI programming assistant capabilities:

- [**planning-with-files**](https://github.com/OthmanAdi/planning-with-files) - File-based project planning and task management skill
- [**Anthropic Skills**](https://github.com/anthropics/skills) - Official Anthropic Skills collection providing various Claude enhancement capabilities
- [**UI/UX Pro Max Skill**](https://github.com/nextlevelbuilder/ui-ux-pro-max-skill) - Professional UI/UX design and development skill

> 💡 **Tip**: These Skills can be used with AI assistants like Claude Code CLI to enhance code generation, project planning, UI design capabilities, and more.

## 🔧 Use Cases

### 💻 Programming & Development
- **Mobile Coding** - Handle code tasks on your phone anytime, emergency bug fixes without limits
- **Remote Development** - Drive AI assistants via browser, no local environment needed
- **Code Review** - AI-assisted code review, test case generation, and refactoring
- **Learning Programming** - Interactive learning with AI, instant feedback for beginners

### 📄 Documentation Work
- **Technical Documentation** - API docs, technical specs, system design documents
- **Project Documentation** - Project plans, progress reports, summary documents
- **User Manuals** - Product guides, operation manuals, FAQ documentation
- **Internal Documentation** - Meeting minutes, work logs, knowledge base organization

### 🎯 Product Management
- **Requirement Analysis** - PRD writing, user story breakdown, priority assessment
- **Feature Design** - Feature specifications, interaction design docs, prototype descriptions
- **Project Planning** - Milestone planning, task decomposition, resource estimation
- **Data Analysis** - User feedback analysis, data reports, trend insights

### 💼 Business Office
- **Business Documents** - Business plans, project proposals, partnership agreements
- **Report Writing** - Work summaries, analysis reports, performance reviews
- **Communication** - Email writing, announcements, training materials
- **Creative Planning** - Marketing proposals, event planning, content creation

## 🛠️ Advanced Configuration

### Workspace Configuration

```json
"CliTools": {
  "TempWorkspaceRoot": "D:\\Temp\\WebCodeCli\\Workspaces",
  "WorkspaceExpirationHours": 24,
  "NpmGlobalPath": "",
  "MaxConcurrentExecutions": 3,
  "DefaultTimeoutSeconds": 300
}
```

| Configuration | Description | Example Value |
|---------------|-------------|---------------|
| `TempWorkspaceRoot` | Temporary workspace root directory for storing session-isolated working files | `D:\\Temp\\WebCodeCli\\Workspaces` |
| `WorkspaceExpirationHours` | Workspace expiration time (hours), automatically cleaned after expiration | `24` |
| `NpmGlobalPath` | NPM global installation path (optional, leave empty for auto-detection) | `C:\\Users\\YourUsername\\AppData\\Roaming\\npm\\` or leave empty `""` |
| `MaxConcurrentExecutions` | Maximum concurrent executions | `3` |
| `DefaultTimeoutSeconds` | Default timeout (seconds) | `300` |

> 💡 **Tips**:
> - **Windows Users**: NPM global path is typically `C:\Users\{username}\AppData\Roaming\npm\`
> - **Linux/Mac Users**: NPM global path is typically `/usr/local/bin/` or `~/.npm-global/bin/`
> - Workspace directory should use absolute path with sufficient disk space


## 🤝 Contributing

Issues and Pull Requests are welcome!

## 📄 License

This project uses the **AGPLv3** open source license.

- Open Source Usage: Follow the [AGPLv3](https://www.gnu.org/licenses/agpl-3.0.html) agreement
- Commercial Licensing: For commercial licensing, please contact **antskpro@qq.com**

For details, please refer to the [LICENSE](LICENSE) file.

---

<p align="center">
  <strong>🌟 Let AI be your coding companion, anytime, anywhere 🌟</strong>
</p>
