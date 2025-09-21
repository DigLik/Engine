namespace Engine.ECS.Components.Rendering;

public struct Visibility(bool isEnabled)
{
    public bool IsEnabled = isEnabled;

    internal bool IsVisibleInFrustum = false;

    public Visibility() : this(true) { }
}

public record struct LocalBounds(Vector3 Min, Vector3 Max);