using System;
using System.Collections.Generic;
using Module.Game;
using TimeSystem;
using UnityEngine;

namespace Module.Effects
{
    public class VfxController : ITick
    {
        private readonly List<Tuple<float, GameObject>> _vfxList = new List<Tuple<float, GameObject>>();

        public void PlayVfx(VfxParams eventParamsVfx, Transform vfxContainer, Vector3 vfxPosition)
        {
            var vfxGameObject = GameModule.Instance.GameObjectPool.GetObject(eventParamsVfx.Prefab, vfxContainer);
            vfxGameObject.transform.position = vfxPosition;
            _vfxList.Add(Tuple.Create(Time.time + eventParamsVfx.Duration, vfxGameObject));
        }

        public void Tick(float deltaTime)
        {
            for (var i = _vfxList.Count - 1; i >= 0; i--)
            {
                var (timeoutTime, vfxGameObject) = _vfxList[i];
                if (timeoutTime <= Time.time)
                {
                    GameModule.Instance.GameObjectPool.UtilizeObject(vfxGameObject);
                    _vfxList.RemoveAt(i);
                }
            }
        }
    }
}