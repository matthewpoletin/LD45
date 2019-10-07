using System;
using UnityEngine;
using EventType = Module.Game.Events.EventType;

namespace Module.Game.Boss
{
    public class BossAnimationView : MonoBehaviour
    {
        [SerializeField] private Animator bossAnimator;
        public Action OnPreAttackLeft = delegate { };
        public Action OnPreAttackRight = delegate { };
        public Action OnAttackLeft = delegate { };
        public Action OnAttackRight = delegate { };
        public Action OnAttackLeftDamage = delegate { };
        public Action OnAttackRightDamage = delegate { };
        public Action OnDeath = delegate { };

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

        public void Death()
        {
            bossAnimator.SetTrigger("Death");
        }

        public void AttackLeftTrigger()
        {
            OnAttackLeft();
            GameModule.Instance.EventManager.ProcessEvent(EventType.TongueAttack);
        }

        public void AttackRightTrigger()
        {
            OnAttackRight();
            GameModule.Instance.EventManager.ProcessEvent(EventType.TongueAttack);
        }

        public void PreAttackLeftTrigger()
        {
            OnPreAttackLeft();
            GameModule.Instance.EventManager.ProcessEvent(EventType.TongueAim);
        }

        public void PreAttackRightTrigger()
        {
            OnPreAttackRight();
            GameModule.Instance.EventManager.ProcessEvent(EventType.TongueAim);
        }

        public void DeathTrigger()
        {
            OnDeath();
            GameModule.Instance.EventManager.ProcessEvent(EventType.TongueDeath);
        }

        public void OnAttackLeftDamageTrigger()
        {
            OnAttackLeftDamage();
        }

        public void OnAttackRightDamageTrigger()
        {
            OnAttackRightDamage();
        }
    }
}