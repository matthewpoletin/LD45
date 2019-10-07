using UnityEngine;
using System.Collections;

namespace Nothing {
    public class Projectile : LevelObject {
        public int damage = 1;
        public LayerMask ignoredLayerMask;

        public Transform[] models;

        private static bool Contains(LayerMask mask, int layer) {
            return mask == (mask | (1 << layer));
        }

        private void Awake() {
            models[Random.Range(0, models.Length)].gameObject.SetActive(true);
        }

        public void OnTriggerEnter(Collider other) {
            var health = other.gameObject.GetComponent<Health>();
            if (health == null)
                return;
            
            if (Contains(ignoredLayerMask, health.gameObject.layer))
                return;

            health.Damage(damage);

            Destroy(gameObject);
        }
    }
}