using Components;
using Components.Events.Game;
using Components.UI;
using Leopotam.Ecs;

namespace Systems.UI
{
    public sealed class EndGameScreenSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ChangedGameStateEvent> _changedGameStateEvents;
        private readonly EcsFilter<EndGameScreens> _endGameScreensFilter;


        void IEcsRunSystem.Run()
        {
            if (_changedGameStateEvents.IsEmpty() || _endGameScreensFilter.IsEmpty()) return;


            switch (_changedGameStateEvents.Get1(0).State)
            {
                case GameState.Win:
                {
                    _endGameScreensFilter.Get1(0).WinScreen.SetActive(true);
                    
                    break;
                }

                case GameState.GameOver:
                {
                    _endGameScreensFilter.Get1(0).GameOverScreen.SetActive(true);
                    
                    break;
                }
            }
        }
    }
}