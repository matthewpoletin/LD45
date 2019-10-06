using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nothing {
    public class ProjectileHolder : MonoBehaviour {
        public static ProjectileHolder Inst {
            get {
                if (inst == null)
                    inst = new GameObject(nameof(ProjectileHolder)).AddComponent<ProjectileHolder>();
                return inst;
            }
        }
        private static ProjectileHolder inst;

        private void Awake() {
            if (inst == this)
                Destroy(gameObject);

            inst = this;
        }
    }
}
