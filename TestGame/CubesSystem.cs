using Engine.ECS;
using Engine.ECS.Components;
using System.Numerics;

namespace TestGame;

public class CubesSystem : SystemBase
{
    public override void OnUpdate()
    {
        float totalTime = Time.TotalTime;

        Query<TransformComponent, RotatableTag>()
            .ForEach((ref transform, ref _) =>
            {
                var axis = Vector3.Normalize(new Vector3(0.5f, 1.0f, 0.0f));
                transform.Rotation = Quaternion.CreateFromAxisAngle(axis, totalTime);
            });

        Query()
            .With<TransformComponent, MoveableTag>()
            .ForEach((ref transform, ref _) =>
            {
                var position = new Vector3(-2, MathF.Sin(totalTime), 0.0f);
                transform.Position = position;
            });

        Query<TransformComponent, SizableTag>()
            .ForEach((ref transform, ref _) =>
            {
                var size = MathF.Abs(MathF.Cos(totalTime)) / 2 + 0.5f;
                transform.Scale = new(size);
            });
    }
}