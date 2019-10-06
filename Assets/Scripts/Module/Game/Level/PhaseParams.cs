using System.Collections.Generic;
using UnityEngine;

namespace Module.Game.Level
{
    [CreateAssetMenu(fileName = "New Phase Params", menuName = "Params/PhaseParams", order = 15)]
    public class PhaseParams : ScriptableObject
    {
        [SerializeField] private List<ChunkParams> chunks = null;
        [SerializeField] private float movementSpeed = 0;
        [SerializeField] private int completeConditionEnemies = 0;

        public List<ChunkParams> Chunks => chunks;
        public float MovementSpeed => movementSpeed;
        public int CompleConditionEnemies => completeConditionEnemies;
    }
}