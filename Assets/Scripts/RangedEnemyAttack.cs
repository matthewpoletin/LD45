using UnityEngine;
using System.Collections;

namespace Nothing
{
    public class RangedEnemyAttack : LevelObject
    {
        public float attackPause = 2;
        public GameObject projectile;

        private void Awake()
        {
            StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                Instantiate(projectile, transform);

                yield return new WaitForSeconds(attackPause);
            }
        }
    }
}
