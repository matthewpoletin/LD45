using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Module.Game.Level.Phase
{
    [Serializable]
    public enum EnemySpawns
    {
        None = 0,
        MeleeOnly = 1,
        RangeOnly = 2,
        MeleeAndRange = 3,
        Boss = 4,
    }

    [Serializable]
    public enum CharacterWeapon
    {
        None = 0,
        Melee = 1,
        Range = 2,
    }

    [CreateAssetMenu(fileName = "New Phase Params", menuName = "Params/PhaseParams", order = 15)]
    public class PhaseParams : ScriptableObject
    {
        [SerializeField] private List<ChunkParams> chunks = null;
        [SerializeField] private float movementSpeed = 0;
        [SerializeField] private bool spawnObstacles = true;
        [SerializeField] private CharacterWeapon characterWeapon = 0;
        [SerializeField] private EnemySpawns _enemySpawns = EnemySpawns.None;
        [SerializeField] private PhaseType phaseType = PhaseType.Timeout;
        [SerializeField] private int completeConditionEnemies = 0;
        [SerializeField] private int completeConditionDuration = 0;
        [SerializeField] private int completeConditionBossHealth = 0;

        public List<ChunkParams> Chunks => chunks;
        public float MovementSpeed => movementSpeed;
        public bool SpawnObstacles => spawnObstacles;
        public CharacterWeapon CharacterWeapon => characterWeapon;
        public EnemySpawns EnemySpawns => _enemySpawns;
        public PhaseType PhaseType => phaseType;
        public int CompleteConditionEnemies => completeConditionEnemies;
        public float CompleteConditionDuration => completeConditionDuration;
        public float CompleteBossHealth => completeConditionBossHealth;
    }
}