using General.Components;
using General.Components.Battle;
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


        public bool MoveFighterTo(ref EcsEntity entity, Vector3 targetPosition)
        {
            if (!entity.Has<GameObj>() || !entity.Has<Movable>()) return false;
            if (entity.Get<Fighter>().State != FighterState.Alive) return false;
                
            
            ref var gameObject = ref entity.Get<GameObj>().Value;
            ref var speed = ref entity.Get<Movable>().Speed;
            var currentPosition = gameObject.transform.position;
            var direction = targetPosition - currentPosition;
            
            currentPosition += direction * (speed * Time.deltaTime);
            gameObject.transform.position = currentPosition;
            
            
            return !(direction.magnitude <= 0.5f);
        }
    }
}