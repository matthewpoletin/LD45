using Module.Audio;
using Module.Effects;
using Module.Game.Events;
using Module.Game.Level;
using Nothing;
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

        [SerializeField] private GameParams gameParams = null;
        [SerializeField] private GameObjectPool gameObjectPool = null;
        [SerializeField] private LevelManager levelManager = null;
        [SerializeField] private UIManager uiManager = null;
        [SerializeField] private SoundManager soundManager = null;
        [SerializeField] private EnemySpawner _enemySpawner = null;
        [SerializeField] private Player _player = null;

        public GameParams GameParams => gameParams;
        public GameObjectPool GameObjectPool => gameObjectPool;
        public LevelManager LevelManager => levelManager;
        public UIManager UiManager => uiManager;
        public SoundManager SoundManager => soundManager;
        public EnemySpawner EnemySpawner => _enemySpawner; 
        public Player Player => _player; 

        private VfxController _vfxController = null;
        private EventManager _eventManager = null;

        public EventManager EventManager => _eventManager;

        private void Init()
        {
            // initialize subsystems
            gameObjectPool.Init();

            // initialize level
            levelManager.Init(gameParams.Levels[0]);
            levelManager.StartLevel();

            uiManager.Init();

            soundManager.Init(gameParams.AudioParams);
            _vfxController = new VfxController();
            _eventManager = new EventManager(soundManager, _vfxController, GameParams.Events);

//            levelManager.OnPhaseLevelChange += uiManager.UpdateProgress;
            levelManager.OnTotalProgressChange += uiManager.UpdateProgress;
            levelManager.OnPhaseCompletion += soundManager.ChangeMusicPhase;
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            
            levelManager.Tick(deltaTime);
            _vfxController.Tick(deltaTime);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}