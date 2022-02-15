using Leopotam.Ecs;

namespace Components.Events.Battle
{
    internal struct EndBattleEvent
    {
        public int AssaultSquadID, DefenceSquadID;
        public EcsEntity Place;
    }
}