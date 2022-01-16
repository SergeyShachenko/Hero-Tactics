﻿using System;

namespace General.Components.Battle
{
    public struct Warrior
    {
        public WarriorType Type;
    }

    
    [Serializable] public enum WarriorType
    {
        Sword,
        SwordShield
    }
}