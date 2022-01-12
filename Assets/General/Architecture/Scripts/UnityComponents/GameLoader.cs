using General.Systems.Battle;
using General.UnityComponents.Data;
using General.UnityComponents.MonoLinks.Base;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents 
{
    sealed class GameLoader : MonoBehaviour
    {
        [SerializeField] private string _presetName = "Forest";
        [SerializeField] private bool _debug = true;
        
        [Header("Data")]
        [SerializeField] private GameData _gameData;
        [SerializeField] private GameServices _gameServices;

        private EcsWorld _world;
        private EcsSystems _generalSystems;

        
        private void Start() 
        {
            _world = new EcsWorld();
            _generalSystems = new EcsSystems(_world);

            InitMonoEntitys(_world);
            Debug(_debug);
            
            _generalSystems
                    //Чистим от беспролезных Warrior
                .Add(new BattlefieldSystem())
                    //Создаёт сущности с компонентами Warrior и Fighter (Задаёт все необходимые данные)
                //.Add(new WarriorSystem())
                    //Спавн всех префабов с компонентом SpawnTag
                //.Add(new SpawnSystem())
                .Inject(_gameData)
                .Inject(_gameServices)
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