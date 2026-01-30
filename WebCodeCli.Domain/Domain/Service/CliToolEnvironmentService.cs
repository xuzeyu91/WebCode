using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebCodeCli.Domain.Common.Extensions;
using WebCodeCli.Domain.Common.Options;
using WebCodeCli.Domain.Repositories.Base.CliToolEnv;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// CLI 工具环境变量服务接口
/// </summary>
public interface ICliToolEnvironmentService
{
    /// <summary>
    /// 获取指定工具的环境变量配置（优先从数据库读取,否则从appsettings读取）
    /// </summary>
    Task<Dictionary<string, string>> GetEnvironmentVariablesAsync(string toolId);

    /// <summary>
    /// 保存指定工具的环境变量配置到数据库
    /// </summary>
    Task<bool> SaveEnvironmentVariablesAsync(string toolId, Dictionary<string, string> envVars);

    /// <summary>
    /// 删除指定工具的环境变量配置
    /// </summary>
    Task<bool> DeleteEnvironmentVariablesAsync(string toolId);

    /// <summary>
    /// 重置为appsettings中的默认配置
    /// </summary>
    Task<Dictionary<string, string>> ResetToDefaultAsync(string toolId);
}

/// <summary>
/// CLI 工具环境变量服务实现
/// </summary>
[ServiceDescription(typeof(ICliToolEnvironmentService), ServiceLifetime.Scoped)]
public class CliToolEnvironmentService : ICliToolEnvironmentService
{
    private readonly ILogger<CliToolEnvironmentService> _logger;
    private readonly CliToolsOption _options;
    private readonly ICliToolEnvironmentVariableRepository _repository;

    public CliToolEnvironmentService(
        ILogger<CliToolEnvironmentService> logger,
        IOptions<CliToolsOption> options,
        ICliToolEnvironmentVariableRepository repository)
    {
        _logger = logger;
        _options = options.Value;
        _repository = repository;
    }

    /// <summary>
    /// 获取指定工具的环境变量配置（优先从数据库读取,否则从appsettings读取）
    /// </summary>
    public async Task<Dictionary<string, string>> GetEnvironmentVariablesAsync(string toolId)
    {
        try
        {
            // 尝试从数据库读取
            var dbEnvVars = await _repository.GetEnvironmentVariablesByToolIdAsync(toolId);
            
            // 如果数据库中有配置,则使用数据库配置（过滤空值）
            if (dbEnvVars.Any())
            {
                _logger.LogInformation("从数据库加载工具 {ToolId} 的环境变量配置", toolId);
                // 过滤掉空值的环境变量，避免空字符串覆盖系统默认配置
                return dbEnvVars
                    .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }

            // 否则从appsettings读取
            var tool = _options.Tools.FirstOrDefault(t => t.Id == toolId);
            if (tool?.EnvironmentVariables != null && tool.EnvironmentVariables.Any())
            {
                _logger.LogInformation("从配置文件加载工具 {ToolId} 的环境变量配置", toolId);
                // 同样过滤掉空值
                return tool.EnvironmentVariables
                    .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }

            return new Dictionary<string, string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取工具 {ToolId} 的环境变量失败", toolId);
            return new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// 保存指定工具的环境变量配置到数据库
    /// </summary>
    public async Task<bool> SaveEnvironmentVariablesAsync(string toolId, Dictionary<string, string> envVars)
    {
        try
        {
            var result = await _repository.SaveEnvironmentVariablesAsync(toolId, envVars);
            if (result)
            {
                _logger.LogInformation("成功保存工具 {ToolId} 的环境变量配置", toolId);
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "保存工具 {ToolId} 的环境变量失败", toolId);
            return false;
        }
    }

    /// <summary>
    /// 删除指定工具的环境变量配置
    /// </summary>
    public async Task<bool> DeleteEnvironmentVariablesAsync(string toolId)
    {
        try
        {
            var result = await _repository.DeleteByToolIdAsync(toolId);
            if (result)
            {
                _logger.LogInformation("成功删除工具 {ToolId} 的环境变量配置", toolId);
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "删除工具 {ToolId} 的环境变量失败", toolId);
            return false;
        }
    }

    /// <summary>
    /// 重置为appsettings中的默认配置
    /// </summary>
    public async Task<Dictionary<string, string>> ResetToDefaultAsync(string toolId)
    {
        try
        {
            // 删除数据库配置
            await _repository.DeleteByToolIdAsync(toolId);

            // 返回appsettings中的配置
            var tool = _options.Tools.FirstOrDefault(t => t.Id == toolId);
            if (tool?.EnvironmentVariables != null && tool.EnvironmentVariables.Any())
            {
                _logger.LogInformation("重置工具 {ToolId} 的环境变量为默认配置", toolId);
                return new Dictionary<string, string>(tool.EnvironmentVariables);
            }

            return new Dictionary<string, string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "重置工具 {ToolId} 的环境变量失败", toolId);
            return new Dictionary<string, string>();
        }
    }
}
