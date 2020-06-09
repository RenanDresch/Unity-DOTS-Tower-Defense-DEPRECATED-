using Game.Contracts.Enums;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct FXProxy_C : IComponentData
    {
        public float3 Position;
        public quaternion Rotation;
        public FXCodeEnum FXCode;
    }
}