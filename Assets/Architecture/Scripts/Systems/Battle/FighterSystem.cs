using Components.Battle;
using Components.Events.Battle;
using Services;
using Leopotam.Ecs;

namespace Systems.Battle
{
    public sealed class FighterSystem : IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<ChangedBattlefieldStateEvent> _changedStateBattlefields;
        private readonly EcsFilter<EndBattleEvent> _endBattles;
        private readonly EcsFilter<Fighter> _fighters;


        void IEcsRunSystem.Run()
        {
            UpdateActions();
        }

        
        private void UpdateActions()
        {
            if (_changedStateBattlefields.IsEmpty() == false)
            {
                foreach (var index in _changedStateBattlefields)
                {
                    ref var changeBattlefieldState = 
                        ref _changedStateBattlefields.GetEntity(index).Get<ChangedBattlefieldStateEvent>();

                    ref var battlefield = ref changeBattlefieldState.Battlefield.Get<Battlefield>();

                    if (battlefield.State != BattlefieldState.Battle) continue;
                    
                    
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
                    
                    _gameTools.Fighter.Squad.SetAction(FighterAction.Attack, assaultSquadID, _fighters);
                    _gameTools.Fighter.Squad.SetAction(FighterAction.Attack, defenceSquadID, _fighters);
                }
            }
            
            if (_endBattles.IsEmpty() == false)
            {
                foreach (var index in _endBattles)
                {
                    ref var endBattle = ref _endBattles.GetEntity(index).Get<EndBattleEvent>();
                
                    _gameTools.Fighter.Squad.SetAction(FighterAction.None, endBattle.AssaultSquadID, _fighters);
                    _gameTools.Fighter.Squad.SetAction(FighterAction.None, endBattle.DefenceSquadID, _fighters);
                }
            }
        }
    }
}