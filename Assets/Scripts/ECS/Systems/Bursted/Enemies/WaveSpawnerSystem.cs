
using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class WaveSpawnerSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var random = new Random(100);
            var deltaTime = Time.DeltaTime;
            var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Entities.ForEach((
                int entityInQueryIndex,
                ref WaveSpawner_C spawner,
                ref Translation position) =>
            {
                if (spawner.CurrentCooldown <= 0)
                {
                    var waveUnits = (int)math.floor(spawner.UnitsPerWave + (spawner.Wave * spawner.UnitsPerWaveModifier * spawner.UnitsPerWave));

                    for (var i = 0; i < waveUnits; i++)
                    {
                        var entity = ecb.Instantiate(entityInQueryIndex, spawner.Prefab);
                        ecb.AddComponent<Translation>(entityInQueryIndex, entity);
                        ecb.AddComponent<Rotation>(entityInQueryIndex, entity);

                        ecb.SetComponent<Translation>(entityInQueryIndex, entity, new Translation()
                        {
                            Value = position.Value + new float3(
                            random.NextFloat(
                                spawner.MinimumPositionOffset.x, spawner.MaximumPositionOffset.x),
                            0,
                            random.NextFloat(
                                spawner.MinimumPositionOffset.y, spawner.MaximumPositionOffset.y))
                        });
                    }

                    spawner.CurrentCooldown = spawner.WaveCooldown + (spawner.Wave * spawner.CooldownModifier);
                    spawner.Wave++;
                }
                else
                {
                    spawner.CurrentCooldown -= deltaTime;
                }

            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}