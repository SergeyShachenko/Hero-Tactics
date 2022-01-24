using System.Collections.Generic;
using General.Components;
using General.Components.Battle;
using General.Components.Events;
using General.Components.Events.Unity;
using General.Components.Tags;
using General.Services;
using General.UnityComponents.MonoLinks;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems
{
    public sealed class MoveHeroSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly Tools _tools;

        private readonly EcsFilter<MoveHeroToPositionEvent> _moveHeroTo;
        private readonly EcsFilter<GameObj, Movable, PlayerTag> _movablePlayers;
        private readonly EcsFilter<OnTriggerEnterEvent, PlayerTag> _playersTriggerEnter;

        private Vector3 _currentPosition, _nextPosition;
        private List<Vector3> _approvedPositions, _standPositions;
        private bool _heroesMove;


        void IEcsInitSystem.Init()
        {
            _approvedPositions = new List<Vector3>();
        }

        void IEcsRunSystem.Run()
        {
            GetTargetPosition(canGet: !_heroesMove);
            GetApprovedPositions();
            MoveHeroes(canMove: _approvedPositions.Contains(_nextPosition));
        }
        

        private void GetTargetPosition(bool canGet)
        {
            if (!canGet || _moveHeroTo.IsEmpty()) return;


            var newPosition = Vector3.zero;
            foreach (var index in _moveHeroTo)
            {
                newPosition = _moveHeroTo.GetEntity(index).Get<MoveHeroToPositionEvent>().Position;
            }
            
            
            if (_approvedPositions.Contains(newPosition) && newPosition != _currentPosition)
            {
                _nextPosition = newPosition;

                var standOffset = 0.9f;
                _standPositions = new List<Vector3>
                {
                    new Vector3(_nextPosition.x, _nextPosition.y, _nextPosition.z + standOffset),
                    new Vector3(_nextPosition.x + standOffset, _nextPosition.y, _nextPosition.z),
                    new Vector3(_nextPosition.x - standOffset, _nextPosition.y, _nextPosition.z),
                    new Vector3(_nextPosition.x, _nextPosition.y, _nextPosition.z - standOffset)
                };
                
                Debug.Log("Move heroes to " + _nextPosition);
            }
        }

        private void GetApprovedPositions()
        {
            if (_playersTriggerEnter.IsEmpty()) return;

            
            _approvedPositions.Clear();

            foreach (var index in _playersTriggerEnter)
            {
                ref var triggerEnter = ref _playersTriggerEnter.GetEntity(index).Get<OnTriggerEnterEvent>();
                var entity = triggerEnter.Collider.gameObject.GetComponent<MonoEntity>().GetEntity();
                
                
                if (!entity.Has<Battlefield>()) return;
                
                
                var approvedWays = entity.Get<Battlefield>().ApprovedWays;
                _currentPosition = entity.Get<GameObj>().Value.transform.position;
                _approvedPositions.Add(_currentPosition);
                
                
                if (approvedWays == null) return;
                
                
                foreach (var approvedWay in approvedWays) _approvedPositions.Add(approvedWay.position);
            }
        }

        private void MoveHeroes(bool canMove)
        {
            if (!canMove || _movablePlayers.IsEmpty()) return;
            

            var standPositionsIndex = 0;
            foreach (var index in _movablePlayers)
            {
                var targetPosition = _standPositions[standPositionsIndex++];
                if (standPositionsIndex >= _standPositions.Count) standPositionsIndex = 0;
                
                ref var entity = ref _movablePlayers.GetEntity(index);
                _heroesMove = _tools.Gameplay.MoveFighterTo(ref entity, targetPosition);
                
                if (!_heroesMove) Debug.Log("Heroes stay");
            }
        }
    }
}