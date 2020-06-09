using Unity.Entities;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct LifeBar_C : IComponentData
    {
        public Entity Master;
        public int Value;
    }
}