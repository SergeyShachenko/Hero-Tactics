using General.Components.Battle;
using Leopotam.Ecs;

namespace General.MonoLinks.Battle
{
    public class BattlefieldMonoLink : MonoLink<Battlefield>
    {
        public override void Link(ref EcsEntity entity)
        {
            entity.Get<Battlefield>() = Value;
        }
    }
}

