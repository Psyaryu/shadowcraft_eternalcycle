using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ShadowCraft
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private AudioClip audioClip = null;

        [SerializeField]
        private CanvasGroup canvasGroup = null;

        [SerializeField]
        private Button battleButton = null;

        [SerializeField]
        private Button adventureButton = null;

        private void Start()
        {
            AudioManager.Instance.PlayAudio(audioClip);
            GameManager.shared.player.Reset();
            GameManager.shared.ai?.Reset();
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
            battleButton.interactable = false;
            adventureButton.interactable = false;

            StartCoroutine(AudioManager.Instance.PlayFadeOutAudio());

            var startTime = Time.time;
            var endTime = startTime + 1f;

            while (Time.time < endTime)
            {
                var t = (Time.time - startTime) / endTime;

                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
                yield return null;
            }

            canvasGroup.alpha = 1f;

            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
        }
    }
}

