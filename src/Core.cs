using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.API.Config;

[assembly: ModInfo("Auto Walk")]

namespace AutoWalk;

class Core : ModSystem
{
    private bool AutoWalk
    {
        get => ClientSettings.Inst.GetBoolSetting("autowalk");
        set => ClientSettings.Inst.Bool["autowalk"] = value;
    }

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

    public override void StartClientSide(ICoreClientAPI capi)
    {
        base.StartClientSide(capi);
        capi.Input.RegisterHotKey("autowalk", Lang.Get("autowalk:toggle-autowalk"), GlKeys.V, HotkeyType.MovementControls);
        capi.Input.SetHotKeyHandler("autowalk", keyCombination => ToggleAutoWalk(keyCombination, capi));
        capi.Event.KeyUp += f => OnKeyUp(f, capi);
        AutoWalk = false;
    }

    private void OnKeyUp(KeyEvent e, ICoreClientAPI capi)
    {
        e.Handled = false;
        var _walkforward = GetKeyEvent(capi, "walkforward");
        if (e.KeyCode == _walkforward.KeyCode && e.KeyCode2 == _walkforward.KeyCode2)
        {
            AutoWalk = false;
        }
    }

    private bool ToggleAutoWalk(KeyCombination keyCombination, ICoreClientAPI capi)
    {
        AutoWalk = !AutoWalk;

        switch (AutoWalk)
        {
            case true:
                (capi.World as ClientMain).CallMethod("OnKeyDown", GetKeyEvent(capi, "walkforward"));
                return true;
            case false:
                (capi.World as ClientMain).CallMethod("OnKeyUp", GetKeyEvent(capi, "walkforward"));
                return true;
        }
    }

    private static KeyEvent GetKeyEvent(ICoreClientAPI capi, string hotkeyCode) => new()
    {
        KeyCode = capi.Input.GetHotKeyByCode(hotkeyCode).CurrentMapping.KeyCode,
        KeyCode2 = capi.Input.GetHotKeyByCode(hotkeyCode).CurrentMapping.SecondKeyCode,
        CtrlPressed = capi.Input.GetHotKeyByCode(hotkeyCode).CurrentMapping.Ctrl,
        AltPressed = capi.Input.GetHotKeyByCode(hotkeyCode).CurrentMapping.Alt,
        ShiftPressed = capi.Input.GetHotKeyByCode(hotkeyCode).CurrentMapping.Shift
    };
}