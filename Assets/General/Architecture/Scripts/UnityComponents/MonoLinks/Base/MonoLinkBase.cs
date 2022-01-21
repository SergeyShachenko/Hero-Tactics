using Leopotam.Ecs;
using UnityEngine;

namespace General.UnityComponents.MonoLinks
{
    public abstract class MonoLinkBase : MonoBehaviour
    {
        public abstract void Link(ref EcsEntity entity);
    }
}