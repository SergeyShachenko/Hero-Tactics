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
        
        
        public void WarriorSpawn(bool isBoss, BattleSide battleSide, WarriorType type, Transform spawnPoint)
        {
            _world.NewEntity().Get<SpawnWarriorEvent>() = new SpawnWarriorEvent
            {
                IsBoss = isBoss,
                BattleSide = battleSide,
                WarriorType = type,
                SpawnPoint = spawnPoint
            };
        }
    }
}