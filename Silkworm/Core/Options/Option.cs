using Il2CppSystem;
using Silkworm.API;
using Stunlock.Localization;

namespace Silkworm.Core.Options;

public delegate void OnChange<T>(T value);

public class Option<T>
{
    public string Name { get; internal set; }
    public string Description { get; internal set; }
    public virtual T Value { get; internal set; }
    public T DefaultValue { get; internal set; }

    internal event OnChange<T> OnChange = delegate { };
    internal readonly LocalizationKey NameKey;
    internal readonly LocalizationKey DescKey;

    public Option(string name, string description, T defaultValue)
    {
        Name = name;
        Description = description;
        DefaultValue = defaultValue;
        Value = defaultValue;
        NameKey = LocalizationManager.CreateKey(name);
        DescKey = LocalizationManager.CreateKey(description);
    }

    public virtual void SetValue(T value)
    {
        Value = value;
        OnChange(Value);
    }

    public void AddListener(OnChange<T> action)
    {
        OnChange += action;
    }
}
