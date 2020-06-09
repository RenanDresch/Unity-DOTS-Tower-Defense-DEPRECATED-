using Unity.Entities;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct Rotator_C : IComponentData
    {
        public float RotationSpeed;
    }
}