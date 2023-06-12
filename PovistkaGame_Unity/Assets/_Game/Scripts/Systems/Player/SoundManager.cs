using UnityEngine;

namespace _Game.Scripts.Systems.Player
{
    public class SoundManager : MonoBehaviour
    {
        [field: SerializeField] 
        private GameObject steps;

        public void StepsSoundEventHandler(InteractionEventArgs state)
        {
            steps.SetActive(state.State);
        }
    }
}