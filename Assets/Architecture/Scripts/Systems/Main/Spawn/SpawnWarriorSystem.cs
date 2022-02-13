using System.Collections.Generic;
using System.Linq;
using Components.Battle;
using Components.Events.Spawn;
using UnityComponents.Data;
using UnityComponents.Services;
using Leopotam.Ecs;

namespace Systems.Main.Spawn
{
    public sealed class SpawnWarriorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly GameData _gameData;
        private readonly GameServices _gameServices;

        private readonly EcsFilter<SpawnWarriorEvent> _spawnWarriorEvents;

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
                var spawnEvent = _spawnWarriorEvents.GetEntity(index).Get<SpawnWarriorEvent>();


                if (spawnEvent.BattleSide == BattleSide.Hero)
                {
                    foreach (var heroData in _heroesData.Where(
                        heroData => heroData.Warrior.Type == spawnEvent.WarriorType))
                    {
                        _gameServices.WarriorFactory.Spawn(
                            _world, 
                            heroData, 
                            spawnEvent.SquadID, 
                            spawnEvent.SpawnPoint.position);
                    }
                }
                else if (spawnEvent.IsBoss)
                {
                    foreach (var enemyData in _enemysData.Where(
                        enemyData => enemyData.IsBoss && enemyData.Warrior.Type == spawnEvent.WarriorType))
                    {
                        _gameServices.WarriorFactory.Spawn(
                            _world,
                            enemyData,
                            spawnEvent.SquadID,
                            spawnEvent.SpawnPoint.position);
                    }
                }
                else
                {
                    foreach (var enemyData in _enemysData.Where(
                        enemyData => !enemyData.IsBoss && enemyData.Warrior.Type == spawnEvent.WarriorType))
                    {
                        _gameServices.WarriorFactory.Spawn(
                            _world,
                            enemyData,
                            spawnEvent.SquadID,
                            spawnEvent.SpawnPoint.position);
                    }
                }
            }
        }
    }
}