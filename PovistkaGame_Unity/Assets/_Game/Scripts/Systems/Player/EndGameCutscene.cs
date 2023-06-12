using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Utils
{
    public class EndGameCutscene : MonoBehaviour
    {
        [field: SerializeField] 
        private GameObject lights;
        
        [field: SerializeField] 
        private GameObject triggers;

        public void OnPlayerStartCutscene(EventArgs _)
        {
            StartCoroutine(LightsOnWithDelayRoutine());
            StartCoroutine(TriggersWithDelayRoutine());
        }
        

        private IEnumerator LightsOnWithDelayRoutine()
        {
            yield return new WaitForSecondsRealtime(2);
            LightsOn();
        }

        private void LightsOn()
        {
            lights.SetActive(false);
        }
        
        private IEnumerator TriggersWithDelayRoutine()
        {
            yield return new WaitForSecondsRealtime(4);
            Triggers();
        }

        private void Triggers()
        {
            triggers.SetActive(true);
        }
    }
}