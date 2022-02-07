using System.Collections.Generic;
using System.Linq;
using General.Components;
using General.Components.Battle;
using General.Services;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    public sealed class BattlefieldSystem : IEcsInitSystem
    {
        private readonly GameTools _gameTools;

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
                
                OptimizeSpawnOnStart(ref battlefield);
                CallSpawnWarriorEvents(ref battlefield, squadID:index);
            }    
        }


        private void OptimizeSpawnOnStart(ref Battlefield battlefield)
        {
            ref var warriors = ref battlefield.SpawnOnStart;
            
            
            if(warriors.Count == 0) return; 
            

            if (battlefield.IsBoss)
            {
                warriors.RemoveRange(1, warriors.Count-1);
                battlefield.BattleSide = BattleSide.Enemy;
                return;
            }
            
            if (warriors.Count > 3)
            {
                warriors.RemoveRange(3, warriors.Count-3);
            }
        }
        
        private void CallSpawnWarriorEvents(ref Battlefield battlefield, int squadID)
        {
            if (battlefield.SpawnOnStart.Count == 0) return;
             
            
            if (battlefield.IsBoss)
            {
                var standPoints = battlefield.StandPoints;
                var spawnPoint = standPoints.GetChild(standPoints.childCount - 1);
                
                _gameTools.Events.SpawnWarrior(
                    battlefield.BattleSide,
                    battlefield.SpawnOnStart.First(),
                    true,
                    squadID,
                    spawnPoint);
            }
            else
            {
                var standPointIndex = 0;
                foreach (var warriorType in battlefield.SpawnOnStart)
                {
                    var standPoints = battlefield.StandPoints;
                    var spawnPoint = standPoints.GetChild(standPointIndex++);
                    
                    if (standPointIndex >= standPoints.childCount - 1) standPointIndex = 0;

                    _gameTools.Events.SpawnWarrior(
                        battlefield.BattleSide,
                        warriorType,
                        false,
                        squadID,
                        spawnPoint);
                }
            }
        }
    }
}
