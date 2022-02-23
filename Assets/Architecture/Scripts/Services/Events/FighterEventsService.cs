using Components.Events.Battle;
using Leopotam.Ecs;

namespace Services.Events
{
    public sealed class FighterEventsService
    {
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;

        public FighterEventsService(EcsWorld world, GameTools gameTools)
        {
            _world = world;
            _gameTools = gameTools;
        }
        
        
        public void Dead(ref EcsEntity fighter)
        {
            _world.NewEntity().Get<DeadFighterEvent>() = new DeadFighterEvent
            {
                Fighter = fighter
            };
        }
    }
}