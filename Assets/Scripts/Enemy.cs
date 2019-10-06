using UnityEngine;
using System.Collections;

namespace Nothing
{
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        public int damage = 1;
        public bool dieInColissionWithObstacles = true;
        public bool dieInColissionWithPlayer = true;

        [SerializeField, HideInInspector]
        private Health health;

        private void Start() {
            health = GetComponent<Health>();
            health.OnHealthDepleated = () => Destroy(gameObject);
        }
        
        public void OnTriggerEnter(Collider other) {
            var obstacleGroup = other.gameObject.GetComponent<ObstacleGroup>();

            if (obstacleGroup != null && dieInColissionWithObstacles) {
                health.Kill();
                return;
            }

            var player = other.gameObject.GetComponent<Player>();

            if (player == null)
                return;

            player.GetComponent<Health>().Damage(damage);

            if (dieInColissionWithPlayer)
                health.Kill();
        }
    }
}
