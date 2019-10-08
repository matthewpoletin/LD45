using System.Threading.Tasks;
using UnityEngine;

namespace Nothing {
    public abstract class Weapon : MonoBehaviour {
        [field: SerializeField, HideInInspector]
        public bool IsAttacking { get; protected set; } = false;

        public abstract void Attack();
    }
}