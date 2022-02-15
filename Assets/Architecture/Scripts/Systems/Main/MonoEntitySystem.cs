using UnityComponents.MonoLinks;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Main
{
    public sealed class MonoEntitySystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;


        void IEcsInitSystem.Init()
        {
            var monoEntities = GameObject.FindObjectsOfType<MonoEntity>();
            
            foreach (var gameObject in monoEntities)
            {
                gameObject.GetComponent<MonoEntity>().Init(_world);
            }
        }
    }
}