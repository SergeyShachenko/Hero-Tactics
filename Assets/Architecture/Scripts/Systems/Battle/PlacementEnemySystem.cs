using System.Collections.Generic;
using System.Linq;
using Systems.Battle;
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
        
        private readonly EcsFilter<BattlefieldChangeStateEvent> _battlefieldChangeStateEvents;

        private List<PlacebleFighter> _enemysForMove, _enemysCompleteMove;
        private int _defencePositionsIndex, _freePositionsIndex;

        
        void IEcsInitSystem.Init()
        {
            _enemysForMove = new List<PlacebleFighter>();
            _enemysCompleteMove = new List<PlacebleFighter>();
        }

        void IEcsRunSystem.Run()
        {
            UpdateEnemysForMove(canUpdate:_battlefieldChangeStateEvents.IsEmpty() == false);

            MoveEnemys(canMove:_enemysForMove.Count > 0);
            
            ClearEnemysForMove(canClear:_enemysForMove.Count == _enemysCompleteMove.Count && _enemysForMove.Count > 0);
        }


        private void UpdateEnemysForMove(bool canUpdate)
        {
            if (canUpdate == false) return;
            
            
            foreach (var index in  _battlefieldChangeStateEvents)
            {
                ref var changeStateEvent = 
                    ref _battlefieldChangeStateEvents.GetEntity(index).Get<BattlefieldChangeStateEvent>();
                
                ref var entity = ref changeStateEvent.BattlefieldEntity;
                ref var visitors = ref entity.Get<Battlefield>().Visitors;
                    
                    
                foreach (var visitor in visitors)
                {
                    if (visitor.Get<Fighter>().BattleSide != BattleSide.Enemy) continue;
                    if (visitor.Get<Fighter>().State != FighterState.Alive) continue;
                        
                        
                    var fighter = new PlacebleFighter {Entity = visitor, Place = entity};

                    if (_enemysForMove.Contains(fighter) == false)
                    { 
                        fighter.Entity.Get<Movable>().IsMovable = false; 
                        _enemysForMove.Add(fighter);
                    }
                }
            }
        }

        private void MoveEnemys(bool canMove)
        {
            if (canMove == false) return;


            foreach (var enemy in _enemysForMove)
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
                            enemy.Entity.Get<Movable>().State = enemyOnTheMove ? MovableState.Walk : MovableState.Stand;
                        }
                        else
                        {
                            enemyOnTheMove = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                defencePlacementPositions[_defencePositionsIndex++], 
                                0.05f);

                            enemy.Entity.Get<GameObj>().Value.transform.LookAt(mainAssaultPoint);
                            enemy.Entity.Get<Movable>().State = enemyOnTheMove ? MovableState.Walk : MovableState.Stand;
                        }
                        
                        
                        if (_defencePositionsIndex >= defencePlacementPositions.Count - 1) _defencePositionsIndex = 0;
                        
                        if (enemyOnTheMove == false && _enemysCompleteMove.Contains(enemy) == false)
                            _enemysCompleteMove.Add(enemy);
                        
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
                            enemy.Entity.Get<Movable>().State = enemyOnTheMove ? MovableState.Walk : MovableState.Stand;
                        }
                        else
                        {
                            enemyOnTheMove = _gameTools.Gameplay.MoveEntityTo(
                                enemy.Entity, 
                                freePlacementPositions[_freePositionsIndex++],
                                0.05f);
                            
                            enemy.Entity.Get<GameObj>().Value.transform.rotation = Quaternion.Euler(0,180,0);
                            enemy.Entity.Get<Movable>().State = enemyOnTheMove ? MovableState.Walk : MovableState.Stand;
                        }

                        
                        if (enemyOnTheMove == false && _enemysCompleteMove.Contains(enemy) == false || 
                            enemy.Entity.Get<Fighter>().State == FighterState.Disabled)
                            _enemysCompleteMove.Add(enemy);

                        if (_freePositionsIndex >= freePlacementPositions.Count - 1) _freePositionsIndex = 0;
                        
                        break;
                }
            }
        }
        
        private void ClearEnemysForMove(bool canClear)
        {
            if (canClear == false) return;


            foreach (var enemy in _enemysCompleteMove) enemy.Entity.Get<Movable>().IsMovable = true;

            _enemysForMove.Clear();
            _enemysCompleteMove.Clear();
        }
    }
}