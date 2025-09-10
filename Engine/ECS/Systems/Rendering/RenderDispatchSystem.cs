using Engine.Rendering;
using System.Runtime.InteropServices;

namespace Engine.ECS.Systems.Rendering;

public sealed class RenderDispatchSystem : SystemBase
{
    private IRenderDevice _renderDevice = null!;
    private RenderQueue _renderQueue = null!;
    private ActiveCameraBuffer _cameraBuffer = null!;

    public override void OnInitialize()
    {
        _renderDevice = GetService<IRenderDevice>();
        _renderQueue = GetService<RenderQueue>();
        _cameraBuffer = GetService<ActiveCameraBuffer>();
    }

    public override void OnUpdate()
    {
        _renderDevice.BeginFrame();

        if (_cameraBuffer.HasCamera)
        {
            _renderDevice.SetCameraUniforms(_cameraBuffer.ViewMatrix, _cameraBuffer.ProjectionMatrix);

            foreach (var (key, matrices) in _renderQueue.Batches)
            {
                if (matrices.Count > 0)
                    _renderDevice.Draw(key.Item1, key.Item2, CollectionsMarshal.AsSpan(matrices));
            }
        }

        _renderDevice.EndFrame();
    }
}