using UnityEngine;

namespace Module.Game.Level
{
    [CreateAssetMenu(fileName = "New Chunk Params", menuName = "Params/ChunkParams", order = 20)]
    public class ChunkParams : ScriptableObject
    {
        [SerializeField] private GameObject chunkPrefab = null;
        [SerializeField] private float chunkLength = 0f;

        public GameObject ChunkPrefab => chunkPrefab;
        public float ChunkLength => chunkLength;
    }
}