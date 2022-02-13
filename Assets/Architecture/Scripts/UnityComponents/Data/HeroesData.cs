using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/HeroesData", fileName = "HeroesData", order = 1)]
    public class HeroesData : ScriptableObject
    {
        public List<HeroData> Heroes;
    }


    [Serializable] public struct HeroData
    {
        public WarriorData Warrior;
    }
}