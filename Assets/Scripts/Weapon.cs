using System.Threading.Tasks;
using UnityEngine;

namespace Nothing {
    public abstract class Weapon : MonoBehaviour {
        public bool IsAttacking { get; protected set; } = false;

        public abstract void Attack();

        public void SetWeaponEnabled(bool enable)
        {
            gameObject.SetActive(enable);
            IsAttacking = false;
        }
    }
}