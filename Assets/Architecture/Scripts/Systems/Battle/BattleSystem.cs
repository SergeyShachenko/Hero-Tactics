using System.Collections.Generic;
using Components;
using Components.Battle;
using Components.Events.Battle;
using Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Battle
{
    public sealed class BattleSystem : IEcsRunSystem
    {
        private GameTools _gameTools;
        
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
                
                if (place.Has<Battlefield>() == false) return;


                ref var battlefield = ref place.Get<Battlefield>();
                
                
                if (battlefield.State != BattlefieldState.Battle) return;
                
                foreach (var visitor in battlefield.Visitors)
                    if (visitor.Get<Movable>().IsMovable == false) return;

                
                var heroSquad = new List<EcsEntity>();
                var enemySquad = new List<EcsEntity>();

                switch (endPlacementSquad.BattleSide)
                {
                    case BattleSide.Hero:
                        
                        heroSquad = endPlacementSquad.Fighters;

                        foreach (var visitor in battlefield.Visitors)
                        {
                            if (visitor.Get<Fighter>().BattleSide == BattleSide.Enemy) enemySquad.Add(visitor);
                        }
                        
                        break;
                    
                    case BattleSide.Enemy:
                        
                        enemySquad = endPlacementSquad.Fighters;

                        foreach (var visitor in battlefield.Visitors)
                        {
                            if (visitor.Get<Fighter>().BattleSide == BattleSide.Hero) heroSquad.Add(visitor);
                        }

                        break;
                }

                _assaultSquad = _gameTools.CreateFighterSquad(ref place, heroSquad);
                _defenceSquad = _gameTools.CreateFighterSquad(ref place, enemySquad);
            }
        }

        private void Battle(bool canFight)
        {
            if (canFight == false) return;
            
            
            var defenceSquad = _defenceSquad.Value;
            var assaultSquad = _assaultSquad.Value;
            

            while (defenceSquad.Stats.Health > 0 && assaultSquad.Stats.Health > 0)
            {
                defenceSquad.Stats.Health -= (assaultSquad.Stats.Damage / 100) * (100 - defenceSquad.Stats.Armor);
                assaultSquad.Stats.Health -= (defenceSquad.Stats.Damage / 100) * (100 - assaultSquad.Stats.Armor);
            }
            
            if (assaultSquad.Stats.Health <= 0) assaultSquad.State = SquadState.Dead;
            if (defenceSquad.Stats.Health <= 0) defenceSquad.State = SquadState.Dead;
            
            _gameTools.Events.EndBattle(assaultSquad.ID, defenceSquad.ID, ref assaultSquad.Place);

            _assaultSquad = assaultSquad;
            _defenceSquad = defenceSquad;
        }

        private void ProcessEndBattle(bool canProcess)
        {
            if (canProcess == false) return;

            
            if (_assaultSquad.Value.State == SquadState.Alive)
            {
                var fighters = _gameTools.GetFighterSquad(_assaultSquad.Value.ID, _fighters);

                foreach (var fighter in fighters)
                    _gameTools.Gameplay.TakeDamageInPercent(20, fighter);
            }
            else if (_assaultSquad.Value.State == SquadState.Dead)
            {
                var fighters = _gameTools.GetFighterSquad(_assaultSquad.Value.ID, _fighters);

                foreach (var fighter in fighters) 
                    _gameTools.Gameplay.TakeDamageInPercent(100, fighter);
            }

            if (_defenceSquad.Value.State == SquadState.Alive)
            {
                var fighters = _gameTools.GetFighterSquad(_defenceSquad.Value.ID, _fighters);

                foreach (var fighter in fighters)
                    _gameTools.Gameplay.TakeDamageInPercent(20, fighter);
            }
            else if (_defenceSquad.Value.State == SquadState.Dead)
            {
                var fighters = _gameTools.GetFighterSquad(_defenceSquad.Value.ID, _fighters);

                foreach (var fighter in fighters) 
                    _gameTools.Gameplay.TakeDamageInPercent(100, fighter);
            }

            _assaultSquad = null;
            _defenceSquad = null;
        }
    }
}