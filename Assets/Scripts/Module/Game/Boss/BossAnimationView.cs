using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventType = Module.Game.Events.EventType;


namespace Module.Game.Boss
{
    public class BossAnimationView : MonoBehaviour
    {
        [SerializeField] private Animator bossAnimator;

        public void ShowBoss()
        {
             GameModule.Instance.EventManager.ProcessEvent(EventType.TongueSpawn);
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

        public void AttackTrigger() {
            GameModule.Instance.EventManager.ProcessEvent(EventType.TongueAttack);
        }

        public void PreAttackTrigger() {
            GameModule.Instance.EventManager.ProcessEvent(EventType.TongueAim);
        }

        public void DeathTrigger() {
            GameModule.Instance.EventManager.ProcessEvent(EventType.TongueDeath);
        }
    }
}
