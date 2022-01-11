using Leopotam.Ecs;
using UnityEngine;

public abstract class MonoLinkBase : MonoBehaviour
{
    public abstract void Link(ref EcsEntity entity);
}