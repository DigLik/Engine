namespace Engine.Input;

public interface IKeyboard
{
    bool IsKeyPressed(Key key);
    bool IsKeyDown(Key key); // Нажата в данный момент
    bool IsKeyUp(Key key);

    event Action<Key> OnKeyDown;
    event Action<Key> OnKeyUp;
}

public interface IMouse
{
    Vector2 Position { get; }
    Vector2 Delta { get; }
    Vector2 ScrollDelta { get; }
    CursorMode CursorMode { get; set; }

    bool IsButtonPressed(MouseButton button);
    bool IsButtonDown(MouseButton button);
    bool IsButtonUp(MouseButton button);

    event Action<MouseButton> OnButtonDown;
    event Action<MouseButton> OnButtonUp;
    event Action<Vector2> OnMove;
    event Action<Vector2> OnScroll;
}