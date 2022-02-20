using Components;
using Components.Battle;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Main
{
    public sealed class DrawWaySystem : IEcsInitSystem
    {
        private readonly EcsFilter<Battlefield, LineRendererComp> _battlefields;

        void IEcsInitSystem.Init()
        {
            if (_battlefields.IsEmpty() == false)
            {
                foreach (var index in _battlefields)
                {
                    ref var entity = ref _battlefields.GetEntity(index);
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
}