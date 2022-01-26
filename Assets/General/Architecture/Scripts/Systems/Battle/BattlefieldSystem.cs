using System.Collections.Generic;
using General.Components.Battle;
using General.Services;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    public sealed class BattlefieldSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private readonly Tools _tools;

        private readonly EcsFilter<Battlefield> _battlefields;
        

        void IEcsInitSystem.Init()
        {
            if (_battlefields.IsEmpty()) return;

            
            foreach (var index in _battlefields)
            {
                ref var battlefield = ref _battlefields.GetEntity(index).Get<Battlefield>();
                battlefield.State = BattlefieldState.Free;
                battlefield.Visitors = new List<EcsEntity>();
                
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
                var spawnPoint = battlefield.StandPositions.GetChild(0);
                _tools.Events.SpawnWarrior(
                    battlefield.BattleSide,
                    battlefield.SpawnOnStart[0],
                    true,
                    squadID,
                    spawnPoint);
            }
            else
            {
                byte spawnPointID = 1;
                foreach (var warriorType in battlefield.SpawnOnStart)
                {
                    var spawnPoint = battlefield.StandPositions.GetChild(spawnPointID++);

                    _tools.Events.SpawnWarrior(
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
