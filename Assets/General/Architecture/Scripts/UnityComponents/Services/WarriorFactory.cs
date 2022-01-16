using General.Components.Battle;
using General.Data;
using General.MonoLinks;
using Leopotam.Ecs;
using UnityEngine;

public class WarriorFactory : MonoBehaviour
{
    public void Spawn(EcsWorld world, HeroData heroData, byte squadID, Transform spawnPoint)
    {
        var monoEntity = Instantiate(heroData.Warrior.Prefab, spawnPoint).GetComponent<MonoEntity>();
        monoEntity.Init(world);

        var entity = monoEntity.GetEntity();
        
        
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
    }

    public void Spawn(EcsWorld world, EnemyData enemyData, byte squadID, Transform spawnPoint)
    {
        var monoEntity = Instantiate(enemyData.Warrior.Prefab, spawnPoint).GetComponent<MonoEntity>();
        monoEntity.Init(world);
        
        var entity = monoEntity.GetEntity();
        
        
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
    }
}