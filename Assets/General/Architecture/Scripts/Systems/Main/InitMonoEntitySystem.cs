using General.UnityComponents.MonoLinks;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems
{
    sealed class InitMonoEntitySystem : IEcsInitSystem
    {
        private EcsWorld _world;


        void IEcsInitSystem.Init()
        {
            var monoEntitys = GameObject.FindObjectsOfType<MonoEntity>();

            
            foreach (var gameObject in monoEntitys)
            {
                gameObject.GetComponent<MonoEntity>().Init(_world);
            }
        }
    }
}