using System;
using Leopotam.Ecs;
using UnityEngine;

namespace Components.Battle
{
    public struct Fighter
    {
        public BattleSide BattleSide;
        public int SquadID;
        public FighterState State;
        public FighterAction Action;
        public FighterStats Stats;
    }
    
    
    public enum BattleSide
    {
        Hero, Enemy
    }

    public enum FighterState
    {
        Disabled, Alive, Dead, 
    }

    public enum FighterAction
    {
        Attack, GetDamage, None
    }

    [Serializable] public struct FighterStats
    {
        public float Health;
        public float Damage;
        [Range(0, 100)] public int Armor;
    }
    
    public struct FighterSquad
    {
        public BattleSide BattleSide;
        public SquadState State;
        public int ID;
        public FighterStats Stats;
        public EcsEntity Place;
    }

    public enum SquadState
    {
        Alive, Dead
    }
}