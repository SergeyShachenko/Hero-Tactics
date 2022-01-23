using General.Components;
using General.Components.Events;
using General.Components.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems
{
    sealed class MoveHeroSystem : IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter<MoveHeroToPositionEvent> _moveHeroTo;
        private EcsFilter<GameObj, Movable, PlayerTag> _movablePlayers;

        private Vector3 _targetPosition;
        private bool _wasHeroMoveEvent;


        void IEcsRunSystem.Run()
        {
            SetTargetPosition();
            MoveHeroTo(_targetPosition);
        }

        private void SetTargetPosition()
        {
            if (_moveHeroTo.IsEmpty()) return;
            

            foreach (var index in _moveHeroTo)
            {
                ref var entity = ref _moveHeroTo.GetEntity(index);
                _targetPosition = entity.Get<MoveHeroToPositionEvent>().Position;


                _wasHeroMoveEvent = true;
                
                Debug.Log("Move heroes to " + _targetPosition);
            }
        }

        private void MoveHeroTo(Vector3 targetPosition)
        {
            if (!_wasHeroMoveEvent || _movablePlayers.IsEmpty()) return;
            
            
            foreach (var index in _movablePlayers)
            {
                ref var entity = ref _movablePlayers.GetEntity(index);
                var gameObject = entity.Get<GameObj>().Value;
                var speed = entity.Get<Movable>().Speed;
                
                var heroPosition = gameObject.transform.position;
                var targetDirection = new Vector3(0, 0, _targetPosition.z - heroPosition.z);
                
                
                gameObject.transform.position += targetDirection * (speed * Time.deltaTime);
            }
        }
    }
}