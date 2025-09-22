using static System.MathF;

namespace TestGame;

public class RotatibleSystem : SystemBase
{
    private PhysicsService _physicsService = null!;

    public override void OnInitialize()
    {
        _physicsService = GetService<PhysicsService>();
    }

    public override void OnUpdate()
    {
        var targetAngularVelocity = new Vector3(0, (Sin(Time.TotalTime) + 2) / 4, 0);

        Query<PhysicsBody, RotatibleTag>()
            .AsParallel()
            .ForEach((entity, ref body, ref _) =>
            {
                if (_physicsService.Simulation.Bodies.BodyExists(body.Handle))
                {
                    var bodyReference = _physicsService.Simulation.Bodies[body.Handle];

                    bodyReference.Velocity.Angular = targetAngularVelocity;

                    bodyReference.Awake = true;
                }
            });
    }
}