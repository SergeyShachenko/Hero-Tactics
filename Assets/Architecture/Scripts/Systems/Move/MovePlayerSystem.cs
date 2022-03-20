using System.Collections.Generic;
using Components;
using Components.Battle;
using Components.Events.Move;
using Components.Events.Physics;
using Components.Tags;
using Services;
using UnityComponents.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Move
{
    public sealed class MovePlayerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        private readonly GameSettings _gameSettings;
        
        private readonly EcsFilter<MovePlayersToEvent> _moveHeroesToEvents;
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggersEnterEvents;

        private Vector3 _currentPosition, _nextPosition;
        private List<Vector3> _availablePositions, _placementPositions;
        private HashSet<EcsEntity> _heroesForMove, _heroesCompleteMove;
        private float _walkOffset, _zOffset;


        void IEcsInitSystem.Init()
        {
            _walkOffset = _gameSettings.WalkOffset;
            _zOffset = _gameSettings.ZOffset;
            
            _availablePositions = new List<Vector3>();
            _placementPositions = new List<Vector3>();

            _heroesForMove = new HashSet<EcsEntity>();
            _heroesCompleteMove = new HashSet<EcsEntity>();
        }

        void IEcsRunSystem.Run()
        {
            UpdateHeroesForMove();
            UpdatePlacementPositions(canUpdate: _heroesForMove.Count > 0);
            UpdateAvailablePositions();
            MoveHeroes(canMove: _availablePositions.Contains(_nextPosition));
            
            ClearHeroesForMove(canClear:
                _heroesForMove.Count == _heroesCompleteMove.Count && _heroesForMove.Count > 0);
        }
        

        private void UpdateHeroesForMove()
        {
            if (_moveHeroesToEvents.IsEmpty()) return;
            
            
            ref var moveHeroesToEvent = ref _moveHeroesToEvents.Get1(0);
            var heroes = moveHeroesToEvent.Players;

            if (moveHeroesToEvent.TargetPosition == _currentPosition) return;


            _nextPosition = moveHeroesToEvent.TargetPosition;
            _heroesForMove.UnionWith(heroes);
        }

        private void UpdatePlacementPositions(bool canUpdate)
        {
            if (canUpdate == false || _moveHeroesToEvents.IsEmpty()) return;


            var targetPosition = new Vector3();
            
            foreach (var index in _moveHeroesToEvents)
            {
                targetPosition = _moveHeroesToEvents.Get1(index).TargetPosition;
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

        private void UpdateAvailablePositions()
        {
            foreach (var index in _onTriggersEnterEvents)
            {
                ref var onTriggerEnterEvent = ref _onTriggersEnterEvents.Get1(index);

                if (onTriggerEnterEvent.Sender.Has<Battlefield>() == false) continue;
                if (onTriggerEnterEvent.Visitor.Has<PlayerTag>() == false) continue;
                
                
                _availablePositions.Clear();
                
                ref var entitySender = ref onTriggerEnterEvent.Sender;
                var approvedWays = entitySender.Get<Battlefield>().Ways;
                
                _currentPosition = entitySender.Get<GameObj>().Value.transform.position;
                _availablePositions.Add(_currentPosition);

                if (approvedWays.Count == 0) continue;
                
                
                foreach (var approvedWay in approvedWays) 
                    _availablePositions.Add(approvedWay.position);
            }
        }

        private void MoveHeroes(bool canMove)
        {
            if (canMove == false || _heroesForMove.Count == 0) return;
            

            var placementPositionsIndex = 0;
            
            foreach (var hero in _heroesForMove)
            {
                if (hero.Get<Movable>().IsMovable == false)
                {
                    _heroesCompleteMove.Add(hero);
                    hero.Get<Movable>().State = MovableState.Idle;
                    continue;
                }
                
                var targetPosition = _placementPositions[placementPositionsIndex++];
                if (placementPositionsIndex >= _placementPositions.Count) placementPositionsIndex = 0;

                var heroIsMoving = _gameTools.Gameplay.MoveEntityTo(hero, targetPosition, 0.5f);
                
                hero.Get<ModelParent>().GameObject.transform.LookAt(targetPosition);
                hero.Get<Movable>().State = heroIsMoving ? MovableState.Run : MovableState.Idle;

                if (heroIsMoving == false) 
                    _heroesCompleteMove.Add(hero);
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