using General.Components.Battle;
using General.Services;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    public sealed class BattlefieldSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;
        private readonly Tools _tools;

        private readonly EcsFilter<Battlefield> _battlefieldFilter;


        void IEcsInitSystem.Init()
        {
            if (_battlefieldFilter.IsEmpty()) return;


            byte squadID = 0;
            foreach (var index in _battlefieldFilter)
            {
                ref var battlefield = ref _battlefieldFilter.GetEntity(index).Get<Battlefield>();
                
                OptimizeData(ref battlefield);
                CreateSpawnWarriorEvents(ref battlefield, squadID++);
            }    
        }


        private void OptimizeData(ref Battlefield battlefield)
        {
            ref var warriors = ref battlefield.WarriorTypes;

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
        
        private void CreateSpawnWarriorEvents(ref Battlefield battlefield, byte squadID)
        {
            if (battlefield.IsBoss)
            {
                var spawnPoint = battlefield.StandPositions.GetChild(0);
                _tools.Events.SpawnWarrior(
                    battlefield.BattleSide,
                    battlefield.WarriorTypes[0],
                    true,
                    squadID,
                    spawnPoint);
            }
            else
            {
                byte spawnPointID = 1;
                foreach (var warriorType in battlefield.WarriorTypes)
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
