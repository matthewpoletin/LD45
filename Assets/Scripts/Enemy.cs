using UnityEngine;
using System.Collections;

namespace Nothing
{
    [RequireComponent(typeof(Health))]
    public class Enemy : LevelObject
    {
        public int damage = 1;

        [SerializeField, HideInInspector]
        private Health health;

        private void Start() {
            health = GetComponent<Health>();
            health.OnHealthDepleated = () => Destroy(gameObject);
        }

        public void OnTriggerEnter(Collider other) {
            var player = other.gameObject.GetComponent<Player>();

            if (player is null)
                return;

            player.GetComponent<Health>().Damage(damage);

            health.Kill();
        }
    }
}
