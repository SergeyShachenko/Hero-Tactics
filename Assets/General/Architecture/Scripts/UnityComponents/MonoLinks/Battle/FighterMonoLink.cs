using General.Components.Battle;
using Leopotam.Ecs;

namespace General.MonoLinks.Battle
{
    public class FighterMonoLink : MonoLink<Fighter>
    {
        public override void Link(ref EcsEntity entity)
        {
            entity.Get<Fighter>() = Value;
        }
    }
}

