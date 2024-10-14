using System.Numerics;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public enum MouseButton {
    Left = sapp_mousebutton.SAPP_MOUSEBUTTON_LEFT,
    Right = sapp_mousebutton.SAPP_MOUSEBUTTON_RIGHT,
    Middle = sapp_mousebutton.SAPP_MOUSEBUTTON_MIDDLE,
    Invalid = sapp_mousebutton.SAPP_MOUSEBUTTON_INVALID	
}

public enum KeyCode {
    Invalid = sapp_keycode.SAPP_KEYCODE_INVALID,
    Space = sapp_keycode.SAPP_KEYCODE_SPACE,
    Apostrophe = sapp_keycode.SAPP_KEYCODE_APOSTROPHE,
    Comma = sapp_keycode.SAPP_KEYCODE_COMMA,
    Minus = sapp_keycode.SAPP_KEYCODE_MINUS,
    Period = sapp_keycode.SAPP_KEYCODE_PERIOD,
    Slash = sapp_keycode.SAPP_KEYCODE_SLASH,
    Num0 = sapp_keycode.SAPP_KEYCODE_0,
    Num1 = sapp_keycode.SAPP_KEYCODE_1,
    Num2 = sapp_keycode.SAPP_KEYCODE_2,
    Num3 = sapp_keycode.SAPP_KEYCODE_3,
    Num4 = sapp_keycode.SAPP_KEYCODE_4,
    Num5 = sapp_keycode.SAPP_KEYCODE_5,
    Num6 = sapp_keycode.SAPP_KEYCODE_6,
    Num7 = sapp_keycode.SAPP_KEYCODE_7,
    Num8 = sapp_keycode.SAPP_KEYCODE_8,
    Num9 = sapp_keycode.SAPP_KEYCODE_9,
    Semicolon = sapp_keycode.SAPP_KEYCODE_SEMICOLON,
    Equal = sapp_keycode.SAPP_KEYCODE_EQUAL,
    A = sapp_keycode.SAPP_KEYCODE_A,
    B = sapp_keycode.SAPP_KEYCODE_B,
    C = sapp_keycode.SAPP_KEYCODE_C,
    D = sapp_keycode.SAPP_KEYCODE_D,
    E = sapp_keycode.SAPP_KEYCODE_E,
    F = sapp_keycode.SAPP_KEYCODE_F,
    G = sapp_keycode.SAPP_KEYCODE_G,
    H = sapp_keycode.SAPP_KEYCODE_H,
    I = sapp_keycode.SAPP_KEYCODE_I,
    J = sapp_keycode.SAPP_KEYCODE_J,
    K = sapp_keycode.SAPP_KEYCODE_K,
    L = sapp_keycode.SAPP_KEYCODE_L,
    M = sapp_keycode.SAPP_KEYCODE_M,
    N = sapp_keycode.SAPP_KEYCODE_N,
    O = sapp_keycode.SAPP_KEYCODE_O,
    P = sapp_keycode.SAPP_KEYCODE_P,
    Q = sapp_keycode.SAPP_KEYCODE_Q,
    R = sapp_keycode.SAPP_KEYCODE_R,
    S = sapp_keycode.SAPP_KEYCODE_S,
    T = sapp_keycode.SAPP_KEYCODE_T,
    U = sapp_keycode.SAPP_KEYCODE_U,
    V = sapp_keycode.SAPP_KEYCODE_V,
    W = sapp_keycode.SAPP_KEYCODE_W,
    X = sapp_keycode.SAPP_KEYCODE_X,
    Y = sapp_keycode.SAPP_KEYCODE_Y,
    Z = sapp_keycode.SAPP_KEYCODE_Z,
    LeftBracket = sapp_keycode.SAPP_KEYCODE_LEFT_BRACKET,
    Backslash = sapp_keycode.SAPP_KEYCODE_BACKSLASH,
    RightBracket = sapp_keycode.SAPP_KEYCODE_RIGHT_BRACKET,
    GraveAccent = sapp_keycode.SAPP_KEYCODE_GRAVE_ACCENT,
    World1 = sapp_keycode.SAPP_KEYCODE_WORLD_1,
    World2 = sapp_keycode.SAPP_KEYCODE_WORLD_2,
    Escape = sapp_keycode.SAPP_KEYCODE_ESCAPE,
    Enter = sapp_keycode.SAPP_KEYCODE_ENTER,
    Tab = sapp_keycode.SAPP_KEYCODE_TAB,
    Backspace = sapp_keycode.SAPP_KEYCODE_BACKSPACE,
    Insert = sapp_keycode.SAPP_KEYCODE_INSERT,
    Delete = sapp_keycode.SAPP_KEYCODE_DELETE,
    Right = sapp_keycode.SAPP_KEYCODE_RIGHT,
    Left = sapp_keycode.SAPP_KEYCODE_LEFT,
    Down = sapp_keycode.SAPP_KEYCODE_DOWN,
    Up = sapp_keycode.SAPP_KEYCODE_UP,
    PageUp = sapp_keycode.SAPP_KEYCODE_PAGE_UP,
    PageDown = sapp_keycode.SAPP_KEYCODE_PAGE_DOWN,
    Home = sapp_keycode.SAPP_KEYCODE_HOME,
    CapsLock = sapp_keycode.SAPP_KEYCODE_CAPS_LOCK,
    ScrollLock = sapp_keycode.SAPP_KEYCODE_SCROLL_LOCK,
    NumLock = sapp_keycode.SAPP_KEYCODE_NUM_LOCK,
    PrintScreen = sapp_keycode.SAPP_KEYCODE_PRINT_SCREEN,
    Pause = sapp_keycode.SAPP_KEYCODE_PAUSE,
    F1 = sapp_keycode.SAPP_KEYCODE_F1,
    F2 = sapp_keycode.SAPP_KEYCODE_F2,
    F3 = sapp_keycode.SAPP_KEYCODE_F3,
    F4 = sapp_keycode.SAPP_KEYCODE_F4,
    F5 = sapp_keycode.SAPP_KEYCODE_F5,
    F6 = sapp_keycode.SAPP_KEYCODE_F6,
    F7 = sapp_keycode.SAPP_KEYCODE_F7,
    F8 = sapp_keycode.SAPP_KEYCODE_F8,
    F9 = sapp_keycode.SAPP_KEYCODE_F9,
    F10 = sapp_keycode.SAPP_KEYCODE_F10,
    F11 = sapp_keycode.SAPP_KEYCODE_F11,
    F12 = sapp_keycode.SAPP_KEYCODE_F12,
    F13 = sapp_keycode.SAPP_KEYCODE_F13,
    F14 = sapp_keycode.SAPP_KEYCODE_F14,
    F15 = sapp_keycode.SAPP_KEYCODE_F15,
    F16 = sapp_keycode.SAPP_KEYCODE_F16,
    F17 = sapp_keycode.SAPP_KEYCODE_F17,
    F18 = sapp_keycode.SAPP_KEYCODE_F18,
    F19 = sapp_keycode.SAPP_KEYCODE_F19,
    F20 = sapp_keycode.SAPP_KEYCODE_F20,
    F21 = sapp_keycode.SAPP_KEYCODE_F21,
    F22 = sapp_keycode.SAPP_KEYCODE_F22,
    F23 = sapp_keycode.SAPP_KEYCODE_F23,
    F24 = sapp_keycode.SAPP_KEYCODE_F24,
    F25 = sapp_keycode.SAPP_KEYCODE_F25,
    KP0 = sapp_keycode.SAPP_KEYCODE_KP_0,
    KP1 = sapp_keycode.SAPP_KEYCODE_KP_1,
    KP2 = sapp_keycode.SAPP_KEYCODE_KP_2,
    KP3 = sapp_keycode.SAPP_KEYCODE_KP_3,
    KP4 = sapp_keycode.SAPP_KEYCODE_KP_4,
    KP5 = sapp_keycode.SAPP_KEYCODE_KP_5,
    KP6 = sapp_keycode.SAPP_KEYCODE_KP_6,
    KP7 = sapp_keycode.SAPP_KEYCODE_KP_7,
    KP8 = sapp_keycode.SAPP_KEYCODE_KP_8,
    KP9 = sapp_keycode.SAPP_KEYCODE_KP_9,
    KPDecimal = sapp_keycode.SAPP_KEYCODE_KP_DECIMAL,
    KPDivide = sapp_keycode.SAPP_KEYCODE_KP_DIVIDE,
    KPMultiply = sapp_keycode.SAPP_KEYCODE_KP_MULTIPLY,
    KPSubtract = sapp_keycode.SAPP_KEYCODE_KP_SUBTRACT,
    KPAdd = sapp_keycode.SAPP_KEYCODE_KP_ADD,
    KPEnter = sapp_keycode.SAPP_KEYCODE_KP_ENTER,
    KPEqual = sapp_keycode.SAPP_KEYCODE_KP_EQUAL,
    LeftShift = sapp_keycode.SAPP_KEYCODE_LEFT_SHIFT,
    LeftControl = sapp_keycode.SAPP_KEYCODE_LEFT_CONTROL,
    LeftAlt = sapp_keycode.SAPP_KEYCODE_LEFT_ALT,
    LeftSuper = sapp_keycode.SAPP_KEYCODE_LEFT_SUPER,
    RightShift = sapp_keycode.SAPP_KEYCODE_RIGHT_SHIFT,
    RightControl = sapp_keycode.SAPP_KEYCODE_RIGHT_CONTROL,
    RightAlt = sapp_keycode.SAPP_KEYCODE_RIGHT_ALT,
    RightSuper = sapp_keycode.SAPP_KEYCODE_RIGHT_SUPER,
    Menu = sapp_keycode.SAPP_KEYCODE_MENU
}

