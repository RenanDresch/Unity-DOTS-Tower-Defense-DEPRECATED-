using Unity.Entities;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct Gun_C : IComponentData
    {
        public Entity ParentUnit;
        public Entity Aim;
        public Entity Pointer;
        public float Cooldown;
        public float CurrentCooldown;
    }
}