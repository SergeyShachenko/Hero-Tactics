using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Services
{
    public sealed class GameplayService : GameToolServiceBase
    {
        public GameplayService(EcsWorld world, GameTools gameTools) : base(world, gameTools) { }
        

        public bool MoveEntityTo(EcsEntity entity, Vector3 targetPosition, float minDistance = 0.05f)
        {
            if (entity.Has<GameObj>() == false || entity.Has<Movable>() == false) return false;

            
            ref var gameObject = ref entity.Get<GameObj>().Value;
            ref var speed = ref entity.Get<Movable>().Speed;

            var currentPosition = gameObject.transform.position;
            var moveDirection = targetPosition - currentPosition;

            gameObject.transform.Translate(moveDirection * speed * Time.fixedDeltaTime);
            
            return Vector3.Distance(gameObject.transform.position, targetPosition) > minDistance;
        }
    }
}