using System.Collections.Generic;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace UnityComponents.MonoLinks
{
    public class MonoEntity : MonoBehaviour
    {
        private EcsEntity _entity;
        private MonoLinkBase[] _monoLinks;


        public void Init(EcsWorld world)
        {
            _entity = world.NewEntity();
            _entity.Get<GameObj>() = new GameObj {Value = gameObject};
            _monoLinks = GetComponents<MonoLinkBase>();


            if (_monoLinks == null) return;
            
            
            foreach (var monoLink in _monoLinks) monoLink.Link(ref _entity);
            foreach (var monoLink in _monoLinks)
            {
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
