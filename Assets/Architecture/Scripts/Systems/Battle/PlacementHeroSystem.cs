using System.Collections.Generic;
using System.Linq;
using Components;
using Components.Battle;
using Components.Events.Battle;
using Components.Events.Physics;
using Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Battle
{
    public sealed class PlacementHeroSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<ChangedBattlefieldStateEvent> _changedStateBattlefieldEvents;
        private readonly EcsFilter<OnTriggerEnterEvent> _onTriggersEnterEvents;

        private HashSet<PlaceableFighter> _heroesForMove, _heroesCompleteMove;
        private int _assaultPositionsIndex, _freePositionsIndex;

        
        void IEcsInitSystem.Init()
        {
            _heroesForMove = new HashSet<PlaceableFighter>();
            _heroesCompleteMove = new HashSet<PlaceableFighter>();
        }

        void IEcsRunSystem.Run()
        {
            UpdateHeroesForMove(canUpdate:
                _onTriggersEnterEvents.IsEmpty() == false || _changedStateBattlefieldEvents.IsEmpty() == false);

            MoveHeroes(canMove: _heroesForMove.Count > 0);
            
            ClearHeroesForMove(canClear:
                _heroesForMove.Count == _heroesCompleteMove.Count && _heroesForMove.Count > 0);
        }
        

        private void UpdateHeroesForMove(bool canUpdate)
        {
            if (canUpdate == false) return;

            
            foreach (var index in _onTriggersEnterEvents)
            {
                ref var onTriggerEnterEvent = ref _onTriggersEnterEvents.Get1(index);
                ref var eventSender = ref onTriggerEnterEvent.Sender;
                ref var eventVisitor = ref onTriggerEnterEvent.Visitor;
                
                if (eventSender.Has<Battlefield>() == false || eventVisitor.Has<Fighter>() == false) continue;
                if (eventVisitor.Get<Fighter>().State != FighterState.Alive) continue;

                
                var hero = new PlaceableFighter {Entity = eventVisitor, Place = eventSender};

                if (hero.Entity.Get<Fighter>().BattleSide == BattleSide.Hero)
                {
                    hero.Entity.Get<Movable>().IsMovable = false;
                    _heroesForMove.Add(hero);
                }
            }

            foreach (var index in  _changedStateBattlefieldEvents)
            {
                ref var changeStateEvent = ref _changedStateBattlefieldEvents.Get1(index);
                ref var entity = ref changeStateEvent.Battlefield;
                ref var visitors = ref entity.Get<Battlefield>().Visitors;
                    
                    
                foreach (var visitor in visitors)
                {
                    if (visitor.Get<Fighter>().BattleSide != BattleSide.Hero) continue;
                    if (visitor.Get<Fighter>().State != FighterState.Alive) continue;
                        
                        
                    var fighter = new PlaceableFighter {Entity = visitor, Place = entity};

                    if (_heroesForMove.Contains(fighter) == false)
                    { 
                        fighter.Entity.Get<Movable>().IsMovable = false; 
                        _heroesForMove.Add(fighter);
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
                            assaultPlacementPositions[_assaultPositionsIndex++]);
                        
                        hero.Entity.Get<ModelParent>().GameObject.transform.LookAt(mainDefencePoint);


                        if (heroOnTheMove == false && _heroesCompleteMove.Contains(hero) == false)
                            _heroesCompleteMove.Add(hero);

                        if (_assaultPositionsIndex >= assaultPlacementPositions.Count) 
                            _assaultPositionsIndex = 0;
                        
                        break;

                    default:
                        
                        var freePlacementPositions = new List<Vector3>();
                        var standPoints = battlefield.StandPoints;

                        for (var i = 0; i < standPoints.childCount; i++)
                            freePlacementPositions.Add(standPoints.GetChild(i).position);


                        heroOnTheMove = _gameTools.Gameplay.MoveEntityTo(
                            hero.Entity, 
                            freePlacementPositions[_freePositionsIndex++]);
                        
                        hero.Entity.Get<ModelParent>().GameObject.transform.rotation = 
                            Quaternion.Euler(battlefield.PlacementHeroRotation);


                        if (heroOnTheMove == false && _heroesCompleteMove.Contains(hero) == false)
                            _heroesCompleteMove.Add(hero);

                        if (_freePositionsIndex >= freePlacementPositions.Count - 1) 
                            _freePositionsIndex = 0;
                        
                        break;
                }
            }
        }
        
        private void ClearHeroesForMove(bool canClear)
        {
            if (canClear == false) return;


            var heroes = new HashSet<EcsEntity>();

            foreach (var hero in _heroesCompleteMove)
            {
                hero.Entity.Get<Movable>().IsMovable = true;
                heroes.Add(hero.Entity);
            }

            _gameTools.Events.Move.EndPlacementFighterSquad(BattleSide.Hero, heroes, _heroesCompleteMove.First().Place);
            
            _heroesForMove.Clear();
            _heroesCompleteMove.Clear();
        }
    }
    
    
    public struct PlaceableFighter
    {
        public EcsEntity Entity;
        public EcsEntity Place;
    }
}