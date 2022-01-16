using General.Data;
using General.MonoLinks;
using General.Services;
using General.Systems.Battle;
using Leopotam.Ecs;
using UnityEngine;

namespace General 
{
    sealed class GameLoader : MonoBehaviour
    {
        [SerializeField] private string _presetName = "Forest";
        [SerializeField] private bool _debug = true;
        
        [Header("Data")]
        [SerializeField] private GameData _gameData;
        [SerializeField] private GameService _gameService;

        private EcsWorld _world;
        private EcsSystems _generalSystems;

        private EventService _eventService;

        
        private void Start() 
        {
            _world = new EcsWorld();
            _generalSystems = new EcsSystems(_world);

            InitServices();
            InitMonoEntitys(_world);
            Debug(_debug);
            
            _generalSystems
                .Add(new BattlefieldSystem())
                .Add(new SpawnWarriorSystem())
                .Inject(_gameData)
                .Inject(_gameService)
                .Inject(_eventService)
                .Init();
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
            
            _world.Destroy();
            _world = null;
        }
        
        
        private void InitServices()
        {
            _eventService = new EventService(_world);
        }
        
        private void InitMonoEntitys(EcsWorld world)
        {
            var monoEntitys = FindObjectsOfType<MonoEntity>();


            foreach (var monoEntity in monoEntitys)
            {
                monoEntity.Init(world);
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
    }
}