using UnityEngine;


namespace Nothing
{
    [RequireComponent(typeof(Collider))]
    public class WeaponPickable : MonoBehaviour
    {
        public Weapon weapon;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();

            if (player is null)
                return;

            player.Weapon = weapon;

            Destroy(gameObject);
        }
    }
}