﻿using System.Collections.Generic;
using General.Components.Battle;
using General.Components.Events;
using General.Components.Events.Battle;
using General.Components.Events.Move;
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

        public void MoveEntitysTo(List<EcsEntity> entities, Vector3 targetPosition)
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

        public void BattlefieldChangeState(ref EcsEntity entity)
        {
            _world.NewEntity().Get<BattlefieldChangeStateEvent>() = new BattlefieldChangeStateEvent
            {
                BattlefieldEntity = entity
            };
        }

        public void EndPlacementFighterSquad(BattleSide battleSide, List<EcsEntity> fighters, EcsEntity place)
        {
            _world.NewEntity().Get<EndPlacementFighterSquadEvent>() = new EndPlacementFighterSquadEvent
            {
                BattleSide = battleSide,
                Fighters = fighters,
                PlaceEntity = place
            };
        }

        public void EndFight(ref EcsEntity entity)
        {
            _world.NewEntity().Get<EndFightEvent>() = new EndFightEvent
            {
                PlaceEntity = entity
            };
        }

        public void WarriorDead(ref EcsEntity entity)
        {
            _world.NewEntity().Get<WarriorDeadEvent>() = new WarriorDeadEvent
            {
                Entity = entity
            };
        }
    }
}