[Flags]
public enum KeyMod {
    Shift = sapp_keymod.SAPP_MODIFIER_SHIFT,
    Control = sapp_keymod.SAPP_MODIFIER_CTRL,
    Alt = sapp_keymod.SAPP_MODIFIER_ALT,
    Super = sapp_keymod.SAPP_MODIFIER_SUPER,
    Lmb = sapp_keymod.SAPP_MODIFIER_LMB,
    Rmb = sapp_keymod.SAPP_MODIFIER_RMB,
    Mmb = sapp_keymod.SAPP_MODIFIER_MMB
}

public class MouseEvent {
    public Vector2 Position;
    public MouseButton Button;
    public KeyMod Modifiers;
    public Vector2 Scroll;
}

public class WindowEvent {
    public Vector2 WindowSize;
    public Vector2 FramebufferSize;
}

public class KeyboardEvent {
    public KeyCode KeyCode;
    public char CharCode;
    public KeyMod Modifiers;
    public bool KeyRepeat;
}

public static class Application
{
    public static event Action OnInit;
    public static event Action OnExit;
    public static event Action<MouseEvent> OnMouseMove;
    public static event Action<MouseEvent> OnMouseDown;
    public static event Action<MouseEvent> OnMouseUp;
    public static event Action<MouseEvent> OnMouseScroll;
    public static event Action<MouseEvent> OnMouseEnter;
    public static event Action<MouseEvent> OnMouseLeave;
    public static event Action<KeyboardEvent> OnKeyDown;
    public static event Action<KeyboardEvent> OnKeyUp;
    public static event Action<KeyboardEvent> OnChar;
    public static event Action<WindowEvent> OnResize;
    public static event Action<WindowEvent> OnIconify;
    public static event Action<WindowEvent> OnRestore;
    public static event Action<WindowEvent> OnFocus;
    public static event Action<WindowEvent> OnBlur;
    public static event Action<WindowEvent> OnSuspend;
    public static event Action<WindowEvent> OnResume;

