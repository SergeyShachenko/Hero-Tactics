using General.Components;
using General.Components.Battle;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    public sealed class WarriorSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Fighter, Warrior> _warriors;


        void IEcsRunSystem.Run()
        {
            if (_warriors.IsEmpty()) return;
            
            
            foreach (var index in _warriors)
            {
                ref var entity = ref _warriors.GetEntity(index);
                ref var fighter = ref entity.Get<Fighter>();

                if (fighter.State == FighterState.Disabled)
                {
                    fighter.State = FighterState.Alive;
                    entity.Get<Movable>().State = MovableState.Stand;
                }
            }
        }
    }
}