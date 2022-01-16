using System;

namespace General.Components.Battle
{
    public struct Fighter
    {
        public BattleSide BattleSide;
        public byte SquadID;
        public FighterState State;
        public FighterStats Stats;
    }
    
    
    public enum BattleSide
    {
        Hero, Enemy
    }

    public enum FighterState
    {
        Alive, Dead, Disabled
    }

    [Serializable] public struct FighterStats
    {
        public byte Health, Armor, Damage;
    }
}