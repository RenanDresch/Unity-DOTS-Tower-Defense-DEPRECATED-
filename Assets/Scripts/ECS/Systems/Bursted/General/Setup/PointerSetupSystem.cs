
using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class PointerSetupSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, ref Pointer_C pointer) =>
            {
                var entity = e;

                while (pointer.ConfigurationState == ComponentConfigurationEnum.Unconfigured)
                {
                    if (HasComponent<Aim_C>(entity))
                    {
                        pointer.ParentAim = entity;
                        pointer.ConfigurationState = ComponentConfigurationEnum.Configured;
                    }
                    else
                    {
                        if (HasComponent<Parent>(entity))
                        {
                            entity = GetComponent<Parent>(entity).Value;
                        }
                        else
                        {
                            pointer.ConfigurationState = ComponentConfigurationEnum.ConfigFailed;
                        }
                    }
                }

            }).ScheduleParallel();
        }
    }
}