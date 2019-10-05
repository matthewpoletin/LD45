using System.Collections.Generic;
using UnityEngine;

namespace Module.Game.Level
{
    [CreateAssetMenu(fileName = "New Wave Params", menuName = "Params/WaveParams", order = 15)]
    public class WaveParams : ScriptableObject
    {
        [SerializeField] private List<ChunkParams> chunks = null;
        [SerializeField] private float movementSpeed = 0;
        [SerializeField] private int completeConditionEnemies = 0;

        public List<ChunkParams> Chunks => chunks;
        public float MovementSpeed => movementSpeed;
        public int CompleConditionEnemies => completeConditionEnemies;
    }
}