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
            AudioManager.Instance.PlayAudio(audioClip, "Background");
        }

        public void OnAdventure()
        {
            SceneManager.LoadScene("AdventureScene", LoadSceneMode.Single);
        }

        public void OnBattle()
        {
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
        }
    }
}

