using System.Numerics;
using SilkMouseButton = Silk.NET.Input.MouseButton;
using SilkCursorMode = Silk.NET.Input.CursorMode;

namespace Engine.Input.Silk;

internal sealed class SilkMouse : IMouse
{
    private readonly global::Silk.NET.Input.IMouse _silkMouse;
    private readonly HashSet<MouseButton> _buttonsPressed = [];

    public event Action<MouseButton>? OnButtonDown;
    public event Action<MouseButton>? OnButtonUp;
    public event Action<Vector2>? OnMove;
    public event Action<Vector2>? OnScroll;

    public Vector2 Position => _silkMouse.Position;

    private Vector2 _lastPosition;
    public Vector2 Delta { get; private set; }

    public Vector2 ScrollDelta { get; private set; }

    public CursorMode CursorMode
    {
        get => (CursorMode)_silkMouse.Cursor.CursorMode;
        set => _silkMouse.Cursor.CursorMode = (SilkCursorMode)value;
    }

    public SilkMouse(global::Silk.NET.Input.IMouse silkMouse)
    {
        _silkMouse = silkMouse;
        _lastPosition = silkMouse.Position;

        _silkMouse.MouseDown += (_, btn) =>
        {
            var engineBtn = (MouseButton)btn;
            _buttonsPressed.Add(engineBtn);
            OnButtonDown?.Invoke(engineBtn);
        };

        _silkMouse.MouseUp += (_, btn) =>
        {
            var engineBtn = (MouseButton)btn;
            OnButtonUp?.Invoke(engineBtn);
        };

        _silkMouse.MouseMove += (_, pos) =>
        {
            OnMove?.Invoke(pos);
        };

        _silkMouse.Scroll += (_, scroll) =>
        {
            var scrollVector = new Vector2(scroll.X, scroll.Y);
            ScrollDelta += scrollVector;
            OnScroll?.Invoke(scrollVector);
        };
    }

    public bool IsButtonDown(MouseButton button) => _silkMouse.IsButtonPressed((SilkMouseButton)button);
    public bool IsButtonPressed(MouseButton button) => _buttonsPressed.Contains(button);
    public bool IsButtonUp(MouseButton button) => !IsButtonDown(button);

    public void Update()
    {
        var currentPos = Position;
        Delta = currentPos - _lastPosition;
        _lastPosition = currentPos;

        ScrollDelta = Vector2.Zero;
        _buttonsPressed.Clear();
    }
}