    private readonly static MouseEvent _mouseEvent = new MouseEvent();
    private readonly static KeyboardEvent _keyboardEvent = new KeyboardEvent();
    private readonly static WindowEvent _windowEvent = new WindowEvent();

    public static void Run()
    {
        unsafe
        {
            nox_run(MyLogFunc, EventCallback, InitCallback, GraphicsDevice.FrameCallback, AudioDevice.AudioCallback);
        }
    }

    private static void InitCallback()
    {
        Renderer2D.Init();
        OnInit?.Invoke();
    }

    private static unsafe void EventCallback(sapp_event* ev)
    {
        switch (ev->type)
        {
            case sapp_event_type.SAPP_EVENTTYPE_QUIT_REQUESTED:
                OnExit?.Invoke();
                Environment.Exit(0);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_MOUSE_MOVE:
                Mouse.Position = _mouseEvent.Position;
                HandleMouseEvent(ev, OnMouseMove);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_MOUSE_DOWN:
                HandleMouseEvent(ev, OnMouseDown);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_MOUSE_UP:
                HandleMouseEvent(ev, OnMouseUp);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_MOUSE_SCROLL:
                HandleMouseEvent(ev, OnMouseScroll);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_MOUSE_ENTER:
                HandleMouseEvent(ev, OnMouseEnter);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_MOUSE_LEAVE:
                HandleMouseEvent(ev, OnMouseLeave);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_KEY_DOWN:
                HandleKeyboardEvent(ev, OnKeyDown);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_KEY_UP:
                HandleKeyboardEvent(ev, OnKeyUp);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_CHAR:
                _keyboardEvent.CharCode = char.ConvertFromUtf32((int) ev->char_code)[0];
                HandleKeyboardEvent(ev, OnChar);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_RESIZED:
                HandleWindowEvent(ev, OnResize);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_ICONIFIED:
                HandleWindowEvent(ev, OnIconify);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_RESTORED:
                HandleWindowEvent(ev, OnRestore);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_FOCUSED:
                HandleWindowEvent(ev, OnFocus);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_UNFOCUSED:
                HandleWindowEvent(ev, OnBlur);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_SUSPENDED:
                HandleWindowEvent(ev, OnSuspend);
                break;
            case sapp_event_type.SAPP_EVENTTYPE_RESUMED:
                HandleWindowEvent(ev, OnResume);
                break;
        }
    }

