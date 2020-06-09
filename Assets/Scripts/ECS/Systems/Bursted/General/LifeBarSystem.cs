
using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace Game.ECS.Systems
{
    public class LifeBarSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            Entities.ForEach((
                Entity e,
                ref LifeBar_C lifebar) =>
            {
                lifebar.Value = GetComponent<Hitpoints_C>(lifebar.Master).CurrentHitpoints;
            }).ScheduleParallel();
        }
    }
}