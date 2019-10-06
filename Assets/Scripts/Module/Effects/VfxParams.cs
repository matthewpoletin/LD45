using UnityEngine;

namespace Module.Effects
{
    [CreateAssetMenu(fileName = "New VFX Params", menuName = "Params/Vfx/VfxParams", order = 0)]
    public class VfxParams : ScriptableObject
    {
        [SerializeField] private GameObject prefab = null;
        [SerializeField] private float duration = 0;

        public GameObject Prefab => prefab;
        public float Duration => duration;
    }
}