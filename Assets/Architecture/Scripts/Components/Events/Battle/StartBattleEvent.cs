using Leopotam.Ecs;

namespace Components.Events.Battle
{
    internal struct StartBattleEvent
    {
        public int AssaultSquadID, DefenceSquadID;
        public EcsEntity Place;
    }
}