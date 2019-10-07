using System.Collections;
using Nothing;
using UnityEngine;
using EventType = Module.Game.Events.EventType;

namespace Module.Game.Boss
{
    //public enum BossState {
    //    Posing,
    //    PreLeftAttack,
    //    PreRightAttack,
    //    LeftAttack,
    //    RightAttack,
    //    Teeth
    //}

    public class BossController : Enemy
    {
        public override void OnEnemyDestroyed()
        {
            GameModule.Instance.LevelManager.EnemiesKilledCounter++;
            GameModule.Instance.EventManager.ProcessEvent(EventType.EnemyDeath);
            view.Death();
            GameModule.Instance.Player.IsActive = false;
            view.OnDeath += () =>
            {
                GameModule.Instance.Player.transform.GetComponent<Rigidbody>().velocity = Vector3.forward * 10f;
                GameModule.Instance.UiManager.EndGame();
            };
            Destroy(gameObject, 10f);
        }

        public float attackDuration = 2f;
        public float durationBetweenAttacksMin = 5f;
        public float durationBetweenAttacksMax = 10f;

        public float teethAttackDuration = 6f;

        public BossSideAttack sideAttack;

        public BossAnimationView view;

        private void Start()
        {
            StartCoroutine(AttackSequence());
        }
        
        private void OnEnable()
        {
            view.OnAttackLeftDamage += AttackLeft;
            view.OnAttackRightDamage += AttackRight;
            Health.OnHealthDepleated += BossDead;
            Health.OnDamageTaken += TookDamage;
        }
        
        
        private void AttackLeft()
        {
            sideAttack.Attack(BossSideAttackVariant.LeftAndMiddle);
        }
        
        private void AttackRight()
        {
            sideAttack.Attack(BossSideAttackVariant.RightAndMiddle);
        }

        private IEnumerator AttackSequence() {
            while (Health.CurrentHealth > 0)
            {
                int rndAttack = Random.Range(0, 3);
                switch (rndAttack)
                {
                    case 0:
                        yield return LeftAttack();
                        break;
                    case 1:
                        yield return RightAttack();
                        break;
//                    TODO: Add teeth attack
//                    case 2:
//                        yield return TeethAttack();
//                        break;
                }

                yield return new WaitForSeconds(Random.Range(durationBetweenAttacksMin, durationBetweenAttacksMax));
            }
        }

        private IEnumerator LeftAttack() {
            view.AttackLeft();

            yield return new WaitForSeconds(attackDuration);
        }
        private IEnumerator RightAttack() {
            view.AttackRight();

            yield return new WaitForSeconds(attackDuration);
        }
        
        private IEnumerator TeethAttack() {
            yield return new WaitForSeconds(teethAttackDuration);
        }

        private void BossDead()
        {
            StopAllCoroutines();
            view.Death();
        }

        private void TookDamage(int dmg)
        {
        }
    }
}