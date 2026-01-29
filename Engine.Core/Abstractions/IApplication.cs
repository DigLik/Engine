namespace Engine.Core.Abstractions;

public interface IApplication
{
    void Run();
    void RequestClose();
}