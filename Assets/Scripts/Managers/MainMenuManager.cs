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
            AudioManager.Instance.PlayAudio(audioClip, "Background");
        }

        public void OnBattle()
        {
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
        }
    }
}

