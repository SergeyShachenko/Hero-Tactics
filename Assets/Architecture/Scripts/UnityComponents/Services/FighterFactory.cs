using Components;
using Components.Battle;
using Leopotam.Ecs;
using UnityComponents.Data;
using UnityComponents.MonoLinks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityComponents.Services
{
    public class FighterFactory : MonoBehaviour
    {
        [SerializeField] private Transform SpawnHeroesTo, SpawnEnemiesTo;


        public void SpawnWarrior(EcsWorld world, HeroData heroData, int squadID, Vector3 spawnPosition)
        {
            var spawnTo = SpawnHeroesTo.Find("Squad[" + squadID + "]");

            if (spawnTo == null)
            {
                var instanceObject = new GameObject("Squad[" + squadID + "]");
                instanceObject.transform.parent = SpawnHeroesTo;
                
                spawnTo = instanceObject.transform;
            }


            var monoEntity = Instantiate(
                heroData.Warrior.Prefab, spawnPosition, Quaternion.identity).GetComponent<MonoEntity>();
            monoEntity.transform.parent = spawnTo;
            monoEntity.Init(world);

            var entity = monoEntity.GetEntity();
            var speedOffset = 0.6f;

            entity.Get<Fighter>() = new Fighter
            {
                BattleSide = BattleSide.Hero,
                SquadID = squadID,
                State = FighterState.Alive,
                Action = FighterAction.None,
                Stats = heroData.Warrior.Stats
            };
            entity.Get<Warrior>() = new Warrior
            {
                Type = heroData.Warrior.Type
            };
            entity.Get<Movable>() = new Movable
            {
                Speed = Random.Range(heroData.Warrior.Movable.Speed - speedOffset, heroData.Warrior.Movable.Speed),
                State = MovableState.Idle,
                IsMovable = true
            };

            if (entity.Has<HealthBar>())
            {
                ref var healthBar = ref entity.Get<HealthBar>();

                healthBar.StartHealth = heroData.Warrior.Stats.Health;
                healthBar.CurrentHealth = healthBar.StartHealth;
            }
        }

        public void SpawnWarrior(EcsWorld world, EnemyData enemyData, int squadID, Vector3 spawnPosition)
        {
            var spawnTo = SpawnEnemiesTo.Find("Squad[" + squadID + "]");

            if (spawnTo == null)
            {
                var instanceObject = new GameObject("Squad[" + squadID + "]");
                instanceObject.transform.parent = SpawnEnemiesTo;
                
                spawnTo = instanceObject.transform;
            }
            
            
            var monoEntity = Instantiate(
                enemyData.Warrior.Prefab, spawnPosition, Quaternion.identity).GetComponent<MonoEntity>();
            monoEntity.transform.parent = spawnTo;
            monoEntity.Init(world);
            
            var entity = monoEntity.GetEntity();
            var speedOffset = 0.6f;

            entity.Get<Fighter>() = new Fighter
            {
                BattleSide = BattleSide.Enemy,
                SquadID = squadID,
                State = FighterState.Alive,
                Action = FighterAction.None,
                Stats = enemyData.Warrior.Stats
            };
            entity.Get<Warrior>() = new Warrior
            {
                Type = enemyData.Warrior.Type
            };
            entity.Get<Movable>() = new Movable
            {
                Speed = Random.Range(enemyData.Warrior.Movable.Speed - speedOffset, enemyData.Warrior.Movable.Speed),
                State = MovableState.Idle,
                IsMovable = true
            };
            
            if (entity.Has<HealthBar>())
            {
                ref var healthBar = ref entity.Get<HealthBar>();

                healthBar.StartHealth = enemyData.Warrior.Stats.Health;
                healthBar.CurrentHealth = healthBar.StartHealth;
            }
        }
    }
}