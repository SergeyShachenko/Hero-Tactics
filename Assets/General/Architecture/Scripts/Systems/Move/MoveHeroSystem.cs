using System.Collections.Generic;
using General.Components;
using General.Components.Battle;
using General.Components.Events;
using General.Components.Events.Unity;
using General.Components.Tags;
using General.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems.Move
{
    public sealed class MoveHeroSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly Tools _tools;

        private readonly EcsFilter<StopMoveHeroSystemEvent> _stopSystemEvent;
        private readonly EcsFilter<MoveHeroToPositionEvent> _moveHeroEvents;
        private readonly EcsFilter<GameObj, Movable, PlayerTag> _movablePlayers;
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggerEnterEvents;

        private Vector3 _currentPosition, _nextPosition;
        private List<Vector3> _approvedWays, _standPositions;
        private bool _heroesMove;


        void IEcsInitSystem.Init()
        {
            _approvedWays = new List<Vector3>();
        }

        void IEcsRunSystem.Run()
        {
            if (!_stopSystemEvent.IsEmpty()) return;
            
            
            GetTargetPosition(canGet: !_heroesMove);
            GetApprovedWays();
            MoveHeroes(canMove:_approvedWays.Contains(_nextPosition));
        }
        

        private void GetTargetPosition(bool canGet)
        {
            if (!canGet || _moveHeroEvents.IsEmpty()) return;


            var newPosition = Vector3.zero;
            foreach (var index in _moveHeroEvents)
            {
                newPosition = _moveHeroEvents.GetEntity(index).Get<MoveHeroToPositionEvent>().Position;
            }
            
            if (_approvedWays.Contains(newPosition) == false || newPosition == _currentPosition) return;
            
            
            _nextPosition = newPosition;

            var standOffset = 0.9f;
            _standPositions = new List<Vector3>
            {
                new Vector3(_nextPosition.x, _nextPosition.y, _nextPosition.z + standOffset),
                new Vector3(_nextPosition.x + standOffset, _nextPosition.y, _nextPosition.z),
                new Vector3(_nextPosition.x - standOffset, _nextPosition.y, _nextPosition.z),
                new Vector3(_nextPosition.x, _nextPosition.y, _nextPosition.z - standOffset)
            };
                
            //Debug.Log("Move heroes to " + _nextPosition);
        }

        private void GetApprovedWays()
        {
            if (_onTriggerEnterEvents.IsEmpty()) return;

            
            _approvedWays.Clear();

            foreach (var index in _onTriggerEnterEvents)
            {
                ref var enterEvent = ref _onTriggerEnterEvents.GetEntity(index).Get<OnTriggerEnterEvent>();

                if (enterEvent.EntitySender.Has<Battlefield>() == false) continue;
                if (enterEvent.EntityVisitor.Has<PlayerTag>() == false) continue;
                
                
                ref var entitySender = ref enterEvent.EntitySender;
                var approvedWays = entitySender.Get<Battlefield>().ApprovedWays;
                _currentPosition = entitySender.Get<GameObj>().Value.transform.position;
                _approvedWays.Add(_currentPosition);

                if (approvedWays.Count == 0) continue;
                
                
                foreach (var approvedWay in approvedWays) _approvedWays.Add(approvedWay.position);
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
            }
        }
    }
}