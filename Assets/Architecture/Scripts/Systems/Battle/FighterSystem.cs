using Components.Battle;
using Components.Events.Battle;
using Services;
using Leopotam.Ecs;

namespace Systems.Battle
{
    public sealed class FighterSystem : IEcsRunSystem
    {
        private readonly GameTools _gameTools;
        
        private readonly EcsFilter<ChangedBattlefieldStateEvent> _changedStateBattlefieldEvents;
        private readonly EcsFilter<EndBattleEvent> _endBattleEvents;
        private readonly EcsFilter<Fighter> _fighterFilter;


        void IEcsRunSystem.Run()
        {
            UpdateActions();
        }

        
        private void UpdateActions()
        {
            foreach (var index in _changedStateBattlefieldEvents)
            {
                ref var changeBattlefieldState = ref _changedStateBattlefieldEvents.Get1(index);
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
                    
                _gameTools.Fighter.Squad.SetAction(FighterAction.Attack, assaultSquadID, _fighterFilter);
                _gameTools.Fighter.Squad.SetAction(FighterAction.Attack, defenceSquadID, _fighterFilter);
            }
            
            foreach (var index in _endBattleEvents)
            {
                ref var endBattle = ref _endBattleEvents.Get1(index);
                
                _gameTools.Fighter.Squad.SetAction(FighterAction.None, endBattle.AssaultSquadID, _fighterFilter);
                _gameTools.Fighter.Squad.SetAction(FighterAction.None, endBattle.DefenceSquadID, _fighterFilter);
            }
        }
    }
}