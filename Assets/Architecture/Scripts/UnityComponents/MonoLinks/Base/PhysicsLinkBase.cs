using Leopotam.Ecs;

namespace UnityComponents.MonoLinks.Base
{
    public abstract class PhysicsLinkBase : MonoLinkBase
    {
        protected EcsWorld World;
        
        
        public override void Link(ref EcsEntity entity)
        {
            World = entity.GetInternalWorld();
        }
    }
}