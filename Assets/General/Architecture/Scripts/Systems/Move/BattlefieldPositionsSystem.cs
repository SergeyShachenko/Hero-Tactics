using General.Components.Events;
using Leopotam.Ecs;

namespace General.Systems.Move
{
    public sealed class BattlefieldPositionsSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;

        private readonly EcsFilter<BattlefieldChangeStateEvent> _battlefieldChangedStateEvents;
        
        
        void IEcsRunSystem.Run()
        {
            if (_battlefieldChangedStateEvents.IsEmpty()) return;


            foreach (var index in _battlefieldChangedStateEvents)
            {
                ref var battlefield = 
                    ref _battlefieldChangedStateEvents.GetEntity(index).Get<BattlefieldChangeStateEvent>().Battlefield;
            }
        }
    }
}