
using Game.ECS.Components;
using Game.MB;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Game.ECS.Systems
{
    public class FXProxySystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer();

            Entities.ForEach((
                Entity e,
                in FXProxy_C proxy) =>
            {

                PoolManager_MB.Instance.GetInstance(
                    FXPicker_MB.Instance.GetFXPrefab(proxy.FXCode),
                    proxy.Position,
                    proxy.Rotation);

                ecb.DestroyEntity(e);

            }).WithoutBurst().Run();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}