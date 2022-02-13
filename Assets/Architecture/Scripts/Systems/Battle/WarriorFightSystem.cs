using System.Collections.Generic;
using Components.Battle;
using Components.Events.Battle;
using Services;
using Leopotam.Ecs;
using Random = UnityEngine.Random;

namespace Systems.Battle
{
    public sealed class WarriorFightSystem : IEcsRunSystem
    {
        private GameTools _gameTools;
        
        private readonly EcsFilter<EndPlacementFighterSquadEvent> _endPlacementFighterSquadEvents;
        private readonly EcsFilter<EndFightEvent> _endFightEvents;
        private readonly EcsFilter<Fighter> _fighters;

        private FighterSquad? _assaultSquad, _defenceSquad;
        
        
        void IEcsRunSystem.Run()
        {
            UpdateFighterSquads(canUpdate:_endPlacementFighterSquadEvents.IsEmpty() == false);
            Fight(canFight:_assaultSquad.HasValue && _defenceSquad.HasValue);
            ProcessEndFight(canProcess:_endFightEvents.IsEmpty() == false);
        }


        private void Fight(bool canFight)
        {
            if (canFight == false) return;


            var defenceSquad = _defenceSquad.Value;
            var assaultSquad = _assaultSquad.Value;

            while (defenceSquad.Health > 0 && assaultSquad.Health > 0)
            {
                defenceSquad.Health -= (assaultSquad.Damage / 100) * (100 - defenceSquad.Armor);
                assaultSquad.Health -= (defenceSquad.Damage / 100) * (100 - assaultSquad.Armor);
            }
            
            
            if (assaultSquad.Health > defenceSquad.Health)
            {
                defenceSquad.State = SquadState.Dead;
            }
            else if (assaultSquad.Health < defenceSquad.Health)
            {
                assaultSquad.State = SquadState.Dead;
            }
            else
            {
                switch (Random.Range(1, 4))
                {
                    case 1: assaultSquad.State = SquadState.Dead; break;
                    
                    case 2: defenceSquad.State = SquadState.Dead; break;
                    
                    case 3:
                        assaultSquad.State = SquadState.Dead;
                        defenceSquad.State = SquadState.Dead;
                        break;
                }
            }

            _assaultSquad = assaultSquad;
            _defenceSquad = defenceSquad;
            
            _gameTools.Events.EndFight(ref assaultSquad.Place);
        }
        
        private void ProcessEndFight(bool canProcess)
        {
            if (canProcess == false) return;

            
            if (_assaultSquad.Value.State == SquadState.Dead)
            {
                var fighters = GetSquad(_assaultSquad.Value.ID);

                foreach (var entity in fighters) 
                    entity.Get<Fighter>().State = FighterState.Dead;
            }

            if (_defenceSquad.Value.State == SquadState.Dead)
            {
                var fighters = GetSquad(_defenceSquad.Value.ID);
                    
                foreach (var entity in fighters) 
                    entity.Get<Fighter>().State = FighterState.Dead;
            }

            _assaultSquad = null;
            _defenceSquad = null;
        }
        
        private void UpdateFighterSquads(bool canUpdate)
        {
            if (canUpdate == false) return;


            foreach (var index in _endPlacementFighterSquadEvents)
            {
                ref var endPlacementFighterSquadEvent =
                    ref _endPlacementFighterSquadEvents.GetEntity(index).Get<EndPlacementFighterSquadEvent>();

                ref var place = ref endPlacementFighterSquadEvent.PlaceEntity;
                
                if (place.Has<Battlefield>() == false) return;


                ref var battlefield = ref place.Get<Battlefield>();
                
                
                if (battlefield.State != BattlefieldState.Battle) return;


                var heroSquad = new List<EcsEntity>();
                var enemySquad = new List<EcsEntity>();

                switch (endPlacementFighterSquadEvent.BattleSide)
                {
                    case BattleSide.Hero:
                        
                        heroSquad = endPlacementFighterSquadEvent.Fighters;

                        foreach (var visitor in battlefield.Visitors)
                        {
                            if (visitor.Get<Fighter>().BattleSide == BattleSide.Enemy) enemySquad.Add(visitor);
                        }
                        
                        break;
                    
                    case BattleSide.Enemy:
                        
                        enemySquad = endPlacementFighterSquadEvent.Fighters;

                        foreach (var visitor in battlefield.Visitors)
                        {
                            if (visitor.Get<Fighter>().BattleSide == BattleSide.Hero) heroSquad.Add(visitor);
                        }

                        break;
                }

                _assaultSquad = CreateFighterSquad(ref place, heroSquad);
                _defenceSquad = CreateFighterSquad(ref place, enemySquad);
            }
        }

        private List<EcsEntity> GetSquad(int squadID)
        {
            if (_fighters.IsEmpty()) return null;


            var squad = new List<EcsEntity>();
            
            foreach (var index in _fighters)
            {
                ref var entity = ref _fighters.GetEntity(index);
                ref var fighter = ref entity.Get<Fighter>();

                if (fighter.SquadID == squadID) squad.Add(entity);
            }

            return squad.Count > 0 ? squad : null;
        }

        private FighterSquad CreateFighterSquad(ref EcsEntity place, List<EcsEntity> fighterSquad)
        {
            int squadHealth = 0, squadArmor = 0, squadDamage = 0, squadID = 0;
            BattleSide squadBattleSide = BattleSide.Hero;


            foreach (var entity in fighterSquad)
            {
                var fighter = entity.Get<Fighter>();

                squadBattleSide = fighter.BattleSide;
                squadID = fighter.SquadID;        
                squadHealth += fighter.Stats.Health;
                squadArmor += fighter.Stats.Armor;
                squadDamage += fighter.Stats.Damage;
            }
                    
            return new FighterSquad
            {
                BattleSide = squadBattleSide,
                State = SquadState.Alive,
                ID = squadID,
                Health = squadHealth,
                Armor = squadArmor,
                Damage = squadDamage,
                Place = place
            };
        }
    }
}