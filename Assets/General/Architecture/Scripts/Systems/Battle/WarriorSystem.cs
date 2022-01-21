using General.Components.Battle;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    sealed class WarriorSystem : IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter<Fighter, Warrior> _warriors;


        void IEcsRunSystem.Run()
        {
            if (_warriors.IsEmpty()) return;
            
            
            foreach (var index in _warriors)
            {
                ref EcsEntity entity = ref _warriors.GetEntity(index);
                entity.Get<Fighter>().State = FighterState.Alive;
            }
        }
    }
}