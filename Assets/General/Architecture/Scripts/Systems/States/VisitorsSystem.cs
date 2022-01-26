using General.Components.Battle;
using General.Components.Events.Unity;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems.States
{
    public sealed class VisitorsSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggerEnterEvents;
        private readonly EcsFilter<OnTriggerExitEvent> _onTriggerExitEvents;

        private bool _debug = false;
        
        
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

                if (enterEvent.EntitySender.Has<Battlefield>() == false) continue;
                if (enterEvent.EntityVisitor.Has<Fighter>() == false) continue;
                    
                    
                ref var battlefield = ref enterEvent.EntitySender.Get<Battlefield>();
                battlefield.Visitors.Add(enterEvent.EntityVisitor);
                
                UpdateState(ref battlefield);
                
                if (_debug) Debug.Log("Add Visitor");
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
                
                UpdateState(ref battlefield);
                
                if (_debug) Debug.Log("Remove Visitor");
            }
        }
        
        private void UpdateState(ref Battlefield battlefield)
        {
            if (battlefield.Visitors.Count == 0) return;

                
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
                if (_debug && battlefield.State != BattlefieldState.Free) 
                    Debug.Log( battlefield + " FREE!");
                
                battlefield.State = BattlefieldState.Free;
            }
            
            if (haveHeroes && haveEnemys == false)
            {
                if (_debug && battlefield.State != BattlefieldState.Free) 
                    Debug.Log( battlefield + " FREE!");
                
                battlefield.State = BattlefieldState.Free;
            }

            if (haveEnemys && haveHeroes == false)
            {
                if (_debug && battlefield.State != BattlefieldState.Occupied) 
                    Debug.Log( battlefield + " OCCUPIED!");
                
                battlefield.State = BattlefieldState.Occupied;
            }

            if (haveHeroes && haveEnemys)
            {
                if (_debug && battlefield.State != BattlefieldState.Battle) 
                    Debug.Log( battlefield + " BATTLE!");
                
                battlefield.State = BattlefieldState.Battle;
            }
        }
    }
}