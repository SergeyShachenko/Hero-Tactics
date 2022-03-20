using Components;
using Components.Battle;
using Components.Others;
using Components.Tags;
using Leopotam.Ecs;

namespace Systems.Startup
{
    public sealed class CameraSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Fighter, PlayerTag> _playerFilter;
        private readonly EcsFilter<CinemachineFreeLookComp> _freeLookCameraFilter;

        private bool _cameraAttached;
        

        void IEcsRunSystem.Run()
        {
            AttachCamera(canAttach: _cameraAttached == false);
        }

        private void AttachCamera(bool canAttach)
        {
            if (canAttach == false || _playerFilter.IsEmpty() || _freeLookCameraFilter.IsEmpty()) return;

            
            var target = _playerFilter.GetEntity(0).Get<GameObj>().Value.transform;
            ref var camera = ref _freeLookCameraFilter.Get1(0).Value;

            camera.m_YAxis.Value = 0.15f;
            camera.m_XAxis.Value = 16f;
            camera.GetRig(2).m_LookAt = target;
            _cameraAttached = true;
        }
    }
}