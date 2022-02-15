using Components.Battle;
using Components.Events.Battle;
using Services;
using Leopotam.Ecs;

namespace Systems.Battle
{
    public sealed class FighterSystem : IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<ChangedStateBattlefieldEvent> _changedStateBattlefields;
        private readonly EcsFilter<StartBattleEvent> _startBattles;
        private readonly EcsFilter<EndBattleEvent> _endBattles;
        private readonly EcsFilter<Fighter> _fighters;


        void IEcsRunSystem.Run()
        {
            UpdateState();
            UpdateActions();
        }


        private void UpdateState()
        {
            if (_fighters.IsEmpty()) return;
            
            
            foreach (var index in _fighters)
            {
                ref var entity = ref _fighters.GetEntity(index);
                ref var fighter = ref entity.Get<Fighter>();


                if (fighter.Stats.Health > 0 && fighter.State != FighterState.Alive && fighter.State != FighterState.Disabled)
                {
                    fighter.State = FighterState.Alive;
                }
                
                if (fighter.Stats.Health <= 0 && fighter.State == FighterState.Alive)
                {
                    fighter.State = FighterState.Dead;
                    _gameTools.Events.DeadFighter(ref entity);
                }
            }
        }
        
        private void UpdateActions()
        {
            if (_changedStateBattlefields.IsEmpty() == false)
            {
                foreach (var index in _changedStateBattlefields)
                {
                    ref var changeBattlefieldState = 
                        ref _changedStateBattlefields.GetEntity(index).Get<ChangedStateBattlefieldEvent>();

                    ref var battlefield = ref changeBattlefieldState.Battlefield.Get<Battlefield>();

                    if (battlefield.State != BattlefieldState.Battle) return;
                    
                    
                    int assaultSquadID = 0, defenceSquadID = 0;
                    
                    foreach (var visitor in battlefield.Visitors)
                    {
                        var fighter = visitor.Get<Fighter>();

                        if (fighter.BattleSide == BattleSide.Hero)
                        {
                            assaultSquadID = fighter.SquadID;
                        }
                        else
                        {
                            defenceSquadID = fighter.SquadID;
                        }
                    }
                    
                    _gameTools.SetActionForFighterSquad(FighterAction.Attack, assaultSquadID, _fighters);
                    _gameTools.SetActionForFighterSquad(FighterAction.Attack, defenceSquadID, _fighters);
                }
            }

            if (_endBattles.IsEmpty() == false)
            {
                foreach (var index in _endBattles)
                {
                    ref var endBattle = ref _endBattles.GetEntity(index).Get<EndBattleEvent>();
                
                    _gameTools.SetActionForFighterSquad(FighterAction.None, endBattle.AssaultSquadID, _fighters);
                    _gameTools.SetActionForFighterSquad(FighterAction.None, endBattle.DefenceSquadID, _fighters);
                }
            }
        }
    }
}