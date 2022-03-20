using Components;
using Components.Others;
using Leopotam.Ecs;

namespace Systems.UI
{
    public sealed class HealthBarBillboardSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CameraComp> _cameraFilter;
        private readonly EcsFilter<HealthBar> _healthBarFilter;
        
        
        void IEcsRunSystem.Run()
        {
            UpdateBillboardView();
        }

        
        private void UpdateBillboardView()
        {
            if (_cameraFilter.IsEmpty()) return;


            foreach (var index in _healthBarFilter)
            {
                ref var canvas = ref _healthBarFilter.Get1(index).Canvas;
                var camera = _cameraFilter.GetEntity(0).Get<GameObj>().Value.transform;

                canvas.LookAt(canvas.position + camera.forward);
            }
        }
    }
}