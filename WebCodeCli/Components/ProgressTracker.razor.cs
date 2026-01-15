using Microsoft.AspNetCore.Components;
using System.Text.Json;
using WebCodeCli.Domain.Domain.Service;

namespace WebCodeCli.Components;

public partial class ProgressTracker : ComponentBase, IDisposable
{
    [Inject] private ILocalizationService L { get; set; } = default!;
    [Parameter] public bool CanCancel { get; set; } = true;
    [Parameter] public EventCallback OnCancel { get; set; }

    private List<ProgressStage> _stages = new();
    private bool _isVisible = false;
    private double _progress = 0;
    private string _currentStageText = "";
    private string _elapsedTime = "0.0s";
    private string _detailMessage = "";
    private bool _canCancel = true;
    private bool _isCompleted = false;
    private bool _isFailed = false;
    private string _currentStageId = string.Empty;
    private Dictionary<string, string> _translations = new();
    private string _currentLanguage = "zh-CN";
    
    private DateTime _startTime;
    private System.Threading.Timer? _timer;

    protected override async Task OnInitializedAsync()
    {
        _currentLanguage = await L.GetCurrentLanguageAsync();
        await LoadTranslationsAsync();
        InitializeStages();
    }

    private void InitializeStages()
    {
        _stages = new List<ProgressStage>
        {
            new ProgressStage
            {
                Id = "thread.started",
                Name = T("progressTracker.stageInit"),
                Icon = "🚀",
                Status = StageStatus.Pending,
                ProgressWeight = 10
            },
            new ProgressStage
            {
                Id = "turn.started",
                Name = T("progressTracker.stageAnalyze"),
                Icon = "💭",
                Status = StageStatus.Pending,
                ProgressWeight = 20
            },
            new ProgressStage
            {
                Id = "item.started",
                Name = T("progressTracker.stageExecute"),
                Icon = "⚙️",
                Status = StageStatus.Pending,
                ProgressWeight = 60
            },
            new ProgressStage
            {
                Id = "turn.completed",
                Name = T("progressTracker.stageComplete"),
                Icon = "✅",
                Status = StageStatus.Pending,
                ProgressWeight = 10
            }
        };
    }

    public void Start()
    {
        _ = RefreshTranslationsAsync();
        _isVisible = true;
        _isCompleted = false;
        _isFailed = false;
        _startTime = DateTime.Now;
        _progress = 0;
        _canCancel = CanCancel;
        _currentStageId = "thread.started";

        // 重置所有阶段状态
        foreach (var stage in _stages)
        {
            stage.Status = StageStatus.Pending;
        }

        // 启动第一个阶段
        UpdateStage("thread.started", StageStatus.Active);

        // 启动计时器
        _timer = new System.Threading.Timer(_ =>
        {
            InvokeAsync(() =>
            {
                UpdateElapsedTime();
                StateHasChanged();
            });
        }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

        StateHasChanged();
    }

    public void UpdateStage(string stageId, StageStatus status, string? detailMessage = null)
    {
        var stage = _stages.FirstOrDefault(s => s.Id == stageId);
        if (stage == null) return;

        // 完成之前的阶段
        var previousStages = _stages.TakeWhile(s => s.Id != stageId);
        foreach (var prev in previousStages)
        {
            if (prev.Status != StageStatus.Failed)
            {
                prev.Status = StageStatus.Completed;
            }
        }

        // 更新当前阶段
        stage.Status = status;
        _currentStageId = stageId;
        _currentStageText = stage.Name;

        if (!string.IsNullOrEmpty(detailMessage))
        {
            _detailMessage = detailMessage;
        }

        // 计算进度
        CalculateProgress();

        // 检查是否完成
        if (stageId == "turn.completed" && status == StageStatus.Completed)
        {
            Complete();
        }

        StateHasChanged();
    }

    public void UpdateProgress(int itemCount, int completedItems)
    {
        var executionStage = _stages.FirstOrDefault(s => s.Id == "item.started");
        if (executionStage != null && executionStage.Status == StageStatus.Active)
        {
            // 根据完成的项目数量动态计算进度
            var baseProgress = _stages.TakeWhile(s => s.Id != "item.started")
                .Sum(s => s.Status == StageStatus.Completed ? s.ProgressWeight : 0);

            var executionProgress = itemCount > 0 
                ? (double)completedItems / itemCount * executionStage.ProgressWeight
                : 0;

            _progress = baseProgress + executionProgress;
            StateHasChanged();
        }
    }

    public void Fail(string errorMessage)
    {
        _isFailed = true;
        _canCancel = false;

        var activeStage = _stages.FirstOrDefault(s => s.Status == StageStatus.Active);
        if (activeStage != null)
        {
            activeStage.Status = StageStatus.Failed;
        }

        _currentStageText = T("common.failed");
        _detailMessage = errorMessage;
        
        StopTimer();
        StateHasChanged();
    }

    public void Complete()
    {
        _isCompleted = true;
        _canCancel = false;
        _progress = 100;

        foreach (var stage in _stages)
        {
            stage.Status = StageStatus.Completed;
        }

        _currentStageText = T("common.completed");
        
        StopTimer();
        StateHasChanged();

        // 3秒后自动隐藏
        Task.Delay(3000).ContinueWith(_ =>
        {
            InvokeAsync(() =>
            {
                _isVisible = false;
                StateHasChanged();
            });
        });
    }

    public void Hide()
    {
        _isVisible = false;
        StopTimer();
        StateHasChanged();
    }

    private async Task RefreshTranslationsAsync()
    {
        try
        {
            var language = await L.GetCurrentLanguageAsync();
            if (!string.Equals(language, _currentLanguage, StringComparison.OrdinalIgnoreCase))
            {
                _currentLanguage = language;
                await LoadTranslationsAsync();
                ApplyStageTranslations();

                if (!string.IsNullOrWhiteSpace(_currentStageId))
                {
                    var stage = _stages.FirstOrDefault(s => s.Id == _currentStageId);
                    if (stage != null)
                    {
                        _currentStageText = stage.Name;
                    }
                }

                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"刷新进度条多语言失败: {ex.Message}");
        }
    }

    private void ApplyStageTranslations()
    {
        foreach (var stage in _stages)
        {
            stage.Name = stage.Id switch
            {
                "thread.started" => T("progressTracker.stageInit"),
                "turn.started" => T("progressTracker.stageAnalyze"),
                "item.started" => T("progressTracker.stageExecute"),
                "turn.completed" => T("progressTracker.stageComplete"),
                _ => stage.Name
            };
        }
    }

    private async Task LoadTranslationsAsync()
    {
        try
        {
            var allTranslations = await L.GetAllTranslationsAsync(_currentLanguage);
            _translations = FlattenTranslations(allTranslations);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"加载进度条翻译资源失败: {ex.Message}");
        }
    }

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

