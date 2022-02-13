using System;
using Components;
using Components.Battle;
using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/GameData", fileName = "GameData", order = 0)]
    public class GameData : ScriptableObject
    {
        [Header("Warriors")]
        public HeroesData HeroesData;
        public EnemysData EnemysData;
    }
    
    
    [Serializable] public struct WarriorData
    {
        public GameObject Prefab;
        public WarriorType Type;
        public FighterStats Stats;
        public Movable Movable;
    }
}
