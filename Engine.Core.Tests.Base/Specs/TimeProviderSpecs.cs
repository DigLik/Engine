using Engine.Core.Abstractions;
using FluentAssertions;

namespace Engine.Core.Tests.Base.Specs;

public abstract class TimeProviderSpecs<TProvider> where TProvider : ITimeProvider
{
    protected abstract TProvider CreateProvider();

    [Fact]
    public void InitialState_ShouldBeZero()
    {
        var provider = CreateProvider();
        var snapshot = provider.GetSnapshot();

        snapshot.TotalTime.Should().Be(TimeSpan.Zero);
        snapshot.FrameCount.Should().Be(0);
    }

    [Fact]
    public void Tick_ShouldAdvanceTimeAndFrames()
    {
        var provider = CreateProvider();
        var delta = TimeSpan.FromSeconds(0.1);

        provider.Tick(delta);
        var snapshot = provider.GetSnapshot();

        snapshot.TotalTime.Should().Be(delta);
        snapshot.FrameCount.Should().Be(1);
    }

    [Fact]
    public void Tick_ShouldClampMaxDelta()
    {
        var provider = CreateProvider();
        var hugeDelta = TimeSpan.FromSeconds(100);

        provider.Tick(hugeDelta);
        var snapshot = provider.GetSnapshot();

        snapshot.TotalTime.Should().BeLessThan(hugeDelta);
        snapshot.TotalTime.Should().Be(provider.MaxDeltaTime);
    }

    [Fact]
    public void Reset_ShouldClearState()
    {
        var provider = CreateProvider();
        provider.Tick(TimeSpan.FromSeconds(1));

        provider.Reset();
        var snapshot = provider.GetSnapshot();

        snapshot.TotalTime.Should().Be(TimeSpan.Zero);
        snapshot.FrameCount.Should().Be(0);
    }
}