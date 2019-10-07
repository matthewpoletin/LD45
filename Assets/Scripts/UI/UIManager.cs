using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Nothing
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private RectTransform restartContainer = null;
        [SerializeField] private Button restartButton = null;
        [SerializeField] private Text restartText = null;

        public void OnButtonClick()
        {
            SceneManager.LoadScene("Game");
        }
        
        [SerializeField]
        private Health playerHealth;

        [SerializeField] private RectTransform heartsContainer;
        [SerializeField] private GameObject heartIcon;

        [Header("Progress Bar")]

        [SerializeField]
        private Slider progressBarSlider;
        [SerializeField] private Image progressBarFillImage;
        [SerializeField] private Image progressBarBackbroundImage;

        [SerializeField] private Color progressBarStartColor;

        [SerializeField] private Color progressBarFinishColor;
        [SerializeField] private Color progressBarBackgroundStartColor;
        [SerializeField] private Color progressBarBackgroundBossColor;

        [Header("Instructions")]

        [SerializeField]
        private GameObject instructionsPanel;

        [SerializeField]
        private Animator instructionsPanelAnimator;

        [Header("Fader")]
        public Image faderImage = null;
        [SerializeField]
        private float fadeInSpeed = 1f;
        
        private static readonly int Instructions = Animator.StringToHash("ShowInstructions");

        private bool _isProgressBarFull;
        private Gradient _progressBarGradient;
        private int _progressBarStage = 0;
        private List<GameObject> _heartIcons = new List<GameObject>();

        private void Awake()
        {
            restartContainer.gameObject.SetActive(false);
        }

        private void OnEnable() {
            playerHealth.OnDamageTaken += OnPlayerDamage;
        }

        private static string[] _strings =
        {
            "In the beginning there nothing was.",
            "When start with nothing, run. Because",
            "Bad tongue is coming, letters near.",
            "You run, they hunt you. They are here.",
        };

        public void EndGame()
        {
            restartText.text = _strings[Random.Range(0, _strings.Length - 1)];
            restartContainer.gameObject.SetActive(true);
        }

        private void OnDisable() {
            playerHealth.OnDamageTaken -= OnPlayerDamage;
        }

        public void Init()
        {
            _progressBarGradient = new Gradient();
            var gradientColorKeys = new GradientColorKey[2];
            gradientColorKeys[0].color = progressBarStartColor;
            gradientColorKeys[0].time = 0.0f;

            gradientColorKeys[1].color = progressBarFinishColor;
            gradientColorKeys[1].time = 1.0f;

            var gradientAlphaKeys = new GradientAlphaKey[0];

            _progressBarGradient.SetKeys(gradientColorKeys, gradientAlphaKeys);

            progressBarBackbroundImage.color = progressBarBackgroundStartColor;

            FadeIn();
            ShowInstructions();
            SetUpHealthIcons(playerHealth.TotalHealth);
            UpdateHealth(playerHealth.TotalHealth);
        }

        private void OnPlayerDamage(int damageAmount) {
            UpdateHealth(playerHealth.CurrentHealth);
        }

        private void UpdateHealth(int healthAmount) {
            for (int i = 0; i < playerHealth.TotalHealth; i++)
            {
                if (i >= healthAmount)
                    _heartIcons[i].SetActive(false);
                else
                {
                    _heartIcons[i].SetActive(true);
                }
            }
        }

        private void SetUpHealthIcons(int totalHealth)
        {
            for (int i = 0; i < totalHealth; i++)
            {
                GameObject heartIconOb = Instantiate(heartIcon, heartsContainer);
                _heartIcons.Add(heartIconOb);
            }
        }

        private void FadeIn(){
            faderImage.CrossFadeAlpha(0, fadeInSpeed, false);
        }

        public void ShowInstructions()
        {
            instructionsPanel.SetActive(true);
            instructionsPanelAnimator.SetTrigger(Instructions);
        }

        public void UpdateProgress(float progressAmount)
        {
            progressBarSlider.value = progressAmount;
            if (_progressBarStage == 0)
                progressBarFillImage.color = _progressBarGradient.Evaluate(progressAmount); 
            if (progressAmount >= 0.99f && !_isProgressBarFull) {
                ProgressBarFull();
            }
        }

        private void ProgressBarFull() {
            _progressBarStage = 1;
            _isProgressBarFull = true;
            progressBarBackbroundImage.color = progressBarBackgroundBossColor;
        }
    }
}
