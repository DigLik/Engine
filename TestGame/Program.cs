using Engine.ECS.Abstractions;
using Engine.Input;
using Engine.Input.Silk;
using Engine.Rendering;
using Engine.Rendering.Abstractions;
using Engine.Rendering.Silk;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;

namespace TestGame;

public static class Program
{
    private static IWindow _window = null!;
    private static GL _gl = null!;
    private static Application _app = null!;
    private static IWorldApi _world = null!;

    private static Entity _cameraEntity;

    public static void Main()
    {
        GlfwWindowing.Use();

        var options = WindowOptions.Default;
        options.Title = "Engine Test Game";
        options.Size = new Vector2D<int>(1280, 720);
        options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion(4, 5));

        _window = Window.Create(options);

        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.Closing += OnClose;
        _window.Resize += OnResize;

        _window.Run();
    }

    private static void OnLoad()
    {
        _gl = _window!.CreateOpenGL();
        var renderDevice = new SilkRenderDevice(_gl);
        var assetService = new AssetService(renderDevice, "Assets");
        var inputService = new SilkInputService(_window);

        _app = Application.CreateBuilder()
            .WithInitialEntityCapacity(16)
            .WithChunkCapacity(16)

            .AddDefaultServices()
            .AddDefaultSystems()

            .AddService<IRenderDevice>(renderDevice)
            .AddService<IAssetService>(assetService)
            .AddService<IInputService>(inputService)

            .AddSystem<RotatibleSystem>()
            .AddSystem<CameraRotateSystem>()

            .Build();

        _world = _app.Services.Resolve<IWorldApi>();

        CreateScene();

        inputService.Keyboard.OnKeyDown += key =>
        {
            if (key == Key.Escape)
                _app.RequestClose();
        };
    }

    private static void CreateScene()
    {
        var assets = _app.Services.Resolve<IAssetService>();
        var renderDevice = _app.Services.Resolve<IRenderDevice>();

        var brickMaterial = assets.LoadMaterial("materials/brick.mat");
        var svoMaterial = assets.LoadMaterial("materials/svo.mat");
        var otvalMaterial = assets.LoadMaterial("materials/otval.mat");

        MeshGenerator.CreateCube(1.0f, out var cubeVertices, out var cubeIndices);
        var cubeMesh = renderDevice.CreateMesh(cubeVertices, cubeIndices);

        MeshGenerator.CreateCapsule(0.5f, 1.0f, 24, 12, out var capsuleVertices, out var capsuleIndices);
        var capsuleMesh = renderDevice.CreateMesh(capsuleVertices, capsuleIndices);

        MeshGenerator.CreateCylinder(0.5f, 1.5f, 32, out var cylinderVertices, out var cylinderIndices);
        var cylinderMesh = renderDevice.CreateMesh(cylinderVertices, cylinderIndices);

        MeshGenerator.CreateIcosphere(0.75f, 3, out var sphereVertices, out var sphereIndices);
        var sphereMesh = renderDevice.CreateMesh(sphereVertices, sphereIndices);

        var fallingCube = _world.CreateEntity();
        _world.Add(fallingCube, new Transform(new Vector3(0, 5, 0), new Vector3(3)));
        _world.Add(fallingCube, new RenderMesh { Mesh = cubeMesh, Material = brickMaterial });
        _world.Add(fallingCube, new Visibility());
        _world.Add(fallingCube, new BoxCollider());
        _world.Add(fallingCube, new PhysicsMaterial(friction: 2.0f));

        var fallingCube2 = _world.CreateEntity();
        _world.Add(fallingCube2, new Transform(new Vector3(0.5f, 14, 0.1f), new(2), Quaternion.CreateFromYawPitchRoll(0.5f, 0.8f, 0.1f)));
        _world.Add(fallingCube2, new RenderMesh { Mesh = cubeMesh, Material = svoMaterial });
        _world.Add(fallingCube2, new Visibility());
        _world.Add(fallingCube2, new BoxCollider());
        _world.Add(fallingCube, new PhysicsMaterial(friction: 2.0f));

        var ground = _world.CreateEntity();
        _world.Add(ground, new Transform { Position = new Vector3(0, -2, 0), Scale = new Vector3(20, 5, 20) });
        _world.Add(ground, new RenderMesh { Mesh = cubeMesh, Material = brickMaterial });
        _world.Add(ground, new Visibility());
        _world.Add(ground, new KinematicTag());
        _world.Add(ground, new BoxCollider());
        _world.Add(ground, new RotatibleTag());
        _world.Add(ground, new PhysicsMaterial(friction: 2.0f));

        var fallingCapsule = _world.CreateEntity();
        _world.Add(fallingCapsule, new Transform(new Vector3(-2f, 9, 0)));
        _world.Add(fallingCapsule, new RenderMesh { Mesh = capsuleMesh, Material = brickMaterial });
        _world.Add(fallingCapsule, new Visibility());
        _world.Add(fallingCapsule, new CapsuleCollider { Radius = 0.5f, Length = 1.0f });
        _world.Add(fallingCube, new PhysicsMaterial(friction: 2.0f));

        var fallingCylinder = _world.CreateEntity();
        _world.Add(fallingCylinder, new Transform(new Vector3(2f, 11, 0.2f), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathF.PI / 4f)));
        _world.Add(fallingCylinder, new RenderMesh { Mesh = cylinderMesh, Material = svoMaterial });
        _world.Add(fallingCylinder, new Visibility());
        _world.Add(fallingCylinder, new CylinderCollider { Radius = 0.5f, Length = 1.5f });
        _world.Add(fallingCube, new PhysicsMaterial(friction: 2.0f));

        var fallingSphere = _world.CreateEntity();
        _world.Add(fallingSphere, new Transform(new Vector3(0, 17, 0)));
        _world.Add(fallingSphere, new RenderMesh { Mesh = sphereMesh, Material = brickMaterial });
        _world.Add(fallingSphere, new Visibility());
        _world.Add(fallingSphere, new SphereCollider { Radius = 0.75f });
        _world.Add(fallingSphere, new PhysicsMaterial(friction: 2.0f, bounciness: 0.1f));

        var sunCube = _world.CreateEntity();
        _world.Add(sunCube, new Transform(new Vector3(0, 10, 0), new Vector3(4.0f)));
        _world.Add(sunCube, new RenderMesh { Mesh = cubeMesh, Material = otvalMaterial });
        _world.Add(sunCube, new Visibility());

        _cameraEntity = _world.CreateEntity();
        _world.Add(_cameraEntity, new Transform(new Vector3(0, 7, 12)));
        _world.Add(_cameraEntity, new Camera
        {
            IsMain = true,
            ProjectionType = ProjectionType.Perspective,
            FieldOfView = float.DegreesToRadians(90.0f),
            NearPlane = 0.1f,
            FarPlane = 100f,
            ViewportSize = new Vector2(_window!.Size.X, _window.Size.Y)
        });
    }

    private static void OnUpdate(double deltaTime) { }

    private static double _timeAccumulator;
    private static int _frameCounter;
    private const double _titleUpgradePeriod = 1;
    private static void OnRender(double deltaTime)
    {
        _timeAccumulator += deltaTime; _frameCounter++;
        if (_timeAccumulator > _titleUpgradePeriod)
        {
            double averageRenderPeriod = _timeAccumulator / _frameCounter;
            double fps = 1 / averageRenderPeriod;
            _window!.Title = $"{fps:F2}";
            _timeAccumulator -= _titleUpgradePeriod;
            _frameCounter = 0;
        }

        if (!_app.Tick((float)deltaTime))
            _window.Close();
    }

    private static void OnResize(Vector2D<int> newSize)
    {
        _gl?.Viewport(newSize);

        if (_world != null && _world.IsAlive(_cameraEntity))
        {
            ref var camera = ref _world.Ref<Camera>(_cameraEntity);
            camera.ViewportSize = new Vector2(newSize.X, newSize.Y);
        }
    }

    private static void OnClose()
    {
        _app?.Dispose();
    }
}