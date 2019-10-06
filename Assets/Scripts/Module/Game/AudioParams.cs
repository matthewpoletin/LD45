using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Module.Game
{
    [CreateAssetMenu(fileName = "New Audio Params", menuName = "Params/AudioParams", order = 0)]
    public class AudioParams : ScriptableObject
    {
        [SerializeField] private AudioMixerGroup musicMixerGroup = null;
        public AudioMixerGroup MusicMixerGroup => musicMixerGroup;
        [SerializeField] private AudioMixerGroup sfxMixerGroup = null;
        public AudioMixerGroup SfxMixerGroup => sfxMixerGroup;

        [SerializeField] private AudioClip[] musicClips;
        public AudioClip[] MusicClips => musicClips;
        [SerializeField] private SfxSound[] sfxSounds;
        public SfxSound[] SfxSounds => sfxSounds;
    }
}
