using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SystemSingleton<AudioManager>
{
    private static AudioMixer main;
    private static AudioMixerGroup master;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(transform.gameObject);
        
        main = Resources.Load<AudioMixer>("Main");
        master = Resources.Load<AudioMixerGroup>("Main");
        
        if(!PlayerPrefs.HasKey("MasterVolume"))
            PlayerPrefs.SetFloat("MasterVolume", 0.5f);
    }

    private void Start()
    {
        float value = PlayerPrefs.GetFloat("MasterVolume");
        main.SetFloat("MasterVolume", Mathf.Log10(value) * 30);
    }

    public void PlaySound(AudioClip audioClip, float volume = 1)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = master;
        audioSource.Play();
        Destroy(audioSource, audioClip.length);
    }
}
