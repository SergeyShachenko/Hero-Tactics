using General.Components.Links;
using Leopotam.Ecs;

namespace General.Systems.Battle
{
    sealed class BattlefieldSystem : IEcsInitSystem
    {
        private EcsWorld _world;

        private EcsFilter<BattlefieldLink> _battlefieldsFilter;
        
        
        void IEcsInitSystem.Init()
        {
            foreach (var index in _battlefieldsFilter)
            {
                ref EcsEntity entity = ref _battlefieldsFilter.GetEntity(index);
                var battlefield = entity.Get<BattlefieldLink>();

                CleaningTrashWarriors(battlefield);
            }    
        }


        private void CleaningTrashWarriors(BattlefieldLink battlefield)
        {
            var warriors = battlefield.Warriors;
            
            
            if (warriors.Count > 3)
            {
                battlefield.Warriors.RemoveRange(3, warriors.Count-3);
            }
            
            foreach (var warrior in warriors)
            {
                if (!warrior.isBoss) continue;
                
                var boss = warrior;
                var tempList = battlefield.Warriors;
                    
                tempList.Clear();
                tempList.Add(boss);

                battlefield.Warriors = tempList;
                    
                break;
            }
        }
    }
}
