using System.Collections.Generic;
using Components.Battle;
using Components.Events.Battle;
using Components.Events.Move;
using Leopotam.Ecs;
using UnityEngine;

namespace Services.Events
{
    public sealed class MoveEventsService
    {
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;

        public MoveEventsService(EcsWorld world, GameTools gameTools)
        {
            _world = world;
            _gameTools = gameTools;
        }
        
        
        public void HeroesTo(List<EcsEntity> heroes, Vector3 targetPosition)
        {
            _world.NewEntity().Get<MoveHeroesToEvent>() = new MoveHeroesToEvent
            {
                Heroes = heroes,
                TargetPosition = targetPosition
            };
        }
        
        public void HeroTo(EcsEntity hero, Vector3 targetPosition)
        {
            _world.NewEntity().Get<MoveHeroToEvent>() = new MoveHeroToEvent
            {
                Entity = hero,
                TargetPosition = targetPosition
            };
        }
        
        public void EndPlacementFighterSquad(BattleSide battleSide, List<EcsEntity> fighters, EcsEntity place)
        {
            _world.NewEntity().Get<EndPlacementFighterSquadEvent>() = new EndPlacementFighterSquadEvent
            {
                BattleSide = battleSide,
                Fighters = fighters,
                Place = place
            };
        }
    }
}