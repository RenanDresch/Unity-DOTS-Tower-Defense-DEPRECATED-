using Unity.Entities;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct Aim_C : IComponentData
    {
        public Entity ParentUnit;
        public float TargetDistance;
        public bool Locked;
        public ComponentConfigurationEnum ConfigurationState;
    }
}