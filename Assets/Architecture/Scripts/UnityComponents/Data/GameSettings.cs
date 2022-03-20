using UnityEngine;

namespace UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Settings/GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public bool ECSDebug => ecsDebug;
        public float ImminentDamageInPercent => _imminentDamageInPercent;
        public float WalkOffset => _walkOffset;
        public float ZOffset => _zOffset;
        
        [Header("Debug")] 
        [SerializeField] private bool ecsDebug = true;

        [Header("Balance")] 
        [SerializeField] [Range(0f, 1f)] private float _imminentDamageInPercent = 0.17f;
        
        [Header("Walk")] 
        [SerializeField] private float _walkOffset = 0.9f;
        [SerializeField] private float _zOffset = 0.3f;
    }
}