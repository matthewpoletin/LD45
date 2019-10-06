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

        [SerializeField]
        private GameObject instructionsPanel;

        [SerializeField]
        private Animator instructionsPanelAnimator;

        private void Update()
        {
            livesText.text = "Lives: " + playerHealth.CurrentHealth;
        }

        public void ShowInstructions() {
            instructionsPanel.SetActive(true);
            instructionsPanelAnimator.SetTrigger("ShowInstructions");
        }

        public void UpdateProgress(float progressAmount) {
            progressBarSlider.value = progressAmount;
        }
    }
}
