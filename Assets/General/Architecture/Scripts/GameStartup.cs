using General.Components.Events;
using General.UnityComponents.Data;
using General.Services;
using General.Systems;
using General.Systems.Battle;
using General.Systems.Spawn;
using General.UnityComponents.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace General 
{
    sealed class GameStartup : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LevelPreset _levelPreset = LevelPreset.Forest;
        [SerializeField] private bool _debug = true;
        
        [Header("Data")]
        [SerializeField] private GameData _gameData;
        [SerializeField] private GameServices _gameServices;

        private EcsWorld _world;
        private EcsSystems _systems, _fixedUpdateSystems;

        private EventService _eventService;

        
        private void Start() 
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world, "Fixed Update Systems");
            
            Debug(_debug);
            
            InitServices();
            InitSystems();
            InitFixedUpdateSystems();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
        }

        private void OnDestroy() 
        {
            if (_systems != null) 
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_fixedUpdateSystems != null)
            {
                _fixedUpdateSystems.Destroy();
                _fixedUpdateSystems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
        
        
        private void InitServices()
        {
            _eventService = new EventService(_world);
        }

        private void InitSystems()
        {
            AddMainSystems();
            AddBattleSystems();
            
            _systems
                .Inject(_gameData)
                .Inject(_gameServices)
                .Inject(_eventService)
                .Init();
        }

        private void InitFixedUpdateSystems()
        {
            AddMoveSystems();
            
            _fixedUpdateSystems
                .OneFrame<OnPointerClickEvent>()
                .Inject(_eventService)
                .Init();
        }
        
        private void AddMainSystems()
        {
            EcsSystems mainSystems = new EcsSystems(_world, "Main Systems");

            mainSystems
                .Add(new InitMonoEntitySystem());
                
            _systems.Add(mainSystems);
        }

        private void AddBattleSystems()
        {
            EcsSystems battleSystems = new EcsSystems(_world, "Battle Systems");

            battleSystems
                .Add(new BattlefieldSystem())
                .Add(new SpawnWarriorSystem())
                .Add(new WarriorSystem())
                .OneFrame<SpawnWarriorEvent>();

            _systems.Add(battleSystems);
        }
        
        private void AddMoveSystems()
        {
            EcsSystems moveSystems = new EcsSystems(_world, "Move Systems");

            moveSystems
                .Add(new PlayerInputSystem())
                .Add(new MoveHeroSystem())
                .OneFrame<MoveHeroToPositionEvent>();

            _fixedUpdateSystems.Add(moveSystems);
        }

        private void Debug(bool isEnable)
        {
            if (!isEnable) return;

            
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_fixedUpdateSystems);
#endif
        }
        
        
        public enum LevelPreset
        {
            Forest, Desert, Winter
        }
    }
}