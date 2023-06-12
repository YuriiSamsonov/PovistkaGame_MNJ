using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class ExitButton : MonoBehaviour
    {
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Exit);
        }

        private void Exit()
        {
            Application.Quit();
        }
    }
}