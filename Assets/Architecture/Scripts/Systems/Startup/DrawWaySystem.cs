using Components;
using Components.Battle;
using Components.Others;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Startup
{
    public sealed class DrawWaySystem : IEcsInitSystem
    {
        private readonly EcsFilter<Battlefield, LineRendererComp> _battlefieldFilter;

        
        void IEcsInitSystem.Init()
        {
            DrawWays();
        }

        
        private void DrawWays()
        {
            foreach (var index in _battlefieldFilter)
            {
                ref var entity = ref _battlefieldFilter.GetEntity(index);
                ref var battlefield = ref entity.Get<Battlefield>();
                ref var lineRenderer = ref entity.Get<LineRendererComp>().Value;

                var startPoint = entity.Get<GameObj>().Value.transform.position;
                var positions = new Vector3[battlefield.Ways.Count * 3];

                var indexPositions = 0;

                foreach (var way in battlefield.Ways)
                {
                    positions[indexPositions] = startPoint;
                    positions[indexPositions + 1] = way.position;
                    positions[indexPositions + 2] = startPoint;

                    indexPositions += 3;
                }

                lineRenderer.positionCount = positions.Length;
                lineRenderer.SetPositions(positions);
            }
        }
    }
}