using System.Collections.Generic;
using Components.Battle;
using Leopotam.Ecs;
using UnityEngine;

namespace Services.Fighters
{
    public sealed class FighterSquadService
    {
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;
        
        public FighterSquadService(EcsWorld world, GameTools gameTools)
        {
            _world = world;
            _gameTools = gameTools;
        }
        
        
        public List<EcsEntity> Get(int squadID, EcsFilter<Fighter> fighters)
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
        
        public FighterSquad Create(ref EcsEntity place, List<EcsEntity> fighterSquad)
        {
            var squadBattleSide = BattleSide.Hero;
            int squadArmor = 0, squadID = 0;
            float squadHealth = 0, squadDamage = 0;

            
            foreach (var entity in fighterSquad)
            {
                var fighter = entity.Get<Fighter>();

                squadBattleSide = fighter.BattleSide;
                squadID = fighter.SquadID;        
                squadHealth += fighter.Stats.MaxHealth;
                squadArmor += fighter.Stats.Armor;
                squadDamage += fighter.Stats.Damage;
            }

            var squadStats = new FighterStats { MaxHealth = squadHealth, Armor = squadArmor, Damage = squadDamage};
            squadStats.CurrentHealth = squadStats.MaxHealth;
                    
            return new FighterSquad
            {
                BattleSide = squadBattleSide,
                State = SquadState.Alive,
                ID = squadID,
                Stats = squadStats,
                Place = place
            };
        }
        
        public void SetAction(FighterAction action, int squadID, EcsFilter<Fighter> fighters)
        {
            var fighterSquad = Get(squadID, fighters);
            
            foreach (var entity in fighterSquad)
            {
                entity.Get<Fighter>().Action = action;
            }
        }

        public void TakeDamage(ref FighterSquad squad, float damage)
        {
            var processedDamage = damage / 100 * (100 - squad.Stats.Armor);
            
            squad.Stats.CurrentHealth = Mathf.Clamp(
                squad.Stats.CurrentHealth - processedDamage, 0, squad.Stats.MaxHealth);
            
            if (Mathf.Approximately(squad.Stats.CurrentHealth, 0)) 
                squad.State = SquadState.Dead;
        }
    }
}