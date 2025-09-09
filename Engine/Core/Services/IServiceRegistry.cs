namespace Engine.Core.Services;

public interface IServiceRegistry
{
    void Register<T>(T service) where T : class;
    bool TryResolve<T>(out T? service) where T : class;
    T Resolve<T>() where T : class;
}