using System.Collections.Generic;
using Components.Battle;
using Components.Events.Battle;
using Components.Events.Physics;
using Components;
using Services;
using Leopotam.Ecs;
using UnityComponents.Data;
using UnityComponents.Services;
using UnityEngine;

namespace Systems.Battle
{
    public sealed class BattlefieldSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        private readonly GameServices _gameServices;
        private readonly GameData _gameData;

        private readonly EcsFilter<ChangedBattlefieldStateEvent> _changedStateBattlefieldEvents;
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggersEnterEvents;
        private readonly EcsFilter<OnTriggerExitEvent> _onTriggersExitEvents;
        private readonly EcsFilter<Battlefield> _battlefieldFilter;
        

        void IEcsInitSystem.Init()
        {
            InitBattlefields();
        }

        void IEcsRunSystem.Run()
        {
            CheckVisitors();
            CheckGoneVisitors();
            UpdateState();
            UpdateModel();
        }

        
        private void InitBattlefields()
        {
            foreach (var index in _battlefieldFilter)
            {
                ref var entity = ref _battlefieldFilter.GetEntity(index);
                ref var gameObj = ref entity.Get<GameObj>().Value;
                ref var battlefield = ref entity.Get<Battlefield>();

                battlefield.State = BattlefieldState.Free;
                battlefield.Visitors = new HashSet<EcsEntity>();
                battlefield.StandPoints = gameObj.transform.GetChild(0);
                battlefield.BattlePoints = gameObj.transform.GetChild(1);
                battlefield.Model = entity.Get<ModelParent>().GameObject.transform;

                OptimizeSpawnWarriorsOnStart(ref battlefield);
                CallSpawnWarriorEvents(ref battlefield, squadID: index);
            }
        }
        
        private void CheckVisitors()
        {
            foreach (var index in _onTriggersEnterEvents)
            {
                ref var onTriggerEnterEvent = ref _onTriggersEnterEvents.Get1(index);

                if (onTriggerEnterEvent.Sender.Has<Battlefield>() == false) continue;
                if (onTriggerEnterEvent.Visitor.Has<Fighter>() == false) continue;
                if (onTriggerEnterEvent.Visitor.Get<Fighter>().State != FighterState.Alive) continue; 
                    
                    
                onTriggerEnterEvent.Sender.Get<Battlefield>().Visitors.Add(onTriggerEnterEvent.Visitor);

                //Debug.Log("Add Visitor");
            }
        }
        
        private void CheckGoneVisitors()
        {
            foreach (var index in _onTriggersExitEvents)
            {
                ref var onTriggerExitEvent = ref _onTriggersExitEvents.Get1(index);

                if (onTriggerExitEvent.Sender.Has<Battlefield>() == false) continue;
                if (onTriggerExitEvent.GoneVisitor.Has<Fighter>() == false) continue;
                if (onTriggerExitEvent.GoneVisitor.Get<Fighter>().State != FighterState.Alive) continue;
                    
                    
                onTriggerExitEvent.Sender.Get<Battlefield>().Visitors.Remove(onTriggerExitEvent.GoneVisitor);

                //Debug.Log("Remove Gone Visitor");
            }
        }

        private void UpdateState()
        {
            foreach (var index in _battlefieldFilter)
            {
                ref var entity = ref _battlefieldFilter.GetEntity(index);
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
                        _gameTools.Events.Battle.ChangedStateBattlefield(ref entity);
                    
                        Debug.Log(battlefield.State);
                    }
                }
                else if (haveHeroes == false && haveEnemies == false)
                {
                    if (battlefield.State != BattlefieldState.Free)
                    {
                        battlefield.State = BattlefieldState.Free;
                        _gameTools.Events.Battle.ChangedStateBattlefield(ref entity);
                    
                        Debug.Log(battlefield.State);
                    }
                }
                else if (haveHeroes == false)
                {
                    if (battlefield.State != BattlefieldState.Occupied)
                    {
                        battlefield.State = BattlefieldState.Occupied;
                        _gameTools.Events.Battle.ChangedStateBattlefield(ref entity);
                    
                        Debug.Log(battlefield.State);
                    }
                }
                else
                {
                    if (battlefield.State != BattlefieldState.Battle)
                    {
                        battlefield.State = BattlefieldState.Battle;
                        _gameTools.Events.Battle.ChangedStateBattlefield(ref entity);
                    
                        Debug.Log(battlefield.State);
                    }
                }
            }
        }
        
        private void UpdateModel()
        {
            foreach (var index in _changedStateBattlefieldEvents)
            {
                ref var entity = ref _changedStateBattlefieldEvents.Get1(index).Battlefield;
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

        private void OptimizeSpawnWarriorsOnStart(ref Battlefield battlefield)
        {
            ref var warriors = ref battlefield.SpawnWarriorsOnStart;

            if (warriors.Count == 0) return; 
            

            if (battlefield.SpawnBoss)
            {
                warriors.RemoveRange(1, warriors.Count-1);
                battlefield.SpawnWarriorBattleSide = BattleSide.Enemy;
                return;
            }
            
            if (warriors.Count > 3)
                warriors.RemoveRange(3, warriors.Count-3);
        }
        
        private void CallSpawnWarriorEvents(ref Battlefield battlefield, int squadID)
        {
            if (battlefield.SpawnWarriorsOnStart.Count == 0) return;
             
            
            if (battlefield.SpawnBoss)
            {
                var standPoints = battlefield.StandPoints;
                var spawnPoint = standPoints.GetChild(standPoints.childCount - 1);
                
                _gameTools.Events.Spawn.Warrior(
                    battlefield.SpawnWarriorBattleSide,
                    battlefield.SpawnWarriorsOnStart[0],
                    true,
                    squadID,
                    spawnPoint);
            }
            else
            {
                var standPointIndex = 0;
                
                foreach (var warriorType in battlefield.SpawnWarriorsOnStart)
                {
                    var standPoints = battlefield.StandPoints;
                    var spawnPoint = standPoints.GetChild(standPointIndex++);
                    
                    if (standPointIndex >= standPoints.childCount - 1) 
                        standPointIndex = 0;

                    _gameTools.Events.Spawn.Warrior(
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