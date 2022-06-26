using Unity.Entities;

namespace Silkworm.Utils;

#nullable enable
public static class WorldUtils
{
    private static World? _ClientWorld;
    private static bool _ClientWorldExists;

    public static World DefaultWorld
    {
        get => World.DefaultGameObjectInjectionWorld;
    }

    public static World? ClientWorld
    {
        get
        {
            FindClientWorld();
            return _ClientWorld;
        }
    }

    public static bool ClientWorldExists
    {
        get
        {
            FindClientWorld();
            return _ClientWorldExists;
        }
    }

    public static World? FindWorld(string name)
    {
        foreach (var world in World.All)
        {
            if (world.Name == name)
                return world;
        }
        return null;
    }

    private static void FindClientWorld()
    {
        if (!_ClientWorldExists)
        {
            _ClientWorld = FindWorld("Client_0");
            _ClientWorldExists = _ClientWorld != null;
        }
    }
}
