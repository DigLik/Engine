using Silk.NET.Input;
using Silk.NET.Windowing;

namespace Engine.Input.Silk;

public sealed class SilkInputService : IInputService, IDisposable
{
    private readonly IInputContext _context;
    private readonly SilkKeyboard _keyboard;
    private readonly SilkMouse _mouse;

    public IKeyboard Keyboard => _keyboard;
    public IMouse Mouse => _mouse;

    public SilkInputService(IWindow window)
    {
        _context = window.CreateInput();

        _keyboard = new SilkKeyboard(_context.Keyboards[0]);
        _mouse = new SilkMouse(_context.Mice[0]);
    }

    public void Update()
    {
        _keyboard.Update();
        _mouse.Update();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}