using Components;
using Components.Events.Game;
using Leopotam.Ecs;

namespace Services.Events
{
    public sealed class EventsService : GameToolServiceBase
    {
        public readonly SpawnEventsService Spawn;
        public readonly MoveEventsService Move;
        public readonly FighterEventsService Fighter;
        public readonly BattleEventsService Battle;

        public EventsService(EcsWorld world, GameTools gameTools) : base(world, gameTools)
        {
            Spawn = new SpawnEventsService(world, gameTools);
            Move = new MoveEventsService(world, gameTools);
            Fighter = new FighterEventsService(world, gameTools);
            Battle = new BattleEventsService(world, gameTools);
        }
        

        public void ChangedGameState(GameState gameState)
        {
            World.NewEntity().Get<ChangedGameStateEvent>() = new ChangedGameStateEvent
            {
                State = gameState
            };
        }
    }
}