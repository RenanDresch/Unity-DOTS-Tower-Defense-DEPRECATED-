
using Game.ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class EnemyDeathSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        [BurstCompile]
        protected override void OnCreate()
        {
            base.OnCreate();

            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Dependency = Entities.ForEach((
                Entity e,
                int entityInQueryIndex,
                ref Unit_C unit,
                in Enemy_C enemy_r,
                in Translation position_r,
                in Rotation rotation_r) =>
            {
                if (!enemy_r.Active)
                {
                    var newProxy = new FXProxy_C()
                    {
                        Position = position_r.Value,
                        Rotation = rotation_r.Value,
                        FXCode = Contracts.Enums.FXCodeEnum.EnemyExplosion_01
                    };

                    ecb.DestroyEntity(entityInQueryIndex, e);
                }
            }).ScheduleParallel(Dependency);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
            Dependency.Complete();
        }
    }
}