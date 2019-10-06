using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Game;
using UnityEngine;
using EventType = Module.Game.Events.EventType;

namespace Nothing {
    public class MeleeWeapon : Weapon {
        public float useTime = .5f;
        
        public Collider weaponCollider;

        private void Awake() {
            weaponCollider.enabled = false;
        }

        public override async Task Attack() {
            if (IsAttacking)
                return;

            IsAttacking = true;

            weaponCollider.enabled = true;
            GameModule.Instance.EventManager.ProcessEvent(EventType.PlayerAttack, transform, transform.position);
            await Task.Delay((int)(useTime * 1000f));
            weaponCollider.enabled = false;

            IsAttacking = false;
        }

        private void OnTriggerEnter(Collider other) {
            var enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy is null)
                return;

            enemy.GetComponent<Health>().Kill();
        }
    }
}
