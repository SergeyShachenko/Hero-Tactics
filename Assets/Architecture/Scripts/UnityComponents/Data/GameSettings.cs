using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Settings/GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        [Header("Debug")] 
        public bool ECSDebug = true;

        [Header("Balance")] 
        [Range(0f, 1f)] public float ImminentDamageInPercent = 0.2f;
        
        [Header("Walk")] 
        public float WalkOffset = 0.9f;
        public float ZOffset = 0.3f;
    }
}