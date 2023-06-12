using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayerIsDeadButton : MonoBehaviour
{
    private void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(RestartTheLevel);
    }

    private void RestartTheLevel()
    {
        Application.Quit();
    }
}