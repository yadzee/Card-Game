using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class GameStartup : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(1);
        }
    }
}