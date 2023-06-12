using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class PlayButton : MonoBehaviour
    {
        
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Play);
        }

        private void Play()
        {
            SceneManager.LoadScene(1);
        }

    }
}