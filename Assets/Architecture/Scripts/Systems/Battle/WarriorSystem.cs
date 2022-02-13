using Components.Battle;
using Services;
using Leopotam.Ecs;

namespace Systems.Battle
{
    public sealed class WarriorSystem : IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<Fighter> _fighters;


        void IEcsRunSystem.Run()
        {
            if (_fighters.IsEmpty()) return;
            
            
            foreach (var index in _fighters)
            {
                ref var entity = ref _fighters.GetEntity(index);
                ref var fighter = ref entity.Get<Fighter>();


                if (fighter.State == FighterState.Dead) _gameTools.Events.WarriorDead(ref entity);
            }
        }
    }
}