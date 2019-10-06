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

        public void Init(LevelParams levelParams)
        {
            _levelParams = levelParams;

            _currentPhaseIndex = 0;

            _chunksController = new ChunkController(chunksContainer, _levelParams);
            _chunksController.CurrentPhaseIndex = _currentPhaseIndex;

            // Load prefab resources
            foreach (var phaseParams in _levelParams.Phases)
            {
                foreach (var chunkParams in phaseParams.Chunks)
                {
                    // Add chunks to object pool
                }
            }

            _chunksController.CreateLevel();
        }

        public void IncrementPhase()
        {
            _currentPhaseIndex += 1;
            if (_currentPhaseIndex > _levelParams.Phases.Count)
            {
                // FIXME: Show win screen
                return;
            }

            _chunksController.CurrentPhaseIndex = _currentPhaseIndex;
            _currentLevelMovementSpeed = _levelParams.Phases[_currentPhaseIndex].MovementSpeed;
        }

        public void Update()
        {
            _chunksController.Tick(Time.deltaTime);
        }
    }
}