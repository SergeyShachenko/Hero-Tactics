using General.Components.Events;
using General.Services;
using General.UnityComponents;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems.Battle
{
    sealed class PlayerInputSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _world;
        private EventService _eventService;
        
        private PlayerInput _playerInput = new PlayerInput();

        
        void IEcsInitSystem.Init()
        {
            _playerInput.Player.Select.canceled += context => CreatePlayerSelectEvent();
            _playerInput.Enable();   
        }

        void IEcsDestroySystem.Destroy()
        {
            _playerInput.Disable();
        }


        private void CreatePlayerSelectEvent()
        {
            _world.NewEntity().Get<PlayerSelectEvent>() = new PlayerSelectEvent
            {
                Target = _playerInput.Player.SelectPosition.ReadValue<Vector2>()
            };
            
            //Debug.Log(_playerInput.Player.SelectPosition.ReadValue<Vector2>());
            //Debug.Log(_playerInput.Player.SelectObject.ReadValue<>());
        }
    }
}