using Module.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nothing {

    public enum BossSideAttackVariant {
        LeftAndMiddle,
        RightAndMiddle
    }

    [RequireComponent(typeof(Collider))]
    public class BossSideAttack : MonoBehaviour {
        public int damage = 1;
        
        public Collider middleCollider;
        public Collider sideCollider;

        public float preAttackDuration = 1.453f;
        public float attackDuration = .756f;

        [SerializeField, HideInInspector]
        private bool isAttacking = false;

        public void Attack(BossSideAttackVariant variant)
        {
            StartCoroutine(_Attack(variant));
        }

        public IEnumerator _Attack(BossSideAttackVariant variant) {
            if (isAttacking)
                yield break;

            isAttacking = true;

            SetCollidersEnabled(true);

            var scale = variant == BossSideAttackVariant.LeftAndMiddle ? -1 : 1;
            transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);

            yield return new WaitForSeconds(attackDuration);

            SetCollidersEnabled(false);

            isAttacking = false;
        }

        private void SetCollidersEnabled(bool value) {
            middleCollider.enabled = value;
            sideCollider.enabled = value;
        }

        private void OnTriggerEnter(Collider other) {
            var player = other.gameObject.GetComponent<Player>();

            if (player == null)
                return;
            
            player.GetComponent<Health>().Damage(damage);
        }

    }
}
