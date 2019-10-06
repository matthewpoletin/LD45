using TimeSystem;
using UnityEngine;

namespace Module.Game.Level.Obstacles
{
    public class ObstacleController : ITick
    {
        private ObstacleGroupParams _obstacleGroupParams = null;
        private Transform _obstacleContainer = null;

        private const float SpawnPauseMin = 0.1f;
        private const float SpawnPauseMax = 0.2f;

        private float _spawnCooldownTime = 0f;

        public ObstacleController(ObstacleGroupParams obstacleGroupParams, Transform obstacleContainer)
        {
            _obstacleGroupParams = obstacleGroupParams;
            _obstacleContainer = obstacleContainer;
        }

        public void Tick(float deltaTime)
        {
            if (Time.time > _spawnCooldownTime)
            {
                var obstacle = GetRandomObstacle();

                if (obstacle == null)
                {
                    return;
                }

                obstacle.transform.localPosition = Vector3.zero;

                var pause = Random.Range(SpawnPauseMin, SpawnPauseMax);
                _spawnCooldownTime = Time.time + pause;
            }
        }

        private GameObject GetRandomObstacle()
        {
            var prefab =
                _obstacleGroupParams.ObstacleGroups[Random.Range(0, _obstacleGroupParams.ObstacleGroups.Count)];
            var go = GameModule.Instance.GameObjectPool.GetObject(prefab, _obstacleContainer);
            return go;
        }
    }
}