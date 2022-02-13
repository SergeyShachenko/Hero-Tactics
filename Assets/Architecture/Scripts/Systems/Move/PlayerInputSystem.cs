using System.Collections.Generic;
using Components;
using Components.Battle;
using Components.Events.Unity;
using Components.Tags;
using Services;
using Leopotam.Ecs;

namespace Systems.Move
{
    public sealed class PlayerInputSystem : IEcsRunSystem
    {
        private readonly GameTools _gameTools;

        private readonly EcsFilter<Movable, PlayerTag> _movableHeroes;
        private readonly EcsFilter<OnPointerClickEvent> _onPointerClickEvents;


        void IEcsRunSystem.Run()
        {
            ClickOnBattlefield();
        }

        
        private void ClickOnBattlefield()
        {
            if (_onPointerClickEvents.IsEmpty()) return;


            foreach (var index in _onPointerClickEvents)
            {
                ref var clickEvent = ref _onPointerClickEvents.GetEntity(index).Get<OnPointerClickEvent>();

                if (clickEvent.EntitySender.Has<Battlefield>() == false || _movableHeroes.IsEmpty()) continue;

                
                var heroes = new List<EcsEntity>();
                
                foreach (var indexHero in _movableHeroes)
                {
                    ref var entity = ref _movableHeroes.GetEntity(indexHero);

                    if (entity.Get<Movable>().IsMovable) heroes.Add(entity);
                }

                _gameTools.Events.MoveEntitysTo(heroes, clickEvent.Sender.transform.position);
            }
        }
    }
}