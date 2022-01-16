using General.Components.Battle;
using General.Services;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    sealed class BattlefieldSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private EventService _eventService;

        private EcsFilter<Battlefield> _battlefieldFilter;


        void IEcsInitSystem.Init()
        {
            if (_battlefieldFilter.IsEmpty()) return;


            byte counter = 0;
            
            
            foreach (var index in _battlefieldFilter)
            {
                ref EcsEntity entity = ref _battlefieldFilter.GetEntity(index);
                var battlefield = entity.Get<Battlefield>();

                
                OptimizeData(battlefield);
                CreateSpawnWarriorEvents(battlefield, counter++);
            }    
        }


        private void OptimizeData(Battlefield battlefield)
        {
            var warriors = battlefield.WarriorTypes;
            
            
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
        
        private void CreateSpawnWarriorEvents(Battlefield battlefield, byte squadID)
        {
            if (battlefield.IsBoss)
            {
                var spawnPoint = battlefield.StandPositions.GetChild(0);
                _eventService.WarriorSpawn(
                    battlefield.BattleSide,
                    battlefield.WarriorTypes[0],
                    true,
                    squadID,
                    spawnPoint);
            }
            else
            {
                byte counter = 1;

                foreach (var warriorType in battlefield.WarriorTypes)
                {
                    var spawnPoint = battlefield.StandPositions.GetChild(counter++);

                    _eventService.WarriorSpawn(
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
