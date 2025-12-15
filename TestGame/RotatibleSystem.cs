using Engine.Input;

namespace TestGame;

public class RotatibleSystem : SystemBase
{
    private PhysicsService _physicsService = null!;

    public override void OnInitialize()
        => _physicsService = GetService<PhysicsService>();

    public override void OnUpdate()
    {
        var targetAngularVelocity = new Vector3(0, (MathF.Sin(Time.TotalTime) + 2) / 4, 0);

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

public class CameraRotateSystem : SystemBase
{
    private IInputService _inputService = null!;
    private bool _isActive;

    public override void OnInitialize()
        => _inputService = GetService<IInputService>();    

    public override void OnUpdate()
    {
        const float RotateSpeed = 0.5f;
        const float MoveSpeed = 15.0f;

        var keyboard = _inputService.Keyboard;

        if (keyboard.IsKeyPressed(Key.Tab))
        {
            _isActive = !_isActive;
            _inputService.Mouse.CursorMode = _isActive ? CursorMode.Disabled : CursorMode.Normal;
        }

        var mouseDelta = -_inputService.Mouse.Delta;
        float yaw = mouseDelta.X * RotateSpeed * Time.DeltaTime;
        float pitch = mouseDelta.Y * RotateSpeed * Time.DeltaTime;

        var moveInput = Vector3.Zero;

        if (keyboard.IsKeyDown(Key.W)) moveInput.Z -= 1;
        if (keyboard.IsKeyDown(Key.S)) moveInput.Z += 1;
        if (keyboard.IsKeyDown(Key.A)) moveInput.X -= 1;
        if (keyboard.IsKeyDown(Key.D)) moveInput.X += 1;
        if (keyboard.IsKeyDown(Key.Space)) moveInput.Y += 1;
        if (keyboard.IsKeyDown(Key.ShiftLeft)) moveInput.Y -= 1;

        if (moveInput.LengthSquared() > 0)
            moveInput = Vector3.Normalize(moveInput);

        Query<Camera, Transform>()
            .ForEach((entity, ref camera, ref transform) =>
            {
                if (!_isActive) return;

                if (yaw != 0 || pitch != 0)
                {
                    var qYaw = Quaternion.CreateFromAxisAngle(Vector3.UnitY, yaw);
                    var qPitch = Quaternion.CreateFromAxisAngle(Vector3.UnitX, pitch);
                    transform.Rotation = Quaternion.Normalize(qYaw * transform.Rotation * qPitch);
                }

                if (moveInput != Vector3.Zero)
                {
                    var moveStep = moveInput * MoveSpeed * Time.DeltaTime;
                    var rotatedMove = Vector3.Transform(moveStep, transform.Rotation);

                    transform.Position += rotatedMove;
                }
            });
    }
}