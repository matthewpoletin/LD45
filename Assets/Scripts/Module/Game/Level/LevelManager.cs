using System;
using System.Collections;
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

        private int _enemiesKilledPhaseCounter = 0;
        private float _phaseTimeoutStartTime = 0f;
        private float _phaseTimeoutEndTime = 0f;
        private PhaseType _currentPhaseType;
        private int _currentPhaseCompletionEnemies;
        private float _currentPhaseBossHealth;

        private int _totalNonBossPhases = 0;

        public int EnemiesKilledCounter
        {
            get => _enemiesKilledPhaseCounter;
            set => _enemiesKilledPhaseCounter = value;
        }

        public event Action OnPhaseCompletion;
        public event Action<float> OnPhaseLevelChange;
        public event Action<float> OnTotalProgressChange;
        private float _phaseProgressLevel = 0f;
        private float _overallProgressLevel = 0f;

        public float PhaseProgressLevel
        {
            set
            {
                _phaseProgressLevel = Mathf.Clamp(value, 0f, 1f);
                OnPhaseLevelChange?.Invoke(_phaseProgressLevel);
                var totalProgressLevel = (float) _currentPhaseIndex / _totalNonBossPhases +
                                         _phaseProgressLevel / _totalNonBossPhases;
                OnTotalProgressChange?.Invoke(totalProgressLevel);
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

            _totalNonBossPhases = Math.Max(_levelParams.Phases.Count - 1, 1);

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

            _enemiesKilledPhaseCounter = 0;

            // conditions of finishing phase
            var nextPhaseParams = _levelParams.Phases[_currentPhaseIndex];

            _currentPhaseType = nextPhaseParams.PhaseType;
            _currentPhaseCompletionEnemies = nextPhaseParams.CompleteConditionEnemies != 0
                ? nextPhaseParams.CompleteConditionEnemies
                : 1;
            var completeConditionDuration = nextPhaseParams.CompleteConditionDuration;
            completeConditionDuration =
                Mathf.Approximately(completeConditionDuration, 0f) ? 40f : completeConditionDuration;
            _phaseTimeoutStartTime = Time.time;
            _phaseTimeoutEndTime = Time.time + completeConditionDuration;

            _obstacleController.SpawnActive = nextPhaseParams.SpawnObstacles;

            // set enemy type
            if (_currentPhaseIndex == 0)
            {
                GameModule.Instance.Player.ChangeWeapon(WeaponType.None);
            }
            else if (_currentPhaseIndex == 1)
            {
                GameModule.Instance.Player.ChangeWeapon(WeaponType.Melee);
            }
            else if (_currentPhaseIndex == 2 || _currentPhaseIndex == 3)
            {
                GameModule.Instance.Player.ChangeWeapon(WeaponType.Ranged);
            }
            else
            {
                GameModule.Instance.Player.ChangeWeapon(WeaponType.Ranged);
            }

            switch (nextPhaseParams.EnemySpawns)
            {
                case EnemySpawns.None:
                {
                    GameModule.Instance.EnemySpawner.EnemyTypes = EnemyType.None;
                    break;
                }
                case EnemySpawns.MeleeOnly:
                {
                    GameModule.Instance.EnemySpawner.EnemyTypes = EnemyType.Melee;
                    break;
                }
                case EnemySpawns.RangeOnly:
                {
                    GameModule.Instance.EnemySpawner.EnemyTypes = EnemyType.Ranged;
                    break;
                }
                case EnemySpawns.MeleeAndRange:
                {
                    GameModule.Instance.EnemySpawner.EnemyTypes = EnemyType.Melee | EnemyType.Ranged;
                    break;
                }

                case EnemySpawns.Boss:
                {
                    GameModule.Instance.EnemySpawner.EnemyTypes = EnemyType.None;
                    StartCoroutine(SpawnBoss(6f));
                    break;
                }
            }

            _chunksController.CurrentPhaseIndex = _currentPhaseIndex;
            _currentLevelMovementSpeed = nextPhaseParams.MovementSpeed;
        }

        private IEnumerator SpawnBoss(float delay)
        {
            yield return new WaitForSeconds(delay);
            var bossGameObject = Instantiate(bossPrefab, bossContainer);
            var bossController = bossGameObject.GetComponent<Boss>();
            var bossView = bossController.view;
            bossView.ShowBoss();
            OnTotalProgressChange -= GameModule.Instance.UiManager.UpdateProgress;
            bossController.Health.onHealthChanged += GameModule.Instance.UiManager.UpdateProgress;
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
                    PhaseProgressLevel = (float) _enemiesKilledPhaseCounter / _currentPhaseCompletionEnemies;

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
                    if (_enemiesKilledPhaseCounter >= _currentPhaseCompletionEnemies)
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