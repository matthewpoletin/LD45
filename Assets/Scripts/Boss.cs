using Module.Game.BossView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nothing {
    //public enum BossState {
    //    Posing,
    //    PreLeftAttack,
    //    PreRightAttack,
    //    LeftAttack,
    //    RightAttack,
    //    Teeth
    //}
    
    [RequireComponent(typeof(Health))]
    public class Boss : MonoBehaviour {

        public float posing1Duration = 5;
        public float posing2Duration = 3;

        public float durationBetweenAttacks = .5f;

        public float teethAttackDuration = 6f;

        public BossSideAttack sideAttack;

        //public BossState State => state;
        //[SerializeField, HideInInspector]
        //private BossState state = BossState.Posing;

        public BossAnimationView view;

        [SerializeField, HideInInspector]
        private Health health;

        private void Start() {
            StartCoroutine(AttackSequence());
        }

        private IEnumerator AttackSequence() {
            health = GetComponent<Health>();

            yield return new WaitForSeconds(posing1Duration);

            yield return LeftAttack();

            yield return new WaitForSeconds(posing2Duration);

            yield return RightAttack();

            while (health.CurrentHealth > 0) {
                yield return TeethAttack();
                yield return LeftAttack();
                yield return RightAttack();

                yield return new WaitForSeconds(durationBetweenAttacks);
            }
        }
        

        private IEnumerator LeftAttack() {
            view.AttackLeft();

            yield return sideAttack.Attack(BossSideAttackVariant.LeftAndMiddle);
        }
        private IEnumerator RightAttack() {
            view.AttackRight();

            yield return sideAttack.Attack(BossSideAttackVariant.RightAndMiddle);
        }

        private void Update() {
            Debug.Log(health.CurrentHealth);
        }

        private IEnumerator TeethAttack() {
            yield return new WaitForSeconds(teethAttackDuration);
        }
    }
}
