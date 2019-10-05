using UnityEngine;


namespace Nothing
{
    [RequireComponent(typeof(Collider))]
    public class MoneyPickable : MonoBehaviour
    {
        public int amount = 1;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();

            if (player is null)
                return;

            player.Money += amount;

            Destroy(gameObject);
        }
    }
}