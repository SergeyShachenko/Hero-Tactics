using Leopotam.Ecs;
using Services.Events;
using Services.Fighters;

namespace Services
{
    public sealed class GameTools
    {
        public readonly GameplayService Gameplay;
        public readonly EventsService Events;
        public readonly FighterService Fighter;

        public GameTools(EcsWorld world)
        {
            Gameplay = new GameplayService(world, this);
            Events = new EventsService(world, this);
            Fighter = new FighterService(world, this);
        }
    }

    public abstract class GameToolServiceBase
    {
        protected readonly EcsWorld World;
        protected readonly GameTools GameTools;

        protected GameToolServiceBase(EcsWorld world, GameTools gameTools)
        {
            World = world;
            GameTools = gameTools;
        }
    }
}