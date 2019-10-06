using System;
using Module.Game.Level.Chunk;
using Module.Game.Level.Obstacles;
using Module.Game.Level.Phase;
using Nothing;
using TimeSystem;
using UnityEngine;

namespace Module.Game.Level
{
    public class LevelManager : MonoBehaviour, ITick
    {
        private float _currentLevelMovementSpeed = 15f;

        public float CurrentLevelMovementSpeed => Mathf.Max(0f, _currentLevelMovementSpeed);


        [SerializeField] private Transform chunksContainer = null;
        [SerializeField] private Transform obstaclesContainer = null;

        [SerializeField] private GameObject bossPrefab = null;
        [SerializeField] private Transform bossContainer = null;

        private LevelParams _levelParams = null;

        private int _currentPhaseIndex = 0;

        private ChunkController _chunksController = null;
        private ObstacleController _obstacleController = null;

        private int _enemiesKilledCounter = 0;
        private float _phaseTimeoutStartTime = 0f;
        private float _phaseTimeoutEndTime = 0f;
        private PhaseType _currentPhaseType;
        private int _currentPhaseCompletionEnemies;
        private float _currentPhaseBossHealth;

        public int EnemiesKilledCounter
        {
            get => _enemiesKilledCounter;
            set => _enemiesKilledCounter = value;
        }

        public event Action OnPhaseCompletion;
        public event Action<float> OnPhaseLevelChange;
        private float _phaseProgressLevel = 0f;
        private float _overallProgressLevel = 0f;

        public float PhaseProgressLevel
        {
            set
            {
                _phaseProgressLevel = Mathf.Clamp(value, 0f, 1f);
                OnPhaseLevelChange?.Invoke(_phaseProgressLevel);
                if (_phaseProgressLevel >= 1f)
                {
                    OnPhaseCompletion?.Invoke();
                }
            }
        }

        public void Init(LevelParams levelParams)
        {
            _levelParams = levelParams;

            _currentPhaseIndex = -1;

            _chunksController = new ChunkController(chunksContainer, _levelParams);

            // Load prefab resources
            foreach (var phaseParams in _levelParams.Phases)
            {
                foreach (var chunkParams in phaseParams.Chunks)
                {
                    GameModule.Instance.GameObjectPool.AddObject(chunkParams.ChunkPrefab);
                }
            }

            _obstacleController = new ObstacleController(levelParams.ObstacleGroupParams, obstaclesContainer);

            _chunksController.CreateLevel();
        }

        public void StartLevel()
        {
            IncrementPhase();
        }

        public void IncrementPhase()
        {
            _currentPhaseIndex += 1;

            Debug.Log($"Phase {_currentPhaseIndex + 1} starts now");

            if (_currentPhaseIndex > _levelParams.Phases.Count)
            {
                // FIXME: Show win screen
                return;
            }

            var nextPhaseParams = _levelParams.Phases[_currentPhaseIndex];

            if (_currentPhaseIndex == _levelParams.Phases.Count)
            {
                var bossGameObject = Instantiate(bossPrefab, bossContainer);
            }

            _currentPhaseType = nextPhaseParams.PhaseType;
            _currentPhaseCompletionEnemies = nextPhaseParams.CompleteConditionEnemies != 0
                ? nextPhaseParams.CompleteConditionEnemies
                : 1;
            var completeConditionDuration = nextPhaseParams.CompleteConditionDuration;
            completeConditionDuration =
                Mathf.Approximately(completeConditionDuration, 0f) ? 40f : completeConditionDuration;
            _phaseTimeoutStartTime = Time.time;
            _phaseTimeoutEndTime = Time.time + completeConditionDuration;

            // set enemy type
            if (_currentPhaseIndex == 1)
            {
                GameModule.Instance.EnemySpawner.EnemyTypes = EnemyType.Melee;
            }
            else if (_currentPhaseIndex == 2 || _currentPhaseIndex == 3)
            {
                GameModule.Instance.EnemySpawner.EnemyTypes = EnemyType.Melee | EnemyType.Ranged;
            }
            else
            {
                GameModule.Instance.EnemySpawner.EnemyTypes = EnemyType.None;
            }

            _chunksController.CurrentPhaseIndex = _currentPhaseIndex;
            _currentLevelMovementSpeed = nextPhaseParams.MovementSpeed;
        }

        public void Tick(float deltaTime)
        {
            UpdateCurrentPhaseState();

            CheckPhaseCompletion();

            _chunksController.Tick(deltaTime);
            _obstacleController.Tick(deltaTime);
        }

        private void CheckPhaseCompletion()
        {
            if (IsPhaseCompleted())
            {
                IncrementPhase();
            }
        }

        private void UpdateCurrentPhaseState()
        {
            switch (_currentPhaseType)
            {
                case PhaseType.Timeout:
                {
                    PhaseProgressLevel = (Time.time - _phaseTimeoutStartTime) /
                                         (_phaseTimeoutEndTime - _phaseTimeoutStartTime);
                    break;
                }

                case PhaseType.EnemiesKilled:
                {
                    PhaseProgressLevel = (float) _enemiesKilledCounter / _currentPhaseCompletionEnemies;

                    break;
                }

                case PhaseType.BossKill:
                {
                    break;
                }
            }
        }

        private bool IsPhaseCompleted()
        {
            switch (_currentPhaseType)
            {
                case PhaseType.Timeout:
                {
                    if (_phaseTimeoutEndTime <= Time.time)
                    {
                        return true;
                    }

                    break;
                }

                case PhaseType.EnemiesKilled:
                {
                    if (_enemiesKilledCounter >= _currentPhaseCompletionEnemies)
                    {
                        return true;
                    }

                    break;
                }

                case PhaseType.BossKill:
                {
                    // FIXME: check if boss is dead
                    break;
                }
            }

            return false;
        }
    }
}