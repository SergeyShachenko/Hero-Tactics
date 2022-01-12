namespace General.Components.Battle
{
    public struct Fighter
    {
        public BattleSide BattleSide;
        public FighterState State;
        public byte Health, Armor, Damage;
    }
    
    public enum BattleSide
    {
        Hero, 
        Enemy
    }

    public enum FighterState
    {
        Alive, 
        Dead
    }
}