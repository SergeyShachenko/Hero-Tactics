using Leopotam.Ecs;

sealed class SpawnSystem : IEcsRunSystem 
{
    private EcsWorld _world;
    private GameServices _gameServices;

    private EcsFilter<SpawnPrefab> _spawnFilter;
    

    void IEcsRunSystem.Run()
    {
        if (_spawnFilter.IsEmpty()) return;

        foreach (var index in _spawnFilter)
        {
            ref EcsEntity entity = ref _spawnFilter.GetEntity(index);
            
            _gameServices.Spawner.Spawn(entity.Get<SpawnPrefab>());
                
            entity.Del<SpawnPrefab>();
        }
    }
}