using UnityEngine;
using Module.Game;

namespace Nothing
{
    public class LevelObject : MonoBehaviour
    {
        private void Update()
        {
            var newZ = transform.localPosition.z - Time.deltaTime * GameModule.Instance.LevelManager.CurrentLevelMovementSpeed;
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
