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

        [field: SerializeField, HideInInspector] public GameParams GameParams { get; private set; }
        [field: SerializeField, HideInInspector] public GameObjectPool GameObjectPool { get; private set; }
        [field: SerializeField, HideInInspector] public LevelManager LevelManager { get; private set; }

        private void Init()
        {
            // initialize subsystems
            GameObjectPool.Init();

            // initialize level
            LevelManager.Init();

            LevelManager.CreateLevel();
        }
    }
}