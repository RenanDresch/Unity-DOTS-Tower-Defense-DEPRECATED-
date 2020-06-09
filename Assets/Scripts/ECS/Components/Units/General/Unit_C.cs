using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct Unit_C : IComponentData
    {
        public Entity TargetEntity;
        public float3 TargetPosition;
        public bool Targeting;
    }
}