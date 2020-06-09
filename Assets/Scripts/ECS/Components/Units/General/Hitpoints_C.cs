using Unity.Entities;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct Hitpoints_C : IComponentData
    {
        public bool Dead;
        public int TotalHitpoints;
        public int CurrentHitpoints;
    }
}