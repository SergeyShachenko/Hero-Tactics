using General.Components.Battle;
using General.Components.Events.Unity;
using General.Services;
using Leopotam.Ecs;

namespace General.Systems.Move
{
    public sealed class PlayerInputSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly Tools _tools;

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

                if (clickEvent.EntitySender.Has<Battlefield>() == false) continue;
                
                
                _tools.Events.MoveHeroTo(clickEvent.Sender.transform.position);
            }
        }
    }
}