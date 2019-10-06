using UnityEngine;
using System.Collections;
using Module.Game;
using Module.Game.Level.Obstacles;

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

            health.OnHealthDepleated += OnEnemyDestroyed;
        }

        void OnEnemyDestroyed()
        {
            GameModule.Instance.LevelManager.EnemiesKilledCounter++;
            Destroy(gameObject);
        }

        public void OnTriggerEnter(Collider other) {
            var obstacleGroup = other.gameObject.GetComponent<ObstacleGroup>();

            if (obstacleGroup != null) {
                health.Kill();
                return;
            }

            var player = other.gameObject.GetComponent<Player>();

            if (player == null)
                return;

            player.GetComponent<Health>().Damage(damage);

            health.Kill();
        }
    }
}
