using System.Collections.Generic;
using Module.Effects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Module.Game.Events
{
    [CreateAssetMenu(fileName = "New Event Params", menuName = "Params/Game/EventParams", order = 0)]
    public class EventParams : ScriptableObject
    {
        [SerializeField] public EventType eventType;
        [FormerlySerializedAs("clip")] [SerializeField] private List<AudioClip> clips = null;
        [SerializeField] private VfxParams vfx = null;

        [SerializeField] private bool isBoss;

        public List<AudioClip> Clips => clips;
        public EventType EventType => eventType;
        public VfxParams Vfx => vfx;

        public bool IsBoss => isBoss;
    }
}