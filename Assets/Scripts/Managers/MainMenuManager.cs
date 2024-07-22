using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
        }
    }
}

