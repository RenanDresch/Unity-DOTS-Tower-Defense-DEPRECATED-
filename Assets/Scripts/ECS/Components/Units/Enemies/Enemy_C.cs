using Unity.Entities;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct Enemy_C : IComponentData
    {
        public bool Active;
    }
}