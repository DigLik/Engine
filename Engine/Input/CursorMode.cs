namespace Engine.Input;

public enum CursorMode
{
    Normal,
    Hidden,
    Disabled, // Заблокирован в центре (для FPS камер)
    Raw       // Raw input без ускорения ОС
}