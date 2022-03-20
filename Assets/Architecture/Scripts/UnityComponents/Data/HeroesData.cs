using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/HeroesData", fileName = "HeroesData", order = 1)]
    public class HeroesData : ScriptableObject
    {
        public List<HeroWarriorData> Warriors => _warriors;

        [SerializeField] private List<HeroWarriorData> _warriors;
    }


    [Serializable] public struct HeroWarriorData
    {
        public WarriorData Warrior;
    }
}