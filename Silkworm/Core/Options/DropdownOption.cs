using System;

namespace Silkworm.Core.Options;

public class DropdownOption : Option<int>
{
    internal string[] Values;

    public DropdownOption(string id, string name, int defaultValue, string[] values) : base(id, name, defaultValue)
    {
        Values = values;
    }

    public T GetEnumValue<T>()
    {
        return (T) Enum.Parse(typeof(T), Values[Value]);
    }
}
