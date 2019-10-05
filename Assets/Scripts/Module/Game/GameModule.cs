using UnityEngine;

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

        [SerializeField] private GameParams GameParams;

        private void Init()
        {
            // FIXME: put initialization for subsystems here
        }
    }
}