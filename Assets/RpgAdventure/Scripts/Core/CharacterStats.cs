using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class CharacterStats : MonoBehaviour
    {
        [Range(0, 360.0f)]
        public float hitAngle = 360.0f;
        [Range(0, 150.0f)]
        public float blockAngle = 150.0f;
        public float invulnerabilityTime = 0.3f;
        public int maxHitPoints;
        public int experience;
        public int power;
        public int currentHitPoints;
    }
}

