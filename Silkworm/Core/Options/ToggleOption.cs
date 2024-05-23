namespace Silkworm.Core.Options;

public class ToggleOption : Option<bool>
{
    public ToggleOption(string name, string description, bool defaultvalue) : base(name, description, defaultvalue)
    {
    }
}
