using Components.Events.Battle;
using Components.Events.Main;
using Components.Events.Move;
using Components.Events.Unity;
using UnityComponents.Data;
using Services;
using Systems.Battle;
using Systems.Game;
using Systems.Main;
using Systems.Main.Spawn;
using Systems.Move;
using Components.Events.Spawn;
using General.Systems;
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
            
            Debug(isEnable:
                GameSettings.ECSDebug);
            
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
                
                .OneFrame<SpawnWarriorEvent>()

                .Inject(GameSettings)
                .Inject(GameData)
                .Inject(GameServices)
                .Inject(_gameTools)
                .Init();
        }

        private void InitGameplaySystems()
        {
            _gameplaySystems
                .Add(MoveSystems())
                .Add(BattleSystems())
                .Add(GameSystems())

                .OneFrame<StartBattleEvent>()
                .OneFrame<EndBattleEvent>()
                .OneFrame<DeadFighterEvent>()

                .OneFrame<OnPointerClickEvent>()
                .OneFrame<OnTriggerEnterEvent>()
                .OneFrame<OnTriggerExitEvent>()
                
                .OneFrame<ChangedStateBattlefieldEvent>()
                .OneFrame<ChangeGameStateEvent>()

                .Inject(GameSettings)
                .Inject(GameData)
                .Inject(GameServices)
                .Inject(_gameTools)
                .Init();
        }

        private EcsSystems StartupSystems()
        {
            var startupSystems = new EcsSystems(_world, "Startup Systems");

            return startupSystems
                .Add(new MonoEntitySystem())
                .Add(new DrawWaySystem());
        }

        private EcsSystems SpawnSystems()
        {
            var spawnSystems = new EcsSystems(_world, "Spawn Systems");

            return spawnSystems
                .Add(new SpawnWarriorSystem());
        }

        private EcsSystems MoveSystems()
        {
            var moveSystems = new EcsSystems(_world, "Move Systems");

            return moveSystems
                .Add(new PlayerInputSystem())
                .Add(new MoveHeroSystem())
                
                .OneFrame<MoveHeroesToEvent>();
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
                .Add(new FighterAnimationSystem())
                .Add(new FighterDeathSystem())

                .OneFrame<EndPlacementFighterSquadEvent>();
        }

        private EcsSystems GameSystems()
        {
            var gameSystems = new EcsSystems(_world, "Game Systems");

            return gameSystems
                .Add(new GameStateSystem());
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