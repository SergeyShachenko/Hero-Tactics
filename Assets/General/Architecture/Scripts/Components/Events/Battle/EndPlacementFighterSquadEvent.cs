using System.Collections.Generic;
using General.Components.Battle;
using Leopotam.Ecs;

namespace General.Components.Events.Battle
{
    internal struct EndPlacementFighterSquadEvent
    {
        public BattleSide BattleSide;
        public List<EcsEntity> Fighters;
        public EcsEntity PlaceEntity;
    }
}