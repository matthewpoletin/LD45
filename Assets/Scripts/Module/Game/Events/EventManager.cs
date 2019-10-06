using System.Collections.Generic;
using Module.Audio;
using Module.Effects;
using UnityEngine;

namespace Module.Game.Events
{
    public class EventManager
    {
        private SoundManager _soundManager = null;
        private VfxController _vfxController = null;
        private Dictionary<EventType, EventParams> _events = new Dictionary<EventType, EventParams>();

        public EventManager(SoundManager soundManager, VfxController vfxController, List<EventParams> events)
        {
            _soundManager = soundManager;
            _vfxController = vfxController;

            foreach (var eventParams in events)
            {
                _events.Add(eventParams.eventType, eventParams);
            }
        }

        public void ProcessEvent(EventType eventType, Transform vfxContainer = null)
        {
            if (!_events.TryGetValue(eventType, out var eventParams))
            {
                Debug.LogError($"Event params for {eventType} not found");
                return;
            }

            if (eventParams.Clips != null || eventParams.Clips.Count != 0)
            {
                _soundManager.PlaySfx(eventParams.Clips);
            }

            if (eventParams.Vfx != null)
            {
                _vfxController.PlayVfx(eventParams.Vfx, vfxContainer);
            }
        }
    }
}