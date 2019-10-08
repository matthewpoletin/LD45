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
    public class RangedWeapon : Weapon {
        public float useTime = .5f;
        public int damage = 1;
    
        public Projectile projectilePrefab;
        
        public override void Attack() {
            if (IsAttacking)
                return;

            IsAttacking = true;
            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine() {
            var projectile = Instantiate(projectilePrefab, ProjectileHolder.Inst.transform);
            projectile.transform.position = transform.position;
            GameModule.Instance.EventManager.ProcessEvent(EventType.PlayerAttack, transform, transform.position);

            yield return new WaitForSeconds(useTime);

            IsAttacking = false;
        }
    }
}
