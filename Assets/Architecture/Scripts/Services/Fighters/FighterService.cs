using Components;
using Components.Battle;
using Leopotam.Ecs;
using UnityEngine;

namespace Services.Fighters
{
    public sealed class FighterService
    {
        public readonly FighterSquadService Squad;
        
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;
        
        public FighterService(EcsWorld world, GameTools gameTools)
        {
            Squad = new FighterSquadService(world, gameTools);
            
            _world = world;
            _gameTools = gameTools;
        }


        public void TakeDamage(ref EcsEntity entity, float damage)
        {
            ref var fighter = ref entity.Get<Fighter>();
            
            
            fighter.Stats.CurrentHealth = Mathf.Clamp(
                fighter.Stats.CurrentHealth - damage, 0, fighter.Stats.MaxHealth);
            
            if (Mathf.Approximately(fighter.Stats.CurrentHealth, 0))
            {
                fighter.State = FighterState.Dead;
                _gameTools.Events.Fighter.Dead(ref entity);
            }
            
            
            if (entity.Has<HealthBar>())
            {
                ref var healthBar = ref entity.Get<HealthBar>().Bar;
                
                healthBar.fillAmount = fighter.Stats.CurrentHealth / (fighter.Stats.MaxHealth / 100) / 100;
            }
        }
        
        public void TakeDamageInPercent(ref EcsEntity entity, float percent)
        {
            var damage = Mathf.Lerp(0, entity.Get<Fighter>().Stats.MaxHealth, percent);
            
            TakeDamage(ref entity, damage);
        }
    }
}