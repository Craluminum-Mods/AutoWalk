using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.API.Config;

[assembly: ModInfo("Auto Walk")]

namespace AutoWalk;

class Core : ModSystem
{
    bool autoWalk;

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

    public override void StartClientSide(ICoreClientAPI capi)
    {
        base.StartClientSide(capi);
        capi.Input.RegisterHotKey("autowalk", Lang.Get("autowalk:toggle-autowalk"), GlKeys.V, HotkeyType.MovementControls);
        capi.Input.SetHotKeyHandler("autowalk", ToggleAutoWalk);
        capi.Event.RegisterGameTickListener(d => OnGameTick(d, capi), 100);
    }

    private void OnGameTick(float delta, ICoreClientAPI capi)
    {
        if (!autoWalk) return;
        var keyEvent = new KeyEvent { KeyCode = (int)GlKeys.W };
        var clientMain = capi.World as ClientMain;
        clientMain.CallMethod("OnKeyDown", keyEvent);
    }

    private bool ToggleAutoWalk(KeyCombination t1)
    {
        autoWalk = !autoWalk;
        return true;
    }
}