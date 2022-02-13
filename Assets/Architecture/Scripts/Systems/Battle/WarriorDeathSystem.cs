using Components;
using Components.Battle;
using Components.Events.Battle;
using Components.Physics;
using Leopotam.Ecs;

namespace Systems.Battle
{
    public sealed class WarriorDeathSystem : IEcsRunSystem
    {
        private readonly EcsFilter<WarriorDeadEvent> _warriorDeadEvents;
        
        
        void IEcsRunSystem.Run()
        {
            if (_warriorDeadEvents.IsEmpty()) return;


            foreach (var index in _warriorDeadEvents)
            {
                ref var entity = ref _warriorDeadEvents.GetEntity(index).Get<WarriorDeadEvent>().Entity;
                
                
                entity.Get<Fighter>().State = FighterState.Disabled;
                
                if (entity.Has<Movable>()) entity.Get<Movable>().IsMovable = false;
                
                if (entity.Has<RigidbodyComponent>())
                {
                    var entityPosition = entity.Get<GameObj>().Value.transform.localPosition;
                    var entityRotation = entity.Get<GameObj>().Value.transform.rotation.eulerAngles;
                    
                    entity.Get<RigidbodyComponent>().Rigidbody.isKinematic = false;

                    if (entityRotation.y < 180)
                    {
                        entity.Get<RigidbodyComponent>().Rigidbody.velocity += entityPosition * 2f;
                    }
                    else
                    {
                        entity.Get<RigidbodyComponent>().Rigidbody.velocity -= entityPosition * 2f;
                    }
                }
            }
        }
    }
}