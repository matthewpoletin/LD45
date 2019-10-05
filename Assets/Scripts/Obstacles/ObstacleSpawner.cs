using UnityEngine;
using System.Collections;
using Module.Game;

namespace Nothing
{
    public class ObstacleSpawner : MonoBehaviour
    {
        public ObstacleGroups obstacleGroups;
        public float spawnPauseMin = 1;
        public float spawnPauseMax = 2;


        public GameObject GetNextObstacle()
        {
            var prefab = obstacleGroups.obstacleGroups[Random.Range(0, obstacleGroups.obstacleGroups.Count)];
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