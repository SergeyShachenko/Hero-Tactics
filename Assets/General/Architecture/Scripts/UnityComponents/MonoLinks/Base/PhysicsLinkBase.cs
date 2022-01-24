using Leopotam.Ecs;

namespace General.UnityComponents.MonoLinks
{
    public abstract class PhysicsLinkBase : MonoLinkBase
    {
        protected EcsEntity Entity;
        
        
        public override void Link(ref EcsEntity entity)
        {
            Entity = entity;
        }
    }
}