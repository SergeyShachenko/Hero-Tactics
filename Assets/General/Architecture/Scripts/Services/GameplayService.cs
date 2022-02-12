using System.Collections.Generic;
using General.Components;
using General.Components.Battle;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Services
{
    public sealed class GameplayService
    {
        private readonly EcsWorld _world;

        private readonly EcsFilter<Fighter> _fighters;
        
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
        
        // public List<EcsEntity> GetSquad(int squadID)
        // {
        //     if (_fighters.IsEmpty()) return null;
        //
        //
        //     var squad = new List<EcsEntity>();
        //     
        //     foreach (var index in _fighters)
        //     {
        //         ref var entity = ref _fighters.GetEntity(index);
        //         ref var fighter = ref entity.Get<Fighter>();
        //
        //         if (fighter.SquadID == squadID) squad.Add(entity);
        //     }
        //
        //     return squad.Count > 0 ? squad : null;
        // }
    }
}