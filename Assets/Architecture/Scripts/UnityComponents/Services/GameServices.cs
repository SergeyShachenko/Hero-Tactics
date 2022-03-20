using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityComponents.Services
{
    public sealed class GameServices : MonoBehaviour
    {
        public GameObjectFactory GameObjectFactory => _gameObjectFactory;
        public FighterFactory FighterFactory => _fighterFactory;
        
        [SerializeField] private GameObjectFactory _gameObjectFactory;
        [SerializeField] private FighterFactory _fighterFactory;


        public void DestroyGameObject(GameObject gameObj) => Destroy(gameObj);

        public void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void CloseApplication() => Application.Quit();
    }
}