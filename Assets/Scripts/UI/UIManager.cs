using UnityEngine;
using System.Collections;
using TMPro;

namespace Nothing
{
    public class UIManager : MonoBehaviour
    {
        public Health playerHealth;

        public TextMeshProUGUI livesText;

        private void Update()
        {
            livesText.text = "Lives: " + playerHealth.CurrentHealth;
        }
    }
}
