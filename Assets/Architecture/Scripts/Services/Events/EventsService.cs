using Systems.Game;
using Components;
using Components.Events.Battle;
using Components.Events.Game;
using Leopotam.Ecs;

namespace Services.Events
{
    public sealed class EventsService
    {
        public readonly SpawnEventsService Spawn;
        public readonly MoveEventsService Move;
        public readonly FighterEventsService Fighter;
        public readonly BattleEventsService Battle;
        
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;
        
        public EventsService(EcsWorld world, GameTools gameTools)
        {
            Spawn = new SpawnEventsService(world, gameTools);
            Move = new MoveEventsService(world, gameTools);
            Fighter = new FighterEventsService(world, gameTools);
            Battle = new BattleEventsService(world, gameTools);
            
            _world = world;
            _gameTools = gameTools;
        }
        

        public void ChangedGameState(GameState gameState)
        {
            _world.NewEntity().Get<ChangedGameStateEvent>() = new ChangedGameStateEvent
            {
                State = gameState
            };
        }
    }
}