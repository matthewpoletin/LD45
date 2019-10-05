using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEditor;
using UnityEngine;

namespace Module.Game.Level
{
    public class LevelManager : MonoBehaviour
    {
        private const float CHUNK_LENGTH = 20.0f;

        public float CurrentLevelMovemnetSpeed = 0f;  

        [SerializeField] private Transform chunksContainer = null;

        private static string[] _chunckResources =
        {
            ApplicationResources.LEVEL_1_CHUNK_1,
            ApplicationResources.LEVEL_1_CHUNK_2,
            ApplicationResources.LEVEL_1_CHUNK_3,
        };

        private List<GameObject> _chunkPrefabs = new List<GameObject>();

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
            for (int i = 0; i < 10; i++)
            {
                var randomChunkPrefab = GetRandomChunk();
                var chunkGameObject = GameModule.Instance.GameObjectPool.GetObject(randomChunkPrefab, chunksContainer);
                chunkGameObject.transform.position = new Vector3(0, 0, i * CHUNK_LENGTH);
            }
        }

        private GameObject GetRandomChunk()
        {
            var index = Random.Range(0, _chunkPrefabs.Count);
            var randomChunkPrefab = _chunkPrefabs.ElementAt(index);
            return randomChunkPrefab;
        }
    }
}