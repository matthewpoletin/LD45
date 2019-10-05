using UnityEngine;
using System.Collections;
using TMPro;

namespace Nothing
{
    public class UIManager : MonoBehaviour
    {
        public Player player;

        public TextMeshProUGUI livesText;

        private void Update()
        {
            livesText.text = "Lives: " + player.Lives;
        }
    }
}
