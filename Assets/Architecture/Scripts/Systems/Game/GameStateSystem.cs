using Components.Battle;
using Components.Events.Battle;
using Components.Tags;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Game
{
    public sealed class GameStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter<ChangedStateBattlefieldEvent> _changedStateBattlefields;
        private readonly EcsFilter<DeadFighterEvent> _deadFighters;
        private readonly EcsFilter<Battlefield> _battlefields;
        private readonly EcsFilter<PlayerTag> _players;

        private int _freeBattlefieldsCount, _deadPlayersCount;
        
            
        void IEcsInitSystem.Init()
        {
            _freeBattlefieldsCount = 1;
            _deadPlayersCount = 0;
        }
        
        void IEcsRunSystem.Run()
        {
            CheckFreeBattlefieldsCount(canCheck:
                _changedStateBattlefields.IsEmpty() == false);
            
            CheckDeadPlayersCount(canUpdate:
                _deadFighters.IsEmpty() == false);
        }


        private void CheckFreeBattlefieldsCount(bool canCheck)
        {
            if (canCheck == false) return;


            foreach (var index in _changedStateBattlefields)
            {
                ref var battlefieldChangeStateEvent = 
                    ref _changedStateBattlefields.GetEntity(index).Get<ChangedStateBattlefieldEvent>();
                
                ref var battlefield = ref battlefieldChangeStateEvent.Battlefield.Get<Battlefield>();
                
                if (battlefield.State == BattlefieldState.Free) _freeBattlefieldsCount++;
            }
            
            
            if (_freeBattlefieldsCount == _battlefields.GetEntitiesCount())
            {
                Debug.Log("Win!");
            }
        }
        
        private void CheckDeadPlayersCount(bool canUpdate)
        {
            if (canUpdate == false) return;


            foreach (var index in _deadFighters)
            {
                ref var deadWarriorEntity = ref _deadFighters.GetEntity(index).Get<DeadFighterEvent>().Fighter;

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