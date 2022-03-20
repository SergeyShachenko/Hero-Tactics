using System.Collections.Generic;
using Components;
using Components.Battle;
using Components.Events.Battle;
using Services;
using Leopotam.Ecs;
using UnityComponents.Data;

namespace Systems.Battle
{
    public sealed class BattleSystem : IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        private readonly GameSettings _gameSettings;
        
        private readonly EcsFilter<EndPlacementFighterSquadEvent> _endPlacementFighterSquadEvents;
        private readonly EcsFilter<EndBattleEvent> _endBattleEvents;
        private readonly EcsFilter<Fighter> _fighterFilter;

        private FighterSquad? _assaultSquad, _defenceSquad;
        
        
        void IEcsRunSystem.Run()
        {
            SetSquads();
            Battle(canFight: _assaultSquad.HasValue && _defenceSquad.HasValue);
            ProcessEndBattle(canProcess: _endBattleEvents.IsEmpty() == false);
        }

        
        private void SetSquads()
        {
            foreach (var index in _endPlacementFighterSquadEvents)
            {
                ref var endPlacementFighterSquadEvent = ref _endPlacementFighterSquadEvents.Get1(index);
                ref var place = ref endPlacementFighterSquadEvent.Place;
                
                if (place.Has<Battlefield>() == false) continue;
                if (place.Get<Battlefield>().State != BattlefieldState.Battle) continue;


                ref var battlefield = ref place.Get<Battlefield>();
                var visitorsEndMove = true;

                foreach (var visitor in battlefield.Visitors)
                {
                    if (visitor.Get<Movable>().IsMovable == false)
                    {
                        visitorsEndMove = false;
                        break;
                    }
                }

                if (visitorsEndMove == false) continue;

                
                var heroSquad = new HashSet<EcsEntity>();
                var enemySquad = new HashSet<EcsEntity>();

                switch (endPlacementFighterSquadEvent.BattleSide)
                {
                    case BattleSide.Hero:
                    {
                        heroSquad = endPlacementFighterSquadEvent.Fighters;

                        foreach (var visitor in battlefield.Visitors)
                        {
                            if (visitor.Get<Fighter>().BattleSide == BattleSide.Enemy)
                                enemySquad.Add(visitor);
                        }

                        break;   
                    }

                    case BattleSide.Enemy:
                    {
                        enemySquad = endPlacementFighterSquadEvent.Fighters;

                        foreach (var visitor in battlefield.Visitors)
                        {
                            if (visitor.Get<Fighter>().BattleSide == BattleSide.Hero)
                                heroSquad.Add(visitor);
                        }

                        break;   
                    }
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
                    var fighters = _gameTools.Fighter.Squad.Get(_assaultSquad.Value.ID, _fighterFilter);

                    foreach (var fighter in fighters)
                    {
                        var entity = fighter;
                        _gameTools.Fighter.TakeDamageInPercent(ref entity, _gameSettings.ImminentDamageInPercent);
                    }

                    break;
                }
                
                case SquadState.Dead:
                {
                    var fighters = _gameTools.Fighter.Squad.Get(_assaultSquad.Value.ID, _fighterFilter);
                
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
                    var fighters = _gameTools.Fighter.Squad.Get(_defenceSquad.Value.ID, _fighterFilter);

                    foreach (var fighter in fighters)
                    {
                        var entity = fighter;
                        _gameTools.Fighter.TakeDamageInPercent(ref entity, _gameSettings.ImminentDamageInPercent);
                    }

                    break;
                }
                
                case SquadState.Dead:
                {
                    var fighters = _gameTools.Fighter.Squad.Get(_defenceSquad.Value.ID, _fighterFilter);

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