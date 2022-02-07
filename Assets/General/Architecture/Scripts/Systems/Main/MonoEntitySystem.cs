using General.UnityComponents.MonoLinks;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems.Main
{
    public sealed class MonoEntitySystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;


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