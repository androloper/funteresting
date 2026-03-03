using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<AppMarketVersionCheckerService>();

builder.Services.AddSingleton<VersionMonitorState>();
builder.Services.AddHostedService<AppVersionMonitorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// =======================================================
// 1️⃣ MARKET VERSION ENDPOINT
// =======================================================
app.MapGet("/version", async (
    AppMarketVersionCheckerService marketService,
    IMemoryCache cache) =>
{
    const string iosBundleId = "unifortunately.development.xxx";
    const string androidPackage = "unifortunately.development.xxx";

    const string iosCacheKey = "ios_store_version";
    const string androidCacheKey = "android_store_version";

    if (!cache.TryGetValue(iosCacheKey, out string? iosVersion))
    {
        iosVersion = await marketService.GetLatestVersionOfAppStoreAsync(iosBundleId);
        if (iosVersion != null)
            cache.Set(iosCacheKey, iosVersion, TimeSpan.FromHours(6));
    }

    if (!cache.TryGetValue(androidCacheKey, out string? androidVersion))
    {
        androidVersion = await marketService.GetLatestVersionOfGooglePlayAsync(androidPackage);
        if (androidVersion != null)
            cache.Set(androidCacheKey, androidVersion, TimeSpan.FromHours(6));
    }

    return Results.Ok(new
    {
        appstore = iosVersion,
        playstore = androidVersion
    });
});


// =======================================================
// 2️⃣ VERSION MONITOR START ENDPOINT
// =======================================================
app.MapPost("/version-monitor/start", (
    string version,
    string callbackUrl,
    VersionMonitorState state) =>
{
    state.TargetVersion = version;
    state.CallbackUrl = callbackUrl;
    state.IsActive = true;

    return Results.Ok(new
    {
        message = "Version monitor started",
        targetVersion = version
    });
});

app.Run();


// =======================================================
// VERSION MONITOR STATE
// =======================================================
public class VersionMonitorState
{
    public string? TargetVersion { get; set; }
    public string? CallbackUrl { get; set; }
    public bool IsActive { get; set; }
}


// =======================================================
// BACKGROUND SERVICE
// =======================================================
public class AppVersionMonitorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly VersionMonitorState _state;

    public AppVersionMonitorService(IServiceProvider serviceProvider,
                                    VersionMonitorState state)
    {
        _serviceProvider = serviceProvider;
        _state = state;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_state.IsActive && !string.IsNullOrEmpty(_state.TargetVersion))
            {
                using var scope = _serviceProvider.CreateScope();
                var marketService = scope.ServiceProvider
                    .GetRequiredService<AppMarketVersionCheckerService>();

                var iosVersion = await marketService
                    .GetLatestVersionOfAppStoreAsync("unifortunately.development.xxx");

                var androidVersion = await marketService
                    .GetLatestVersionOfGooglePlayAsync("unifortunately.development.xxx");

                if (iosVersion == _state.TargetVersion &&
                    androidVersion == _state.TargetVersion)
                {
                    _state.IsActive = false;

                    if (!string.IsNullOrEmpty(_state.CallbackUrl))
                    {
                        using var httpClient = new HttpClient();
                        await httpClient.PostAsync(_state.CallbackUrl, null);
                    }
                }
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}


// =======================================================
// MARKET SERVICE
// =======================================================
public class AppMarketVersionCheckerService
{
    private readonly HttpClient _httpClient;

    public AppMarketVersionCheckerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // 🍎 APP STORE
    public async Task<string?> GetLatestVersionOfAppStoreAsync(string bundleId)
    {
        var url = $"https://itunes.apple.com/lookup?bundleId={bundleId}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.GetProperty("resultCount").GetInt32() == 0)
            return null;

        return root
            .GetProperty("results")[0]
            .GetProperty("version")
            .GetString();
    }

    // 🤖 GOOGLE PLAY (HTML PARSE)
    public async Task<string?> GetLatestVersionOfGooglePlayAsync(string packageName)
    {
        var url = $"https://play.google.com/store/apps/details?id={packageName}&hl=en&gl=us";

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        var html = await response.Content.ReadAsStringAsync();

        var match = Regex.Match(
            html,
            @"\[\[\[""([\d\.]+)""\]\]",
            RegexOptions.Compiled);

        return match.Success ? match.Groups[1].Value : null;
    }
}