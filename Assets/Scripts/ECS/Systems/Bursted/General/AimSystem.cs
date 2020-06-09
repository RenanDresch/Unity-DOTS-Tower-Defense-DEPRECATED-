
using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class AimSystem : SystemBase
    {
        private const float ROTATION_SPEED = 3;

        [BurstCompile]
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((
                Entity entity,
                ref Rotation rotation,
                ref Aim_C aim,
                in LocalToWorld ltw_r,
                in Translation position_r
                ) =>
            {
                if (aim.ConfigurationState == ComponentConfigurationEnum.Configured)
                {
                    if (HasComponent<Unit_C>(aim.ParentUnit))
                    {
                        var unit = GetComponent<Unit_C>(aim.ParentUnit);

                        var currentForward = math.forward(rotation.Value);
                        var worldPosition = math.transform(ltw_r.Value, position_r.Value);
                        var targetDirection = new float3(0, 0, 1);

                        if (unit.Targeting)
                        {
                            targetDirection = worldPosition - unit.TargetPosition;
                            targetDirection.y = 0;
                            targetDirection = math.normalize(targetDirection);
                        }

                        var targetAngle = GetSignedAngle(currentForward, targetDirection);
                        var nextRotation = ROTATION_SPEED * deltaTime * math.sign(targetAngle);

                        if (math.abs(nextRotation) < math.abs(targetAngle))
                        {
                            aim.TargetDistance = 0;
                            rotation.Value = math.mul(quaternion.RotateY(nextRotation), rotation.Value);
                            aim.Locked = false;
                        }
                        else
                        {
                            aim.Locked = unit.Targeting;
                            aim.TargetDistance = unit.Targeting ? math.distance(worldPosition, unit.TargetPosition) : 0;
                            rotation.Value = quaternion.LookRotation(targetDirection, new float3(0, 1, 0));
                        }

                    }
                }
            }).ScheduleParallel();
        }

        [BurstCompile]
        public static float GetSignedAngle(float3 forward, float3 direction)
        {
            var angle = math.acos(math.dot(math.normalize(forward), math.normalize(direction)));
            var cross = math.cross(forward, direction);
            if (math.dot(new float3(0, 1, 0), cross) < 0)
            {
                angle = -angle;
            }
            return angle;
        }
    }
}