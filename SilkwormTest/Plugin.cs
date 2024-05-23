using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Silkworm.Utils;

namespace SilkwormTest
{
    [BepInPlugin("SilkwormTest", "SilkwormTest", "1.0.0")]
    public class Plugin : BasePlugin
    {
        internal static ManualLogSource Logger;

        public override void Load()
        {
            Logger = Log;

            AddComponent<SilkwormTest>();
            Logger.LogInfo("Plugin SilkwormTest is loaded!");
        }
    }
}