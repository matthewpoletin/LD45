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

        private void Init()
        {
            gameObjectPool.Init();
            // FIXME: put initialization for subsystems here
        }
    }
}