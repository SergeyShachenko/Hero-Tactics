using Leopotam.Ecs;

namespace General.Services
{
    public sealed class Tools
    {
        public readonly GameplayService Gameplay;
        public readonly EventService Events;
        
        public Tools(EcsWorld world)
        {
            Gameplay = new GameplayService(world);
            Events = new EventService(world);
        }
    }
}