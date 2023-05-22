using HarmonyLib;
using MonoMod.RuntimeDetour;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Silkworm.Utils;

public static class DetourUtils
{
    public static NativeDetour Create<T>(string typeName, string methodName, T to, out T original) where T : System.Delegate
    {
        return Create(Type.GetType(typeName), methodName, to, out original);
    }

    public static NativeDetour Create<T>(string typeName, string innerTypeName, string methodName, T to, out T original) where T : System.Delegate
    {
        return Create(GetInnerType(Type.GetType(typeName), innerTypeName), methodName, to, out original);
    }

    public static NativeDetour Create<T>(Type type, string innerTypeName, string methodName, T to, out T original) where T : System.Delegate
    {
        return Create(GetInnerType(type, innerTypeName), methodName, to, out original);
    }

    public static NativeDetour Create<T>(Type type, string methodName, T to, out T original) where T : System.Delegate
    {
        return Create(type.GetMethod(methodName, AccessTools.all), to, out original);
    }

    private static NativeDetour Create<T>(MethodInfo method, T to, out T original) where T : System.Delegate
    {
        var address = Il2CppMethodResolver.ResolveFromMethodInfo(method);
        Plugin.Logger.LogDebug($"Detouring {method.DeclaringType.FullName}.{method.Name} at {address.ToString("X")}");
        var detour = new NativeDetour(address, Marshal.GetFunctionPointerForDelegate(to));
        original = detour.GenerateTrampoline<T>();
        return detour;
    }

    private static Type GetInnerType(Type type, string innerTypeName)
    {
        return type.GetNestedTypes().First(x => x.Name.Contains(innerTypeName));
    }
}
