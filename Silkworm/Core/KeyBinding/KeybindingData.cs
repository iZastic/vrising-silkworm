using UnityEngine;

namespace Silkworm.Core.KeyBinding;

public class KeybindingData
{
    public string Id { get; internal set; }
    public string Name { get; internal set; }
    public KeyCode Primary { get; internal set; }
    public KeyCode Secondary { get; internal set; }

    public KeybindingData(string id, string name, KeyCode primary, KeyCode secondary)
    {
        Id = id;
        Name = name;
        Primary = primary;
        Secondary = secondary;
    }
}
