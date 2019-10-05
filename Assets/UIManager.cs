using UnityEngine;
using System.Collections;
using TMPro;

namespace Nothing
{
    public class UIManager : MonoBehaviour
    {
        public Player player;

        public TextMeshProUGUI moneyText;

        private void Update()
        {
            moneyText.text = "Money: " + player.Money;
        }
    }
}
