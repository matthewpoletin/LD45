using UnityEngine;
using System.Collections;

namespace Nothing
{
    public class RangedEnemyAttack : MonoBehaviour
    {
        public float attackPause = 2;
        public GameObject projectilePrefab;

        private void Start()
        {
            StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                var projectile = Instantiate(projectilePrefab, ProjectileHolder.Inst.transform);
                projectile.transform.position = transform.position;

                yield return new WaitForSeconds(attackPause);
            }
        }
    }
}
