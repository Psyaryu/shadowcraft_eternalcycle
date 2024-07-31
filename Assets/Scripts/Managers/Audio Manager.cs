using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Audio Manager Null");
            }
            return _instance;
        }
    }

    [SerializeField]
    private AudioSource Audiosource = null;

    [SerializeField]
    private AudioSource battleLightAudioSource = null;

    [SerializeField]
    private AudioSource battleDarkAudioSource = null;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);
    }

    public void PlayAudio(AudioClip clip)
    {
        battleLightAudioSource.Stop();
        battleDarkAudioSource.Stop();

        Audiosource.volume = 0f;

        StartCoroutine(FadeInAudio(clip));
    }

    private IEnumerator FadeInAudio(AudioClip clip, float duration = 1f)
    {
        Audiosource.clip = clip;
        Audiosource.Play();

        var startTime = Time.time;
        var endTime = startTime + duration;

        while (Time.time < endTime)
        {
            var t = 1 - ((endTime - Time.time) / endTime);
            Audiosource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        Audiosource.volume = 1f;
    }

    public void FadeOutAudio()
    {
        StartCoroutine(PlayFadeOutAudio());
    }

    public IEnumerator PlayFadeOutAudio(float duration = 1f)
    {
        var startTime = Time.time;
        var endTime = startTime + duration;

        while (Time.time < endTime)
        {
            var t = 1 - ((endTime - Time.time) / endTime);
            Audiosource.volume = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        Audiosource.volume = 0f;
        Audiosource.Stop();
    }

    public void PlayBattleMusic()
    {
        Audiosource.Stop();
        battleLightAudioSource.Play();
        battleDarkAudioSource.Play();

        AdjustBattleMusic(1, 0, false);

        battleLightAudioSource.volume = 0f;
        battleDarkAudioSource.volume = 0f;

        StartCoroutine(FadeInBattle());
    }

    public IEnumerator FadeOutBattle(float duration = 1f)
    {
        var startTime = Time.time;
        var endTime = startTime + duration;

        while (Time.time < endTime)
        {
            var t = 1 - ((endTime - Time.time) / endTime);
            battleLightAudioSource.volume = Mathf.Lerp(1f, 0f, t);
            battleDarkAudioSource.volume = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        battleLightAudioSource.volume = 0;
        battleDarkAudioSource.volume = 0;
    }

    public IEnumerator FadeInBattle(float duration = 1f)
    {
        var startTime = Time.time;
        var endTime = startTime + duration;

        while (Time.time < endTime)
        {
            var t = 1 - ((endTime - Time.time) / endTime);
            battleLightAudioSource.volume = Mathf.Lerp(0f, 1f, t);
            battleDarkAudioSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        battleLightAudioSource.volume = 1f;
        battleDarkAudioSource.volume = 1f;
    }

    public void AdjustBattleMusic(float light, float dark, bool animate = true)
    {
        var lightValue = light / (light + dark);
        var darkValue = dark / (light + dark);

        if (animate)
            StartCoroutine(TransitionBattleMusic(lightValue, darkValue));
        else
        {
            battleLightAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("Light", 22000);
            battleLightAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("Dark", 10);
        }
    }

    private IEnumerator TransitionBattleMusic(float light, float dark)
    {
        var duration = 0.25f;
        var startTime = Time.time;
        var endTime = startTime + duration;

        var minFrequencyCutOff = 10;
        var maxFrequencyCutOff = 22000;

        battleLightAudioSource.outputAudioMixerGroup.audioMixer.GetFloat("Light", out float currentLight);
        battleLightAudioSource.outputAudioMixerGroup.audioMixer.GetFloat("Dark", out float currentDark);

        var endLight = Mathf.Lerp(minFrequencyCutOff, maxFrequencyCutOff, light);
        var endDark = Mathf.Lerp(minFrequencyCutOff, maxFrequencyCutOff, dark);

        while (Time.time < endTime)
        {
            var t = (endTime - Time.time) / (endTime - startTime);
            var lightValue = Mathf.Lerp(currentLight, endLight, t);
            var darkValue = Mathf.Lerp(currentDark, endDark, t);

            battleLightAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("Light", lightValue);
            battleLightAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("Dark", darkValue);
            yield return null;
        }

        battleLightAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("Light", endLight);
        battleLightAudioSource.outputAudioMixerGroup.audioMixer.SetFloat("Dark", endDark);
    }
}
