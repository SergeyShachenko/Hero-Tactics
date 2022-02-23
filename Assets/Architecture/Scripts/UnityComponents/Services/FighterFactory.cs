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


        public void SpawnWarrior(EcsWorld world, HeroWarriorData heroWarriorData, int squadID, Vector3 spawnPosition)
        {
            var parentGameObject = SpawnHeroesTo.Find("Squad[" + squadID + "]");

            if (parentGameObject == null)
            {
                var instanceObject = new GameObject("Squad[" + squadID + "]");
                instanceObject.transform.parent = SpawnHeroesTo;
                
                parentGameObject = instanceObject.transform;
            }


            var monoEntity = Instantiate(
                heroWarriorData.Warrior.Prefab, spawnPosition, Quaternion.identity).GetComponent<MonoEntity>();
            
            monoEntity.transform.parent = parentGameObject;
            monoEntity.Init(world);

            var entity = monoEntity.GetEntity();
            var speedOffset = 0.6f;
            
            
            heroWarriorData.Warrior.Stats.CurrentHealth = heroWarriorData.Warrior.Stats.MaxHealth;

            entity.Get<Fighter>() = new Fighter
            {
                BattleSide = BattleSide.Hero,
                SquadID = squadID,
                State = FighterState.Alive,
                Action = FighterAction.None,
                Stats = heroWarriorData.Warrior.Stats
            };
            
            entity.Get<Warrior>() = new Warrior
            {
                Type = heroWarriorData.Warrior.Type
            };
            
            entity.Get<Movable>() = new Movable
            {
                Speed = Random.Range(heroWarriorData.Warrior.Movable.Speed - speedOffset, heroWarriorData.Warrior.Movable.Speed),
                State = MovableState.Idle,
                IsMovable = true
            };
        }

        public void SpawnWarrior(EcsWorld world, EnemyWarriorData enemyWarriorData, int squadID, Vector3 spawnPosition)
        {
            var parentGameObject = SpawnEnemiesTo.Find("Squad[" + squadID + "]");

            if (parentGameObject == null)
            {
                var instanceObject = new GameObject("Squad[" + squadID + "]");
                instanceObject.transform.parent = SpawnEnemiesTo;
                
                parentGameObject = instanceObject.transform;
            }
            
            
            var monoEntity = Instantiate(
                enemyWarriorData.Warrior.Prefab, spawnPosition, Quaternion.identity).GetComponent<MonoEntity>();
            
            monoEntity.transform.parent = parentGameObject;
            monoEntity.Init(world);
            
            var entity = monoEntity.GetEntity();
            var speedOffset = 0.6f;

            
            enemyWarriorData.Warrior.Stats.CurrentHealth = enemyWarriorData.Warrior.Stats.MaxHealth;
            
            entity.Get<Fighter>() = new Fighter
            {
                BattleSide = BattleSide.Enemy,
                SquadID = squadID,
                State = FighterState.Alive,
                Action = FighterAction.None,
                Stats = enemyWarriorData.Warrior.Stats
            };
            
            entity.Get<Warrior>() = new Warrior
            {
                Type = enemyWarriorData.Warrior.Type
            };
            
            entity.Get<Movable>() = new Movable
            {
                Speed = Random.Range(enemyWarriorData.Warrior.Movable.Speed - speedOffset, enemyWarriorData.Warrior.Movable.Speed),
                State = MovableState.Idle,
                IsMovable = true
            };
        }
    }
}