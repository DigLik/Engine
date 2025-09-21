using BepuPhysics.Constraints;

namespace Engine.ECS.Components.Physics;

/// <summary>
/// Определяет физические свойства поверхности для расчетов столкновений.
/// </summary>
public struct PhysicsMaterial
{
    /// <summary>
    /// Коэффициент трения. Определяет, насколько "скользкая" поверхность.
    /// Значения обычно находятся в диапазоне [0, 1], но могут быть и выше.
    /// 0 - нет трения (лед), 1 - высокое трение.
    /// </summary>
    public float Friction;

    /// <summary>
    /// Коэффициент упругости (restitution). Определяет, насколько "прыгучий" объект.
    /// 0 - полностью неупругое столкновение (объект не отскакивает), 1 - полностью упругое.
    /// </summary>
    public float Bounciness;

    /// <summary>
    /// Настройки пружины для контакта, влияющие на то, как объекты "проседают" друг в друга.
    /// </summary>
    public SpringSettings SpringSettings;

    public PhysicsMaterial(float friction = 0.5f, float bounciness = 0.0f, SpringSettings? springSettings = null)
    {
        Friction = friction;
        Bounciness = bounciness;
        SpringSettings = springSettings ?? new SpringSettings(30, 1);
    }
}