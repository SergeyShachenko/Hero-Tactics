using General.Components.Battle;
using General.Components.Events.Unity;
using General.Services;
using Leopotam.Ecs;

namespace General.Systems
{
    public sealed class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly Tools _tools;

        private readonly EcsFilter<Battlefield, OnPointerClickEvent> _onPointerClicks;


        void IEcsRunSystem.Run()
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
                
                _tools.Events.MoveHeroTo(clickPosition);
            }
        }
    }
}