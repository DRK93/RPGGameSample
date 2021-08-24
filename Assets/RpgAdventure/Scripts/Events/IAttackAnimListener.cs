using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgAdventure
{
    interface IAttackAnimListener
    {
        public void MeleeAttackStart();
        public void MeleeAttackEnd();
    }
}
