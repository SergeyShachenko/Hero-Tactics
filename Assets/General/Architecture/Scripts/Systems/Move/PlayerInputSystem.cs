﻿using General.Components.Battle;
using General.Components.Events.Unity;
using General.Services;
using Leopotam.Ecs;

namespace General.Systems
{
    sealed class PlayerInputSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EventService _eventService;

        private EcsFilter<Battlefield, OnPointerClickEvent> _onPointerClicks;
        

        public void Run()
        {
            OnPointerClick();
        }

        
        private void OnPointerClick()
        {
            if (_onPointerClicks.IsEmpty()) return;


            foreach (var index in _onPointerClicks)
            {
                ref var entity = ref _onPointerClicks.GetEntity(index);
                var clickPosition = entity.Get<OnPointerClickEvent>().GameObject.transform.position;

                
                _eventService.MoveHeroTo(clickPosition);
            }
        }
    }
}