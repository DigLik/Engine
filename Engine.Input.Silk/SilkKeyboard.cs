using SilkKey = Silk.NET.Input.Key;

namespace Engine.Input.Silk;

internal sealed class SilkKeyboard : IKeyboard
{
    private readonly global::Silk.NET.Input.IKeyboard _silkKeyboard;

    private readonly HashSet<Key> _keysDown = [];
    private readonly HashSet<Key> _keysPressed = [];
    private readonly HashSet<Key> _keysReleased = [];

    public event Action<Key>? OnKeyDown;
    public event Action<Key>? OnKeyUp;

    public SilkKeyboard(global::Silk.NET.Input.IKeyboard silkKeyboard)
    {
        _silkKeyboard = silkKeyboard;
        _silkKeyboard.KeyDown += OnSilkKeyDown;
        _silkKeyboard.KeyUp += OnSilkKeyUp;
    }

    public bool IsKeyDown(Key key) => _keysDown.Contains(key);
    public bool IsKeyPressed(Key key) => _keysPressed.Contains(key);
    public bool IsKeyUp(Key key) => _keysReleased.Contains(key);

    private void OnSilkKeyDown(global::Silk.NET.Input.IKeyboard kb, SilkKey key, int scancode)
    {
        var engineKey = (Key)key;
        if (_keysDown.Add(engineKey))
        {
            _keysPressed.Add(engineKey);
            OnKeyDown?.Invoke(engineKey);
        }
    }

    private void OnSilkKeyUp(global::Silk.NET.Input.IKeyboard kb, SilkKey key, int scancode)
    {
        var engineKey = (Key)key;
        if (_keysDown.Remove(engineKey))
        {
            _keysReleased.Add(engineKey);
            OnKeyUp?.Invoke(engineKey);
        }
    }

    public void Update()
    {
        _keysPressed.Clear();
        _keysReleased.Clear();
    }
}