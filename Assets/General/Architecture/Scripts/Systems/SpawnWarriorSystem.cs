using System.Collections.Generic;
using System.Linq;
using General.Components.Battle;
using General.Components.Events;
using General.Data;
using Leopotam.Ecs;

sealed class SpawnWarriorSystem : IEcsInitSystem, IEcsRunSystem 
{
    private EcsWorld _world;
    private GameData _gameData;
    private GameService _gameService;

    private EcsFilter<SpawnWarriorEvent> _spawnWarriorEvents;

    private List<HeroData> _heroesData;
    private List<EnemyData> _enemysData;
    

    void IEcsInitSystem.Init()
    {
        _heroesData = _gameData.HeroesData.Heroes;
        _enemysData = _gameData.EnemysData.Enemys;
    }
    
    void IEcsRunSystem.Run()
    {
        if (_spawnWarriorEvents.IsEmpty()) return;

        
        foreach (var index in _spawnWarriorEvents)
        {
            ref EcsEntity entity = ref _spawnWarriorEvents.GetEntity(index);
            var spawnEvent = entity.Get<SpawnWarriorEvent>();

            
            switch (spawnEvent.BattleSide)
            {
                case BattleSide.Hero:
                    foreach (var heroData in _heroesData.Where(
                        heroData => heroData.Warrior.Type == spawnEvent.WarriorType))
                    {
                        _gameService.WarriorFactory.Spawn(
                            _world, 
                            heroData, 
                            spawnEvent.SquadID, 
                            spawnEvent.SpawnPoint);
                        break;
                    }
                    break;

                case BattleSide.Enemy:
                    if (spawnEvent.IsBoss)
                    {
                        foreach (var enemyData in _enemysData.Where(
                            enemyData => enemyData.IsBoss && enemyData.Warrior.Type == spawnEvent.WarriorType))
                        {
                            _gameService.WarriorFactory.Spawn(
                                _world, 
                                enemyData, 
                                spawnEvent.SquadID, 
                                spawnEvent.SpawnPoint);
                            break;
                        }
                    }
                    else
                    {
                        foreach (var enemyData in _enemysData.Where(
                            enemyData => !enemyData.IsBoss && enemyData.Warrior.Type == spawnEvent.WarriorType))
                        {
                            _gameService.WarriorFactory.Spawn(
                                _world, 
                                enemyData, 
                                spawnEvent.SquadID, 
                                spawnEvent.SpawnPoint);
                            break;
                        }
                    }
                    break;
            }

            entity.Del<SpawnWarriorEvent>();
        }
    }
}