using System.Text.Json;

namespace QuickFileOrganizer;

internal sealed class AppSettings
{
    public string LastFolder { get; set; } = string.Empty;
    public string Name1 { get; set; } = string.Empty;
    public string Name2 { get; set; } = string.Empty;
    public string Language { get; set; } = "zh-TW";
    public string DateMode { get; set; } = "today";
    public string ManualDate { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
    public string FileDateField { get; set; } = "modified";
    public string DatePosition { get; set; } = "before";
    public int StartNumber { get; set; } = 1;
    public int Digits { get; set; } = 3;
    public bool ContinueLastNumber { get; set; }
    public Dictionary<string, int> LastNumbers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public List<RenameLogEntry> LastRename { get; set; } = [];

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static string SettingsDirectory => Path.Combine(AppContext.BaseDirectory, "Data");
    public static string SettingsPath => Path.Combine(SettingsDirectory, "settings.json");

    public static AppSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsPath)) return new AppSettings();
            return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsPath), JsonOptions) ?? new AppSettings();
        }
        catch { return new AppSettings(); }
    }

    public void Save()
    {
        Directory.CreateDirectory(SettingsDirectory);
        File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, JsonOptions));
    }
}

internal sealed class RenameLogEntry
{
    public string OldPath { get; set; } = string.Empty;
    public string NewPath { get; set; } = string.Empty;
}
