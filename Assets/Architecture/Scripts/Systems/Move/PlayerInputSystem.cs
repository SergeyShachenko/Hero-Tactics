using System.Collections.Generic;
using Components;
using Components.Battle;
using Components.Events.Physics;
using Components.Tags;
using Services;
using Leopotam.Ecs;

namespace Systems.Move
{
    public sealed class PlayerInputSystem : IEcsRunSystem
    {
        private readonly GameTools _gameTools;

        private readonly EcsFilter<OnPointerClickEvent> _onPointerClickEvents;
        private readonly EcsFilter<Fighter, Movable, PlayerTag> _movablePlayerFilter;


        void IEcsRunSystem.Run()
        {
            ClickOnBattlefield();
        }

        
        private void ClickOnBattlefield()
        {
            foreach (var index in _onPointerClickEvents)
            {
                ref var onPointerClickEvent = ref _onPointerClickEvents.Get1(index);

                if (onPointerClickEvent.Sender.Has<Battlefield>() == false || _movablePlayerFilter.IsEmpty()) continue;

                
                var heroes = new HashSet<EcsEntity>();
                
                foreach (var indexHero in _movablePlayerFilter)
                {
                    ref var entity = ref _movablePlayerFilter.GetEntity(indexHero);

                    if (entity.Get<Movable>().IsMovable) 
                        heroes.Add(entity);
                }

                _gameTools.Events.Move.PlayersTo(heroes, onPointerClickEvent.SenderGameObj.transform.position);
            }
        }
    }
}