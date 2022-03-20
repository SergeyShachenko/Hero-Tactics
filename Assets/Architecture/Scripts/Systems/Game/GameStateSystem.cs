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
        
        private readonly EcsFilter<ChangedBattlefieldStateEvent> _changedBattlefieldStateEvents;
        private readonly EcsFilter<DeadFighterEvent> _deadFighterEvents;
        private readonly EcsFilter<GamePerformance> _gamePerformanceFilter;
        private readonly EcsFilter<Battlefield> _battlefieldFilter;
        private readonly EcsFilter<Fighter, PlayerTag> _playerFilter;
        private readonly EcsFilter<Fighter, BossTag> _bossFilter;

        private int _deadPlayersCount, _deadBossesCount;
        private bool _gameEnd;
        
        
        void IEcsInitSystem.Init()
        {
            _world.NewEntity().Get<GamePerformance>().State = GameState.Process;

            _deadPlayersCount = 0;
            _deadBossesCount = 0;
        }
        
        void IEcsRunSystem.Run()
        {
            UpdateState(canUpdate: _gameEnd == false);
        }

        
        private void UpdateState(bool canUpdate)
        {
            if (canUpdate == false) return;


            ref var gameState = ref _gamePerformanceFilter.Get1(0).State;

            if (DeadAllPlayers())
            {
                gameState = GameState.GameOver;
                _gameEnd = true;
                
                _gameTools.Events.ChangedGameState(gameState);
                
                Debug.Log("Game Over!");
                return;
            }

            if (AllBossesKilled() && _bossFilter.GetEntitiesCount() > 0)
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
            if (_deadFighterEvents.IsEmpty()) return false;


            foreach (var index in _deadFighterEvents)
            {
                ref var entity = ref _deadFighterEvents.Get1(index).Fighter;

                if (entity.Has<PlayerTag>()) 
                    _deadPlayersCount++;
            }

            return _deadPlayersCount == _playerFilter.GetEntitiesCount();
        }
        
        private bool AllBossesKilled()
        {
            if (_deadFighterEvents.IsEmpty()) return false;


            foreach (var index in _deadFighterEvents)
            {
                ref var entity = ref _deadFighterEvents.Get1(index).Fighter;

                if (entity.Has<BossTag>()) 
                    _deadBossesCount++;
            }

            return _deadBossesCount == _bossFilter.GetEntitiesCount();
        }
    }
}