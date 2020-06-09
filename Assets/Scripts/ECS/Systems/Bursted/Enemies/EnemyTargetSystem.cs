
using Game.ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class EnemyTargetSystem : SystemBase
    {

        [BurstCompile]
        protected override void OnUpdate()
        {

            EntityQuery nexuses = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Nexus_C>(), ComponentType.ReadOnly<Translation>());
            var nexusesTranslations = nexuses.ToComponentDataArray<Translation>(Allocator.TempJob);

            Dependency = Entities.ForEach((
                ref Unit_C unit,
                in Enemy_C enemy_r) =>
            {
                unit.Targeting = nexusesTranslations.Length > 0;
                if (unit.Targeting)
                {
                    unit.TargetPosition = nexusesTranslations[0].Value;
                }

            }).ScheduleParallel(Dependency);

            Dependency.Complete();
            nexusesTranslations.Dispose();
        }
    }
}