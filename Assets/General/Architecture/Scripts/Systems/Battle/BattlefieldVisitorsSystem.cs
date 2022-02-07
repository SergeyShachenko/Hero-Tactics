using General.Components.Battle;
using General.Components.Events.Unity;
using General.Services;
using General.UnityComponents.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems.Battle
{
    public sealed class BattlefieldVisitorsSystem : IEcsRunSystem
    {
        private readonly GameSettings _gameSettings;
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggerEnterEvents;
        private readonly EcsFilter<OnTriggerExitEvent> _onTriggerExitEvents;
        
        
        void IEcsRunSystem.Run()
        {
            CheckVisitors();
            CheckGoneVisitors();
        }
        
        
        private void CheckVisitors()
        {
            if (_onTriggerEnterEvents.IsEmpty()) return;
            
            
            foreach (var index in _onTriggerEnterEvents)
            {
                ref var enterEvent = ref _onTriggerEnterEvents.GetEntity(index).Get<OnTriggerEnterEvent>();

                if (enterEvent.SenderEntity.Has<Battlefield>() == false) continue;
                if (enterEvent.VisitorEntity.Has<Fighter>() == false) continue;
                    
                    
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
        
        private void CheckGoneVisitors()
        {
            if (_onTriggerExitEvents.IsEmpty()) return;
            
            
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