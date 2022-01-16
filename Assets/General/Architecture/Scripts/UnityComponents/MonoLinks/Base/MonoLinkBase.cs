using Leopotam.Ecs;
using UnityEngine;

namespace General.MonoLinks
{
    public abstract class MonoLinkBase : MonoBehaviour
    {
        public abstract void Link(ref EcsEntity entity);
    }
}