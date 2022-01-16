using General.Components.Battle;
using General.Data;
using General.MonoLinks;
using Leopotam.Ecs;
using UnityEngine;

public class WarriorFactory : MonoBehaviour
{
    public void Spawn(EcsWorld world, HeroData heroData, Transform spawnPoint)
    {
        var monoEntity = Instantiate(heroData.Warrior.Prefab, spawnPoint).GetComponent<MonoEntity>();
        monoEntity.Init(world);


        monoEntity.Get<Fighter>().Value = new Fighter
        {
            BattleSide = BattleSide.Hero,
            State = FighterState.Disabled,
            Stats = heroData.Warrior.Stats
        };
        
        monoEntity.Get<Warrior>().Value = new Warrior
        {
            Type = heroData.Warrior.Type
        };
    }

    public void Spawn(EcsWorld world, EnemyData enemyData, Transform spawnPoint)
    {
        var monoEntity = Instantiate(enemyData.Warrior.Prefab, spawnPoint).GetComponent<MonoEntity>();
        monoEntity.Init(world);
        
        
        monoEntity.Get<Fighter>().Value = new Fighter
        {
            BattleSide = BattleSide.Enemy,
            State = FighterState.Disabled,
            Stats = enemyData.Warrior.Stats
        };
        
        monoEntity.Get<Warrior>().Value = new Warrior
        {
            Type = enemyData.Warrior.Type
        };
    }
}