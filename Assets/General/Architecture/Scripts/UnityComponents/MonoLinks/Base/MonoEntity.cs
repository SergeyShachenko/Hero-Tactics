using System.Collections.Generic;
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

        // public List<T> GetEntityComponents<T>() where T : struct 
        // {
        //     var components = new List<T>();
        //     var countComponents = _entity.GetComponentsCount();
        //
        //     for (var i = 0; i < _entity.GetComponentsCount(); i++)
        //     {
        //         if (_entity.1())
        //         {
        //             
        //         }
        //     }
        //     
        //     
        //     while (_entity.Has<T>())
        //     {
        //         components.Add(_entity.Get<T>());
        //     }
        //
        //     return components;
        // }
    }
}
