using General.Components.Events;
using General.UnityComponents.Data;
using General.Services;
using General.Systems.Battle;
using General.Systems.Spawn;
using General.UnityComponents.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace General 
{
    sealed class GameStartup : MonoBehaviour
    {
        [SerializeField] private LevelPreset _levelPreset = LevelPreset.Forest;
        [SerializeField] private bool _debug = true;
        
        [Header("Data")]
        [SerializeField] private GameData _gameData;
        [SerializeField] private GameServices _gameServices;

        private EcsWorld _world;
        private EcsSystems _generalSystems;

        private EventService _eventService;

        
        private void Start() 
        {
            _world = new EcsWorld();
            _generalSystems = new EcsSystems(_world);

            Debug(_debug);
            
            InitServices();
            InitGeneralSystems();
        }

        private void Update()
        {
            _generalSystems?.Run();
        }

        private void OnDestroy() 
        {
            if (_generalSystems != null) 
            {
                _generalSystems.Destroy();
                _generalSystems = null;
                
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
        
        
        private void Debug(bool isEnable)
        {
            if (!isEnable) return;

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_generalSystems);
#endif
        }
        
        private void InitServices()
        {
            _eventService = new EventService(_world);
        }

        private void InitGeneralSystems()
        {
            _generalSystems
                .Add(new InitMonoEntitySystem())
                .Add(new BattlefieldSystem())
                .Add(new SpawnWarriorSystem())
                //.Add(new WarriorSystem())
                //.Add(new PlayerInputSystem())
                .Add(new MoveWarriorSystem())
                .OneFrame<SpawnWarriorEvent>()
                .OneFrame<OnPointerClickEvent>()
                .Inject(_gameData)
                .Inject(_gameServices)
                .Inject(_eventService)
                .Init();
        }


        public enum LevelPreset
        {
            Forest, Desert, Winter
        }
    }
}