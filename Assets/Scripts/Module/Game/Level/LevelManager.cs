using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEditor;
using UnityEngine;

namespace Module.Game.Level
{
    public class ChunkData
    {
        public ChunkData(GameObject gameObject, float chunkLength)
        {
            _gameObject = gameObject;
            _chunkLength = chunkLength;
        }

        private GameObject _gameObject;
        private float _chunkLength;

        public GameObject GameObject => _gameObject;
        public float StartPositionZ => _gameObject.transform.position.z;
        public float EndPositionZ => _gameObject.transform.position.z + _chunkLength;
    }

    public class LevelManager : MonoBehaviour
    {
        private float _currentLevelMovementSpeed = 15f;

        public float CurrentLevelMovementSpeed => Mathf.Max(0f, _currentLevelMovementSpeed);

        [SerializeField] private Transform chunksContainer = null;

        private List<ChunkData> _chunks = new List<ChunkData>();

        private LevelParams _levelParams = null;

        private int _currentWaveIndex = 0;

        public void Init(LevelParams levelParams)
        {
            _levelParams = levelParams;

            _currentWaveIndex = 0;

            // Load prefab resources
            foreach (var waveParams in _levelParams.Waves)
            {
                foreach (var chunkParams in waveParams.Chunks)
                {
                    // Add chunks to object pool
                }
            }

            CreateLevel();
        }

        private void CreateLevel()
        {
            for (var i = 0; i < 10; i++)
            {
                CreateChunk();
            }
        }

        private void CreateChunk()
        {
            var previousChunkZ =
                _chunks.Count != 0 ? _chunks[_chunks.Count - 1].EndPositionZ : chunksContainer.transform.position.z;

            var randomChunkParams = GetRandomChunk();
            var chunkGameObject = GameModule.Instance.GameObjectPool.GetObject(randomChunkParams.ChunkPrefab, chunksContainer);
            chunkGameObject.transform.position = new Vector3(0, 0, previousChunkZ);

            var chunkData = new ChunkData(chunkGameObject, randomChunkParams.ChunkLength);
            _chunks.Add(chunkData);
        }

        private ChunkParams GetRandomChunk()
        {
            var currentWaveChunks = _levelParams.Waves[_currentWaveIndex].Chunks;
            var index = Random.Range(0, currentWaveChunks.Count);
            var chunkParams = currentWaveChunks.ElementAt(index);
            return chunkParams;
        }

        public void Update()
        {
            // move all chunks
            var movementDelta = -1f * CurrentLevelMovementSpeed * chunksContainer.transform.forward;
            foreach (var chunkData in _chunks)
            {
                chunkData.GameObject.transform.position += movementDelta * Time.deltaTime;
            }

            // remove old chunks
            if (_chunks.Count != 0)
            {
                var firstChunk = _chunks[0];
                var firstChunkStartPositionZ = firstChunk.StartPositionZ;
                if (firstChunkStartPositionZ < chunksContainer.position.z - 100f)
                {
                    _chunks.RemoveAt(0);
                    GameModule.Instance.GameObjectPool.UtilizeObject(firstChunk.GameObject);
                }
            }

            // add more chunks
            if (_chunks.Count != 0)
            {
                var lastChunkEndPositionZ = _chunks[_chunks.Count - 1].EndPositionZ;
                if (lastChunkEndPositionZ < chunksContainer.position.z + 100f)
                {
                    CreateChunk();
                }
            }
        }
    }
}