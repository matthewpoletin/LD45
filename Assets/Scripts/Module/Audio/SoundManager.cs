using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioClip[] musicClips;
    private AudioSource musicSource;
    private AudioClip currentClip;
    private AudioClip nextClip;
    private int currentPhase = 0;

    private bool changeClip;

    public void Init() {
        musicSource = GetComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        PlayMusic(musicClips[0]);
    }

    public void ChangeMusicPhase() {
        currentPhase ++;
                if (currentPhase > musicClips.Length) {
            currentPhase = musicClips.Length;
        }
        PlayMusic(musicClips[currentPhase]);
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
        if (musicSource == null) return;
        if(changeClip && musicSource.time >= currentClip.length) {
            changeClip = false;
            ChangeClip(nextClip);
        }

        if (musicSource.clip != null && !musicSource.isPlaying) {
            musicSource.Play();
        }

        //TODO: Debug
        if (Input.GetKeyDown(KeyCode.L)) {
            ChangeMusicPhase();
        }
    }

    private void ChangeClip(AudioClip clip) {
        currentClip = clip;
        musicSource.clip = clip;
        musicSource.Play();
    }
}
