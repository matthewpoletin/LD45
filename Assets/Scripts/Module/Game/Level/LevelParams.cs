using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Module.Game.Level
{
    [CreateAssetMenu(fileName = "New Level Params", menuName = "Params/LevelParams", order = 5)]
    public class LevelParams : ScriptableObject
    {
        [FormerlySerializedAs("waves")] [SerializeField] private List<PhaseParams> phases = null;

        public List<PhaseParams> Phases => phases;
    }
}