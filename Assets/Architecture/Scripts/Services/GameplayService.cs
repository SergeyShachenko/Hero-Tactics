using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Services
{
    public sealed class GameplayService
    {
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;

        private readonly EcsFilter<Components.Battle.Fighter> _fighters;
        
        public GameplayService(EcsWorld world, GameTools gameTools)
        {
            _world = world;
            _gameTools = gameTools;
        }
        

        public bool MoveEntityTo(EcsEntity entity, Vector3 targetPosition, float minDistance)
        {
            if (entity.Has<GameObj>() == false || entity.Has<Movable>() == false) return false;

            
            ref var gameObject = ref entity.Get<GameObj>().Value;
            ref var speed = ref entity.Get<Movable>().Speed;

            var currentPosition = gameObject.transform.position;
            var moveDirection = targetPosition - currentPosition;
            var newPosition = currentPosition + moveDirection * (speed * Time.fixedDeltaTime);

            gameObject.transform.position = newPosition;

            return Vector3.Distance(newPosition, targetPosition) >= minDistance;
        }
    }
}