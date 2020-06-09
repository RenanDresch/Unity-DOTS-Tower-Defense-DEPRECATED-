using Unity.Entities;
using Unity.Mathematics;

namespace Game.ECS.Components
{

    [GenerateAuthoringComponent]
    public struct Pawn_C : IComponentData
    {
        public bool Moving;
        public float3 Direction;
    }
}