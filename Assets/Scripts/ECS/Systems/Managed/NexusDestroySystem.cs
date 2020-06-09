using Game.ECS.Components;
using Unity.Entities;
using Unity.Jobs;

namespace Game.ECS.Systems
{
    public class NexusDestroySystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Entities.ForEach((
                in GameManager_C gameManager_r) =>
            {
                SetSingleton<GameManager_C>(gameManager_r);
            }).WithoutBurst().Run();

        }

        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref Hitpoints_C hitpoints,
                in Nexus_C nexus_r) =>
            {
                if (hitpoints.CurrentHitpoints <= 0 && !hitpoints.Dead)
                {
                    hitpoints.Dead = true;
                    var gameManager = GetSingleton<GameManager_C>();
                    var gameManagerBehaviour = EntityManager.GetComponentObject<GameManager>(gameManager.Manager); ;
                    gameManagerBehaviour.TriggerGameOver();
                }
            }).WithoutBurst().Run();
        }
    }
}