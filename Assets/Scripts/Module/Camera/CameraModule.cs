using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.CamearModule
{
    public class CameraModule : MonoBehaviour, IBaseModuleView
    {
        private Vector3 defaultPos;
        private Camera sceneCam;

        private void Awake() {
            Init();
        }

        public void Shake(float duration, float amount) {
            if (sceneCam == null) return;
            StartCoroutine(_shake(duration, amount));
        }

        IEnumerator _shake(float duration, float amount) {
            float timeCount = duration;
            while(timeCount > 0) {
                sceneCam.transform.localPosition = defaultPos + Random.insideUnitSphere * amount;
                timeCount -= Time.deltaTime;

                yield return null;
            }

            sceneCam.transform.localPosition = defaultPos;
        }

        public void Init()
        {
            sceneCam = Camera.main;
            defaultPos = sceneCam.transform.localPosition;
        }
    }
}
