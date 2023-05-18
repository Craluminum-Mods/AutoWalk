using HarmonyLib;

namespace AutoWalk;

public static class HarmonyReflectionExtensions
{
    public static void CallMethod(this object instance, string method, params object[] args)
    {
        AccessTools.Method(instance.GetType(), method)?.Invoke(instance, args);
    }
}
