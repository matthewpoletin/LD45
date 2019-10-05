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

        [SerializeField] private GameParams gameParams;

        [SerializeField] private GameObjectPool gameObjectPool;

        [SerializeField] private LevelManager levelManager;

        public GameObjectPool GameObjectPool => gameObjectPool;

        private void Init()
        {
            // initialize subsystems
            gameObjectPool.Init();

            // initialize level
            levelManager.Init();

            levelManager.CreateLevel();
        }
    }
}