using Components;
using Components.Battle;
using Components.Events.Battle;
using Components.Tags;
using Leopotam.Ecs;
using Services;
using UnityEngine;

namespace Systems.Game
{
    public sealed class GameStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<ChangedBattlefieldStateEvent> _changedBattlefieldsState;
        private readonly EcsFilter<GamePerformance> _gamePerformance;
        private readonly EcsFilter<DeadFighterEvent> _deadFighters;
        private readonly EcsFilter<Battlefield> _battlefields;
        private readonly EcsFilter<PlayerTag> _players;
        private readonly EcsFilter<BossTag> _bosses;
        
        private int _deadPlayersCount, _deadBossesCount, _freeBattlefieldsCount;
        private bool _gameHaveBosses, _gameEnd;
        
        
        void IEcsInitSystem.Init()
        {
            _world.NewEntity().Get<GamePerformance>().State = GameState.Process;

            _deadPlayersCount = 0;
            _deadBossesCount = 0;
            _freeBattlefieldsCount = 1;
            
            _gameHaveBosses = _bosses.GetEntitiesCount() > 0;
        }
        
        void IEcsRunSystem.Run()
        {
            UpdateState(canUpdate:
                _gameEnd == false);
        }

        
        private void UpdateState(bool canUpdate)
        {
            if (canUpdate == false) return;


            ref var gameState = ref _gamePerformance.GetEntity(0).Get<GamePerformance>().State;

            if (DeadAllPlayers())
            {
                gameState = GameState.GameOver;
                _gameEnd = true;
                
                _gameTools.Events.ChangedGameState(gameState);
                
                Debug.Log("Game Over!");
                return;
            }

            if (AllBossesKilled() && _gameHaveBosses)
            {
                gameState = GameState.Win;
                _gameEnd = true;
                
                _gameTools.Events.ChangedGameState(gameState);
                
                Debug.Log("Win!");
                return;
            }
            
            if (AllBattlefieldsFreed())
            {
                gameState = GameState.Win;
                _gameEnd = true;
                
                _gameTools.Events.ChangedGameState(gameState);
                
                Debug.Log("Win!");
                return;
            }
        }

        private bool DeadAllPlayers()
        {
            if (_deadFighters.IsEmpty()) 
                return false;


            foreach (var index in _deadFighters)
            {
                ref var deadFighterEntity = ref _deadFighters.GetEntity(index).Get<DeadFighterEvent>().Fighter;

                if (deadFighterEntity.Has<PlayerTag>()) _deadPlayersCount++;
            }

            return _deadPlayersCount == _players.GetEntitiesCount();
        }
        
        private bool AllBossesKilled()
        {
            if (_deadFighters.IsEmpty()) 
                return false;


            foreach (var index in _deadFighters)
            {
                ref var deadFighterEntity = ref _deadFighters.GetEntity(index).Get<DeadFighterEvent>().Fighter;

                if (deadFighterEntity.Has<BossTag>()) _deadBossesCount++;
            }

            return _deadBossesCount == _bosses.GetEntitiesCount();
        }

        private bool AllBattlefieldsFreed()
        {
            if (_changedBattlefieldsState.IsEmpty()) 
                return false;


            foreach (var index in _changedBattlefieldsState)
            {
                ref var changedBattlefieldState = 
                    ref _changedBattlefieldsState.GetEntity(index).Get<ChangedBattlefieldStateEvent>();
                
                ref var battlefield = ref changedBattlefieldState.Battlefield.Get<Battlefield>();
                
                if (battlefield.State == BattlefieldState.Free) 
                    _freeBattlefieldsCount++;
            }
            
            return _freeBattlefieldsCount == _battlefields.GetEntitiesCount();
        }
    }
}