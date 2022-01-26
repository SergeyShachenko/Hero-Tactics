using Leopotam.Ecs;

namespace General.UnityComponents.MonoLinks
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