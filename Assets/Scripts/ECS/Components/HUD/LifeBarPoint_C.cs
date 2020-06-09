using Unity.Entities;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct LifeBarPoint_C : IComponentData
    {
        public Entity Bar;
        public int Value;
    }
}