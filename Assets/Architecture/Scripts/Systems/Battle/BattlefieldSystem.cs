using System.Collections.Generic;
using Components;
using Components.Battle;
using Components.Events.Battle;
using Components.Events.Unity;
using Services;
using Leopotam.Ecs;
using UnityComponents.Data;
using UnityComponents.Services;
using UnityEngine;

namespace Systems.Battle
{
    public sealed class BattlefieldSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameServices _gameServices;
        private readonly GameData _gameData;
        private readonly GameTools _gameTools;

        private readonly EcsFilter<ChangedStateBattlefieldEvent> _changedStateBattlefields;
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggersEnter;
        private readonly EcsFilter<OnTriggerExitEvent> _onTriggersExit;
        private readonly EcsFilter<Battlefield> _battlefields;
        

        void IEcsInitSystem.Init()
        {
            if (_battlefields.IsEmpty()) return;

            
            foreach (var index in _battlefields)
            {
                ref var entity = ref _battlefields.GetEntity(index);
                ref var gameObj = ref entity.Get<GameObj>().Value;
                ref var battlefield = ref entity.Get<Battlefield>();

                battlefield.State = BattlefieldState.Free;
                battlefield.Visitors = new List<EcsEntity>();
                battlefield.StandPoints = gameObj.transform.GetChild(0);
                battlefield.BattlePoints = gameObj.transform.GetChild(1);
                battlefield.Model = entity.Get<ModelParent>().GameObject.transform;

                OptimizeSpawnOnStart(ref battlefield);
                CallSpawnWarriorEvents(ref battlefield, squadID:index);
            }    
        }
        
        void IEcsRunSystem.Run()
        {
            CheckVisitors(canCheck:
                _onTriggersEnter.IsEmpty() == false);
            
            CheckGoneVisitors(canCheck:
                _onTriggersExit.IsEmpty() == false);
            
            UpdateState();

            UpdateColor(canUpdate:
                _changedStateBattlefields.IsEmpty() == false);
        }

        private void CheckVisitors(bool canCheck)
        {
            if (canCheck == false) return;
            
            
            foreach (var index in _onTriggersEnter)
            {
                ref var triggerEnter = ref _onTriggersEnter.GetEntity(index).Get<OnTriggerEnterEvent>();

                if (triggerEnter.Sender.Has<Battlefield>() == false) continue;
                if (triggerEnter.Visitor.Has<Fighter>() == false) continue;
                if (triggerEnter.Visitor.Get<Fighter>().State != FighterState.Alive) continue; 
                    
                    
                ref var battlefield = ref triggerEnter.Sender.Get<Battlefield>();

                if (battlefield.Visitors.Contains(triggerEnter.Visitor) == false)
                    battlefield.Visitors.Add(triggerEnter.Visitor);

                //Debug.Log("Add Visitor");
            }
        }
        
        private void CheckGoneVisitors(bool canCheck)
        {
            if (canCheck == false) return;
            
            
            foreach (var index in _onTriggersExit)
            {
                ref var triggerExit = ref _onTriggersExit.GetEntity(index).Get<OnTriggerExitEvent>();

                if (triggerExit.Sender.Has<Battlefield>() == false) continue;
                if (triggerExit.GoneVisitor.Has<Fighter>() == false) continue;
                    
                    
                ref var battlefield = ref triggerExit.Sender.Get<Battlefield>();
                battlefield.Visitors.Remove(triggerExit.GoneVisitor);

                //Debug.Log("Remove Gone Visitor");
            }
        }

