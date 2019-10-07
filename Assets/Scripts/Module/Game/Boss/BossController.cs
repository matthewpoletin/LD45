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
            view.OnDeath += () =>
            {
                GameModule.Instance.Player.transform.GetComponent<Rigidbody>().velocity = Vector3.forward * 10f;
                GameModule.Instance.UiManager.EndGame();
            };
            Destroy(gameObject, 10f);
        }

        public float posing1Duration = 5;
        public float posing2Duration = 3;

        public float durationBetweenAttacks = .5f;

        public float teethAttackDuration = 6f;

        public BossSideAttack sideAttack;

        //public BossState State => state;
        //[SerializeField, HideInInspector]
        //private BossState state = BossState.Posing;

        public BossAnimationView view;

        private void Start()
        {
            StartCoroutine(AttackSequence());
        }

        private IEnumerator AttackSequence()
        {
            yield return new WaitForSeconds(posing1Duration);

            yield return LeftAttack();

            yield return new WaitForSeconds(posing2Duration);

            yield return RightAttack();

            while (Health.CurrentHealth > 0)
            {
                yield return TeethAttack();
                yield return LeftAttack();
                yield return RightAttack();

                yield return new WaitForSeconds(durationBetweenAttacks);
            }
        }

        private IEnumerator LeftAttack()
        {
            view.AttackLeft();

            yield return sideAttack.Attack(BossSideAttackVariant.LeftAndMiddle);
        }

        private IEnumerator RightAttack()
        {
            view.AttackRight();

            yield return sideAttack.Attack(BossSideAttackVariant.RightAndMiddle);
        }

        private IEnumerator TeethAttack()
        {
            yield return new WaitForSeconds(teethAttackDuration);
        }
    }
}