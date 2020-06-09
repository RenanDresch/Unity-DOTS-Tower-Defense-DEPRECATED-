using Unity.Entities;
using Unity.Mathematics;

namespace Game.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct WaveSpawner_C : IComponentData
    {
        public Entity Prefab;
        public float2 MinimumPositionOffset;
        public float2 MaximumPositionOffset;
        public int UnitsPerWave;
        public float UnitsPerWaveModifier;
        public int Wave;
        public float WaveCooldown;
        public float CooldownModifier;
        public float CurrentCooldown;
    }
}