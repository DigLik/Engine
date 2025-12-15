using Engine.Core.Memory;
using Engine.ECS.Components.Rendering;
using Engine.Rendering;
using Engine.Rendering.Abstractions;
using System.Runtime.InteropServices;

namespace Engine.ECS.Systems.Rendering;

public sealed class RenderDispatchSystem : SystemBase
{
    private struct RenderBatchItem : IComparable<RenderBatchItem>
    {
        public int MeshId;
        public int MaterialId;
        public Matrix4x4 Transform;

        public readonly int CompareTo(RenderBatchItem other)
        {
            int matCmp = MaterialId.CompareTo(other.MaterialId);
            if (matCmp != 0) return matCmp;
            return MeshId.CompareTo(other.MeshId);
        }
    }

    private RenderBatchItem[] _batchBuffer = new RenderBatchItem[2048];

    private IRenderDevice _renderDevice = null!;
    private RenderQueue _renderQueue = null!;
    private ActiveCameraBuffer _cameraBuffer = null!;
    private LinearAllocator _frameAllocator = null!;

    private readonly Dictionary<(MeshHandle, MaterialHandle), List<Matrix4x4>> _batches = [];

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

            int count = _renderQueue.Items.Count;
            if (_batchBuffer.Length < count) Array.Resize(ref _batchBuffer, count * 2);

            var itemsSpan = CollectionsMarshal.AsSpan(_renderQueue.Items);
            for (int i = 0; i < count; i++)
            {
                ref var src = ref itemsSpan[i];
                _batchBuffer[i] = new RenderBatchItem
                {
                    MeshId = src.Mesh.Mesh.Id,
                    MaterialId = src.Mesh.Material.Id,
                    Transform = src.Transform
                };
            }

            Array.Sort(_batchBuffer, 0, count);

            int batchStart = 0;
            while (batchStart < count)
            {
                var current = _batchBuffer[batchStart];
                int batchCount = 1;

                while (batchStart + batchCount < count)
                {
                    var next = _batchBuffer[batchStart + batchCount];
                    if (next.MaterialId != current.MaterialId || next.MeshId != current.MeshId) break;
                    batchCount++;
                }

                var instances = _frameAllocator.Allocate<Matrix4x4>(batchCount);
                for (int k = 0; k < batchCount; k++)
                    instances[k] = _batchBuffer[batchStart + k].Transform;

                _renderDevice.Draw(new MeshHandle(current.MeshId, default), new MaterialHandle(current.MaterialId), instances);

                batchStart += batchCount;
            }
        }
        _renderDevice.EndFrame();
    }
}
