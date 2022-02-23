using System.Collections.Generic;
using System.Linq;
using Components;
using Components.Battle;
using Components.Events.Battle;
using Services;
using Leopotam.Ecs;
using UnityComponents.Data;
using UnityEngine;

namespace Systems.Battle
{
    public sealed class BattleSystem : IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        private readonly GameSettings _gameSettings;
        
        private readonly EcsFilter<EndPlacementFighterSquadEvent> _endPlacementFighterSquads;
        private readonly EcsFilter<EndBattleEvent> _endBattles;
        private readonly EcsFilter<Fighter> _fighters;

        private FighterSquad? _assaultSquad, _defenceSquad;
        
        
        void IEcsRunSystem.Run()
        {
            UpdateFighterSquads(canUpdate:
                _endPlacementFighterSquads.IsEmpty() == false);
            
            Battle(canFight:
                _assaultSquad.HasValue && _defenceSquad.HasValue);
            
            ProcessEndBattle(canProcess:
                _endBattles.IsEmpty() == false);
        }

        
        private void UpdateFighterSquads(bool canUpdate)
        {
            if (canUpdate == false) return;


            foreach (var index in _endPlacementFighterSquads)
            {
                ref var endPlacementSquad =
                    ref _endPlacementFighterSquads.GetEntity(index).Get<EndPlacementFighterSquadEvent>();

                ref var place = ref endPlacementSquad.Place;
                
                if (place.Has<Battlefield>() == false) continue;
                if (place.Get<Battlefield>().State != BattlefieldState.Battle) continue;


                ref var battlefield = ref place.Get<Battlefield>();
                var visitorsEndMove = true;
                
                foreach (var visitor in battlefield.Visitors.Where(
                    visitor => visitor.Get<Movable>().IsMovable == false)) visitorsEndMove = false;
                
                if (visitorsEndMove == false) continue;

                
                var heroSquad = new List<EcsEntity>();
                var enemySquad = new List<EcsEntity>();

                switch (endPlacementSquad.BattleSide)
                {
                    case BattleSide.Hero:
                        
                        heroSquad = endPlacementSquad.Fighters;

                        enemySquad.AddRange(battlefield.Visitors.Where(
                            visitor => visitor.Get<Fighter>().BattleSide == BattleSide.Enemy));

                        break;
                    
                    case BattleSide.Enemy:
                        
                        enemySquad = endPlacementSquad.Fighters;

                        heroSquad.AddRange(battlefield.Visitors.Where(
                            visitor => visitor.Get<Fighter>().BattleSide == BattleSide.Hero));

                        break;
                }

                _assaultSquad = _gameTools.Fighter.Squad.Create(ref place, heroSquad);
                _defenceSquad = _gameTools.Fighter.Squad.Create(ref place, enemySquad);
            }
        }

        private void Battle(bool canFight)
        {
            if (canFight == false) return;
            
            
            var defenceSquad = _defenceSquad.Value;
            var assaultSquad = _assaultSquad.Value;

            while (defenceSquad.State == SquadState.Alive && assaultSquad.State == SquadState.Alive)
            {
                _gameTools.Fighter.Squad.TakeDamage(ref defenceSquad, assaultSquad.Stats.Damage);
                _gameTools.Fighter.Squad.TakeDamage(ref assaultSquad, defenceSquad.Stats.Damage);
            }

            _gameTools.Events.Battle.End(assaultSquad.ID, defenceSquad.ID, ref assaultSquad.Place);

            _assaultSquad = assaultSquad;
            _defenceSquad = defenceSquad;
        }

        private void ProcessEndBattle(bool canProcess)
        {
            if (canProcess == false) return;

            
            switch (_assaultSquad.Value.State)
            {
                case SquadState.Alive:
                {
                    var fighters = _gameTools.Fighter.Squad.Get(_assaultSquad.Value.ID, _fighters);

                    foreach (var fighter in fighters)
                    {
                        var entity = fighter;
                        _gameTools.Fighter.TakeDamageInPercent(ref entity, _gameSettings.ImminentDamageInPercent);
                    }

                    break;
                }
                
                case SquadState.Dead:
                {
                    var fighters = _gameTools.Fighter.Squad.Get(_assaultSquad.Value.ID, _fighters);
                
                    foreach (var fighter in fighters)
                    {
                        var entity = fighter;
                        _gameTools.Fighter.TakeDamageInPercent(ref entity, 1f);
                    }

                    break;
                }
            }

            switch (_defenceSquad.Value.State)
            {
                case SquadState.Alive:
                {
                    var fighters = _gameTools.Fighter.Squad.Get(_defenceSquad.Value.ID, _fighters);

                    foreach (var fighter in fighters)
                    {
                        var entity = fighter;
                        _gameTools.Fighter.TakeDamageInPercent(ref entity, _gameSettings.ImminentDamageInPercent);
                    }

                    break;
                }
                
                case SquadState.Dead:
                {
                    var fighters = _gameTools.Fighter.Squad.Get(_defenceSquad.Value.ID, _fighters);

                    foreach (var fighter in fighters)
                    {
                        var entity = fighter;
                        _gameTools.Fighter.TakeDamageInPercent(ref entity, 1f);
                    }

                    break;
                }
            }

            _assaultSquad = null;
            _defenceSquad = null;
        }
    }
}