using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private AudioSource musicSource;
    private AudioClip currentClip;
    private AudioClip nextClip;

    private bool changeClip;

    private void Awake() {
        musicSource = GetComponent<AudioSource>();
    }
    
     public void PlayMusic(AudioClip newClip) {
         if (currentClip == null) {
            ChangeClip(newClip);
         } else {
            nextClip = newClip;
            changeClip = true;
         }
    }

    private void Update() {
        if(changeClip && musicSource.time >= currentClip.length) {
            changeClip = false;
            ChangeClip(nextClip);
        }

        if (musicSource.clip != null && !musicSource.isPlaying) {
            musicSource.Play();
        }
    }

    private void ChangeClip(AudioClip clip) {
        currentClip = clip;
        musicSource.clip = clip;
        musicSource.Play();
    }
}
