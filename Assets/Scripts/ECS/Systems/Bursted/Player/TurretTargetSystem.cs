
using Game.ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class TurretTargetSystem : SystemBase
    {
        private const float MAXIMUM_DISTANCE = 12;

        [BurstCompile]
        protected override void OnUpdate()
        {
            EntityQuery drones = EntityManager.CreateEntityQuery(
                ComponentType.ReadOnly<DroneUnit_C>(),
                ComponentType.ReadOnly<Translation>());

            var droneTranslations = drones.ToComponentDataArray<Translation>(Allocator.TempJob);
            var droneEntities = drones.ToEntityArray(Allocator.TempJob);

            Dependency = Entities.ForEach((
                ref Unit_C unit,
                in TurretUnit_C turret_r,
                in Translation position_r) =>
            {
                var closestTargetDistance = 9999f;
                unit.Targeting = false;
                for (int i = 0; i < droneTranslations.Length; i++)
                {
                    var targetDistance = math.distance(position_r.Value, droneTranslations[i].Value);
                    if (targetDistance < closestTargetDistance && targetDistance < MAXIMUM_DISTANCE)
                    {
                        closestTargetDistance = targetDistance;
                        unit.TargetPosition = droneTranslations[i].Value;
                        unit.Targeting = true;
                        unit.TargetEntity = droneEntities[i];
                    }
                }

            }).ScheduleParallel(Dependency);

            Dependency.Complete();
            droneTranslations.Dispose();
            droneEntities.Dispose();
        }
    }
}