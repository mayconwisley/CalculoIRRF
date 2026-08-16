using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

namespace CalculoIRRF.Presentation;

public enum ThemeMode { Automatico, Claro, Escuro }

[SupportedOSPlatform("windows")]
public static class ThemeManager
{
    private static readonly string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CalculoIRRF", "settings.json");
    public static ThemeMode CurrentMode { get; private set; } = ThemeMode.Automatico;

    public static void Initialize()
    {
        CurrentMode = LoadMode();
        SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
        Apply(CurrentMode, false);
    }

    public static void Dispose() => SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;

    public static void Apply(ThemeMode mode, bool persist = true)
    {
        CurrentMode = mode;
        var palette = mode == ThemeMode.Escuro || mode == ThemeMode.Automatico && IsWindowsUsingDarkTheme() ? Dark : Light;
        foreach (var (key, color) in palette)
            System.Windows.Application.Current.Resources[key] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));

        if (persist) SaveMode(mode);
    }

    private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
    {
        if (CurrentMode == ThemeMode.Automatico)
            System.Windows.Application.Current.Dispatcher.BeginInvoke(() => Apply(CurrentMode, false));
    }

    private static bool IsWindowsUsingDarkTheme()
    {
        const string Key = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize";
        return Registry.GetValue(Key, "AppsUseLightTheme", 1) is int lightTheme && lightTheme == 0;
    }

    private static ThemeMode LoadMode()
    {
        try
        {
            if (!File.Exists(SettingsPath)) return ThemeMode.Automatico;
            var settings = JsonSerializer.Deserialize<ThemeSettings>(File.ReadAllText(SettingsPath));
            return Enum.TryParse<ThemeMode>(settings?.Theme, true, out var mode) ? mode : ThemeMode.Automatico;
        }
        catch { return ThemeMode.Automatico; }
    }

    private static void SaveMode(ThemeMode mode)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(new ThemeSettings { Theme = mode.ToString() }));
        }
        catch { }
    }

    private static readonly (string Key, string Color)[] Light =
    [
        ("ApplicationBackgroundBrush", "#F8FAFC"), ("SurfaceBrush", "#FFFFFF"), ("InputBackgroundBrush", "#FFFFFF"), ("ResultBackgroundBrush", "#F8FAFC"), ("BorderBrush", "#E2E8F0"), ("TextBrush", "#101828"), ("MutedTextBrush", "#667085"), ("AccentBrush", "#1570EF"), ("AccentHoverBrush", "#175CD3"), ("AccentForegroundBrush", "#FFFFFF"), ("ButtonBackgroundBrush", "#F9FAFB"), ("ButtonHoverBrush", "#F2F4F7"), ("DisabledBrush", "#EAECF0"), ("DataGridAlternateBrush", "#F9FAFB")
    ];

    private static readonly (string Key, string Color)[] Dark =
    [
        ("ApplicationBackgroundBrush", "#101828"), ("SurfaceBrush", "#182230"), ("InputBackgroundBrush", "#101828"), ("ResultBackgroundBrush", "#101828"), ("BorderBrush", "#344054"), ("TextBrush", "#F9FAFB"), ("MutedTextBrush", "#98A2B3"), ("AccentBrush", "#2E90FA"), ("AccentHoverBrush", "#53B1FD"), ("AccentForegroundBrush", "#FFFFFF"), ("ButtonBackgroundBrush", "#25354D"), ("ButtonHoverBrush", "#344054"), ("DisabledBrush", "#25354D"), ("DataGridAlternateBrush", "#1D2939")
    ];

    private sealed class ThemeSettings { public string Theme { get; set; } = ThemeMode.Automatico.ToString(); }
}
