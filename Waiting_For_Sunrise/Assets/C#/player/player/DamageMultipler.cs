using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.C_.player.player
{
    class DamageMultipler
    {
        public int damageMultipler { get;private set; }
         public DamageMultipler()
        {
            Init();
        }
        public void Init()
        {
            damageMultipler = 100;
        }
    }
}
