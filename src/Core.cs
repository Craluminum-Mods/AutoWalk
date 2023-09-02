using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.API.Config;

[assembly: ModInfo(name: "Auto Walk", modID: "autowalk", Side = "Client")]

namespace AutoWalk;

public class Core : ModSystem
{
    private bool Enabled { get; set; }
    private static string Name => Lang.Get("autowalk:Toggle", Lang.Get("autowalk:AutoWalk"));

    public override void StartClientSide(ICoreClientAPI capi)
    {
        base.StartClientSide(capi);
        capi.Input.RegisterHotKey("autowalk", Name, GlKeys.V, HotkeyType.MovementControls);
        capi.Input.SetHotKeyHandler("autowalk", _ => Toggle(capi));
        capi.Event.KeyUp += e => OnKeyUp(e, capi);
        capi.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }

    private void OnKeyUp(KeyEvent e, ICoreClientAPI capi)
    {
        e.Handled = false;
        KeyEvent _walkforward = GetKeyEvent(capi, "walkforward");
        if (e.KeyCode == _walkforward.KeyCode && e.KeyCode2 == _walkforward.KeyCode2)
        {
            Enabled = false;
        }
    }

    private bool Toggle(ICoreClientAPI capi)
    {
        Enabled = !Enabled;

        switch (Enabled)
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