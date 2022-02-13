using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/EnemysData", fileName = "EnemysData", order = 2)]
    public class EnemysData : ScriptableObject
    {
        public List<EnemyData> Enemys;
    }
    
    
    [Serializable] public struct EnemyData
    {
        public bool IsBoss;
        public WarriorData Warrior;
    }
}