using Module.Game.Level;
using UnityEngine;
using Utils;

namespace Module.Game
{
    public class GameModule : MonoBehaviour
    {
        #region singleton

        public static GameModule Instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }

            Init();
        }

        #endregion

        public GameObjectPool GameObjectPool => gameObjectPool;
        public LevelManager LevelManager => levelManager;

        [SerializeField] private GameParams gameParams = null;
        [SerializeField] private GameObjectPool gameObjectPool = null;
        [SerializeField] private LevelManager levelManager = null;

        private void Init()
        {
            // initialize subsystems
            gameObjectPool.Init();

            // initialize level
            levelManager.Init(gameParams.Levels[0]);
        }
    }
}