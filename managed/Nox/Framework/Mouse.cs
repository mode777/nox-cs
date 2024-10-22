using System.Collections.Generic;
using System.Numerics;

namespace Nox.Framework;

public class Keyboard {
    private static Dictionary<KeyCode, bool> _state = new();
    internal static void SetKeyState(KeyCode key, bool state) => _state[key] = state;
    public static bool IsKeyDown(KeyCode key) => _state.TryGetValue(key, out var down) && down;
}

public class Mouse
{
    public static Vector2 Position;
    public static Vector2 GetPosition() => Position;
}