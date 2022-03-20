using System.Collections.Generic;
using Components.Battle;
using Leopotam.Ecs;

namespace Components.Events.Battle
{
    internal struct EndPlacementFighterSquadEvent
    {
        public BattleSide BattleSide;
        public HashSet<EcsEntity> Fighters;
        public EcsEntity Place;
    }
}