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
        private const float CHUNK_LENGTH = 20.0f;

        private float _currentLevelMovementSpeed = 15f;

        public float CurrentLevelMovementSpeed => Mathf.Max(0f, _currentLevelMovementSpeed);

        [SerializeField] private Transform chunksContainer = null;

        private static string[] _chunckResources =
        {
            ApplicationResources.LEVEL_1_CHUNK_1,
            ApplicationResources.LEVEL_1_CHUNK_2,
            ApplicationResources.LEVEL_1_CHUNK_3,
        };

        private List<GameObject> _chunkPrefabs = new List<GameObject>();

        private List<ChunkData> _chunks = new List<ChunkData>();

        public void Init()
        {
            // Load prefab resources
            foreach (var chunkResourceFile in _chunckResources)
            {
                var chunkPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(chunkResourceFile);
                _chunkPrefabs.Add(chunkPrefab);
            }
        }

        /// <summary>Create first level chunks</summary>
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
                _chunks.Count != 0 ? _chunks[_chunks.Count - 1].EndPositionZ : chunksContainer.transform.position.z;

            var randomChunkPrefab = GetRandomChunk();
            var chunkGameObject = GameModule.Instance.GameObjectPool.GetObject(randomChunkPrefab, chunksContainer);
            chunkGameObject.transform.position = new Vector3(0, 0, previousChunkZ);

            var chunkData = new ChunkData(chunkGameObject, CHUNK_LENGTH);
            _chunks.Add(chunkData);
        }

        private GameObject GetRandomChunk()
        {
            var index = Random.Range(0, _chunkPrefabs.Count);
            var randomChunkPrefab = _chunkPrefabs.ElementAt(index);
            return randomChunkPrefab;
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