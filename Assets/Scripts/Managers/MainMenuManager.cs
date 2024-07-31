using System.Collections.Generic;
using System.Collections;
using System;
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
            AudioManager.Instance.PlayAudio(audioClip);
        }

        public void OnAdventure()
        {
            SceneManager.LoadScene("AdventureScene", LoadSceneMode.Single);
        }

        public void OnBattle()
        {
            StartCoroutine(PlayBattle());
        }

        IEnumerator PlayBattle()
        {
            yield return AudioManager.Instance.PlayFadeOutAudio();
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
        }
    }
}

