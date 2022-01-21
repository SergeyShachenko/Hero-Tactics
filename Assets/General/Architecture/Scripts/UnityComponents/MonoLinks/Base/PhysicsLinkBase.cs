using Leopotam.Ecs;

namespace General.UnityComponents.MonoLinks
{
    public abstract class PhysicsLinkBase : MonoLinkBase
    {
        protected EcsEntity _entity;
        
        
        public override void Link(ref EcsEntity entity)
        {
            _entity = entity;
        }
    }
}