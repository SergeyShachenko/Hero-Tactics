using System.Collections.Generic;
using Components.Battle;
using Leopotam.Ecs;

namespace Components.Events.Battle
{
    internal struct EndPlacementFighterSquadEvent
    {
        public BattleSide BattleSide;
        public List<EcsEntity> Fighters;
        public EcsEntity Place;
    }
}