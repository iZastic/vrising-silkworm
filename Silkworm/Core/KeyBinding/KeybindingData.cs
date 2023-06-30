using UnityEngine;

namespace Silkworm.Core.KeyBinding;

public class KeybindingData
{
    /// <summary>
    /// Unique ID for the keybinding.
    /// </summary>
    public string Id { get; internal set; }
    /// <summary>
    /// Name for the keybinding as displayed in Controls in-game.
    /// </summary>
    public string Name { get; internal set; }
    /// <summary>
    /// Primary key for the keybinding.
    /// </summary>
    public KeyCode Primary { get; internal set; }
    /// <summary>
    /// Secondary key for the keybinding.
    /// </summary>
    public KeyCode Secondary { get; internal set; }

    public KeybindingData(string id, string name, KeyCode primary, KeyCode secondary)
    {
        Id = id;
        Name = name;
        Primary = primary;
        Secondary = secondary;
    }
}
