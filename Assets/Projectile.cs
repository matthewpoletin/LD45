using UnityEngine;
using System.Collections;

namespace Nothing
{
    public class Projectile : LevelObject
    {
        public int damage = 0;

        public void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();

            if (player is null)
                return;

            player.GetComponent<Health>().Damage(damage);

            Destroy(gameObject);
        }
    }
}