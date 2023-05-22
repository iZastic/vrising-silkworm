using Il2CppSystem;
using ProjectM;
using Stunlock.Localization;
using System.Collections.Generic;

namespace Silkworm.API;

public static class LocalizationManager
{
    public static class Format
    {
        public const string Default = "default";
        public const string Percent = "percent";
    }

    public static readonly LocalizationKey DefaultValue;
    public static readonly LocalizationKey PercentValue;

    private static readonly Dictionary<AssetGuid, string> guids;

    static LocalizationManager()
    {
        guids = new();
        DefaultValue = CreateKey("{value}");
        PercentValue = CreateKey("{value}%");
    }

    public static LocalizationKey GetFormatKey(string format)
    {
        return format switch
        {
            Format.Percent => PercentValue,
            _ => DefaultValue,
        };
    }

    public static LocalizationKey CreateKey(string value)
    {
        var key = new LocalizationKey(AssetGuid.FromGuid(Guid.NewGuid()));
        guids.Add(key.GetGuid(), value);
        return key;
    }

    internal static string GetKey(AssetGuid guid)
    {
        return guids[guid];
    }

    internal static string GetKey(LocalizationKey key)
    {
        return GetKey(key.GetGuid());
    }

    internal static bool HasKey(AssetGuid guid)
    {
        return guids.ContainsKey(guid);
    }
}
