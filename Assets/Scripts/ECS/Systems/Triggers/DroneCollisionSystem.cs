using Game.ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Game.ECS.Systems
{
    public class NexusTriggerSystem : SystemBase
    {
        private BuildPhysicsWorld m_BuildPhysicsWorldSystem;
        private StepPhysicsWorld m_StepPhysicsWorldSystem;

        protected override void OnCreate()
        {
            m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
            m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        [BurstCompile]
        struct NexusTriggerJob : ITriggerEventsJobBase
        {
            [ReadOnly]
            public ComponentDataFromEntity<DroneUnit_C> DroneGroup;
            [ReadOnly]
            public ComponentDataFromEntity<Nexus_C> NexusGroup;

            public ComponentDataFromEntity<Enemy_C> EnemyGroup;
            public ComponentDataFromEntity<Hitpoints_C> HitpointsGroup;

            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.Entities.EntityA;
                Entity entityB = triggerEvent.Entities.EntityB;

                bool isADrone = DroneGroup.Exists(entityA);
                bool isBNexus = NexusGroup.Exists(entityB);

                if (isADrone && isBNexus)
                {
                    var droneEntity = entityA;

                    var enemyComponent = EnemyGroup[droneEntity];

                    if (enemyComponent.Active)
                    {
                        enemyComponent.Active = false;
                        EnemyGroup[droneEntity] = enemyComponent;

                        var nexusEntity = entityB;
                        var hitpointComponent = HitpointsGroup[nexusEntity];
                        hitpointComponent.CurrentHitpoints--;
                        HitpointsGroup[nexusEntity] = hitpointComponent;
                    }
                }
            }
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            Dependency = new NexusTriggerJob
            {
                DroneGroup = GetComponentDataFromEntity<DroneUnit_C>(true),
                NexusGroup = GetComponentDataFromEntity<Nexus_C>(true),
                EnemyGroup = GetComponentDataFromEntity<Enemy_C>(),
                HitpointsGroup = GetComponentDataFromEntity<Hitpoints_C>()
            }.Schedule(m_StepPhysicsWorldSystem.Simulation,
                       ref m_BuildPhysicsWorldSystem.PhysicsWorld, Dependency);

            Dependency.Complete();
        }
    }
}