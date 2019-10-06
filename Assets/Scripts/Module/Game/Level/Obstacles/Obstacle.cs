using UnityEngine;

namespace Module.Game.Level.Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();
            var deleter = other.gameObject.GetComponent<LevelObjectDeleter>();

            if (deleter != null)
                GameModule.Instance.GameObjectPool.UtilizeObject(gameObject);

            if (player is null)
                return;

            player.GetComponent<Health>().Kill();
        }
    }
}