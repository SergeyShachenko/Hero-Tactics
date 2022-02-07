using General.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Services
{
    public sealed class GameplayService
    {
        private readonly EcsWorld _world;
        
        public GameplayService(EcsWorld world)
        {
            _world = world;
        }
        

        public bool MoveEntityTo(EcsEntity entity, Vector3 targetPosition, float minDistance)
        {
            if (entity.Has<GameObj>() == false || entity.Has<Movable>() == false) return false;

            
            ref var gameObject = ref entity.Get<GameObj>().Value;
            ref var speed = ref entity.Get<Movable>().Speed;
            
            var newPosition = gameObject.transform.position;
            var moveDirection = targetPosition - newPosition;

            newPosition += moveDirection * (speed * Time.deltaTime);

            gameObject.transform.position = newPosition;

            return Vector3.Distance(newPosition, targetPosition) >= minDistance;
        }
    }
}