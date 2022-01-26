using System;

namespace General.Components.Battle
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
        public byte Health, Armor, Damage;
    }
}