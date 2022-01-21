using System.Collections.Generic;
using System.Linq;
using General.Components.Battle;
using General.Components.Events;
using General.UnityComponents.Data;
using General.UnityComponents.Services;
using Leopotam.Ecs;

namespace General.Systems.Spawn
{
    sealed class SpawnWarriorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private GameData _gameData;
        private GameServices _gameServices;

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
                            _gameServices.WarriorFactory.Spawn(
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
                                _gameServices.WarriorFactory.Spawn(
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
                                _gameServices.WarriorFactory.Spawn(
                                    _world,
                                    enemyData,
                                    spawnEvent.SquadID,
                                    spawnEvent.SpawnPoint);
                                break;
                            }
                        }

                        break;
                }
            }
        }
    }
}