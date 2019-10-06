using System.Collections.Generic;
using Module.Game;
using UnityEngine;

namespace Module.Audio
{
    public class SoundManager : MonoBehaviour
    {
        private AudioParams _audioParams;

        private AudioSource musicSource;
        private AudioClip currentClip;
        private AudioClip nextClip;

        private int _currentPhase = 0;
        private bool _changeClip = false;

        public void Init(AudioParams audioParams)
        {
            musicSource = GetComponent<AudioSource>();
            _audioParams = audioParams;
            musicSource.outputAudioMixerGroup = audioParams.MusicMixerGroup;
            PlayMusic(audioParams.MusicClips[0]);
        }

        public void ChangeMusicPhase()
        {
            _currentPhase++;
            if (_currentPhase >= _audioParams.MusicClips.Count)
            {
                _currentPhase = _audioParams.MusicClips.Count - 1;
            }

            PlayMusic(_audioParams.MusicClips[_currentPhase]);
        }

        private void PlayMusic(AudioClip newClip)
        {
            if (currentClip == null)
            {
                ChangeClip(newClip);
            }
            else
            {
                nextClip = newClip;
                _changeClip = true;
            }
        }

        private void Update()
        {
            if (musicSource == null) return;
            if (_changeClip && musicSource.time >= currentClip.length)
            {
                _changeClip = false;
                ChangeClip(nextClip);
            }

            if (musicSource.clip != null && !musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }

        private void ChangeClip(AudioClip clip)
        {
            currentClip = clip;
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySfx(List<AudioClip> audioClips)
        {
            if (audioClips == null || audioClips.Count == 0)
            {
                return;
            }

            var soundObject = new GameObject("SFX");
            var clip = audioClips[Random.Range(0, audioClips.Count)];
            var audSrc = soundObject.AddComponent<AudioSource>();

            audSrc.outputAudioMixerGroup = _audioParams.SfxMixerGroup;
            audSrc.clip = clip;
            audSrc.Play();
            soundObject.name = clip.name;
            Destroy(soundObject, clip.length);
        }
    }
}