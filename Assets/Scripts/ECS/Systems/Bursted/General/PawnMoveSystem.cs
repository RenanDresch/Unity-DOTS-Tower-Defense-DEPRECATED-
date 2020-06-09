using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class PawnMoveSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var random = new Random(50);

            Entities.ForEach(
            (
                ref Rotation rotation,
                ref PhysicsVelocity velocity,
                ref Pawn_C pawn,
                ref Translation position,
                in Unit_C unit_r
            ) =>
            {
                if (pawn.Moving)
                {
                    var currentPosition = position.Value;
                    currentPosition.y = 0;
                    position.Value = currentPosition;

                    if (unit_r.Targeting)
                    {
                        pawn.Direction = math.normalize(unit_r.TargetPosition - position.Value);
                    }
                    else if (pawn.Direction.Equals(float3.zero))
                    {
                        pawn.Direction = math.normalize(new float3(random.NextFloat(-10, 10), 0, random.NextFloat(-10, 10)));
                    }
                    rotation.Value = quaternion.LookRotationSafe(pawn.Direction, new float3(0, 1, 0));
                    velocity.Linear = pawn.Direction * 2;
                }
            }).ScheduleParallel();
        }
    }
}