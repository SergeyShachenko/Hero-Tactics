using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/EnemiesData", fileName = "EnemiesData", order = 2)]
    public class EnemiesData : ScriptableObject
    {
        public List<EnemyWarriorData> Warriors => _warriors;

        [SerializeField] private List<EnemyWarriorData> _warriors;
    }
    
    
    [Serializable] public struct EnemyWarriorData
    {
        public bool IsBoss;
        public WarriorData Warrior;
    }
}