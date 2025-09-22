using BepuPhysics.Constraints;

namespace Engine.ECS.Components.Physics;

public struct PhysicsMaterial(float friction = 0.5f, float bounciness = 0.0f, SpringSettings? springSettings = null)
{
    public float Friction = friction;

    public float Bounciness = bounciness;

    public SpringSettings SpringSettings = springSettings ?? new SpringSettings(120, 1);
}