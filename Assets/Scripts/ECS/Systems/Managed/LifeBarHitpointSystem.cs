
using Game.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;

namespace Game.ECS.Systems
{
    public class LifeBarHitpointSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        [BurstCompile]
        protected override void OnCreate()
        {
            base.OnCreate();

            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }


        [BurstCompile]
        protected override void OnUpdate()
        {
            var meshGroup = GetEntityQuery(
                ComponentType.ReadOnly<LifeBar_C>(),
                ComponentType.ReadOnly<RenderMesh>());

            var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer();

            Entities.ForEach((
                Entity e,
                ref LifeBarPoint_C barPoint) =>
            {
                var currentLife = GetComponent<LifeBar_C>(barPoint.Bar).Value;
                if (barPoint.Value > currentLife)
                {
                    var teste = EntityManager.GetSharedComponentData<RenderMesh>(e);
                    teste.layer = 8;
                    ecb.SetSharedComponent(e, teste);
                }
            }).WithoutBurst().Run();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}