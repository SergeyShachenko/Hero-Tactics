using System.Collections.Generic;
using General.Components.Battle;
using General.Components.Links;
using General.Components.Tags;
using General.UnityComponents.Data;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    sealed class WarriorsSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private GameData _gameData;
        
        private List<HeroData> _heroesData;
        private List<EnemyData> _enemysData;
        private List<List<EcsEntity>> _warriorSquads = new List<List<EcsEntity>>();

        private EcsFilter<BattlefieldLink> _battlefieldsFilter;
        
        
        void IEcsInitSystem.Init()
        {
            _heroesData = _gameData.HeroesData.Heroes;
            _enemysData = _gameData.EnemysData.Enemys;
            
            foreach (var index in _battlefieldsFilter)
            {
                ref EcsEntity entity = ref _battlefieldsFilter.GetEntity(index);
                var battlefield = entity.Get<BattlefieldLink>();
                
                CreateWarriorEntitys(battlefield);
            }
        }


        private void CreateWarriorEntitys(BattlefieldLink battlefield)
        {
            List<EcsEntity> squad = new List<EcsEntity>();

            foreach (var warrior in battlefield.Warriors)
            {
                if (battlefield.BattleSide == BattleSide.Hero)
                {
                    HeroData heroData = new HeroData();
                    
                
                    foreach (var data in _heroesData)
                    {
                        if (data.Type == warrior.Type)
                        {
                            heroData = data;
                        }
                    }

                    var warriorEntity = _world.NewEntity();
                    warriorEntity.Get<Warrior>() = new Warrior
                    {
                        isBoss = false,
                        Type = warrior.Type
                    };
                    warriorEntity.Get<Fighter>() = new Fighter
                    {
                        BattleSide = battlefield.BattleSide,
                        Health = heroData.Health,
                        Armor = heroData.Armor,
                        Damage = heroData.Damage,
                        State = FighterState.Alive
                    };

                    squad.Add(warriorEntity);
                }

                if (battlefield.BattleSide == BattleSide.Enemy)
                {
                    EnemyData enemyData = new EnemyData();
                    
                
                    foreach (var data in _enemysData)
                    {
                        if (data.Type == warrior.Type && data.isBoss == warrior.isBoss)
                        {
                            enemyData = data;
                        }
                    }

                    var warriorEntity = _world.NewEntity();
                    warriorEntity.Get<Warrior>() = new Warrior
                    {
                        isBoss = warrior.isBoss,
                        Type = warrior.Type
                    };
                    warriorEntity.Get<Fighter>() = new Fighter
                    {
                        BattleSide = battlefield.BattleSide,
                        Health = enemyData.Health,
                        Armor = enemyData.Armor,
                        Damage = enemyData.Damage,
                        State = FighterState.Alive
                    };

                    if (enemyData.isBoss)
                    {
                        warriorEntity.Get<BossTag>();
                    }

                    squad.Add(warriorEntity);
                }
                
            }

            _warriorSquads.Add(squad);
        }
    }
}
