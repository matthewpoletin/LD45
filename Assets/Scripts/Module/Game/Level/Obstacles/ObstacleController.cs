using TimeSystem;
using UnityEngine;

namespace Module.Game.Level.Obstacles
{
    public class ObstacleController : ITick
    {
        private ObstacleGroupParams _obstacleGroupParams = null;
        private Transform _obstacleContainer = null;

        private const float SpawnPauseMin = 0.4f;
        private const float SpawnPauseMax = 0.8f;

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

                var row = Random.Range(-1, 2);

                obstacle.transform.localPosition = new Vector3(
                    row * GameModule.Instance.GameParams.lineWidth,
                    obstacle.transform.localPosition.y,
                    obstacle.transform.localPosition.z);

                var rot = obstacle.transform.localRotation;
                obstacle.transform.localRotation = Quaternion.Euler(rot.x, rot.y + Random.Range(0, 360), rot.z);

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