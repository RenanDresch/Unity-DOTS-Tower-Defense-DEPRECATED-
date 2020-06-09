
using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class PointerSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((
                ref NonUniformScale scale,
                in Pointer_C pointer_r
                ) =>
            {
                if (pointer_r.ConfigurationState == ComponentConfigurationEnum.Configured)
                {
                    if (HasComponent<Aim_C>(pointer_r.ParentAim))
                    {
                        var aim = GetComponent<Aim_C>(pointer_r.ParentAim);
                        scale.Value = new float3(1, 1, aim.TargetDistance - 0.5f);
                    }
                }
            }).ScheduleParallel();
        }
    }
}