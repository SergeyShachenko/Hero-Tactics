using UnityEngine;

namespace General.UnityComponents.Data
{
    [CreateAssetMenu(menuName = "Develop/Data/GameData", fileName = "GameData", order = 0)]
    public class GameData : ScriptableObject
    {
        public GameObject StickmanPrefab;
    }
}
