using General.Components.Events;
using General.Components.Events.Unity;
using General.UnityComponents.Data;
using General.Services;
using General.Systems.Battle;
using General.Systems.Main;
using General.Systems.Move;
using General.Systems.Spawn;
using General.Systems.States;
using General.UnityComponents.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace General 
{
    public sealed class GameStartup : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private EnvironmentPreset Environment = EnvironmentPreset.Forest;
        [SerializeField] private bool ECSDebug = true;
        
        [Header("Data")]
        [SerializeField] private GameData GameData;
        [SerializeField] private GameServices GameServices;

        private EcsWorld _world;
        private EcsSystems _systems, _fixedUpdateSystems;
        private Tools _tools;

        
        private void Start() 
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world, "Fixed Update Systems");
            
            Debug(ECSDebug);
            
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
            _tools = new Tools(_world);
        }

        private void InitSystems()
        {
            AddMainSystems();
            AddBattleSystems();
            
            _systems
                .Inject(GameData)
                .Inject(GameServices)
                .Inject(_tools)
                .Init();
        }

        private void InitFixedUpdateSystems()
        {
            AddMoveSystems();
            
            _fixedUpdateSystems
                .OneFrame<OnPointerClickEvent>()
                .OneFrame<OnTriggerEnterEvent>()
                .OneFrame<OnTriggerExitEvent>()
                .Inject(_tools)
                .Init();
        }
        
        private void AddMainSystems()
        {
            var mainSystems = new EcsSystems(_world, "Main Systems");

            mainSystems
                .Add(new InitMonoEntitySystem());
                
            _systems.Add(mainSystems);
        }

        private void AddBattleSystems()
        {
            var battleSystems = new EcsSystems(_world, "Battle Systems");

            battleSystems
                .Add(new BattlefieldSystem())
                .Add(new SpawnWarriorSystem())
                .Add(new WarriorSystem())
                .OneFrame<SpawnWarriorEvent>()
                .OneFrame<BattlefieldChangeStateEvent>();

            _systems.Add(battleSystems);
        }
        
        private void AddMoveSystems()
        {
            var moveSystems = new EcsSystems(_world, "Move Systems");

            moveSystems
                .Add(new PlayerInputSystem())
                .Add(new MoveHeroSystem())
                .Add(new VisitorsSystem())
                .Add(new BattlefieldPositionsSystem())
                .OneFrame<MoveHeroToPositionEvent>()
                .OneFrame<StopMoveHeroSystemEvent>();

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
        
        
        public enum EnvironmentPreset
        {
            Forest, Desert, Winter
        }
    }
}