using BepInEx;
using System;
using System.IO;
using System.Text.Json;

namespace Silkworm.Utils;

#nullable enable
public static class FileUtils
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        IncludeFields = true
    };

    public static bool Exists(string filename)
    {
        return File.Exists(Path.Join(Paths.ConfigPath, PluginInfo.PLUGIN_NAME, filename));
    }

    public static void WriteJson(string filename, object? data)
    {
        try
        {
            var serialized = JsonSerializer.Serialize(data, jsonSerializerOptions);
            Directory.CreateDirectory(Path.Join(Paths.ConfigPath, PluginInfo.PLUGIN_NAME));
            File.WriteAllText(Path.Join(Paths.ConfigPath, PluginInfo.PLUGIN_NAME, filename), serialized);
        }
        catch
        {
            Plugin.Logger.LogWarning($"Error saving {filename}");
        }
    }

    public static T? ReadJson<T>(string filename)
    {
        try
        {
            var content = File.ReadAllText(Path.Join(Paths.ConfigPath, PluginInfo.PLUGIN_NAME, filename));
            var deserialized = JsonSerializer.Deserialize<T>(content, jsonSerializerOptions);
            return deserialized;
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogWarning($"Error reading {filename}");
            Plugin.Logger.LogError(ex);
            return default;
        }
    }
}
