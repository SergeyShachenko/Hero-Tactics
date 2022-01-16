using General.Components.Battle;
using Leopotam.Ecs;

namespace General.MonoLinks.Battle
{
    public class WarriorMonoLink : MonoLink<Warrior>
    {
        public override void Link(ref EcsEntity entity)
        {
            entity.Get<Warrior>() = Value;
        }
    }
}