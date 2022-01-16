using Leopotam.Ecs;
using UnityEngine;

namespace General.MonoLinks
{
    public class MonoEntity : MonoBehaviour
    {
        private EcsWorld _world;
        
        private MonoLinkBase[] _monoLinks;
        private EcsEntity _entity;


        public void Init(EcsWorld world)
        {
            _world = world;
            _entity = _world.NewEntity();
            _monoLinks = GetComponents<MonoLinkBase>();
        

            foreach (var monoLink in _monoLinks)
            {
                monoLink.Link(ref _entity);
            }
        }


        public EcsEntity GetEntity()
        {
            return _entity;
        }
    }
}
