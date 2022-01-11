using Leopotam.Ecs;

sealed class SpawnSystem : IEcsRunSystem 
{
    private readonly EcsWorld _world;
    private readonly GameServices _gameServices;

    private readonly EcsFilter<SpawnPrefab> _spawnFilter;
    

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