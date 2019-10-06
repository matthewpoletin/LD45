using System.Collections.Generic;
using Module.Game.Level.Chunk;
using Module.Game.Level.Phase;
using UnityEngine;

namespace Module.Game.Level
{
    [CreateAssetMenu(fileName = "New Phase Params", menuName = "Params/PhaseParams", order = 15)]
    public class PhaseParams : ScriptableObject
    {

        [SerializeField] private List<ChunkParams> chunks = null;
        [SerializeField] private float movementSpeed = 0;
        [SerializeField] private PhaseType phaseType = PhaseType.Timeout;
        [SerializeField] private int completeConditionEnemies = 0;
        [SerializeField] private int completeConditionDuration = 0;
        [SerializeField] private int completeConditionBossHealth = 0;

        public List<ChunkParams> Chunks => chunks;
        public float MovementSpeed => movementSpeed;
        public PhaseType PhaseType => phaseType;
        public int CompleteConditionEnemies => completeConditionEnemies;
        public float CompleteConditionDuration => completeConditionDuration;
        public float CompleteBossHealth => completeConditionBossHealth;
    }
}