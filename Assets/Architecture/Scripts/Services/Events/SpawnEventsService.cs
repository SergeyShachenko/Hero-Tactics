using Components.Battle;
using Components.Events.Spawn;
using Leopotam.Ecs;
using UnityEngine;

namespace Services.Events
{
    public sealed class SpawnEventsService : GameToolServiceBase
    {
        public SpawnEventsService(EcsWorld world, GameTools gameTools) : base(world, gameTools) {}


        public void Warrior(BattleSide battleSide, WarriorType type, bool isBoss, int squadID, Transform spawnPoint)
        {
            World.NewEntity().Get<SpawnWarriorEvent>() = new SpawnWarriorEvent
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