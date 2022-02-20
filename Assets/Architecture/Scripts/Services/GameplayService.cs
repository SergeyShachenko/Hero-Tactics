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
            if (entity.Has<Fighter>())
            {
                ref var fighter = ref entity.Get<Fighter>();
            
                var newHealth = fighter.Stats.Health;
                newHealth -= Mathf.Lerp(0, newHealth, percent);
                
                fighter.Stats.Health = newHealth;
            }

            if (entity.Has<HealthBar>())
            {
                ref var healthBar = ref entity.Get<HealthBar>();

                healthBar.CurrentHealth -= Mathf.Round(Mathf.Lerp(0, healthBar.StartHealth, percent));
                healthBar.Bar.fillAmount = healthBar.CurrentHealth / (healthBar.StartHealth / 100) / 100;
            }
        }
    }
}