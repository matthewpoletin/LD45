using UnityEngine;
using System.Collections;
using Module.Game;

namespace Nothing
{
    public class EnemySpawner : MonoBehaviour
    {
        public float spawnPauseMin = 1;
        public float spawnPauseMax = 2;
        public GameObject enemy;


        private void Start()
        {
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                var go = Instantiate(enemy, transform);

                var row = Random.Range(-1, 2);

                go.transform.localPosition = new Vector3(
                    row * GameModule.Instance.GameParams.lineWidth,
                    go.transform.localPosition.y,
                    go.transform.localPosition.z);

                yield return new WaitForSeconds(Random.Range(spawnPauseMin, spawnPauseMax));
            }
        }
    }
}