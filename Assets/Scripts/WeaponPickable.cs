using UnityEngine;


namespace Nothing
{
    [RequireComponent(typeof(Collider))]
    public class WeaponPickable : LevelObject
    {
        public WeaponType weaponType;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();

            if (player is null)
                return;

            player.ChangeWeapon(weaponType);

            Destroy(gameObject);
        }
    }
}