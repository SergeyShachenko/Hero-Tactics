using System;
using System.Collections.Generic;
using General.Components.Battle;
using UnityEngine;

namespace General.UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/EnemysData", fileName = "EnemysData", order = 2)]
    public class EnemysData : ScriptableObject
    {
        public List<EnemyData> Enemys;
    }
    
    
    [Serializable] public struct EnemyData
    {
        public GameObject Prefab;
        public bool isBoss;
        public WarriorType Type;
        public byte Health, Armor, Damage;
    }
}