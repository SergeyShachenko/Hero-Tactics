using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void Spawn(SpawnPrefab prefab)
    {
        Instantiate(prefab.Prefab, prefab.Position, prefab.Rotation, prefab.Parent);
    }
}