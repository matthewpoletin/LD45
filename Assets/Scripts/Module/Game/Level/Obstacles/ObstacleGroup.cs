using Nothing;
using UnityEngine;

namespace Module.Game.Level.Obstacles
{
    public class ObstacleGroup : LevelObject
    {
        public void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();
            var deleter = other.gameObject.GetComponent<LevelObjectDeleter>();

            if (deleter != null)
            {
                GameModule.Instance.GameObjectPool.UtilizeObject(gameObject);
            }

            if (player == null)
            {
                return;
            }

            player.GetComponent<Health>().Damage(9999);
        }
    }
}