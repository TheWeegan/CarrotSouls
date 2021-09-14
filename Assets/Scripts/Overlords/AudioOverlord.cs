using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioOverlord
{
    private static AudioOverlord _instance = new AudioOverlord();
    public static AudioOverlord GetInstance { get => _instance; }

    
    public void PlayerOneShot(GameObject gameObject, AudioClip audioClip) {
        if (gameObject.TryGetComponent(out AudioSource audioSource)) {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void PauseAudio(GameObject gameObject) {
        if (gameObject.TryGetComponent(out AudioSource audioSource)) {
            audioSource.Pause();
        }
    }

    public void UnPauseAudio(GameObject gameObject) {
        if (gameObject.TryGetComponent(out AudioSource audioSource)) {
            audioSource.UnPause();
        }
    }

    public void StopAudio(GameObject gameObject) {
        if (gameObject.TryGetComponent(out AudioSource audioSource)) {
            audioSource.Stop();
        }
    }

}
