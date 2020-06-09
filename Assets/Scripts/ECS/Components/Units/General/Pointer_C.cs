using Unity.Entities;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct Pointer_C : IComponentData
    {
        public Entity ParentAim;
        public ComponentConfigurationEnum ConfigurationState;
    }
}