using System.Collections.Generic;
using General.Components.Battle;
using General.Components.Events;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems.Battle
{
    sealed class MoveWarriorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter<OnPointerClickEvent> _pointerClick;
        private EcsFilter<Fighter, Warrior> _warriors;

        private List<EcsEntity> _heroes = new List<EcsEntity>();


        void IEcsInitSystem.Init()
        {
              
        }
        
        void IEcsRunSystem.Run()
        {
            if (_pointerClick.IsEmpty()) return;
            
            
            foreach (var index in _warriors)
            {
                ref EcsEntity entity = ref _warriors.GetEntity(index);
            
                if (entity.Get<Fighter>().BattleSide == BattleSide.Hero)
                {
                    _heroes.Add(entity);
                }
            }

            foreach (var index in _pointerClick)
            {
                ref EcsEntity entity = ref _pointerClick.GetEntity(index);
                var pointerClick = entity.Get<OnPointerClickEvent>();
                
                Debug.Log(pointerClick.GameObject);
            }
            
            
            _heroes.Clear();
        }
    }
}