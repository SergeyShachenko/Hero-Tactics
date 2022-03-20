using System;
using System.Collections.Generic;
using Components;
using Components.Battle;
using Components.Others;
using Leopotam.Ecs;

namespace Systems.Battle
{
    public sealed class FighterAnimatorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter<Fighter, AnimatorComp> _animatedFighterFilter;
        
        private List<AnimatedFighter> _animatedWarriors;


        void IEcsInitSystem.Init()
        {
            _animatedWarriors = new List<AnimatedFighter>();
        }
        
        void IEcsRunSystem.Run()
        {
            DoAnimateWarriors();
            ProcessAnimatedWarriors(canProcess: _animatedWarriors.Count > 0);
        }


        private void DoAnimateWarriors()
        {
            foreach (var index in _animatedFighterFilter)
            {
                ref var entity = ref _animatedFighterFilter.GetEntity(index);
                ref var fighter = ref entity.Get<Fighter>();
                ref var animator = ref entity.Get<AnimatorComp>().Value;

                
                switch (fighter.State)
                {
                    case FighterState.Disabled: continue;
                    
                    case FighterState.Dead:
                    {
                        var animatedWarrior = new AnimatedFighter {Entity = entity, PreviousState = FighterState.Dead};
                    
                        if (_animatedWarriors.Contains(animatedWarrior) == false)
                        {
                            animator.SetTrigger("Death");
                            _animatedWarriors.Add(animatedWarrior);
                        }
                    
                        continue;
                    }
                }

                switch (fighter.Action)
                {
                    case FighterAction.Attack:
                    {
                        var animatedWarrior = new AnimatedFighter {Entity = entity, PreviousState = FighterAction.Attack};
                    
                        if (_animatedWarriors.Contains(animatedWarrior) == false)
                        {
                            animator.SetTrigger("Attack");
                            _animatedWarriors.Add(animatedWarrior);
                        }
                    
                        continue;
                    }
                    
                    case FighterAction.GetDamage:
                    {
                        var animatedWarrior = new AnimatedFighter {Entity = entity, PreviousState = FighterAction.GetDamage};
                    
                        if (_animatedWarriors.Contains(animatedWarrior) == false)
                        {
                            animator.SetTrigger("GetDamage");
                            _animatedWarriors.Add(animatedWarrior);
                        }
                    
                        continue;
                    }
                }

                if (entity.Has<Movable>())
                {
                    ref var movable = ref entity.Get<Movable>(); 
                    
                    switch (movable.State)
                    {
                        case MovableState.Idle:
                        {
                            var animatedWarrior = new AnimatedFighter {Entity = entity, PreviousState = MovableState.Idle};
                                            
                            if (_animatedWarriors.Contains(animatedWarrior) == false)
                            {
                                animator.SetTrigger("Idle");
                                _animatedWarriors.Add(animatedWarrior);
                            }
                                            
                            continue;
                        }
                        
                        case MovableState.Walk:
                        {
                            var animatedWarrior = new AnimatedFighter {Entity = entity, PreviousState = MovableState.Walk};
                                            
                            if (_animatedWarriors.Contains(animatedWarrior) == false)
                            {
                                animator.SetTrigger("Walk");
                                _animatedWarriors.Add(animatedWarrior);
                            }
                                            
                            continue;
                        }
                        
                        case MovableState.Run:
                        {
                            var animatedWarrior = new AnimatedFighter {Entity = entity, PreviousState = MovableState.Run};
                    
                            if (_animatedWarriors.Contains(animatedWarrior) == false)
                            {
                                animator.SetTrigger("Run");
                                _animatedWarriors.Add(animatedWarrior);
                            }
                    
                            continue;
                        }
                    }
                }
            }
        }
        
        private void ProcessAnimatedWarriors(bool canProcess)
        {
            if (canProcess == false) return;


            for (var i = 0; i < _animatedWarriors.Count;)
            {
                var animatedWarrior = _animatedWarriors[i];
                ref var fighter = ref animatedWarrior.Entity.Get<Fighter>();
                ref var movable = ref animatedWarrior.Entity.Get<Movable>();

                
                switch (animatedWarrior.PreviousState)
                {
                    case FighterState.Dead:
                    {
                        if (fighter.State != FighterState.Dead)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;

                            continue;
                        }

                        break;
                    }

                    case FighterAction.Attack:
                    {
                        if (fighter.Action != FighterAction.Attack)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            
                            continue;
                        }
                        
                        break;
                    }

                    case FighterAction.GetDamage:
                    {
                        if (fighter.Action != FighterAction.GetDamage)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            
                            continue;
                        }
                        
                        break;
                    }

                    case MovableState.Idle:
                    {
                        if (movable.State != MovableState.Idle)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            
                            continue;
                        }
                        
                        break;
                    }

                    case MovableState.Walk:
                    {
                        if (movable.State != MovableState.Walk)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            
                            continue;
                        }
                        
                        break;   
                    }

                    case MovableState.Run:
                    {
                        if (movable.State != MovableState.Run)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            
                            continue;
                        }
                        
                        break;   
                    }
                }
                
                i++;
            }
        }


        private struct AnimatedFighter
        {
            public EcsEntity Entity;
            public Enum PreviousState;
        }
    }
}