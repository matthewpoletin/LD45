using UnityEngine;

namespace Module.Game.Level.Chunk
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
}