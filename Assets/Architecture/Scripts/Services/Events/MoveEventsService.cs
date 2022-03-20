using System.Collections.Generic;
using Components.Battle;
using Components.Events.Battle;
using Components.Events.Move;
using Leopotam.Ecs;
using UnityEngine;

namespace Services.Events
{
    public sealed class MoveEventsService : GameToolServiceBase
    {
        public MoveEventsService(EcsWorld world, GameTools gameTools) : base(world, gameTools) {}


        public void PlayersTo(HashSet<EcsEntity> heroes, Vector3 targetPosition)
        {
            World.NewEntity().Get<MovePlayersToEvent>() = new MovePlayersToEvent
            {
                Players = heroes,
                TargetPosition = targetPosition
            };
        }
        
        public void PlayerTo(EcsEntity hero, Vector3 targetPosition)
        {
            World.NewEntity().Get<MovePlayerToEvent>() = new MovePlayerToEvent
            {
                Entity = hero,
                TargetPosition = targetPosition
            };
        }
        
        public void EndPlacementFighterSquad(BattleSide battleSide, HashSet<EcsEntity> fighters, EcsEntity place)
        {
            World.NewEntity().Get<EndPlacementFighterSquadEvent>() = new EndPlacementFighterSquadEvent
            {
                BattleSide = battleSide,
                Fighters = fighters,
                Place = place
            };
        }
    }
}