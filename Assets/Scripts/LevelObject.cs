using UnityEngine;
using Module.Game;

namespace Nothing {
    public enum LevelObjectMovementDirection {
        ToPlayer,
        AwayFromPlayer
    }

    public class LevelObject : MonoBehaviour
    {
        public LevelObjectMovementDirection direction = LevelObjectMovementDirection.ToPlayer;
        public float speed = 1;

        private void Update()
        {
            var changeInPos = Time.deltaTime * GameModule.Instance.LevelManager.CurrentLevelMovementSpeed * speed;
            changeInPos *= direction == LevelObjectMovementDirection.ToPlayer ? -1 : 1;
            var newZ = transform.localPosition.z + changeInPos;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZ);
        }

        private void OnTriggerEnter(Collider other)
        {
            var deleter = other.gameObject.GetComponent<LevelObjectDeleter>();

            if (deleter != null)
                GameModule.Instance.GameObjectPool.UtilizeObject(gameObject);
        }
    }
}
