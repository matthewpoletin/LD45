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

        [SerializeField]
        private Slider progressBarSlider;
        [SerializeField] private Image progressBarFillImage;
        [SerializeField] private Image progressBarBackbroundImage;

        [SerializeField] private Color progressBarStartColor;

        [SerializeField] private Color progressBarFinishColor;
        [SerializeField] private Color progressBarBackgroundStartColor;
        [SerializeField] private Color progressBarBackgroundBossColor;

        [SerializeField]
        private GameObject instructionsPanel;

        [SerializeField]
        private Animator instructionsPanelAnimator;

        private bool isProgressBarFull;

        private Gradient progressBarGradient;
        private int progressBarStage = 0;

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

            ShowInstructions();
        }

        private void Update()
        {
            if (playerHealth == null) return;
            livesText.text = "Lives: " + playerHealth.CurrentHealth;
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
