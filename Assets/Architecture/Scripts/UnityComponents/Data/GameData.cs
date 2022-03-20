using System;
using Components;
using Components.Battle;
using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/GameData", fileName = "GameData", order = 0)]
    public class GameData : ScriptableObject
    {
        public GameObject PrefabFreeBattlefield => _prefabFreeBattlefield;
        public GameObject PrefabOccupiedBattlefield => _prefabOccupiedBattlefield;
        public HeroesData Heroes => _heroes;
        public EnemiesData Enemies => _enemies;
        
        [Header("Battlefields")] 
        [SerializeField] private GameObject _prefabFreeBattlefield;
        [SerializeField] private GameObject _prefabOccupiedBattlefield;
        
        [Header("Warriors")]
        [SerializeField] private HeroesData _heroes;
        [SerializeField] private EnemiesData _enemies;
    }
    
    
    [Serializable] public struct WarriorData
    {
        public GameObject Prefab;
        public WarriorType Type;
        public FighterStats Stats;
        public Movable Movable;
    }
}
