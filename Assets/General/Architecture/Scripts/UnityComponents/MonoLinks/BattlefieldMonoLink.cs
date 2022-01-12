using General.Components.Links;
using General.UnityComponents.MonoLinks.Base;
using Leopotam.Ecs;

namespace General.UnityComponents.MonoLinks
{
    public class BattlefieldMonoLink : MonoLink<BattlefieldLink>
    {
        public override void Link(ref EcsEntity entity)
        {
            entity.Get<BattlefieldLink>() = Value;
        }
    }
}

