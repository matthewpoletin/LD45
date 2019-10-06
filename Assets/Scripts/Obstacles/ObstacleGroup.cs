using UnityEngine;
using System.Collections;
using Module.Game;
using UnityEngine.SceneManagement;

namespace Nothing
{
    public class ObstacleGroup : LevelObject
    {
        public void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();
            var deleter = other.gameObject.GetComponent<LevelObjectDeleter>();

            if (deleter != null)
                GameModule.Instance.GameObjectPool.UtilizeObject(gameObject);

            if (player is null)
                return;

            player.GetComponent<Health>().Damage(9999);
        }
    }
}