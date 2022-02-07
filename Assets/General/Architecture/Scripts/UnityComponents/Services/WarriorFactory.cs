using General.Components;
using General.Components.Battle;
using General.UnityComponents.Data;
using General.UnityComponents.MonoLinks;
using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.Services
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
                State = FighterState.Disabled,
                Stats = heroData.Warrior.Stats
            };
            entity.Get<Warrior>() = new Warrior
            {
                Type = heroData.Warrior.Type
            };
            entity.Get<Movable>() = new Movable
            {
                Speed = Random.Range(heroData.Warrior.Movable.Speed - speedOffset, heroData.Warrior.Movable.Speed),
                State = MovableState.Stand,
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
                State = FighterState.Disabled,
                Stats = enemyData.Warrior.Stats
            };
            entity.Get<Warrior>() = new Warrior
            {
                Type = enemyData.Warrior.Type
            };
            entity.Get<Movable>() = new Movable
            {
                Speed = Random.Range(enemyData.Warrior.Movable.Speed - speedOffset, enemyData.Warrior.Movable.Speed),
                State = MovableState.Stand,
                IsMovable = true
            };
        }
    }
}