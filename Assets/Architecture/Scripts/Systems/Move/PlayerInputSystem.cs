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

        private readonly EcsFilter<OnPointerClickEvent> _onPointerClicks;
        private readonly EcsFilter<Movable, PlayerTag> _movableHeroes;


        void IEcsRunSystem.Run()
        {
            ClickOnBattlefield();
        }

        
        private void ClickOnBattlefield()
        {
            if (_onPointerClicks.IsEmpty()) return;


            foreach (var index in _onPointerClicks)
            {
                ref var clickEvent = ref _onPointerClicks.GetEntity(index).Get<OnPointerClickEvent>();

                if (clickEvent.Sender.Has<Battlefield>() == false || _movableHeroes.IsEmpty()) continue;

                
                var heroes = new List<EcsEntity>();
                
                foreach (var indexHero in _movableHeroes)
                {
                    ref var entity = ref _movableHeroes.GetEntity(indexHero);

                    if (entity.Get<Movable>().IsMovable) heroes.Add(entity);
                }

                _gameTools.Events.Move.HeroesTo(heroes, clickEvent.GameObjSender.transform.position);
            }
        }
    }
}