using System.Collections.Generic;
using System.Linq;
using TimeSystem;
using UnityEngine;

namespace Module.Game.Level.Chunk
{
    public class ChunkController : ITick
    {
        private LevelParams _levelParams = null;

        private List<ChunkData> _chunks = new List<ChunkData>();

        private Transform _chunksContainer = null;

        private int _currentPhaseIndex = 0;

        public int CurrentPhaseIndex
        {
            set
            {
                _currentPhaseIndex = value;
                _movementSpeed = _levelParams.Phases[_currentPhaseIndex].MovementSpeed;
            }
        }

        private float _movementSpeed = 0f;

        public ChunkController(Transform chunksContainer, LevelParams levelParams)
        {
            _chunksContainer = chunksContainer;
            _levelParams = levelParams;
        }

        public void CreateLevel()
        {
            for (var i = 0; i < 10; i++)
            {
                CreateChunk();
            }
        }

        private void CreateChunk()
        {
            var previousChunkZ =
                _chunks.Count != 0 ? _chunks[_chunks.Count - 1].EndPositionZ : _chunksContainer.transform.position.z;

            var randomChunkParams = GetRandomChunk();
            var chunkGameObject =
                GameModule.Instance.GameObjectPool.GetObject(randomChunkParams.ChunkPrefab, _chunksContainer);
            chunkGameObject.transform.position = new Vector3(
                _chunksContainer.transform.position.x,
                _chunksContainer.transform.position.y,
                previousChunkZ);

            var chunkData = new ChunkData(chunkGameObject, randomChunkParams.ChunkLength);
            _chunks.Add(chunkData);
        }

        private ChunkParams GetRandomChunk()
        {
            var currentPhaseChunks = _levelParams.Phases[_currentPhaseIndex].Chunks;
            var index = Random.Range(0, currentPhaseChunks.Count);
            var chunkParams = currentPhaseChunks.ElementAt(index);
            return chunkParams;
        }

        public void Tick(float deltaTime)
        {
            // move all chunks
            var movementDelta = -1f * _movementSpeed * _chunksContainer.transform.forward;
            foreach (var chunkData in _chunks)
            {
                chunkData.GameObject.transform.position += movementDelta * deltaTime;
            }

            // remove old chunks
            if (_chunks.Count != 0)
            {
                var firstChunk = _chunks[0];
                var firstChunkStartPositionZ = firstChunk.StartPositionZ;
                if (firstChunkStartPositionZ < _chunksContainer.position.z - 100f)
                {
                    _chunks.RemoveAt(0);
                    GameModule.Instance.GameObjectPool.UtilizeObject(firstChunk.GameObject);
                }
            }

            // add more chunks
            if (_chunks.Count != 0)
            {
                var lastChunkEndPositionZ = _chunks[_chunks.Count - 1].EndPositionZ;
                if (lastChunkEndPositionZ < _chunksContainer.position.z + 100f)
                {
                    CreateChunk();
                }
            }
        }
    }
}