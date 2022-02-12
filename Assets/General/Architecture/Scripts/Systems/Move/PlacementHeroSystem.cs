﻿using System.Collections.Generic;
using System.Linq;
using General.Components;
using General.Components.Battle;
using General.Components.Events.Battle;
using General.Components.Events.Unity;
using General.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace General.Systems.Move
{
    public sealed class PlacementHeroSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggerEnterEvents;
        private readonly EcsFilter<BattlefieldChangeStateEvent> _battlefieldChangeStateEvents;

        private List<PlacebleFighter> _heroesForMove, _heroesCompleteMove;
        private int _assaultPositionsIndex, _freePositionsIndex;

        
        void IEcsInitSystem.Init()
        {
            _heroesForMove = new List<PlacebleFighter>();
            _heroesCompleteMove = new List<PlacebleFighter>();
        }

        void IEcsRunSystem.Run()
        {
            UpdateHeroesForMove(canUpdate:
                _onTriggerEnterEvents.IsEmpty() == false || _battlefieldChangeStateEvents.IsEmpty() == false);

            MoveHeroes(canMove:
                _heroesForMove.Count > 0);
            
            ClearHeroesForMove(canClear:
                _heroesForMove.Count == _heroesCompleteMove.Count && _heroesForMove.Count > 0);
        }
        

        private void UpdateHeroesForMove(bool canUpdate)
        {
            if (canUpdate == false) return;

            if (_onTriggerEnterEvents.IsEmpty() == false)
            {
                foreach (var index in _onTriggerEnterEvents)
                {
                    ref var triggerEvent = ref _onTriggerEnterEvents.GetEntity(index).Get<OnTriggerEnterEvent>();
                    ref var eventSender = ref triggerEvent.SenderEntity;
                    ref var eventVisitor = ref triggerEvent.VisitorEntity;
                
                    if (eventSender.Has<Battlefield>() == false || eventVisitor.Has<Fighter>() == false) continue;
                    if (eventVisitor.Get<Fighter>().State != FighterState.Alive) continue;

                
                    var hero = new PlacebleFighter {Entity = eventVisitor, Place = eventSender};

                    if (_heroesForMove.Contains(hero) == false && hero.Entity.Get<Fighter>().BattleSide == BattleSide.Hero)
                    {
                        hero.Entity.Get<Movable>().IsMovable = false;
                        _heroesForMove.Add(hero);
                    }
                }
            }

            if (_battlefieldChangeStateEvents.IsEmpty() == false)
            {
                foreach (var index in  _battlefieldChangeStateEvents)
                {
                    ref var changeStateEvent = 
                        ref _battlefieldChangeStateEvents.GetEntity(index).Get<BattlefieldChangeStateEvent>();
                
                    ref var entity = ref changeStateEvent.BattlefieldEntity;
                    ref var visitors = ref entity.Get<Battlefield>().Visitors;
                    
                    
                    foreach (var visitor in visitors)
                    {
                        if (visitor.Get<Fighter>().BattleSide != BattleSide.Hero) continue;
                        if (visitor.Get<Fighter>().State != FighterState.Alive) continue;
                        
                        
                        var fighter = new PlacebleFighter {Entity = visitor, Place = entity};

                        if (_heroesForMove.Contains(fighter) == false)
                        { 
                            fighter.Entity.Get<Movable>().IsMovable = false; 
                            _heroesForMove.Add(fighter);
                        }
                    }
                }
            }
        }

        private void MoveHeroes(bool canMove)
        {
            if (canMove == false) return;


            foreach (var hero in _heroesForMove)
            {
                ref var battlefield = ref hero.Place.Get<Battlefield>();
                bool heroOnTheMove;
                
                switch (battlefield.State)
                {
                    case BattlefieldState.Battle:
                        
                        var assaultPlacementPositions = new List<Vector3>();
                        var assaultPoints = battlefield.BattlePoints.GetChild(0);
                        var mainDefencePoint = battlefield.BattlePoints.GetChild(1).GetChild(0);

                        for (var i = 0; i < assaultPoints.childCount; i++)
                            assaultPlacementPositions.Add(assaultPoints.GetChild(i).position);


                        heroOnTheMove = _gameTools.Gameplay.MoveEntityTo(
                            hero.Entity, 
                            assaultPlacementPositions[_assaultPositionsIndex++], 
                            0.05f);
                        
                        hero.Entity.Get<GameObj>().Value.transform.LookAt(mainDefencePoint);
                        hero.Entity.Get<Movable>().State = heroOnTheMove ? MovableState.Walk : MovableState.Stand;


                        if (heroOnTheMove == false && _heroesCompleteMove.Contains(hero) == false)
                            _heroesCompleteMove.Add(hero);

                        if (_assaultPositionsIndex >= assaultPlacementPositions.Count) _assaultPositionsIndex = 0;
                        
                        break;

                    default:
                        
                        var freePlacementPositions = new List<Vector3>();
                        var standPoints = battlefield.StandPoints;

                        for (var i = 0; i < standPoints.childCount; i++)
                            freePlacementPositions.Add(standPoints.GetChild(i).position);


                        heroOnTheMove = _gameTools.Gameplay.MoveEntityTo(
                            hero.Entity, 
                            freePlacementPositions[_freePositionsIndex++], 
                            0.05f);
                        
                        hero.Entity.Get<GameObj>().Value.transform.rotation = Quaternion.Euler(Vector3.zero);
                        hero.Entity.Get<Movable>().State = heroOnTheMove ? MovableState.Run : MovableState.Stand;

                        
                        if (heroOnTheMove == false && _heroesCompleteMove.Contains(hero) == false)
                            _heroesCompleteMove.Add(hero);

                        if (_freePositionsIndex >= freePlacementPositions.Count - 1) _freePositionsIndex = 0;
                        
                        break;
                }
            }
        }
        
        private void ClearHeroesForMove(bool canClear)
        {
            if (canClear == false) return;


            var heroes = new List<EcsEntity>();

            foreach (var hero in _heroesCompleteMove)
            {
                hero.Entity.Get<Movable>().IsMovable = true;
                heroes.Add(hero.Entity);
            }

            _gameTools.Events.EndPlacementFighterSquad(BattleSide.Hero, heroes, _heroesCompleteMove.First().Place);
            
            _heroesForMove.Clear();
            _heroesCompleteMove.Clear();
        }
    }
    
    
    public struct PlacebleFighter
    {
        public EcsEntity Entity;
        public EcsEntity Place;
    }
}