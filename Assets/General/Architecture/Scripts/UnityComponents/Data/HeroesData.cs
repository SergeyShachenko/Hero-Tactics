using System;
using System.Collections.Generic;
using General.Components.Battle;
using UnityEngine;

namespace General.UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/HeroesData", fileName = "HeroesData", order = 1)]
    public class HeroesData : ScriptableObject
    {
        public List<HeroData> Heroes;
    }
    
    
    [Serializable] public struct HeroData
    {
        public GameObject Prefab;
        public WarriorType Type;
        public byte Health, Armor, Damage;
    }
}