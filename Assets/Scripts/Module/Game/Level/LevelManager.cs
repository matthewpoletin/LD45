using Module.Game.Level.Chunk;
using UnityEngine;

namespace Module.Game.Level
{
    public class LevelManager : MonoBehaviour
    {
        private float _currentLevelMovementSpeed = 15f;

        public float CurrentLevelMovementSpeed => Mathf.Max(0f, _currentLevelMovementSpeed);

        [SerializeField] private Transform chunksContainer = null;

        private LevelParams _levelParams = null;

        private int _currentPhaseIndex = 0;

        private ChunkController _chunksController = null;

        private int _enemiesKilledCounter = 0;
        private float _phaseTimeoutTime = 0f;
        private PhaseType _currentPhaseType;
        private int _currentPhaseCompletionEnemies;
        private float _currentPhaseBossHealth;

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
                    // FIXME: Add chunks to object pool
                }
            }

            _chunksController.CreateLevel();
        }

        public void StartLevel()
        {
            IncrementPhase();
        }

        public void IncrementPhase()
        {
            _currentPhaseIndex += 1;
            
            Debug.Log($"Phase {_currentPhaseIndex} starts now");
            
            if (_currentPhaseIndex > _levelParams.Phases.Count)
            {
                // FIXME: Show win screen
                return;
            }

            var nextPhaseParams = _levelParams.Phases[_currentPhaseIndex];

            _currentPhaseType = nextPhaseParams.PhaseType;
            _currentPhaseCompletionEnemies = nextPhaseParams.CompleteConditionEnemies;
            var completeConditionDuration = nextPhaseParams.CompleteConditionDuration;
            completeConditionDuration =
                Mathf.Approximately(completeConditionDuration, 0f) ? 40f : completeConditionDuration;
            _phaseTimeoutTime = Time.time + completeConditionDuration;

            _chunksController.CurrentPhaseIndex = _currentPhaseIndex;
            _currentLevelMovementSpeed = nextPhaseParams.MovementSpeed;
        }

        public void Update()
        {
            if (IsPhaseCompleted())
            {
                IncrementPhase();
            }

            _chunksController.Tick(Time.deltaTime);
        }

        private bool IsPhaseCompleted()
        {
            switch (_currentPhaseType)
            {
                case PhaseType.Timeout:
                {
                    if (_phaseTimeoutTime <= Time.time)
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