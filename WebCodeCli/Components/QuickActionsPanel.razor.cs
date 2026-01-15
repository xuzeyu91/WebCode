using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using WebCodeCli.Domain.Domain.Model;
using WebCodeCli.Domain.Domain.Service;

namespace WebCodeCli.Components;

public partial class QuickActionsPanel : ComponentBase
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private ILocalizationService L { get; set; } = default!;

    [Parameter] public EventCallback<string> OnActionSelected { get; set; }

    private List<QuickAction> _actions = new();
    private bool _isLoading = true;
    private bool _showCustomizeModal = false;
    private bool _showActionDialog = false;
    private QuickAction? _editingAction = null;
    private ActionFormModel _actionForm = new();
    private bool _isCollapsed = true; // 默认折叠状态
    
    // 本地化相关
    private Dictionary<string, string> _translations = new();
    private string _currentLanguage = "zh-CN";

    private string LocalizedTitle => T("quickActions.title");
    private string LocalizedCustomize => T("quickActions.customize");
    private const string StorageKey = "webcli_quick_actions";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected override async Task OnInitializedAsync()
    {
        await LoadTranslationsAsync();
        await LoadActionsAsync();
    }

    private async Task LoadActionsAsync()
    {
        _isLoading = true;
        StateHasChanged();

        try
        {
            // 等待 IndexedDB 准备就绪
            var isReady = await WaitForIndexedDBReady();
            
            if (isReady)
            {
                // 使用 IndexedDB 加载
                var savedActions = await JSRuntime.InvokeAsync<List<QuickAction>?>(
                    "webCliIndexedDB.getAllQuickActions");
                
                if (savedActions is { Count: > 0 })
                {
                    _actions = savedActions;
                    Console.WriteLine($"[快捷操作] 成功从 IndexedDB 加载 {savedActions.Count} 个快捷操作");
                }
                else
                {
                    Console.WriteLine("[快捷操作] IndexedDB 无数据，初始化默认快捷操作");
                    await ResetActionsToDefaultAsync();
                }
            }
            else
            {
                Console.WriteLine("[快捷操作] IndexedDB 未就绪，使用默认快捷操作");
                _actions = GetDefaultActions();
            }
        }
        catch (JSException jsEx)
        {
            Console.WriteLine($"[快捷操作] JS 交互异常，已回退默认快捷操作: {jsEx.Message}");
            _actions = GetDefaultActions();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[快捷操作] 加载失败: {ex.Message}");
            _actions = GetDefaultActions();
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }
    
    /// <summary>
    /// 等待 IndexedDB 准备就绪
    /// </summary>
    private async Task<bool> WaitForIndexedDBReady(int maxWaitMs = 3000)
    {
        var startTime = DateTime.Now;
        while ((DateTime.Now - startTime).TotalMilliseconds < maxWaitMs)
        {
            try
            {
                var isReady = await JSRuntime.InvokeAsync<bool>("webCliIndexedDB.isReady");
                if (isReady)
                {
                    return true;
                }
            }
            catch
            {
                // 忽略错误，继续等待
            }
            
            await Task.Delay(50);
        }
        return false;
    }

    private List<QuickAction> GetDefaultActions()
    {
        return new List<QuickAction>
        {
            new()
            {
                Id = "optimize-code",
                Title = "优化代码",
                Content = "请优化以下代码的性能和可读性，并说明优化的原因：",
                Icon = "🔧",
                Order = 1,
                IsCustom = false,
                IsEnabled = true,
                Hotkey = "Ctrl+1"
            },
            new()
            {
                Id = "add-comments",
                Title = "添加注释",
                Content = "请为以下代码添加详细的中文注释，包括函数说明、参数说明和关键逻辑说明：",
                Icon = "📝",
                Order = 2,
                IsCustom = false,
                IsEnabled = true,
                Hotkey = "Ctrl+2"
            },
            new()
            {
                Id = "fix-bug",
                Title = "修复 Bug",
                Content = "请帮我分析并修复以下代码中的 Bug，并解释问题原因：",
                Icon = "🐛",
                Order = 3,
                IsCustom = false,
                IsEnabled = true,
                Hotkey = "Ctrl+3"
            },
            new()
            {
                Id = "refactor-code",
                Title = "重构代码",
                Content = "请重构以下代码，提高代码质量和可维护性，遵循 SOLID 原则：",
                Icon = "🔄",
                Order = 4,
                IsCustom = false,
                IsEnabled = true,
                Hotkey = "Ctrl+4"
            },
            new()
            {
                Id = "generate-tests",
                Title = "生成测试",
                Content = "请为以下代码生成单元测试用例：",
                Icon = "🧪",
                Order = 5,
                IsCustom = false,
                IsEnabled = true,
                Hotkey = "Ctrl+5"
            },
            new()
            {
                Id = "code-review",
                Title = "代码审查",
                Content = "请进行代码审查，指出潜在问题和改进建议，包括：\n1. 代码质量\n2. 安全性\n3. 性能\n4. 可维护性",
                Icon = "👁️",
                Order = 6,
                IsCustom = false,
                IsEnabled = true,
                Hotkey = "Ctrl+6"
            }
        };
    }

    private async Task SaveActionsAsync()
    {
        try
        {
            // 使用 IndexedDB 保存
            await JSRuntime.InvokeVoidAsync("webCliIndexedDB.saveAllQuickActions", _actions);
            Console.WriteLine($"[快捷操作] 已保存 {_actions.Count} 个快捷操作到 IndexedDB");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[快捷操作] 保存失败: {ex.Message}");
        }
    }

    private async Task OnActionClick(QuickAction action)
    {
        // 增加使用次数
        action.UsageCount++;
        await SaveActionsAsync();

        // 触发事件
        if (OnActionSelected.HasDelegate)
        {
            await OnActionSelected.InvokeAsync(action.Content);
        }
    }

    private void ToggleCollapse()
    {
        _isCollapsed = !_isCollapsed;
    }

    private void ToggleCustomize()
    {
        _showCustomizeModal = !_showCustomizeModal;
    }

    private void CloseCustomizeModal()
    {
        _showCustomizeModal = false;
    }

    private async Task ToggleActionEnabled(QuickAction action)
    {
        action.IsEnabled = !action.IsEnabled;
        await SaveActionsAsync();
        StateHasChanged();
    }

    private void ShowAddDialog()
    {
        _editingAction = null;
        _actionForm = new ActionFormModel
        {
            Icon = "⭐"
        };
        _showActionDialog = true;
    }

    private void ShowEditDialog(QuickAction action)
    {
        _editingAction = action;
        _actionForm = new ActionFormModel
        {
            Title = action.Title,
            Content = action.Content,
            Icon = action.Icon,
            Hotkey = action.Hotkey
        };
        _showActionDialog = true;
    }

    private void CloseActionDialog()
    {
        _showActionDialog = false;
        _editingAction = null;
        _actionForm = new ActionFormModel();
    }

    private async Task SaveAction()
    {
        if (string.IsNullOrWhiteSpace(_actionForm.Title) || string.IsNullOrWhiteSpace(_actionForm.Content))
        {
            return;
        }

        if (_editingAction != null)
        {
            // 编辑现有操作
            _editingAction.Title = _actionForm.Title;
            _editingAction.Content = _actionForm.Content;
            _editingAction.Icon = _actionForm.Icon;
            _editingAction.Hotkey = _actionForm.Hotkey;
            _editingAction.UpdatedAt = DateTime.Now;
        }
        else
        {
            // 添加新操作
            var newAction = new QuickAction
            {
                Id = Guid.NewGuid().ToString(),
                Title = _actionForm.Title,
                Content = _actionForm.Content,
                Icon = _actionForm.Icon,
                Hotkey = _actionForm.Hotkey,
                Order = _actions.Count + 1,
                IsCustom = true,
                IsEnabled = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _actions.Add(newAction);
        }

        await SaveActionsAsync();
        CloseActionDialog();
        StateHasChanged();
    }

    private async Task DeleteAction(QuickAction action)
    {
        if (!action.IsCustom)
        {
            return;
        }

        _actions.Remove(action);
        await SaveActionsAsync();
        StateHasChanged();
    }

    private async Task ResetActionsToDefaultAsync()
    {
        Console.WriteLine("[快捷操作] 重置为默认快捷操作");
        _actions = GetDefaultActions();
        try
        {
            // 清除 IndexedDB 中的快捷操作
            await JSRuntime.InvokeVoidAsync("webCliIndexedDB.clearAllQuickActions");
            Console.WriteLine("[快捷操作] 已清除 IndexedDB 中的快捷操作");
            await SaveActionsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[快捷操作] 重置存储失败: {ex.Message}");
        }
    }

    private static string NormalizeSavedJson(string savedJson)
    {
        var trimmed = savedJson.Trim();

        // 检查是否为空或无效字符
        if (string.IsNullOrEmpty(trimmed))
        {
            throw new JsonException("数据为空字符串");
        }

        // 检查是否以有效的 JSON 字符开头
        if (!trimmed.StartsWith("[") && !trimmed.StartsWith("{") && !trimmed.StartsWith("\""))
        {
            throw new JsonException($"无效的 JSON 起始字符: '{trimmed[0]}'");
        }

        // 处理可能出现的双重序列化场景（例如字符串里再次包裹了 JSON）
        if (trimmed.StartsWith("\"[") && trimmed.EndsWith("]\""))
        {
            try
            {
                var unescaped = JsonSerializer.Deserialize<string>(trimmed, JsonOptions);
                if (!string.IsNullOrEmpty(unescaped))
                {
                    return unescaped;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[快捷操作] 解包双重序列化失败: {ex.Message}");
                // 如果解包失败，继续使用原始字符串进入后续容错流程
            }
        }

        return trimmed;
    }

    private class ActionFormModel
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Hotkey { get; set; } = string.Empty;
    }

    #region 本地化辅助方法

    /// <summary>
    /// 加载翻译资源
    /// </summary>
    private async Task LoadTranslationsAsync()
    {
        try
        {
            _currentLanguage = await L.GetCurrentLanguageAsync();
            var allTranslations = await L.GetAllTranslationsAsync(_currentLanguage);
            _translations = FlattenTranslations(allTranslations);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[快捷操作] 加载翻译资源失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 将嵌套的翻译字典展平为点分隔的键
    /// </summary>
    private Dictionary<string, string> FlattenTranslations(Dictionary<string, object> source, string prefix = "")
    {
        var result = new Dictionary<string, string>();
        
        foreach (var kvp in source)
        {
            var key = string.IsNullOrEmpty(prefix) ? kvp.Key : $"{prefix}.{kvp.Key}";
            
            if (kvp.Value is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.Object)
                {
                    var nested = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonElement.GetRawText());
                    if (nested != null)
                    {
                        foreach (var item in FlattenTranslations(nested, key))
                        {
                            result[item.Key] = item.Value;
                        }
                    }
                }
                else if (jsonElement.ValueKind == JsonValueKind.String)
                {
                    result[key] = jsonElement.GetString() ?? key;
                }
            }
            else if (kvp.Value is Dictionary<string, object> dict)
            {
                foreach (var item in FlattenTranslations(dict, key))
                {
                    result[item.Key] = item.Value;
                }
            }
            else if (kvp.Value is string str)
            {
                result[key] = str;
            }
        }
        
        return result;
    }

    /// <summary>
    /// 获取翻译文本
    /// </summary>
    private string T(string key)
    {
        if (_translations.TryGetValue(key, out var translation))
        {
            return translation;
        }
        
        // 返回键的最后一部分作为默认值
        var parts = key.Split('.');
        return parts.Length > 0 ? parts[^1] : key;
    }

    #endregion
}