        private void UpdateState()
        {
            if (_battlefields.IsEmpty()) return;


            foreach (var index in _battlefields)
            {
                ref var entity = ref _battlefields.GetEntity(index);
                ref var battlefield = ref entity.Get<Battlefield>();


                bool haveHeroes = false, haveEnemies = false;

                foreach (var visitor in battlefield.Visitors)
                {
                    if (visitor.Get<Fighter>().BattleSide == BattleSide.Hero) haveHeroes = true;
                    else haveEnemies = true;
                }

            
                if (haveHeroes && haveEnemies == false)
                {
                    if (battlefield.State != BattlefieldState.Free)
                    {
                        battlefield.State = BattlefieldState.Free;
                        _gameTools.Events.ChangedStateBattlefield(ref entity);
                    
                        Debug.Log(battlefield.State);
                    }
                }
                else if (haveHeroes == false && haveEnemies == false)
                {
                    if (battlefield.State != BattlefieldState.Free)
                    {
                        battlefield.State = BattlefieldState.Free;
                        _gameTools.Events.ChangedStateBattlefield(ref entity);
                    
                        Debug.Log(battlefield.State);
                    }
                }
                else if (haveHeroes == false)
                {
                    if (battlefield.State != BattlefieldState.Occupied)
                    {
                        battlefield.State = BattlefieldState.Occupied;
                        _gameTools.Events.ChangedStateBattlefield(ref entity);
                    
                        Debug.Log(battlefield.State);
                    }
                }
                else
                {
                    if (battlefield.State != BattlefieldState.Battle)
                    {
                        battlefield.State = BattlefieldState.Battle;
                        _gameTools.Events.ChangedStateBattlefield(ref entity);
                    
                        Debug.Log(battlefield.State);
                    }
                }
            }
        }
        
        private void UpdateColor(bool canUpdate)
        {
            if (canUpdate == false) return;


            foreach (var index in _changedStateBattlefields)
            {
                ref var entity = 
                    ref _changedStateBattlefields.GetEntity(index).Get<ChangedStateBattlefieldEvent>().Battlefield;

                ref var battlefield = ref entity.Get<Battlefield>();
                
                
                switch (battlefield.State)
                {
                    case BattlefieldState.Free:

                        _gameServices.DestroyGameObject(battlefield.Model.GetChild(0).gameObject);
                        _gameServices.GameObjectFactory.Spawn(_gameData.PrefabFreeBattlefield, battlefield.Model);
                        
                        break;
                    
                    default:
                    
                        _gameServices.DestroyGameObject(battlefield.Model.GetChild(0).gameObject);
                        _gameServices.GameObjectFactory.Spawn(_gameData.PrefabOccupiedBattlefield, battlefield.Model);
                        
                        break;
                }
            }
        }

        private void OptimizeSpawnOnStart(ref Battlefield battlefield)
        {
            ref var warriors = ref battlefield.SpawnWarriorOnStart;
            
            
            if(warriors.Count == 0) return; 
            

            if (battlefield.SpawnBoss)
            {
                warriors.RemoveRange(1, warriors.Count-1);
                battlefield.SpawnWarriorBattleSide = BattleSide.Enemy;
                return;
            }
            
            if (warriors.Count > 3)
            {
                warriors.RemoveRange(3, warriors.Count-3);
            }
        }
        
        private void CallSpawnWarriorEvents(ref Battlefield battlefield, int squadID)
        {
            if (battlefield.SpawnWarriorOnStart.Count == 0) return;
             
            
            if (battlefield.SpawnBoss)
            {
                var standPoints = battlefield.StandPoints;
                var spawnPoint = standPoints.GetChild(standPoints.childCount - 1);
                
                _gameTools.Events.SpawnWarrior(
                    battlefield.SpawnWarriorBattleSide,
                    battlefield.SpawnWarriorOnStart[0],
                    true,
                    squadID,
                    spawnPoint);
            }
            else
            {
                var standPointIndex = 0;
                foreach (var warriorType in battlefield.SpawnWarriorOnStart)
                {
                    var standPoints = battlefield.StandPoints;
                    var spawnPoint = standPoints.GetChild(standPointIndex++);
                    
                    if (standPointIndex >= standPoints.childCount - 1) standPointIndex = 0;

                    _gameTools.Events.SpawnWarrior(
                        battlefield.SpawnWarriorBattleSide,
                        warriorType,
                        false,
                        squadID,
                        spawnPoint);
                }
            }
        }
    }
}
