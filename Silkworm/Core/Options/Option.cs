using Silkworm.API;
using Stunlock.Localization;
using System;
using UnityEngine.Events;

namespace Silkworm.Core.Options;

public class Option<T>
{
    public string Id { get; internal set; }
    public string Name { get; internal set; }
    public virtual T Value { get; internal set; }
    public T DefaultValue { get; internal set; }
    public UnityEvent<T> OnChange { get; internal set; } = new();

    internal readonly LocalizationKey NameKey;

    public Option(string id, string name, T defaultValue)
    {
        Id = id;
        Name = name;
        Value = defaultValue;
        DefaultValue = defaultValue;
        NameKey = LocalizationManager.CreateKey(name);
    }

    public virtual void SetValue(T value)
    {
        Value = value;
        OnChange?.Invoke(Value);
    }

    public void AddListener(Action<T> action) => OnChange.AddListener(action);
}
