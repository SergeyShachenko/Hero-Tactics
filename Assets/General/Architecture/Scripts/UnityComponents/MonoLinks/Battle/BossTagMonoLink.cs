using General.Components.Tags;
using Leopotam.Ecs;

namespace General.MonoLinks.Battle
{
    public class BossTagMonoLink : MonoLink<BossTag>
    {
        public override void Link(ref EcsEntity entity)
        {
            entity.Get<BossTag>() = Value;
        }
    }
}