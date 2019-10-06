using System;
using System.Collections;
using System.Collections.Generic;
using Module.Game;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private AudioParams audioParams;

    private AudioSource musicSource;
    private AudioClip currentClip;
    private AudioClip nextClip;


    private int currentPhase = 0;

    private bool changeClip;

    public void Init(AudioParams audioParams) {
        musicSource = GetComponent<AudioSource>();
        this.audioParams = audioParams;
        musicSource.outputAudioMixerGroup = audioParams.MusicMixerGroup;
        PlayMusic(audioParams.MusicClips[0]);
    }

    public void ChangeMusicPhase() {
        currentPhase ++;
        Debug.Log(currentPhase);
        if (currentPhase >= audioParams.MusicClips.Length) {
            currentPhase = audioParams.MusicClips.Length - 1;
        }
        PlayMusic(audioParams.MusicClips[currentPhase]);
    }
    
     private void PlayMusic(AudioClip newClip) {
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

    public void PlaySfx(SfxType sfxType) {
        GameObject soundObject = new GameObject("SFX");
        AudioClip clip = GetSfxSound(sfxType);
        AudioSource audSrc = soundObject.AddComponent<AudioSource>();

        audSrc.outputAudioMixerGroup = audioParams.SfxMixerGroup;
        audSrc.clip = clip;
        audSrc.Play();
        soundObject.name = clip.name;
        Destroy(soundObject, clip.length);
    }
    
    private AudioClip GetSfxSound(SfxType sfxType) {
        List<AudioClip> selectedSounds = new List<AudioClip>();
        foreach (SfxSound sfx in audioParams.SfxSounds) {
            if (sfx.sfxType == sfxType) {
                selectedSounds.Add(sfx.Clip);
            }
        }
        if (selectedSounds.Count <= 0)
            return null;

        return SelectRandomClip(selectedSounds);
    }

    private AudioClip SelectRandomClip(List<AudioClip> clips) {
        return clips[UnityEngine.Random.Range(0, clips.Count)];
    }
}

public enum  SfxType
{
    Attack,
    Damaged,
    Jump,
    Humiliate,
}

[Serializable]
public class SfxSound {
    [SerializeField] private AudioClip clip;
    public AudioClip Clip => clip;
    [SerializeField] public SfxType sfxType;
    public SfxType SfxType => sfxType;
}
