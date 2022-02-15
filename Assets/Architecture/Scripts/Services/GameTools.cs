using System.Collections.Generic;
using Components.Battle;
using Leopotam.Ecs;
using UnityEngine;

namespace Services
{
    public sealed class GameTools
    {
        public readonly GameplayService Gameplay;
        public readonly EventService Events;

        public GameTools(EcsWorld world)
        {
            Gameplay = new GameplayService(world, this);
            Events = new EventService(world);
        }
        
        
        public List<EcsEntity> GetFighterSquad(int squadID, EcsFilter<Fighter> fighters)
        {
            if (fighters.IsEmpty()) return null;


            var warriorSquad = new List<EcsEntity>();
            
            foreach (var index in fighters)
            {
                ref var entity = ref fighters.GetEntity(index);
                ref var fighter = ref entity.Get<Fighter>();

                if (fighter.SquadID == squadID) warriorSquad.Add(entity);
            }

            return warriorSquad.Count > 0 ? warriorSquad : null;
        }
        
        public void SetActionForFighterSquad(FighterAction action, int squadID, EcsFilter<Fighter> fighters)
        {
            var fighterSquad = GetFighterSquad(squadID, fighters);
            
            foreach (var entity in fighterSquad)
            {
                entity.Get<Fighter>().Action = action;
            }
        }
        
        public FighterSquad CreateFighterSquad(ref EcsEntity place, List<EcsEntity> fighterSquad)
        {
            var squadBattleSide = BattleSide.Hero;
            int squadArmor = 0, squadID = 0;
            float squadHealth = 0, squadDamage = 0;

            
            foreach (var entity in fighterSquad)
            {
                var fighter = entity.Get<Fighter>();

                squadBattleSide = fighter.BattleSide;
                squadID = fighter.SquadID;        
                squadHealth += fighter.Stats.Health;
                squadArmor += fighter.Stats.Armor;
                squadDamage += fighter.Stats.Damage;
            }

            var squadStats = new FighterStats { Health = squadHealth, Armor = squadArmor, Damage = squadDamage};
                    
            return new FighterSquad
            {
                BattleSide = squadBattleSide,
                State = SquadState.Alive,
                ID = squadID,
                Stats = squadStats,
                Place = place
            };
        }
        
        // public void UpdateBattlefieldState(ref EcsEntity entity)
        // {
        //     if (entity.Has<Battlefield>() == false) return;
        //
        //
        //     ref var battlefield = ref entity.Get<Battlefield>();
        //     
        //     
        //     if (battlefield.Visitors.Count == 0) return;
        //
        //         
        //     bool haveHeroes = false, haveEnemies = false;
        //
        //     foreach (var visitor in battlefield.Visitors)
        //     {
        //         if (visitor.Get<Fighter>().BattleSide == BattleSide.Hero) haveHeroes = true;
        //         else haveEnemies = true;
        //     }
        //
        //     
        //     if (haveHeroes && haveEnemies == false)
        //     {
        //         if (battlefield.State != BattlefieldState.Free)
        //         {
        //             battlefield.State = BattlefieldState.Free;
        //             Events.ChangedStateBattlefield(ref entity);
        //             
        //             Debug.Log(battlefield.State);
        //         }
        //     }
        //     else if (haveHeroes == false && haveEnemies == false)
        //     {
        //         if (battlefield.State != BattlefieldState.Free)
        //         {
        //             battlefield.State = BattlefieldState.Free;
        //             Events.ChangedStateBattlefield(ref entity);
        //             
        //             Debug.Log(battlefield.State);
        //         }
        //     }
        //     else if (haveHeroes == false)
        //     {
        //         if (battlefield.State != BattlefieldState.Occupied)
        //         {
        //             battlefield.State = BattlefieldState.Occupied;
        //             Events.ChangedStateBattlefield(ref entity);
        //             
        //             Debug.Log(battlefield.State);
        //         }
        //     }
        //     else
        //     {
        //         if (battlefield.State != BattlefieldState.Battle)
        //         {
        //             battlefield.State = BattlefieldState.Battle;
        //             Events.ChangedStateBattlefield(ref entity);
        //             
        //             Debug.Log(battlefield.State);
        //         }
        //     }
        // }
    }
}