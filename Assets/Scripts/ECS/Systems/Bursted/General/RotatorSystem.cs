using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class RotatorSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
          
            Entities.ForEach(
            (
                ref Rotation rotation,
                in Rotator_C rotator_r
            ) =>
            {
                rotation.Value = math.mul(
                    quaternion.RotateY(rotator_r.RotationSpeed*deltaTime),
                    rotation.Value);
            }).ScheduleParallel();
        }
    }
}