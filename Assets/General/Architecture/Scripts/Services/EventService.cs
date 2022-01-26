using General.Components.Battle;
using General.Components.Events;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Services
{
    public sealed class EventService
    {
        private readonly EcsWorld _world;
        
        public EventService(EcsWorld world)
        {
            _world = world;
        }
        
        
        public void SpawnWarrior(BattleSide battleSide, WarriorType type, bool isBoss, int squadID, Transform spawnPoint)
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

        public void MoveHeroTo(Vector3 position)
        {
            _world.NewEntity().Get<MoveHeroToPositionEvent>() = new MoveHeroToPositionEvent
            {
                Position = position
            };
        }
    }
}