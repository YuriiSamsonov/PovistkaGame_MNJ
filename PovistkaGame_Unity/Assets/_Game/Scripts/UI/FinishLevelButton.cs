using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FinishLevelButton : MonoBehaviour
{
    private void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(ExitToMenu);

    }

    private void ExitToMenu()
    {
        StartCoroutine(EndTheLevelRoutine());
    }

    private IEnumerator EndTheLevelRoutine()
    {
        yield return new WaitForSecondsRealtime(0);
        
        SceneManager.LoadScene(0);
    }
}
