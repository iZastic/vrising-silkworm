using BepInEx.Unity.IL2CPP.Hook;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace Silkworm.Utils;

public static class DetourUtils
{
    public static INativeDetour Create<T>(Type type, string innerTypeName, string methodName, T to, out T original) where T : System.Delegate
    {
        return Create(GetInnerType(type, innerTypeName), methodName, to, out original);
    }

    public static INativeDetour Create<T>(Type type, string methodName, T to, out T original) where T : System.Delegate
    {
        return Create(type.GetMethod(methodName, AccessTools.all), to, out original);
    }

    private static INativeDetour Create<T>(MethodInfo method, T to, out T original) where T : System.Delegate
    {
        var address = Il2CppMethodResolver.ResolveFromMethodInfo(method);
        Plugin.Logger.LogDebug($"Detouring {method.DeclaringType.FullName}.{method.Name} at {address.ToString("X")}");
        return INativeDetour.CreateAndApply(address, to, out original);
    }

    private static Type GetInnerType(Type type, string innerTypeName)
    {
        return type.GetNestedTypes().First(x => x.Name.Contains(innerTypeName));
    }
}
