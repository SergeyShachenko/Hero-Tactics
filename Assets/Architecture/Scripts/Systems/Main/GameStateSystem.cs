using Components.Battle;
using Components.Events.Battle;
using Components.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Main
{
    public sealed class GameStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter<BattlefieldChangeStateEvent> _battlefieldChangeStateEvents;
        private readonly EcsFilter<Battlefield> _battlefields;
        private readonly EcsFilter<WarriorDeadEvent> _warriorDeadEvents;
        private readonly EcsFilter<PlayerTag> _players;

        private int _freeBattlefieldsCount, _deadPlayersCount;
        
            
        void IEcsInitSystem.Init()
        {
            _freeBattlefieldsCount = 1;
            _deadPlayersCount = 0;
        }
        
        void IEcsRunSystem.Run()
        {
            CheckFreeBattlefieldsCount(canCheck:_battlefieldChangeStateEvents.IsEmpty() == false);
            CheckDeadPlayersCount(canUpdate: _warriorDeadEvents.IsEmpty() == false);
        }


        private void CheckFreeBattlefieldsCount(bool canCheck)
        {
            if (canCheck == false) return;


            foreach (var index in _battlefieldChangeStateEvents)
            {
                ref var battlefieldChangeStateEvent = 
                    ref _battlefieldChangeStateEvents.GetEntity(index).Get<BattlefieldChangeStateEvent>();
                
                ref var battlefield = ref battlefieldChangeStateEvent.BattlefieldEntity.Get<Battlefield>();
                
                if (battlefield.State == BattlefieldState.Free) _freeBattlefieldsCount++;
            }
            
            
            if (_freeBattlefieldsCount == _battlefields.GetEntitiesCount())
            {
                Debug.Log("Win!");
                _freeBattlefieldsCount = 1;
            }
        }
        
        private void CheckDeadPlayersCount(bool canUpdate)
        {
            if (canUpdate == false) return;


            foreach (var index in _warriorDeadEvents)
            {
                ref var deadWarriorEntity = ref _warriorDeadEvents.GetEntity(index).Get<WarriorDeadEvent>().Entity;

                if (deadWarriorEntity.Has<PlayerTag>()) _deadPlayersCount++;
            }


            if (_deadPlayersCount == _players.GetEntitiesCount())
            {
                Debug.Log("GameOver");
                _deadPlayersCount = 0;
            }
        }
    }


    public enum GameState
    {
        Process, Win, GameOver
    }
}