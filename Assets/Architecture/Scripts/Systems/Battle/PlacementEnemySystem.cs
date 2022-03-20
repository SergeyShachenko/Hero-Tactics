using System.Collections.Generic;
using System.Linq;
using Components;
using Components.Battle;
using Components.Events.Battle;
using Components.Tags;
using Leopotam.Ecs;
using Services;
using UnityEngine;

namespace Systems.Battle
{
    public sealed class PlacementEnemySystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<ChangedBattlefieldStateEvent> _changedStateBattlefieldEvents;

        private HashSet<PlaceableFighter> _enemiesForMove, _enemiesCompleteMove;
        private int _defencePositionsIndex, _freePositionsIndex;

        
        void IEcsInitSystem.Init()
        {
            _enemiesForMove = new HashSet<PlaceableFighter>();
            _enemiesCompleteMove = new HashSet<PlaceableFighter>();
        }

        void IEcsRunSystem.Run()
        {
            UpdateEnemiesForMove();
            MoveEnemies(canMove: _enemiesForMove.Count > 0);
            ClearEnemiesForMove(canClear: _enemiesForMove.Count <= _enemiesCompleteMove.Count && _enemiesForMove.Count > 0);
        }


        private void UpdateEnemiesForMove()
        {
            foreach (var index in  _changedStateBattlefieldEvents)
            {
                ref var changeStateBattlefieldEvent = ref _changedStateBattlefieldEvents.Get1(index);
                ref var entity = ref changeStateBattlefieldEvent.Battlefield;
                ref var visitors = ref entity.Get<Battlefield>().Visitors;


                foreach (var visitor in visitors)
                {
                    if (visitor.Get<Fighter>().BattleSide != BattleSide.Enemy) continue;
                    if (visitor.Get<Fighter>().State != FighterState.Alive) continue;
                    
                    
                    visitor.Get<Movable>().IsMovable = false;
                    
                    _enemiesForMove.Add(new PlaceableFighter {Entity = visitor, Place = entity});
                }
            }
        }

        private void MoveEnemies(bool canMove)
        {
            if (canMove == false) return;


            foreach (var enemy in _enemiesForMove)
            {
                ref var battlefield = ref enemy.Place.Get<Battlefield>();
                bool enemyIsMoving;
                
                switch (battlefield.State)
                {
                    case BattlefieldState.Battle:
                        
                        var mainAssaultPoint = battlefield.BattlePoints.GetChild(0).GetChild(0).position;
                        var defencePlacementPositions = new List<Vector3>();
                        var defencePoints = battlefield.BattlePoints.GetChild(1);

                        for (var i = 0; i < defencePoints.childCount; i++)
                            defencePlacementPositions.Add(defencePoints.GetChild(i).position);


                        if (enemy.Entity.Has<BossTag>())
                        {
                            enemyIsMoving = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                defencePlacementPositions.Last());
                            
                            enemy.Entity.Get<ModelParent>().GameObject.transform.LookAt(mainAssaultPoint);
                        }
                        else
                        {
                            enemyIsMoving = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                defencePlacementPositions[_defencePositionsIndex++]);

                            enemy.Entity.Get<ModelParent>().GameObject.transform.LookAt(mainAssaultPoint);
                        }
                        
                        
                        if (_defencePositionsIndex >= defencePlacementPositions.Count - 1) 
                            _defencePositionsIndex = 0;
                        
                        if (enemyIsMoving == false || enemy.Entity.Get<Fighter>().State != FighterState.Alive)
                            _enemiesCompleteMove.Add(enemy);
                        
                        break;

                    default:
                        
                        var freePlacementPositions = new List<Vector3>();
                        var standPoints = battlefield.StandPoints;

                        for (var i = 0; i < standPoints.childCount; i++)
                            freePlacementPositions.Add(standPoints.GetChild(i).position);


                        if (enemy.Entity.Has<BossTag>())
                        {
                            enemyIsMoving = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                freePlacementPositions.Last());
                            
                            enemy.Entity.Get<ModelParent>().GameObject.transform.rotation = 
                                Quaternion.Euler(battlefield.PlacementEnemyRotation);;
                        }
                        else
                        {
                            enemyIsMoving = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                freePlacementPositions[_freePositionsIndex++]);
                            
                            enemy.Entity.Get<ModelParent>().GameObject.transform.rotation = 
                                Quaternion.Euler(battlefield.PlacementEnemyRotation);;
                        }

                        
                        if (enemyIsMoving == false || enemy.Entity.Get<Fighter>().State != FighterState.Alive)
                            _enemiesCompleteMove.Add(enemy);

                        if (_freePositionsIndex >= freePlacementPositions.Count - 1) 
                            _freePositionsIndex = 0;
                        
                        break;
                }
            }
        }
        
        private void ClearEnemiesForMove(bool canClear)
        {
            if (canClear == false) return;


            var enemies = new HashSet<EcsEntity>();

            foreach (var enemy in _enemiesCompleteMove)
            {
                enemy.Entity.Get<Movable>().IsMovable = true;
                enemies.Add(enemy.Entity);
            }

            _gameTools.Events.Move.EndPlacementFighterSquad(BattleSide.Enemy, enemies, _enemiesCompleteMove.First().Place);
            
            _enemiesForMove.Clear();
            _enemiesCompleteMove.Clear();
        }
    }
}