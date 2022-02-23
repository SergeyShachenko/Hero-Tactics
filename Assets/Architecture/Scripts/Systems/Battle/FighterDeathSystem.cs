using Components;
using Components.Battle;
using Components.Events.Battle;
using Leopotam.Ecs;
using Services;
using UnityEngine;

namespace Systems.Battle
{
    public sealed class FighterDeathSystem : IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<DeadFighterEvent> _deadFighters;
        private readonly EcsFilter<EndBattleEvent> _endBattles;
        
        
        void IEcsRunSystem.Run()
        {
            ProcessDeadFighters(canProcess:
                _deadFighters.IsEmpty() == false);
            
            RemoveDeadFightersFromBattlefields(canRemove:
                _endBattles.IsEmpty() == false);
        }

        private void ProcessDeadFighters(bool canProcess)
        {
            if (canProcess == false) return;


            foreach (var index in _deadFighters)
            {
                ref var entity = ref _deadFighters.GetEntity(index).Get<DeadFighterEvent>().Fighter;

                if (entity.Has<Movable>()) 
                    entity.Get<Movable>().IsMovable = false;
                
                if (entity.Has<HealthBar>())
                {
                    ref var healthBar = ref entity.Get<HealthBar>();
                    healthBar.Frame.color = new Color(0f, 0f, 0f, 0f);
                    healthBar.Background.color = new Color(0f, 0f, 0f, 0f);
                }
            }
        }

        private void RemoveDeadFightersFromBattlefields(bool canRemove)
        {
            if (canRemove == false) return;
        
        
            foreach (var index in _endBattles)
            {
                ref var endBattle = ref _endBattles.GetEntity(index).Get<EndBattleEvent>();
                
                if (endBattle.Place.Has<Battlefield>() == false) continue;
                
                
                ref var battlefield = ref endBattle.Place.Get<Battlefield>();
                
                for (var i = 0; i < battlefield.Visitors.Count;)
                {
                    var visitor = battlefield.Visitors[i];
        
                    if (visitor.Get<Fighter>().State != FighterState.Alive)
                    {
                        battlefield.Visitors.Remove(visitor);
                        i = 0;
                        
                        //Debug.Log("Remove Dead Visitor");
                        continue;
                    }
        
                    i++;
                }
            }
        }
    }
}