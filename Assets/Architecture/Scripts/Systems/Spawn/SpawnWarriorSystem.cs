using System.Collections.Generic;
using System.Linq;
using Components.Battle;
using Components.Events.Spawn;
using UnityComponents.Data;
using UnityComponents.Services;
using Leopotam.Ecs;

namespace Systems.Spawn
{
    public sealed class SpawnWarriorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly GameServices _gameServices;
        private readonly GameData _gameData;

        private readonly EcsFilter<SpawnWarriorEvent> _spawnWarriorEvents;

        private List<HeroWarriorData> _heroesData;
        private List<EnemyWarriorData> _enemiesData;
        

        void IEcsInitSystem.Init()
        {
            _heroesData = _gameData.Heroes.Warriors;
            _enemiesData = _gameData.Enemies.Warriors;
        }
        
        void IEcsRunSystem.Run()
        {
            SpawnWarrior();
        }

        
        private void SpawnWarrior()
        {
            foreach (var index in _spawnWarriorEvents)
            {
                var spawnEvent = _spawnWarriorEvents.Get1(index);

                if (spawnEvent.BattleSide == BattleSide.Hero)
                {
                    foreach (var heroData in _heroesData.Where(
                        heroData => heroData.Warrior.Type == spawnEvent.WarriorType))
                    {
                        _gameServices.FighterFactory.SpawnWarrior(
                            _world,
                            heroData,
                            spawnEvent.SquadID,
                            spawnEvent.Parent.position);
                    }
                }
                else if (spawnEvent.IsBoss)
                {
                    foreach (var enemyData in _enemiesData.Where(
                        enemyData => enemyData.IsBoss && enemyData.Warrior.Type == spawnEvent.WarriorType))
                    {
                        _gameServices.FighterFactory.SpawnWarrior(
                            _world,
                            enemyData,
                            spawnEvent.SquadID,
                            spawnEvent.Parent.position);
                    }
                }
                else
                {
                    foreach (var enemyData in _enemiesData.Where(
                        enemyData => !enemyData.IsBoss && enemyData.Warrior.Type == spawnEvent.WarriorType))
                    {
                        _gameServices.FighterFactory.SpawnWarrior(
                            _world,
                            enemyData,
                            spawnEvent.SquadID,
                            spawnEvent.Parent.position);
                    }
                }
            }
        }
    }
}