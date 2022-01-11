using Leopotam.Ecs;
using UnityEngine;

namespace Tactics 
{
    sealed class LevelLoader : MonoBehaviour 
    {
        [SerializeField] private string _presetName = "Forest";
        [SerializeField] private bool _debug = true;
        
        private EcsWorld _world;
        private EcsSystems _systems;

        
        private void Start() 
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            
            Debug(_debug);
            
            _systems
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy() 
        {
            if (_systems != null) 
            {
                _systems.Destroy();
                _systems = null;
            }
            
            _world.Destroy();
            _world = null;
        }
        
        
        private void Debug(bool isEnable)
        {
            if (!isEnable) return;

            #if UNITY_EDITOR
                Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
                Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
            #endif
        }
    }
}