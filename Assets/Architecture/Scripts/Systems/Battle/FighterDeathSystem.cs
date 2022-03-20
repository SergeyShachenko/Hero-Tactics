using System.Linq;
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
        
        private readonly EcsFilter<DeadFighterEvent> _deadFighterEvents;
        private readonly EcsFilter<EndBattleEvent> _endBattleEvents;
        
        
        void IEcsRunSystem.Run()
        {
            ProcessDeadFighters();
            RemoveDeadFightersFromBattlefields();
        }

        private void ProcessDeadFighters()
        {
            foreach (var index in _deadFighterEvents)
            {
                ref var entity = ref _deadFighterEvents.Get1(index).Fighter;

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

        private void RemoveDeadFightersFromBattlefields()
        {
            foreach (var index in _endBattleEvents)
            {
                ref var endBattle = ref _endBattleEvents.Get1(index);
                
                if (endBattle.Place.Has<Battlefield>() == false) continue;


                ref var battlefield = ref endBattle.Place.Get<Battlefield>();
                var newVisitors = battlefield.Visitors.ToList();

                for (var i = 0; i < newVisitors.Count;)
                {
                    var visitor = newVisitors[i];
        
                    if (visitor.Get<Fighter>().State != FighterState.Alive)
                    {
                        newVisitors.Remove(visitor);
                        i = 0;
                        
                        //Debug.Log("Remove Dead Visitor");
                        continue;
                    }
        
                    ++i;
                }

                battlefield.Visitors = newVisitors.ToHashSet();
            }
        }
    }
}