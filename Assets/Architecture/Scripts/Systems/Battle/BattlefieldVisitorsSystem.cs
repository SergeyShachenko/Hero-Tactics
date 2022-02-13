using Components.Battle;
using Components.Events.Battle;
using Components.Events.Unity;
using Services;
using UnityComponents.Data;
using Leopotam.Ecs;

namespace Systems.Battle
{
    public sealed class BattlefieldVisitorsSystem : IEcsRunSystem
    {
        private readonly GameSettings _gameSettings;
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggerEnterEvents;
        private readonly EcsFilter<OnTriggerExitEvent> _onTriggerExitEvents;
        private readonly EcsFilter<EndFightEvent> _endFightEvents;
        
        
        void IEcsRunSystem.Run()
        {
            CheckVisitors(canCheck:_onTriggerEnterEvents.IsEmpty() == false);
            CheckGoneVisitors(canCheck:_onTriggerExitEvents.IsEmpty() == false);
            CheckDeadVisitors(canCheck:_endFightEvents.IsEmpty() == false);
        }


        private void CheckVisitors(bool canCheck)
        {
            if (canCheck == false) return;
            
            
            foreach (var index in _onTriggerEnterEvents)
            {
                ref var enterEvent = ref _onTriggerEnterEvents.GetEntity(index).Get<OnTriggerEnterEvent>();

                if (enterEvent.SenderEntity.Has<Battlefield>() == false) continue;
                if (enterEvent.VisitorEntity.Has<Fighter>() == false) continue;
                if (enterEvent.VisitorEntity.Get<Fighter>().State != FighterState.Alive) continue; 
                    
                    
                ref var battlefield = ref enterEvent.SenderEntity.Get<Battlefield>();
                battlefield.Visitors.Add(enterEvent.VisitorEntity);


                if (UpdateState(ref battlefield))
                {
                    _gameTools.Events.BattlefieldChangeState(ref enterEvent.SenderEntity);
                    //Debug.Log(battlefield.State);
                }
                
                //Debug.Log("Add Visitor");
            }
        }
        
        private void CheckGoneVisitors(bool canCheck)
        {
            if (canCheck == false) return;
            
            
            foreach (var index in _onTriggerExitEvents)
            {
                ref var exitEvent = ref _onTriggerExitEvents.GetEntity(index).Get<OnTriggerExitEvent>();

                if (exitEvent.EntitySender.Has<Battlefield>() == false) continue;
                if (exitEvent.EntityGoneVisitor.Has<Fighter>() == false) continue;
                    
                    
                ref var battlefield = ref exitEvent.EntitySender.Get<Battlefield>();
                battlefield.Visitors.Remove(exitEvent.EntityGoneVisitor);
                
                
                if (UpdateState(ref battlefield))
                {
                    _gameTools.Events.BattlefieldChangeState(ref exitEvent.EntitySender);
                    //Debug.Log(battlefield.State);
                }
                
                //Debug.Log("Remove Visitor");
            }
        }
        
        private void CheckDeadVisitors(bool canCheck)
        {
            if (canCheck == false) return;


            foreach (var index in _endFightEvents)
            {
                ref var endFightEvent = ref _endFightEvents.GetEntity(index).Get<EndFightEvent>();
                
                if (endFightEvent.PlaceEntity.Has<Battlefield>() == false) return;
                
                
                ref var battlefield = ref endFightEvent.PlaceEntity.Get<Battlefield>();
                
                for (int i = 0; i < battlefield.Visitors.Count; i++)
                {
                    var visitor = battlefield.Visitors[i];
                    
                    if (visitor.Get<Fighter>().State == FighterState.Dead) battlefield.Visitors.Remove(visitor);
                }
            }
        }
        
        private bool UpdateState(ref Battlefield battlefield)
        {
            if (battlefield.Visitors.Count == 0) return false;

                
            bool haveHeroes = false, haveEnemys = false;

            foreach (var fighter in battlefield.Visitors)
            {
                if (fighter.Get<Fighter>().BattleSide == BattleSide.Hero)
                {
                    haveHeroes = true;
                }
                else
                {
                    haveEnemys = true;
                }
            }

            if (haveHeroes == false && haveEnemys == false)
            {
                if (battlefield.State != BattlefieldState.Free)
                {
                    battlefield.State = BattlefieldState.Free;
                    return true;
                }
            }
            
            if (haveHeroes && haveEnemys == false)
            {
                if (battlefield.State != BattlefieldState.Free)
                {
                    battlefield.State = BattlefieldState.Free;
                    return true;
                }
            }

            if (haveEnemys && haveHeroes == false)
            {
                if (battlefield.State != BattlefieldState.Occupied)
                {
                    battlefield.State = BattlefieldState.Occupied;
                    return true;
                }
            }

            if (haveHeroes && haveEnemys)
            {
                if (battlefield.State != BattlefieldState.Battle)
                {
                    battlefield.State = BattlefieldState.Battle;
                    return true;
                }
            }

            return false;
        }
    }
}