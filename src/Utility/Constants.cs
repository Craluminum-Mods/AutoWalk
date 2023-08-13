using Vintagestory.API.Config;

namespace AutoWalk;

public static class Constants
{
    public static string StringEnabled => Lang.Get("worldconfig-snowAccum-Enabled");
    public static string StringDisabled => Lang.Get("worldconfig-snowAccum-Disabled");

    public static string ToggleName(string name) => Lang.Get("autowalk:Toggle", name);
    public static string StringToggle(bool state, string name) => Lang.Get("autowalk:Toggle." + state, name, state ? StringEnabled : StringDisabled);
}