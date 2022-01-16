using General.Components.Battle;
using General.Components.Events;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Services
{
    public class EventService
    {
        private EcsWorld _world;


        public EventService(EcsWorld world)
        {
            _world = world;
        }
        
        
        public void WarriorSpawn(BattleSide battleSide, WarriorType type, bool isBoss, byte squadID, Transform spawnPoint)
        {
            _world.NewEntity().Get<SpawnWarriorEvent>() = new SpawnWarriorEvent
            {
                BattleSide = battleSide,
                WarriorType = type,
                IsBoss = isBoss,
                SquadID = squadID,
                SpawnPoint = spawnPoint
            };
        }
    }
}