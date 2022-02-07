using System.Collections.Generic;
using General.Components;
using General.Components.Battle;
using General.Components.Events;
using General.Components.Events.Unity;
using General.Components.Tags;
using General.Services;
using General.UnityComponents.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems.Move
{
    public sealed class MoveHeroSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        private readonly GameSettings _gameSettings;
        
        private readonly EcsFilter<MoveHeroesToEvent> _moveHeroesEvents;
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggerEnterEvents;

        private Vector3 _currentPosition, _nextPosition;
        private List<Vector3> _availablePositions, _placementPositions;
        private List<EcsEntity> _heroesForMove, _heroesCompleteMove;
        private float _walkOffset, _zOffset;


        void IEcsInitSystem.Init()
        {
            _nextPosition = new Vector3();
            _currentPosition = new Vector3();
            _availablePositions = new List<Vector3>();
            _placementPositions = new List<Vector3>();
            
            _heroesForMove = new List<EcsEntity>();
            _heroesCompleteMove = new List<EcsEntity>();
            
            _walkOffset = _gameSettings.WalkOffset;
            _zOffset = _gameSettings.ZOffset;
        }

        void IEcsRunSystem.Run()
        {
            UpdateHeroesForMove(canUpdate:_moveHeroesEvents.IsEmpty() == false);
            UpdatePlacementPositions(canUpdate:_heroesForMove.Count > 0);
            UpdateAvailablePositions(canUpdate:_onTriggerEnterEvents.IsEmpty() == false);

            MoveHeroes(canMove:_availablePositions.Contains(_nextPosition));
            ClearHeroesForMove(canClear:_heroesForMove.Count == _heroesCompleteMove.Count && _heroesForMove.Count > 0);
        }
        

        private void UpdateHeroesForMove(bool canUpdate)
        {
            if (canUpdate == false) return;
            
            
            var heroes = new List<EcsEntity>();
            var targetPosition = new Vector3();
            
            foreach (var index in _moveHeroesEvents)
            {
                ref var moveEvent = ref _moveHeroesEvents.GetEntity(index).Get<MoveHeroesToEvent>();
                heroes = moveEvent.Heroes;
                targetPosition = moveEvent.TargetPosition;
            }

            if (targetPosition == _currentPosition) return;


            _nextPosition = targetPosition;
            
            foreach (var hero in heroes)
            {
                if (_heroesForMove.Contains(hero) == false) _heroesForMove.Add(hero);
            }
        }

        private void UpdatePlacementPositions(bool canUpdate)
        {
            if (canUpdate == false || _moveHeroesEvents.IsEmpty()) return;


            var targetPosition = new Vector3();
            
            foreach (var index in _moveHeroesEvents)
            {
                targetPosition = _moveHeroesEvents.GetEntity(index).Get<MoveHeroesToEvent>().TargetPosition;
            }
            
            if (_availablePositions.Contains(targetPosition) == false || targetPosition == _currentPosition) return;
            
            
            _placementPositions = new List<Vector3>
            {
                new Vector3(targetPosition.x, targetPosition.y, targetPosition.z + _walkOffset - _zOffset),
                new Vector3(targetPosition.x + _walkOffset, targetPosition.y, targetPosition.z - _zOffset),
                new Vector3(targetPosition.x - _walkOffset, targetPosition.y, targetPosition.z - _zOffset),
            };
                
            //Debug.Log("Move heroes to " + _nextPosition);
        }

        private void UpdateAvailablePositions(bool canUpdate)
        {
            if (canUpdate == false) return;

            
            _availablePositions.Clear();

            foreach (var index in _onTriggerEnterEvents)
            {
                ref var enterEvent = ref _onTriggerEnterEvents.GetEntity(index).Get<OnTriggerEnterEvent>();

                if (enterEvent.SenderEntity.Has<Battlefield>() == false) continue;
                if (enterEvent.VisitorEntity.Has<PlayerTag>() == false) continue;
                
                
                ref var entitySender = ref enterEvent.SenderEntity;
                var approvedWays = entitySender.Get<Battlefield>().AvailablePositions;
                _currentPosition = entitySender.Get<GameObj>().Value.transform.position;
                _availablePositions.Add(_currentPosition);

                if (approvedWays.Count == 0) continue;
                
                
                foreach (var approvedWay in approvedWays) _availablePositions.Add(approvedWay.position);
            }
        }

        private void MoveHeroes(bool canMove)
        {
            if (canMove == false || _heroesForMove.Count == 0) return;
            

            var placementPositionsIndex = 0;
            
            foreach (var hero in _heroesForMove)
            {
                var targetPosition = _placementPositions[placementPositionsIndex++];
                if (placementPositionsIndex >= _placementPositions.Count) placementPositionsIndex = 0;
                
                var heroOnTheMove = _gameTools.Gameplay.MoveEntityTo(hero, targetPosition, 0.5f);
                hero.Get<GameObj>().Value.transform.LookAt(targetPosition);
                hero.Get<Movable>().State = heroOnTheMove ? MovableState.Run : MovableState.Stand;

                
                if (heroOnTheMove == false || hero.Get<Movable>().IsMovable == false)
                {
                    if (_heroesCompleteMove.Contains(hero) == false) _heroesCompleteMove.Add(hero);
                }
            }
        }
        
        private void ClearHeroesForMove(bool canClear)
        {
            if (canClear == false) return;
            
            
            _heroesForMove.Clear();
            _heroesCompleteMove.Clear();
        }
    }
}