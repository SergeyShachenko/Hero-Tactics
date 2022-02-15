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
        
        private readonly EcsFilter<ChangedStateBattlefieldEvent> _changedStateBattlefields;

        private List<PlacebleFighter> _enemiesForMove, _enemiesCompleteMove;
        private int _defencePositionsIndex, _freePositionsIndex;

        
        void IEcsInitSystem.Init()
        {
            _enemiesForMove = new List<PlacebleFighter>();
            _enemiesCompleteMove = new List<PlacebleFighter>();
        }

        void IEcsRunSystem.Run()
        {
            UpdateEnemiesForMove(canUpdate:
                _changedStateBattlefields.IsEmpty() == false);

            MoveEnemies(canMove:
                _enemiesForMove.Count > 0);
            
            ClearEnemiesForMove(canClear:
                _enemiesForMove.Count <= _enemiesCompleteMove.Count && _enemiesForMove.Count > 0);
        }


        private void UpdateEnemiesForMove(bool canUpdate)
        {
            if (canUpdate == false) return;
            
            
            foreach (var index in  _changedStateBattlefields)
            {
                ref var changeStateEvent = 
                    ref _changedStateBattlefields.GetEntity(index).Get<ChangedStateBattlefieldEvent>();
                
                ref var entity = ref changeStateEvent.Battlefield;
                ref var visitors = ref entity.Get<Battlefield>().Visitors;


                foreach (var visitor in visitors)
                {
                    if (visitor.Get<Fighter>().BattleSide != BattleSide.Enemy) continue;
                    if (visitor.Get<Fighter>().State != FighterState.Alive) continue;
                        
                        
                    var fighter = new PlacebleFighter {Entity = visitor, Place = entity};

                    if (_enemiesForMove.Contains(fighter) == false)
                    { 
                        fighter.Entity.Get<Movable>().IsMovable = false; 
                        _enemiesForMove.Add(fighter);
                    }
                }
            }
        }

        private void MoveEnemies(bool canMove)
        {
            if (canMove == false) return;


            foreach (var enemy in _enemiesForMove)
            {
                ref var battlefield = ref enemy.Place.Get<Battlefield>();
                bool enemyOnTheMove;
                
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
                            enemyOnTheMove = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                defencePlacementPositions.Last(), 
                                0.05f);
                            
                            enemy.Entity.Get<GameObj>().Value.transform.LookAt(mainAssaultPoint);
                        }
                        else
                        {
                            enemyOnTheMove = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                defencePlacementPositions[_defencePositionsIndex++], 
                                0.05f);

                            enemy.Entity.Get<GameObj>().Value.transform.LookAt(mainAssaultPoint);
                        }
                        
                        
                        if (_defencePositionsIndex >= defencePlacementPositions.Count - 1) _defencePositionsIndex = 0;
                        
                        if (enemyOnTheMove == false && _enemiesCompleteMove.Contains(enemy) == false || 
                            enemy.Entity.Get<Fighter>().State == FighterState.Disabled)
                            _enemiesCompleteMove.Add(enemy);
                        
                        break;

                    default:
                        
                        var freePlacementPositions = new List<Vector3>();
                        var standPoints = battlefield.StandPoints;

                        for (var i = 0; i < standPoints.childCount; i++)
                            freePlacementPositions.Add(standPoints.GetChild(i).position);


                        if (enemy.Entity.Has<BossTag>())
                        {
                            enemyOnTheMove = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                freePlacementPositions.Last(), 
                                0.05f);
                            
                            enemy.Entity.Get<GameObj>().Value.transform.rotation = Quaternion.Euler(0,180,0);
                        }
                        else
                        {
                            enemyOnTheMove = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                freePlacementPositions[_freePositionsIndex++],
                                0.05f);
                            
                            enemy.Entity.Get<GameObj>().Value.transform.rotation = Quaternion.Euler(0,180,0);
                        }

                        
                        if (enemyOnTheMove == false && _enemiesCompleteMove.Contains(enemy) == false || 
                            enemy.Entity.Get<Fighter>().State == FighterState.Disabled)
                            _enemiesCompleteMove.Add(enemy);

                        if (_freePositionsIndex >= freePlacementPositions.Count - 1) _freePositionsIndex = 0;
                        
                        break;
                }
            }
        }
        
        private void ClearEnemiesForMove(bool canClear)
        {
            if (canClear == false) return;


            var enemies = new List<EcsEntity>();

            foreach (var enemy in _enemiesCompleteMove)
            {
                enemy.Entity.Get<Movable>().IsMovable = true;
                enemies.Add(enemy.Entity);
            }

            _gameTools.Events.EndPlacementFighterSquad(BattleSide.Enemy, enemies, _enemiesCompleteMove.First().Place);
            
            _enemiesForMove.Clear();
            _enemiesCompleteMove.Clear();
        }
    }
}