using General.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks
{
    public class MonoEntity : MonoBehaviour
    {
        private EcsEntity _entity;
        private MonoLinkBase[] _monoLinks;


        public void Init(EcsWorld world)
        {
            _entity = world.NewEntity();
            _monoLinks = GetComponents<MonoLinkBase>();
            
            _entity.Get<GameObj>() = new GameObj {Value = gameObject};


            if (_monoLinks == null) return;
            
            
            foreach (var monoLink in _monoLinks)
            {
                monoLink.Link(ref _entity);
                if (monoLink is PhysicsLinkBase) return;
                Destroy(monoLink);
            }
        }

        public EcsEntity GetEntity()
        {
            return _entity;
        }
    }
}
