﻿using System;
using System.Collections.Generic;
using Components;
using Components.Battle;
using Leopotam.Ecs;

namespace General.Systems
{
    public sealed class FighterAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter<Fighter, AnimatorComponent> _warriors;
        
        private List<AnimatedWarrior> _animatedWarriors;


        void IEcsInitSystem.Init()
        {
            _animatedWarriors = new List<AnimatedWarrior>();
        }
        
        void IEcsRunSystem.Run()
        {
            DoAnimateWarriors(canAnimate:
                _warriors.IsEmpty() == false);
            
            ProcessAnimatedWarriors(canProcess:
                _animatedWarriors.Count > 0);
        }


        private void DoAnimateWarriors(bool canAnimate)
        {
            if (canAnimate == false) return;


            foreach (var index in _warriors)
            {
                ref var entity = ref _warriors.GetEntity(index);
                ref var fighter = ref entity.Get<Fighter>();
                ref var animator = ref entity.Get<AnimatorComponent>().Animator;

                if (fighter.State == FighterState.Disabled) continue;
                

                if (fighter.State == FighterState.Dead)
                {
                    var animatedWarrior = new AnimatedWarrior {Entity = entity, PreviousState = FighterState.Dead};
                    
                    if (_animatedWarriors.Contains(animatedWarrior) == false)
                    {
                        animator.SetTrigger("Death");
                        _animatedWarriors.Add(animatedWarrior);
                    }
                    
                    continue;
                }

                if (fighter.Action == FighterAction.Attack)
                {
                    var animatedWarrior = new AnimatedWarrior {Entity = entity, PreviousState = FighterAction.Attack};
                    
                    if (_animatedWarriors.Contains(animatedWarrior) == false)
                    {
                        animator.SetTrigger("Attack");
                        _animatedWarriors.Add(animatedWarrior);
                    }
                    
                    continue;
                }
                
                if (fighter.Action == FighterAction.GetDamage)
                {
                    var animatedWarrior = new AnimatedWarrior {Entity = entity, PreviousState = FighterAction.GetDamage};
                    
                    if (_animatedWarriors.Contains(animatedWarrior) == false)
                    {
                        animator.SetTrigger("GetDamage");
                        _animatedWarriors.Add(animatedWarrior);
                    }
                    
                    continue;
                }
                
                if (entity.Has<Movable>() && entity.Get<Movable>().State == MovableState.Idle)
                {
                    var animatedWarrior = new AnimatedWarrior {Entity = entity, PreviousState = MovableState.Idle};
                    
                    if (_animatedWarriors.Contains(animatedWarrior) == false)
                    {
                        animator.SetTrigger("Idle");
                        _animatedWarriors.Add(animatedWarrior);
                    }
                    
                    continue;
                }
                
                if (entity.Has<Movable>() && entity.Get<Movable>().State == MovableState.Walk)
                {
                    var animatedWarrior = new AnimatedWarrior {Entity = entity, PreviousState = MovableState.Walk};
                    
                    if (_animatedWarriors.Contains(animatedWarrior) == false)
                    {
                        animator.SetTrigger("Walk");
                        _animatedWarriors.Add(animatedWarrior);
                    }
                    
                    continue;
                }
                
                if (entity.Has<Movable>() && entity.Get<Movable>().State == MovableState.Run)
                {
                    var animatedWarrior = new AnimatedWarrior {Entity = entity, PreviousState = MovableState.Run};
                    
                    if (_animatedWarriors.Contains(animatedWarrior) == false)
                    {
                        animator.SetTrigger("Run");
                        _animatedWarriors.Add(animatedWarrior);
                    }
                    
                    continue;
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

                        if (fighter.State != FighterState.Dead)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            continue;
                        }

                        break;
                    
                    case FighterAction.Attack:

                        if (fighter.Action != FighterAction.Attack)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            continue;
                        }
                        
                        break;
                    
                    case FighterAction.GetDamage:

                        if (fighter.Action != FighterAction.GetDamage)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            continue;
                        }
                        
                        break;
                    
                    case MovableState.Idle:

                        if (movable.State != MovableState.Idle)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            continue;
                        }
                        
                        break;
                    
                    case MovableState.Walk:

                        if (movable.State != MovableState.Walk)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            continue;
                        }
                        
                        break;
                    
                    case MovableState.Run:

                        if (movable.State != MovableState.Run)
                        {
                            _animatedWarriors.Remove(animatedWarrior);
                            i = 0;
                            continue;
                        }
                        
                        break;
                }

                i++;
            }
        }
    }


    public struct AnimatedWarrior
    {
        public EcsEntity Entity;
        public Enum PreviousState;
    }
}