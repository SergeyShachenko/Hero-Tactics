using System;
using Components;
using Components.Battle;
using Leopotam.Ecs;
using UnityComponents.Data;
using UnityComponents.MonoLinks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityComponents.Services
{
    public class WarriorFactory : MonoBehaviour
    {
        [SerializeField] private Transform SpawnHeroesTo, SpawnEnemysTo;


        public void Spawn(EcsWorld world, HeroData heroData, int squadID, Vector3 spawnPosition)
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
        }

        public void Spawn(EcsWorld world, EnemyData enemyData, int squadID, Vector3 spawnPosition)
        {
            var spawnTo = SpawnEnemysTo.Find("Squad[" + squadID + "]");

            if (spawnTo == null)
            {
                var instanceObject = new GameObject("Squad[" + squadID + "]");
                instanceObject.transform.parent = SpawnEnemysTo;
                
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
        }
    }
    
    
    [Serializable] public struct SpawnedWarrior
    {
        public BattleSide BattleSide;
        public WarriorType Type;
    }
}