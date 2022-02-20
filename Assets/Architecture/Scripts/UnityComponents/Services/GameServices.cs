using UnityEngine;

namespace UnityComponents.Services
{
    public class GameServices : MonoBehaviour
    {
        public GameObjectFactory GameObjectFactory;
        public FighterFactory FighterFactory;


        public void DestroyGameObject(GameObject gameObj)
        {
            Destroy(gameObj);
        }
    }
}