using Leopotam.Ecs;
using UnityComponents.MonoLinks.Base;
using UnityEngine;

namespace Systems.Startup
{
    public sealed class MonoEntitySystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;


        void IEcsInitSystem.Init()
        {
            InitMonoEntities();
        }

        
        private void InitMonoEntities()
        {
            var monoEntities = GameObject.FindObjectsOfType<MonoEntity>();

            foreach (var gameObject in monoEntities)
                gameObject.GetComponent<MonoEntity>().Init(_world);
        }
    }
}