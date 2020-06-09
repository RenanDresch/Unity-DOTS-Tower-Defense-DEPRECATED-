using Unity.Entities;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct GameManager_C : IComponentData
    {
        public Entity Manager;
    }
}