using Module.Game;
using UnityEngine;

namespace Module.Effects
{
    public class VfxController
    {
        public void PlayVfx(VfxParams eventParamsVfx, Transform vfxContainer)
        {
            GameModule.Instance.GameObjectPool.GetObject(eventParamsVfx.Prefab, vfxContainer);
        }
    }
}