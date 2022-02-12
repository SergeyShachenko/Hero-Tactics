using General.Components;
using General.Components.Battle;
using General.Components.Events.Battle;
using General.Components.Physics;
using Leopotam.Ecs;

namespace General.Systems.Battle
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
                
                if (entity.Has<RigidB>())
                {
                    var entityPosition = entity.Get<GameObj>().Value.transform.localPosition;
                    var entityRotation = entity.Get<GameObj>().Value.transform.rotation.eulerAngles;
                    
                    entity.Get<RigidB>().Rigidbody.isKinematic = false;

                    if (entityRotation.y < 180)
                    {
                        entity.Get<RigidB>().Rigidbody.velocity += entityPosition ;
                    }
                    else
                    {
                        entity.Get<RigidB>().Rigidbody.velocity -= entityPosition ;
                    }
                }
            }
        }
    }
}