using UnityEngine;
using System.Collections;
using Module.Game;
using UnityEngine.SceneManagement;

namespace Nothing
{
    public class Obstacle : MonoBehaviour
    {
        public ObstacleGroup group;

        private void OnTriggerEnter(Collider other)
        {
            group.OnTriggerEnter(other);
        }
    }
}
