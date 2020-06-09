
using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace Game.ECS.Systems
{
    public class ShootSystem : SystemBase
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
            var deltaTime = Time.DeltaTime;

            var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Entities.ForEach((
                int entityInQueryIndex,
                Entity e,
                ref Gun_C gun) =>
            {
                if (gun.CurrentCooldown <= 0)
                {
                    var hasUnitComponent = HasComponent<Unit_C>(gun.ParentUnit);
                    var hasAimComponent = HasComponent<Aim_C>(gun.Aim);
                    if(hasUnitComponent && hasAimComponent)
                    {
                        var unit = GetComponent<Unit_C>(gun.ParentUnit);
                        var aim = GetComponent<Aim_C>(gun.Aim);
                        if(unit.Targeting && aim.Locked)
                        {
                            gun.CurrentCooldown = gun.Cooldown;

                            var targetHasHitpoints = HasComponent<Hitpoints_C>(unit.TargetEntity);
                            if(targetHasHitpoints)
                            {
                                var hitpoints = GetComponent<Hitpoints_C>(unit.TargetEntity);
                                hitpoints.CurrentHitpoints--;
                                ecb.SetComponent(entityInQueryIndex, unit.TargetEntity, hitpoints);
                            }
                        }
                    }
                }
                else
                {
                    gun.CurrentCooldown -= deltaTime;
                }
            }).ScheduleParallel(Dependency);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}