    private static unsafe void HandleMouseEvent(sapp_event* ev, Action<MouseEvent> eventHandler)
    {
        _mouseEvent.Position = new Vector2(ev->mouse_x, ev->mouse_y);
        _mouseEvent.Button = (MouseButton)ev->mouse_button;
        _mouseEvent.Modifiers = (KeyMod)ev->modifiers;
        _mouseEvent.Scroll = new Vector2(ev->scroll_x, ev->scroll_y);
        eventHandler?.Invoke(_mouseEvent);
    }

    private static unsafe void HandleKeyboardEvent(sapp_event* ev, Action<KeyboardEvent> eventHandler)
    {
        _keyboardEvent.KeyCode = (KeyCode)ev->key_code;
        _keyboardEvent.Modifiers = (KeyMod)ev->modifiers;
        _keyboardEvent.KeyRepeat = ev->key_repeat;
        eventHandler?.Invoke(_keyboardEvent);
    }

    private static unsafe void HandleWindowEvent(sapp_event* ev, Action<WindowEvent> eventHandler)
    {
        _windowEvent.WindowSize = new Vector2(ev->window_width, ev->window_height);
        _windowEvent.FramebufferSize = new Vector2(ev->framebuffer_width, ev->framebuffer_height);
        eventHandler?.Invoke(_windowEvent);
    }
}
