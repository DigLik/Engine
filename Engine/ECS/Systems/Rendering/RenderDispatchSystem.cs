using Engine.Core.Memory;
using Engine.ECS.Components.Rendering;
using Engine.Rendering;
using Engine.Rendering.Abstractions;
using System.Runtime.InteropServices;

namespace Engine.ECS.Systems.Rendering;

public sealed class RenderDispatchSystem : SystemBase
{
    private IRenderDevice _renderDevice = null!;
    private RenderQueue _renderQueue = null!;
    private ActiveCameraBuffer _cameraBuffer = null!;
    private LinearAllocator _frameAllocator = null!;

    private readonly Dictionary<(MeshHandle, MaterialHandle), List<Matrix4x4>> _batches = new();

    public override void OnInitialize()
    {
        _renderDevice = GetService<IRenderDevice>();
        _renderQueue = GetService<RenderQueue>();
        _cameraBuffer = GetService<ActiveCameraBuffer>();
        _frameAllocator = GetService<LinearAllocator>();
    }

    public override void OnUpdate()
    {
        _renderDevice.BeginFrame();

        if (_cameraBuffer.HasCamera && _renderQueue.Items.Count > 0)
        {
            _renderDevice.SetCameraUniforms(_cameraBuffer.ViewMatrix, _cameraBuffer.ProjectionMatrix);

            foreach (var (mesh, transform) in _renderQueue.Items)
            {
                var key = (mesh.Mesh, mesh.Material);
                ref var batchList = ref CollectionsMarshal.GetValueRefOrAddDefault(_batches, key, out var exists);
                if (!exists) batchList = [];
                batchList!.Add(transform);
            }

            foreach (var (key, matrices) in _batches)
            {
                if (matrices.Count == 0) continue;

                var matrixSpan = _frameAllocator.Allocate<Matrix4x4>(matrices.Count);
                CollectionsMarshal.AsSpan(matrices).CopyTo(matrixSpan);

                _renderDevice.Draw(key.Item1, key.Item2, matrixSpan);

                matrices.Clear();
            }
        }

        _renderDevice.EndFrame();
    }
}