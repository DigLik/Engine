using Engine.ECS;
using Engine.ECS.Components;
using System.Numerics;

namespace TestGame;

public class CubesSystem : SystemBase
{
    public override void OnUpdate()
    {
        float totalTime = Time.TotalTime;

        Query<RotatableTag, TransformComponent>().ForEach((ref _, ref transform) =>
        {
            var axis = Vector3.Normalize(new Vector3(0.5f, 1.0f, 0.0f));
            transform.Rotation = Quaternion.CreateFromAxisAngle(axis, Time.TotalTime);
        });

        Query<MoveableTag, TransformComponent>().ForEach((ref _, ref transform) =>
        {
            var position = new Vector3(-2, MathF.Sin(totalTime), 0.0f);
            transform.Position = position;
        });

        Query<SizableTag, TransformComponent>().ForEach((ref _, ref transform) =>
        {
            var size = MathF.Abs(MathF.Cos(totalTime)) / 2 + 0.5f;
            transform.Scale = new(size);
        });
    }
}