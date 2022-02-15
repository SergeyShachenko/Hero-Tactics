using System.Collections.Generic;
using Systems.Game;
using Components.Battle;
using Components.Events.Battle;
using Components.Events.Main;
using Components.Events.Move;
using Components.Events.Spawn;
using Leopotam.Ecs;
using UnityEngine;

namespace Services
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

        public void MoveEntitiesTo(List<EcsEntity> entities, Vector3 targetPosition)
        {
            _world.NewEntity().Get<MoveHeroesToEvent>() = new MoveHeroesToEvent
            {
                Heroes = entities,
                TargetPosition = targetPosition
            };
        }
        
        public void MoveEntityTo(EcsEntity entity, Vector3 targetPosition)
        {
            _world.NewEntity().Get<MoveHeroToEvent>() = new MoveHeroToEvent
            {
                Entity = entity,
                Position = targetPosition
            };
        }

        public void ChangedStateBattlefield(ref EcsEntity battlefield)
        {
            _world.NewEntity().Get<ChangedStateBattlefieldEvent>() = new ChangedStateBattlefieldEvent
            {
                Battlefield = battlefield
            };
        }

        public void ChangeGameState(GameState state)
        {
            _world.NewEntity().Get<ChangeGameStateEvent>() = new ChangeGameStateEvent
            {
                State = state
            };
        }

        public void EndPlacementFighterSquad(BattleSide battleSide, List<EcsEntity> fighters, EcsEntity place)
        {
            _world.NewEntity().Get<EndPlacementFighterSquadEvent>() = new EndPlacementFighterSquadEvent
            {
                BattleSide = battleSide,
                Fighters = fighters,
                Place = place
            };
        }

        public void StartBattle(int assaultSquadID, int defenceSquadID, ref EcsEntity place)
        {
            _world.NewEntity().Get<StartBattleEvent>() = new StartBattleEvent
            {
                AssaultSquadID = assaultSquadID,
                DefenceSquadID = defenceSquadID,
                Place = place
            };
        }
        
        public void EndBattle(int assaultSquadID, int defenceSquadID, ref EcsEntity place)
        {
            _world.NewEntity().Get<EndBattleEvent>() = new EndBattleEvent
            {
                AssaultSquadID = assaultSquadID,
                DefenceSquadID = defenceSquadID,
                Place = place
            };
        }

        public void DeadFighter(ref EcsEntity fighter)
        {
            _world.NewEntity().Get<DeadFighterEvent>() = new DeadFighterEvent
            {
                Fighter = fighter
            };
        }
    }
}