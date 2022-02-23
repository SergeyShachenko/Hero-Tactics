using Components.Battle;
using Components.Events.Spawn;
using Leopotam.Ecs;
using UnityEngine;

namespace Services.Events
{
    public sealed class SpawnEventsService
    {
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;

        public SpawnEventsService(EcsWorld world, GameTools gameTools)
        {
            _world = world;
            _gameTools = gameTools;
        }
        
        
        public void Warrior(BattleSide battleSide, WarriorType type, bool isBoss, int squadID, Transform spawnPoint)
        {
            _world.NewEntity().Get<SpawnWarriorEvent>() = new SpawnWarriorEvent
            {
                BattleSide = battleSide,
                WarriorType = type,
                IsBoss = isBoss,
                SquadID = squadID,
                Parent = spawnPoint
            };
        }
    }
}