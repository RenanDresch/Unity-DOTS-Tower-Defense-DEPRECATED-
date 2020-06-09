using Unity.Entities;
using Unity.Mathematics;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct FX_C : IComponentData
    {
        public bool Available;
        public float3 Position;
        public quaternion Rotation;
        public ComponentConfigurationEnum ConfigurationState;
    }
}