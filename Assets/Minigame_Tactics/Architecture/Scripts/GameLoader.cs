using Leopotam.Ecs;
using UnityEngine;

namespace Tactics 
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
            
            Debug(_debug);
            
            _generalSystems
                .Add(new SpawnSystem())
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