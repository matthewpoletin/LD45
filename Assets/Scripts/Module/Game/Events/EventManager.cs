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
        private Dictionary<EventType, List<EventParams>> _events = new Dictionary<EventType, List<EventParams>>();

        public EventManager(SoundManager soundManager, VfxController vfxController, List<EventParams> events)
        {
            _soundManager = soundManager;
            _vfxController = vfxController;

            foreach (var eventParams in events)
            {
                List<EventParams> list; 
                if (!_events.TryGetValue(eventParams.eventType, out list))
                {
                    list = new List<EventParams>();
                    _events.Add(eventParams.eventType, list);
                }

                list.Add(eventParams);
            }
        }

        public void ProcessEvent(EventType eventType, Transform vfxContainer = null)
        {
            if (!_events.TryGetValue(eventType, out var eventParamsList))
            {
                Debug.LogError($"Event params for {eventType} not found");
                return;
            }

            if (eventParamsList == null || eventParamsList.Count == 0)
            {
                return;
            }

            var eventParams = eventParamsList[Random.Range(0, eventParamsList.Count)];

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