using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace RpgAdventure.Scripts.Hud
{
    public class HudManager : MonoBehaviour
    {
        public Slider healthSlider;
        public void SetMaxHealth ( int health, int currentHealth)
        {
            healthSlider.maxValue = health;
            SetHealth(currentHealth);
        }
        public void SetHealth(int health)
        {
            healthSlider.value = health;
        }
    }
}