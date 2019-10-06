using UnityEngine;
using System.Collections;
using Module.Game;
using System.Collections.Generic;

namespace Nothing {

    [System.Flags]
    public enum EnemyType {
        Melee = 1,
        Ranged = 2,
        Teeth = 4
    }

    public class EnemySpawner : MonoBehaviour {
        public float spawnPauseMin = 1;
        public float spawnPauseMax = 2;
        public GameObject meleeEnemyPrefab;
        public GameObject rangedEnemyPrefab;
        public GameObject teethEnemyPrefab;

        public EnemyType EnemyTypes { get; set; } = EnemyType.Melee;

        private void Start() {
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies() {
            while (true) {
                var prefabs = new List<GameObject>();
                if (EnemyTypes.HasFlag(EnemyType.Melee))
                    prefabs.Add(meleeEnemyPrefab);
                if (EnemyTypes.HasFlag(EnemyType.Ranged))
                    prefabs.Add(rangedEnemyPrefab);
                if (EnemyTypes.HasFlag(EnemyType.Teeth))
                    prefabs.Add(teethEnemyPrefab);

                if (prefabs.Count != 0) {
                    var prefab = prefabs[Random.Range(0, prefabs.Count)];

                    var go = Instantiate(prefab, transform);

                    var row = Random.Range(-1, 2);

                    go.transform.localPosition = new Vector3(
                        row * GameModule.Instance.GameParams.lineWidth,
                        go.transform.localPosition.y,
                        go.transform.localPosition.z);
                }

                yield return new WaitForSeconds(Random.Range(spawnPauseMin, spawnPauseMax));
            }
        }
    }
}