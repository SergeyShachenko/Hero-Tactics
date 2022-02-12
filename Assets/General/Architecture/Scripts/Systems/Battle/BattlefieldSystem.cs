using System.Collections.Generic;
using General.Components;
using General.Components.Battle;
using General.Components.Events.Battle;
using General.Services;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    public sealed class BattlefieldSystem : IEcsInitSystem
    {
        private readonly GameTools _gameTools;

        private readonly EcsFilter<Battlefield> _battlefields;
        private readonly EcsFilter<BattlefieldChangeStateEvent> _battlefieldChangeStateEvents;
        

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
                
                OptimizeSpawnOnStart(ref battlefield);
                CallSpawnWarriorEvents(ref battlefield, squadID:index);
            }    
        }
        
        
        private void OptimizeSpawnOnStart(ref Battlefield battlefield)
        {
            ref var warriors = ref battlefield.SpawnWarriorOnStart;
            
            
            if(warriors.Count == 0) return; 
            

            if (battlefield.SpawnBoss)
            {
                warriors.RemoveRange(1, warriors.Count-1);
                battlefield.WarriorBattleSide = BattleSide.Enemy;
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
                    battlefield.WarriorBattleSide,
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
                        battlefield.WarriorBattleSide,
                        warriorType,
                        false,
                        squadID,
                        spawnPoint);
                }
            }
        }
    }
}
