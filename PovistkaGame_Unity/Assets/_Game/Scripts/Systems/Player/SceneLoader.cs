using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Systems.Player
{
    public class SceneLoader : MonoBehaviour
    {
        public void OnPlayerFinishedLevelEventHandler(EventArgs _)
        {
            StartCoroutine(LoadSceneWithDelayRoutine());
        }

        private IEnumerator LoadSceneWithDelayRoutine()
        {
            yield return new WaitForSecondsRealtime(1);
            LoadScene();
        }

        private static void LoadScene()
        {
            SceneManager.LoadScene(2);
        }
    }
}