    private string T(string key)
    {
        if (_translations.TryGetValue(key, out var translation))
        {
            return translation;
        }

        var parts = key.Split('.');
        return parts.Length > 0 ? parts[^1] : key;
    }

    private void CalculateProgress()
    {
        _progress = _stages
            .Where(s => s.Status == StageStatus.Completed)
            .Sum(s => s.ProgressWeight);

        // 活动阶段算50%
        var activeStage = _stages.FirstOrDefault(s => s.Status == StageStatus.Active);
        if (activeStage != null)
        {
            _progress += activeStage.ProgressWeight * 0.5;
        }

        _progress = Math.Min(_progress, 100);
    }

    private void UpdateElapsedTime()
    {
        var elapsed = DateTime.Now - _startTime;
        _elapsedTime = $"{elapsed.TotalSeconds:F1}s";
    }

    private void StopTimer()
    {
        _timer?.Dispose();
        _timer = null;
    }

    private async Task OnCancelClick()
    {
        if (OnCancel.HasDelegate)
        {
            await OnCancel.InvokeAsync();
        }
        Hide();
    }

    private string GetStageClass(ProgressStage stage)
    {
        return stage.Status switch
        {
            StageStatus.Completed => "bg-green-600",
            StageStatus.Active => "bg-gray-700 animate-pulse",
            StageStatus.Failed => "bg-red-600",
            _ => "bg-gray-100"
        };
    }

    private string GetStageNameClass(ProgressStage stage)
    {
        return stage.Status switch
        {
            StageStatus.Completed => "text-green-700",
            StageStatus.Active => "text-gray-700",
            StageStatus.Failed => "text-red-700",
            _ => "text-gray-400"
        };
    }

    private string GetConnectorClass(ProgressStage stage)
    {
        return stage.Status == StageStatus.Completed 
            ? "bg-green-600" 
            : "bg-gray-200";
    }

    private string GetProgressBarClass()
    {
        if (_isFailed)
        {
            return "bg-red-600";
        }
        
        if (_isCompleted)
        {
            return "bg-green-600";
        }

        return "bg-gray-700";
    }

    private string GetCurrentStageTextClass()
    {
        if (_isFailed)
        {
            return "text-red-700";
        }
        
        if (_isCompleted)
        {
            return "text-green-700";
        }

        return "text-gray-700";
    }

    public void Dispose()
    {
        StopTimer();
    }

    private class ProgressStage
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public StageStatus Status { get; set; }
        public double ProgressWeight { get; set; }
    }

    public enum StageStatus
    {
        Pending,
        Active,
        Completed,
        Failed
    }
}

