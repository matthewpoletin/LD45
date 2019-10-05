using UnityEngine;
using System.Collections;
using Module.Game;

namespace Nothing
{
    public class ObstacleSpawner : MonoBehaviour
    {
        public Obstacles obstacles;
        public float spawnPauseMin = 1;
        public float spawnPauseMax = 2;


        public GameObject GetNextObstacle()
        {
            var prefab = obstacles.obstacles[Random.Range(0, obstacles.obstacles.Count)];
            var go = GameModule.Instance.GameObjectPool.GetObject(prefab, transform);
            return go;
        }

        private void Start()
        {
            StartCoroutine(SpawnObstacles());
        }

        private IEnumerator SpawnObstacles()
        {
            while (true)
            {
                var obstacle = GetNextObstacle();

                obstacle.transform.localPosition = Vector3.zero;

                var pause = Random.Range(spawnPauseMin, spawnPauseMax);
                yield return new WaitForSeconds(pause);
            }
        }
    }
}