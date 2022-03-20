using Components.Events.Battle;
using Leopotam.Ecs;

namespace Services.Events
{
    public sealed class BattleEventsService : GameToolServiceBase
    {
        public BattleEventsService(EcsWorld world, GameTools gameTools) : base(world, gameTools) {}


        public void ChangedStateBattlefield(ref EcsEntity battlefield)
        {
            World.NewEntity().Get<ChangedBattlefieldStateEvent>() = new ChangedBattlefieldStateEvent
            {
                Battlefield = battlefield
            };
        }

        public void End(int assaultSquadID, int defenceSquadID, ref EcsEntity place)
        {
            World.NewEntity().Get<EndBattleEvent>() = new EndBattleEvent
            {
                AssaultSquadID = assaultSquadID,
                DefenceSquadID = defenceSquadID,
                Place = place
            };
        }
    }
}