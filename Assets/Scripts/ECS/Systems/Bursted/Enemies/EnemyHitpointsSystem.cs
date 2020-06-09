
using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace Game.ECS.Systems
{
    public class EnemyHitpointsSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            Entities.ForEach((
                Entity e,
                int entityInQueryIndex,
                ref Enemy_C enemy,
                in Hitpoints_C hitpoints_r) =>
            {
                if (enemy.Active)
                {
                    if(hitpoints_r.CurrentHitpoints <= 0)
                    {
                        enemy.Active = false;
                    }
                }
            }).ScheduleParallel();
        }
    }
}