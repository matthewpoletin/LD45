using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Module.Game.Boss
{
    public class BossAnimationView : MonoBehaviour
    {
        [SerializeField] private Animator bossAnimator;

        public void ShowBoss()
        {
            // bossAnimator.
        }

        public void AttackLeft()
        {
            bossAnimator.SetTrigger("AttackLeft");
        }

        public void AttackRight()
        {
            bossAnimator.SetTrigger("AttackRight");
        }

        public void Death() {
            bossAnimator.SetTrigger("Death");
        }
    }
}
