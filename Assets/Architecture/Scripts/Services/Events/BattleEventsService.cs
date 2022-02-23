using Components.Events.Battle;
using Leopotam.Ecs;

namespace Services.Events
{
    public sealed class BattleEventsService
    {
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;

        public BattleEventsService(EcsWorld world, GameTools gameTools)
        {
            _world = world;
            _gameTools = gameTools;
        }
        
        
        public void ChangedStateBattlefield(ref EcsEntity battlefield)
        {
            _world.NewEntity().Get<ChangedBattlefieldStateEvent>() = new ChangedBattlefieldStateEvent
            {
                Battlefield = battlefield
            };
        }

        public void End(int assaultSquadID, int defenceSquadID, ref EcsEntity place)
        {
            _world.NewEntity().Get<EndBattleEvent>() = new EndBattleEvent
            {
                AssaultSquadID = assaultSquadID,
                DefenceSquadID = defenceSquadID,
                Place = place
            };
        }
    }
}