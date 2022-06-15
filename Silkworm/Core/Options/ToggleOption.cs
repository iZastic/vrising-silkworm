namespace Silkworm.Core.Options;

public class ToggleOption : Option<bool>
{
    public ToggleOption(string id, string name, bool defaultvalue) : base(id, name, defaultvalue)
    {
    }
}
