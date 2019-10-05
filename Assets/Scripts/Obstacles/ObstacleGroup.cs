using UnityEngine;
using System.Collections;
using Module.Game;
using UnityEngine.SceneManagement;

namespace Nothing
{
    public class ObstacleGroup : MonoBehaviour
    {
        private void Update()
        {
            var newZ = transform.localPosition.z - Time.deltaTime * GameModule.Instance.LevelManager.CurrentLevelMovementSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZ);
        }

        public void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<Player>();
            var deleter = other.gameObject.GetComponent<ObstaclesDeleter>();

            if (deleter != null)
                Destroy(gameObject);

            if (player is null)
                return;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}