using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks.Base
{
    public class MonoEntity : MonoBehaviour
    {
        private EcsWorld _world;
        private MonoLinkBase[] _monoLinks;


        public void Init(EcsWorld world)
        {
            _world = world;
            
            var entity = _world.NewEntity();
            _monoLinks = GetComponents<MonoLinkBase>();
        

            foreach (var monoLink in _monoLinks)
            {
                monoLink.Link(ref entity);
            }
        }
        
        public MonoLink<T> Get<T>() where T : struct
        {
            foreach (var monoLink in _monoLinks)
            {
                if (monoLink is MonoLink<T> targetMonoLink) return targetMonoLink;
            }

            return null;
        }
    }
}
