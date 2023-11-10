using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure.Scripts.DamageSystem
{
    public partial class Damageable : MonoBehaviour
    {
        public struct DamageMessage
        {
            public MonoBehaviour damager;
            public int amount;
            public GameObject damageSource;
        }
    }
}

