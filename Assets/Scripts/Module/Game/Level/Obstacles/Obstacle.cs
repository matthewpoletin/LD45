using UnityEngine;

namespace Module.Game.Level.Obstacles
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