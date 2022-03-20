using Components.Events.Battle;
using Leopotam.Ecs;

namespace Services.Events
{
    public sealed class FighterEventsService : GameToolServiceBase
    {
        public FighterEventsService(EcsWorld world, GameTools gameTools) : base(world, gameTools) {}


        public void Dead(ref EcsEntity fighter)
        {
            World.NewEntity().Get<DeadFighterEvent>() = new DeadFighterEvent
            {
                Fighter = fighter
            };
        }
    }
}