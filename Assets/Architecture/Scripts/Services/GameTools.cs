using Leopotam.Ecs;

namespace Services
{
    public sealed class GameTools
    {
        public readonly GameplayService Gameplay;
        public readonly EventService Events;
        
        public GameTools(EcsWorld world)
        {
            Gameplay = new GameplayService(world);
            Events = new EventService(world);
        }
    }
}