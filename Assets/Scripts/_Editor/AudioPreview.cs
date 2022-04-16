#if (UNITY_EDITOR)

using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class AudioPreview
{
    private static GameObject currentAudio;

    static AudioPreview()
    {
        Selection.selectionChanged += Change;
    }

    public static void Change()
    {
        var audioClips = Selection.GetFiltered<AudioClip>(SelectionMode.Assets);
        foreach (AudioClip audioClip in audioClips)
        {
            if (currentAudio)
                Object.DestroyImmediate(currentAudio);
            PlayClipAtPoint(audioClip, Vector3.zero, 1, 5);
        }
    }

    public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1, float maxPlayTime = 5)
    {
        currentAudio = new GameObject("One shot audio");
        currentAudio.transform.position = position;
        AudioSource audioSource = (AudioSource) currentAudio.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        DestroyAsync((Object) currentAudio, Mathf.Min(clip.length, maxPlayTime));
    }

    static async void DestroyAsync(Object o, float length)
    {
        await Task.Delay(Mathf.CeilToInt(length * 1000));
        Object.DestroyImmediate(o);
    }
}

#endif