using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/EnemiesData", fileName = "EnemiesData", order = 2)]
    public class EnemiesData : ScriptableObject
    {
        public List<EnemyData> Enemies;
    }
    
    
    [Serializable] public struct EnemyData
    {
        public bool IsBoss;
        public WarriorData Warrior;
    }
}