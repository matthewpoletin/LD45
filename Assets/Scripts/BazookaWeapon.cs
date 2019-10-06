using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nothing {
    public class BazookaWeapon : Weapon {
        public override Task Attack() => throw new NotImplementedException();
    }
}
