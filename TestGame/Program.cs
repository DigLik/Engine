using Engine.Core;
using Engine.ECS;
using Engine.ECS.Components;
using Engine.ECS.Components.Rendering;
using Engine.ECS.Systems.Rendering;
using Engine.Rendering;
using Engine.Rendering.Silk;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using StbImageSharp;
using System.Numerics;

namespace TestGame;

public static class Program
{
    private static IWindow _window = null!;
    private static GL _gl = null!;
    private static Application _app = null!;
    private static SilkRenderDevice _renderDevice = null!;
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
        _renderDevice = new SilkRenderDevice(_gl);

        _app = Application.CreateBuilder()
            .AddService<IRenderDevice>(_renderDevice)
            .AddService(new ActiveCameraBuffer())
            .AddService(new RenderQueue())
            .AddSystem<CubesSystem>()
            .AddSystem<TransformHierarchySystem>()
            .AddSystem<CameraSystem>()
            .AddSystem<RenderBatchingSystem>()
            .AddSystem<RenderDispatchSystem>()
            .Build();

        _world = _app.Services.Resolve<IWorldApi>();

        var shader = CreateTextureShader();

        ImageResult image = ImageResult.FromMemory(File.ReadAllBytes("Assets/image.png"), ColorComponents.RedGreenBlueAlpha);
        ImageResult image2 = ImageResult.FromMemory(File.ReadAllBytes("Assets/svo.png"), ColorComponents.RedGreenBlueAlpha);
        var textureHandle = _renderDevice.CreateTexture(image.Width, image.Height, image.Data);
        var textureHandle2 = _renderDevice.CreateTexture(image2.Width, image2.Height, image2.Data);

        var material = _renderDevice.CreateMaterial(shader, new Dictionary<string, object>
        {
            { "u_Texture", textureHandle }
        });

        var material2 = _renderDevice.CreateMaterial(shader, new Dictionary<string, object>
        {
            { "u_Texture", textureHandle2 }
        });

        var cubeMesh = CreateCubeMesh();
        var rotatableCubeEntity = _world.CreateEntity();
        _world.Add(rotatableCubeEntity, new TransformComponent());
        _world.Add(rotatableCubeEntity, new RenderMesh { Mesh = cubeMesh, Material = material });
        _world.Add(rotatableCubeEntity, new VisibleTag());
        _world.Add(rotatableCubeEntity, new RotatableTag());

        var moveableCubeEntity = _world.CreateEntity();
        _world.Add(moveableCubeEntity, new TransformComponent());
        _world.Add(moveableCubeEntity, new RenderMesh { Mesh = cubeMesh, Material = material });
        _world.Add(moveableCubeEntity, new VisibleTag());
        _world.Add(moveableCubeEntity, new MoveableTag());

        var sizableCubeEntity = _world.CreateEntity();
        _world.Add(sizableCubeEntity, new TransformComponent { Position = new(2, 0, 0) });
        _world.Add(sizableCubeEntity, new RenderMesh { Mesh = cubeMesh, Material = material2 });
        _world.Add(sizableCubeEntity, new VisibleTag());
        _world.Add(sizableCubeEntity, new SizableTag());

        _world.SetParent(sizableCubeEntity, rotatableCubeEntity);
        _world.SetParent(moveableCubeEntity, sizableCubeEntity);

        _cameraEntity = _world.CreateEntity();
        _world.Add(_cameraEntity, new TransformComponent(new Vector3(0, 0, 3)));
        _world.Add(_cameraEntity, new Camera
        {
            IsMain = true,
            ProjectionType = ProjectionType.Perspective,
            FieldOfView = float.DegreesToRadians(75.0f),
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

        _app?.Tick((float)deltaTime);
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
        _renderDevice?.Dispose();
        _gl?.Dispose();
    }

    private static ShaderHandle CreateTextureShader()
    {
        const string vertexSource = @"
            #version 450 core
            layout (location = 0) in vec3 aPos;
            layout (location = 1) in vec3 aNormal;
            layout (location = 2) in vec2 aTexCoords;
            layout (location = 3) in mat4 instanceMatrix;

            uniform mat4 view;
            uniform mat4 projection;
            
            out vec2 TexCoords;

            void main()
            {
                gl_Position = projection * view * instanceMatrix * vec4(aPos, 1.0);
                TexCoords = aTexCoords;
            }";

        const string fragmentSource = @"
            #version 450 core
            out vec4 FragColor;

            in vec2 TexCoords;

            uniform sampler2D u_Texture;

            void main()
            {
                FragColor = texture(u_Texture, TexCoords);
            }";

        return _renderDevice!.CreateShader(vertexSource, fragmentSource);
    }

    private static MeshHandle CreateCubeMesh()
    {
        CubeGenerator.Create(1.0f, out var vertices, out var indices);
        return _renderDevice!.CreateMesh(vertices, indices);
    }
}