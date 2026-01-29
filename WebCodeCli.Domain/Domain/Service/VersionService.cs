using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using WebCodeCli.Domain.Common.Extensions;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// GitHub Release API 响应模型
/// </summary>
public class GitHubReleaseResponse
{
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; } = string.Empty;
    
    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; } = string.Empty;
    
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;
    
    [JsonPropertyName("published_at")]
    public DateTime? PublishedAt { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// 版本服务实现
/// </summary>
[ServiceDescription(typeof(IVersionService), ServiceLifetime.Scoped)]
public class VersionService : IVersionService
{
    private const string GitHubApiUrl = "https://api.github.com/repos/shuyu-labs/WebCode/releases/latest";
    private const string GitHubReleasesUrl = "https://github.com/shuyu-labs/WebCode/releases";
    
    // 缓存机制
    private static VersionCheckResult? _cachedResult;
    private static DateTime _cacheTime = DateTime.MinValue;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    
    private readonly HttpClient _httpClient;
    
    public VersionService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "WebCode-Version-Checker");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }
    
    /// <summary>
    /// 获取当前版本号
    /// </summary>
    public string GetCurrentVersion()
    {
        try
        {
            // 从程序集获取版本号
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            
            if (version != null)
            {
                // 返回 Major.Minor.Build 格式
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
            
            // 尝试从 AssemblyInformationalVersion 获取
            var infoVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (infoVersion != null)
            {
                // 去除可能的后缀（如 +commitHash）
                var versionStr = infoVersion.InformationalVersion;
                var plusIndex = versionStr.IndexOf('+');
                if (plusIndex > 0)
                {
                    versionStr = versionStr[..plusIndex];
                }
                return versionStr;
            }
            
            return "0.0.0";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VersionService] 获取当前版本失败: {ex.Message}");
            return "0.0.0";
        }
    }
    
    /// <summary>
    /// 检查更新
    /// </summary>
    public async Task<VersionCheckResult> CheckForUpdateAsync()
    {
        var currentVersion = GetCurrentVersion();
        
        // 检查缓存
        if (_cachedResult != null && DateTime.Now - _cacheTime < CacheDuration)
        {
            // 更新当前版本（以防万一）
            _cachedResult.CurrentVersion = currentVersion;
            _cachedResult.HasUpdate = CompareVersions(_cachedResult.LatestVersion, currentVersion) > 0;
            return _cachedResult;
        }
        
        await _semaphore.WaitAsync();
        try
        {
            // 双重检查
            if (_cachedResult != null && DateTime.Now - _cacheTime < CacheDuration)
            {
                _cachedResult.CurrentVersion = currentVersion;
                _cachedResult.HasUpdate = CompareVersions(_cachedResult.LatestVersion, currentVersion) > 0;
                return _cachedResult;
            }
            
            var result = new VersionCheckResult
            {
                CurrentVersion = currentVersion
            };
            
            try
            {
                var response = await _httpClient.GetAsync(GitHubApiUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var release = await response.Content.ReadFromJsonAsync<GitHubReleaseResponse>();
                    
                    if (release != null)
                    {
                        // 去除版本号前的 'v' 前缀
                        var latestVersion = release.TagName.TrimStart('v', 'V');
                        
                        result.LatestVersion = latestVersion;
                        result.ReleaseUrl = release.HtmlUrl;
                        result.ReleaseNotes = release.Body ?? string.Empty;
                        result.PublishedAt = release.PublishedAt;
                        result.HasUpdate = CompareVersions(latestVersion, currentVersion) > 0;
                    }
                }
                else
                {
                    result.ErrorMessage = $"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}";
                    result.ReleaseUrl = GitHubReleasesUrl;
                }
            }
            catch (TaskCanceledException)
            {
                result.ErrorMessage = "请求超时";
                result.ReleaseUrl = GitHubReleasesUrl;
            }
            catch (HttpRequestException ex)
            {
                result.ErrorMessage = $"网络错误: {ex.Message}";
                result.ReleaseUrl = GitHubReleasesUrl;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ReleaseUrl = GitHubReleasesUrl;
            }
            
            // 更新缓存
            _cachedResult = result;
            _cacheTime = DateTime.Now;
            
            return result;
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    /// <summary>
    /// 比较版本号
    /// </summary>
    /// <param name="version1">版本1</param>
    /// <param name="version2">版本2</param>
    /// <returns>大于0表示version1更新，小于0表示version2更新，等于0表示相同</returns>
    private static int CompareVersions(string version1, string version2)
    {
        try
        {
            // 去除可能的 'v' 前缀
            version1 = version1.TrimStart('v', 'V');
            version2 = version2.TrimStart('v', 'V');
            
            var parts1 = version1.Split('.').Select(s => int.TryParse(s, out var n) ? n : 0).ToArray();
            var parts2 = version2.Split('.').Select(s => int.TryParse(s, out var n) ? n : 0).ToArray();
            
            var maxLength = Math.Max(parts1.Length, parts2.Length);
            
            for (var i = 0; i < maxLength; i++)
            {
                var p1 = i < parts1.Length ? parts1[i] : 0;
                var p2 = i < parts2.Length ? parts2[i] : 0;
                
                if (p1 > p2) return 1;
                if (p1 < p2) return -1;
            }
            
            return 0;
        }
        catch
        {
            return string.Compare(version1, version2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
