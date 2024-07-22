using UnityEngine;

namespace ShadowCraft
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private AudioClip audioClip = null;

        private void Start()
        {
            // TODO: Play the audio clip here.
        }

        public void OnBattle()
        {
            Debug.Log("BATTLE!!!");
        }
    }
}

