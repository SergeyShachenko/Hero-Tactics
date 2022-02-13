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

    [Serializable] public struct FighterStats
    {
        public int Health;
        public int Damage;
        [Range(0, 100)] public int Armor;
    }
    
    public struct FighterSquad
    {
        public BattleSide BattleSide;
        public SquadState State;
        public int ID;
        public float Health, Armor, Damage;
        public EcsEntity Place;
    }

    public enum SquadState
    {
        Alive, Dead
    }
}