using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public partial class Damageable : MonoBehaviour
    {
        public struct DamageMessage
        {
            public MonoBehaviour damager;
            public int amount;
            public GameObject damageSource;
            public int tool;
            // tool = 1 --- it will be from melee weapon
            // tool = 2 --- it will be from spell
        }
    }
}

