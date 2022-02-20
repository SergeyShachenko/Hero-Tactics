using System;
using Components;
using Components.Battle;
using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/GameData", fileName = "GameData", order = 0)]
    public class GameData : ScriptableObject
    {
        [Header("Battlefields")] 
        public GameObject PrefabFreeBattlefield;
        public GameObject PrefabOccupiedBattlefield;
        
        [Header("Warriors")]
        public HeroesData Heroes;
        public EnemiesData Enemies;
    }
    
    
    [Serializable] public struct WarriorData
    {
        public GameObject Prefab;
        public WarriorType Type;
        public FighterStats Stats;
        public Movable Movable;
    }
}
