using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public class GamepadInstance {

    internal NoxGamepadState _state = new();
    internal GamepadInstance(int id) {
        Id = id;
    }   

    public int Id { get; }

    public bool IsConnected => _state.connected == 1;
    public float LeftX => MathF.Round(_state.left_x, 2);
    public float LeftY => MathF.Round(_state.left_y, 2);
    public float RightX => MathF.Round(_state.right_x, 2);
    public float RightY => MathF.Round(_state.right_y, 2);
    public float LeftTrigger => MathF.Round(_state.trigger_l, 2);
    public float RightTrigger => MathF.Round(_state.trigger_r, 2);
}

public static class Gamepad {
    internal static Dictionary<int, GamepadInstance> Gamepads = new();
    public static IEnumerable<GamepadInstance> All => Gamepads.Values;

    internal static void UpdateGamepads() {
        IntPtr gamepadsPtr;
        int count;

        // Call the native function
        nox_get_gamepads(out gamepadsPtr, out count);
        if(count == 0) {
            return;
        }

        // We are using int instead of uint because Marshal.Copy does not support uint
        var gamepads = new int[count];
        Marshal.Copy(gamepadsPtr, gamepads, 0, count);

        NoxGamepadState state = new NoxGamepadState();
        for (int i = 0; i < count; i++)
        {
            var id = gamepads[i];
            if(!Gamepads.ContainsKey(id)) {
                Gamepads[id] = new GamepadInstance(id);
            }
            nox_get_gamepad_state((uint)id, ref Gamepads[id]._state);
        }
    }
    
}