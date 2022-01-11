using Leopotam.Ecs;
using UnityEngine;

public class MonoEntity : MonoBehaviour
{
    private EcsWorld _world;
    private MonoLinkBase[] _monoLinks;


    private void Awake()
    {
        var entity = _world.NewEntity();
        _monoLinks = GetComponents<MonoLinkBase>();
        

        foreach (var monoLink in _monoLinks)
        {
            monoLink.Link(ref entity);
        }
    }

    
    public MonoLink<T> Get<T>() where T : struct
    {
        foreach (var monoLink in _monoLinks)
        {
            if (monoLink is MonoLink<T> targetMonoLink) return targetMonoLink;
        }

        return null;
    }
}