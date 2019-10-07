using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace Nothing
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Health playerHealth;

        [SerializeField]
        private TextMeshProUGUI livesText;

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

        private bool isProgressBarFull;

        private Gradient progressBarGradient;
        private int progressBarStage = 0;

        private void OnEnable() {
            playerHealth.OnDamageTaken += OnPlayerDamage;
        }

        private void OnDisable() {
            playerHealth.OnDamageTaken -= OnPlayerDamage;
        }

        public void Init()
        {
            progressBarGradient = new Gradient();
            var gradientColorKeys = new GradientColorKey[2];
            gradientColorKeys[0].color = progressBarStartColor;
            gradientColorKeys[0].time = 0.0f;

            gradientColorKeys[1].color = progressBarFinishColor;
            gradientColorKeys[1].time = 1.0f;

            var gradientAlphaKeys = new GradientAlphaKey[0];

            progressBarGradient.SetKeys(gradientColorKeys, gradientAlphaKeys);

            progressBarBackbroundImage.color = progressBarBackgroundStartColor;

            FadeIn();
            ShowInstructions();
            UpdateHealth(playerHealth.TotalHealth);
        }

        private void OnPlayerDamage(int damageAmount) {
            UpdateHealth(playerHealth.CurrentHealth);
        }

        private void UpdateHealth(int healthAmount) {
            livesText.text = "Lives: " + healthAmount;
        }

        private void FadeIn(){
            faderImage.CrossFadeAlpha(0, fadeInSpeed, false);
        }

        public void ShowInstructions()
        {
            instructionsPanel.SetActive(true);
            instructionsPanelAnimator.SetTrigger("ShowInstructions");
        }

        public void UpdateProgress(float progressAmount)
        {
            progressBarSlider.value = progressAmount;
            if (progressBarStage == 0)
                progressBarFillImage.color = progressBarGradient.Evaluate(progressAmount); 
            if (progressAmount >= 0.99f && !isProgressBarFull) {
                ProgressBarFull();
            }
        }

        private void ProgressBarFull() {
            progressBarStage = 1;
            isProgressBarFull = true;
            progressBarBackbroundImage.color = progressBarBackgroundBossColor;
        }
    }
}
