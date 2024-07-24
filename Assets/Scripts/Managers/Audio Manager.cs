using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [SerializeField]
    private AudioSource Audiosource = null;

    [SerializeField]
    private AudioMixer Audiomixer = null; 


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _instance = this;
    }

    public void PlayAudio(AudioClip clip, string OutputGroup)
    {


        if (Audiomixer.FindMatchingGroups(OutputGroup) == null)
        {
            Debug.Log("AudioMixer Group not found!");
            return;
        }

        if (Audiosource.clip != clip && OutputGroup == "Master")
        {
            Audiosource.clip = clip;

            Audiosource.outputAudioMixerGroup = Audiomixer.FindMatchingGroups("Master")[0];
            Audiosource.Play();
        }

        if (Audiosource.clip != clip && OutputGroup == "Background")
        {
            Audiosource.clip = clip;

            Audiosource.outputAudioMixerGroup = Audiomixer.FindMatchingGroups("Background")[0];
            Audiosource.Play();
        }

        if (Audiosource.clip != clip && OutputGroup == "SFX")
        {
            Audiosource.clip = clip;

            Audiosource.outputAudioMixerGroup = Audiomixer.FindMatchingGroups("SFX")[0];
            Audiosource.Play();
        }
    }



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
}
