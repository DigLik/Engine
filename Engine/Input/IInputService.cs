namespace Engine.Input;

public interface IInputService
{
    IKeyboard Keyboard { get; }
    IMouse Mouse { get; }

    // Метод для обновления состояния (сброс дельт мыши и т.д.)
    // Вызывается движком в начале кадра
    void Update();
}