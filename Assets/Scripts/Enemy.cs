using UnityEngine;
using System.Collections;

namespace Nothing
{
    public class Enemy : MonoBehaviour
    {
        public int damage = 1;

        public void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();

            if (player is null)
                return;

            player.Lives -= damage;
        }
    }
}
