using Engine.Core.Abstractions;
using FluentAssertions;

namespace Engine.Core.Tests.Base.Specs;

public abstract class ServiceRegistrySpecs<TRegistry> where TRegistry : IServiceRegistry
{
    protected abstract TRegistry CreateRegistry();

    private class ServiceA { }
    private class ServiceB { }

    [Fact]
    public void Resolve_RegisteredService_ShouldReturnInstance()
    {
        var registry = CreateRegistry();
        var instance = new ServiceA();
        registry.Register(instance);

        var resolved = registry.Resolve<ServiceA>();

        resolved.Should().BeSameAs(instance);
    }

    [Fact]
    public void Resolve_UnregisteredService_ShouldThrow()
    {
        var registry = CreateRegistry();

        var action = registry.Resolve<ServiceA>;

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void TryResolve_RegisteredService_ShouldReturnTrueAndInstance()
    {
        var registry = CreateRegistry();
        var instance = new ServiceB();
        registry.Register(instance);

        var result = registry.TryResolve<ServiceB>(out var resolved);

        result.Should().BeTrue();
        resolved.Should().BeSameAs(instance);
    }

    [Fact]
    public void TryResolve_UnregisteredService_ShouldReturnFalse()
    {
        var registry = CreateRegistry();

        var result = registry.TryResolve<ServiceA>(out var resolved);

        result.Should().BeFalse();
        resolved.Should().BeNull();
    }
}