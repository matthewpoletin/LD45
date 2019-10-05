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

        private int _currentWaveIndex = 0;

        private ChunkController _chunksController = null;

        public void Init(LevelParams levelParams)
        {
            _levelParams = levelParams;

            _currentWaveIndex = 0;

            _chunksController = new ChunkController(chunksContainer, _levelParams);
            _chunksController.CurrentWaveIndex = _currentWaveIndex;

            // Load prefab resources
            foreach (var waveParams in _levelParams.Waves)
            {
                foreach (var chunkParams in waveParams.Chunks)
                {
                    // Add chunks to object pool
                }
            }

            _chunksController.CreateLevel();
        }

        public void IncrementWave()
        {
            _currentWaveIndex += 1;
            if (_currentWaveIndex > _levelParams.Waves.Count)
            {
                // FIXME: Show win screen
                return;
            }

            _chunksController.CurrentWaveIndex = _currentWaveIndex;
            _currentLevelMovementSpeed = _levelParams.Waves[_currentWaveIndex].MovementSpeed;
        }

        public void Update()
        {
            _chunksController.Tick(Time.deltaTime);
        }
    }
}