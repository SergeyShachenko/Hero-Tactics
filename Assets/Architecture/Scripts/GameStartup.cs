using Components.Events.Battle;
using Components.Events.Game;
using Components.Events.Move;
using Components.Events.Physics;
using UnityComponents.Data;
using Services;
using Systems.Battle;
using Systems.Game;
using Systems.Spawn;
using Systems.Move;
using Systems.UI;
using Components.Events.Spawn;
using Systems.Startup;
using UnityComponents.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace General 
{
    public sealed class GameStartup : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private GameData GameData;
        [SerializeField] private GameSettings GameSettings;
        [SerializeField] private GameServices GameServices;

        private EcsWorld _world;
        private EcsSystems _mainSystems, _gameplaySystems;
        private GameTools _gameTools;


        private void Start() 
        {
            _world = new EcsWorld();
            _mainSystems = new EcsSystems(_world, "Main Systems");
            _gameplaySystems = new EcsSystems(_world, "Gameplay Systems");
            
            Debug(isEnable: GameSettings.ECSDebug);
            
            InitServices();
            InitMainSystems();
            InitGameplaySystems();
        }

        private void Update()
        {
            _mainSystems?.Run();
        }

        private void FixedUpdate()
        {
            _gameplaySystems?.Run();
        }

        private void OnDestroy() 
        {
            if (_mainSystems != null) 
            {
                _mainSystems.Destroy();
                _mainSystems = null;
            }

            if (_gameplaySystems != null)
            {
                _gameplaySystems.Destroy();
                _gameplaySystems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
        
        
        private void InitServices()
        {
            _gameTools = new GameTools(_world);
        }

        private void InitMainSystems()
        {
            _mainSystems
                .Add(StartupSystems())
                .Add(SpawnSystems())

                .Inject(_gameTools)
                .Inject(GameServices)
                .Inject(GameSettings)
                .Inject(GameData)
                .Init();
        }

        private void InitGameplaySystems()
        {
            _gameplaySystems
                .Add(MoveSystems())
                .Add(BattleSystems())
                .Add(GameSystems())
                .Add(UISystems())

                .OneFrame<EndBattleEvent>()
                .OneFrame<DeadFighterEvent>()

                .OneFrame<ChangedBattlefieldStateEvent>()
                .OneFrame<ChangedGameStateEvent>()

                .OneFrame<OnPointerClickEvent>()
                .OneFrame<OnTriggerEnterEvent>()
                .OneFrame<OnTriggerExitEvent>()

                .Inject(_gameTools)
                .Inject(GameServices)
                .Inject(GameSettings)
                .Inject(GameData)
                .Init();
        }

        private EcsSystems StartupSystems()
        {
            var startupSystems = new EcsSystems(_world, "Startup Systems");

            return startupSystems
                .Add(new MonoEntitySystem())
                .Add(new CameraSystem())
                .Add(new DrawWaySystem());
        }

        private EcsSystems SpawnSystems()
        {
            var spawnSystems = new EcsSystems(_world, "Spawn Systems");

            return spawnSystems
                .Add(new SpawnWarriorSystem())
            
                .OneFrame<SpawnWarriorEvent>();
        }

        private EcsSystems MoveSystems()
        {
            var moveSystems = new EcsSystems(_world, "Move Systems");

            return moveSystems
                .Add(new PlayerInputSystem())
                .Add(new MovePlayerSystem())
                
                .OneFrame<MovePlayersToEvent>();
        }
        
        private EcsSystems BattleSystems()
        {
            var battleSystems = new EcsSystems(_world, "Battle Systems");

            return battleSystems
                .Add(new BattlefieldSystem())
                .Add(new PlacementHeroSystem())
                .Add(new PlacementEnemySystem())
                
                .Add(new BattleSystem())

                .Add(new FighterSystem())
                .Add(new FighterAnimatorSystem())
                .Add(new FighterDeathSystem())

                .OneFrame<EndPlacementFighterSquadEvent>();
        }

        private EcsSystems GameSystems()
        {
            var gameSystems = new EcsSystems(_world, "Game Systems");

            return gameSystems
                .Add(new GameStateSystem());
        }
        
        private EcsSystems UISystems()
        {
            var uiSystems = new EcsSystems(_world, "UI Systems");

            return uiSystems
                .Add(new HealthBarBillboardSystem())
                .Add(new EndGameScreenSystem());
        }

        private void Debug(bool isEnable)
        {
            if (isEnable == false) return;

            
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_mainSystems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_gameplaySystems);
#endif
        }
    }
}