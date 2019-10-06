using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Module.Game
{
    [CreateAssetMenu(fileName = "New Audio Params", menuName = "Params/AudioParams", order = 0)]
    public class AudioParams : ScriptableObject
    {
        [SerializeField] private AudioMixerGroup musicMixerGroup = null;
        [SerializeField] private AudioMixerGroup sfxMixerGroup = null;
        [SerializeField] private List<AudioClip> musicClips = null;
//        [SerializeField] private SfxSound[] sfxSounds;

        public List<AudioClip> MusicClips => musicClips;
        public AudioMixerGroup MusicMixerGroup => musicMixerGroup;
        public AudioMixerGroup SfxMixerGroup => sfxMixerGroup;
//        public SfxSound[] SfxSounds => sfxSounds;
    }
}
