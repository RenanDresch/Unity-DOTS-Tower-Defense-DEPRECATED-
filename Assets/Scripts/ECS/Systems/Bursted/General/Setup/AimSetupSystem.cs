
using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class AimSetupSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, ref Aim_C aim) =>
            {
                var entity = e;

                while (aim.ConfigurationState == ComponentConfigurationEnum.Unconfigured)
                {
                    if (HasComponent<Unit_C>(entity))
                    {
                        aim.ParentUnit = entity;
                        aim.ConfigurationState = ComponentConfigurationEnum.Configured;
                    }
                    else
                    {
                        if (HasComponent<Parent>(entity))
                        {
                            entity = GetComponent<Parent>(entity).Value;
                        }
                        else
                        {
                            aim.ConfigurationState = ComponentConfigurationEnum.ConfigFailed;
                        }
                    }
                }

            }).ScheduleParallel();
        }
    }
}