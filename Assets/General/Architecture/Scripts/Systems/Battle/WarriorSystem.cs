using General.Components.Battle;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    public sealed class WarriorSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;

        private readonly EcsFilter<Fighter, Warrior> _warriors;


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