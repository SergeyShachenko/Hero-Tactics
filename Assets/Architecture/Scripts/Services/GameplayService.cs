using System.Collections.Generic;
using Components;
using Components.Battle;
using Leopotam.Ecs;
using UnityEngine;

namespace Services
{
    public sealed class GameplayService
    {
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;

        private readonly EcsFilter<Fighter> _fighters;
        
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
            
            var newPosition = gameObject.transform.position;
            var moveDirection = targetPosition - newPosition;

            newPosition += moveDirection * (speed * Time.deltaTime);

            gameObject.transform.position = newPosition;

            return Vector3.Distance(newPosition, targetPosition) >= minDistance;
        }

        public void TakeDamageInPercent(float percent, EcsEntity entity)
        {
            if (entity.Has<Fighter>() == false) return;

            ref var fighter = ref entity.Get<Fighter>();
            
            var newHealth = fighter.Stats.Health;
            newHealth -= newHealth / 100 * percent;

            if (newHealth < 0) newHealth = 0;

            fighter.Stats.Health = newHealth;
        }
    }
}