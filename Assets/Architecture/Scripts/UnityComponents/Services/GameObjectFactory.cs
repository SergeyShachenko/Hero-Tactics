using UnityEngine;

namespace UnityComponents.Services
{
    public sealed class GameObjectFactory : MonoBehaviour
    {
        public void Spawn(GameObject prefab, Transform parent)
        {
            var spawnedObject = Instantiate(prefab, parent);
            spawnedObject.transform.rotation = Quaternion.identity;
        }
    }
}