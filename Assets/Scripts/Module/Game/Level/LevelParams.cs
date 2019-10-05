using System.Collections.Generic;
using UnityEngine;

namespace Module.Game.Level
{
    [CreateAssetMenu(fileName = "New Level Params", menuName = "Params/LevelParams", order = 5)]
    public class LevelParams : ScriptableObject
    {
        [SerializeField] private List<WaveParams> waves = null;

        public List<WaveParams> Waves => waves;
    }
}