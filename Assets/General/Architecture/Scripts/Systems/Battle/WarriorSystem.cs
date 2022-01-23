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
                ref var fighter = ref _warriors.GetEntity(index).Get<Fighter>();


                if (fighter.State == FighterState.Disabled)
                {
                    fighter.State = FighterState.Alive;   
                }
            }
        }
    }
}