using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RpgAdventure
{
    public class FireballFromBtn : MonoBehaviour
    {
        public Slider fireballSlider;
        private Button m_FireballBtn;
        private float m_CooldownTime = 2f;
        private float m_TimeCounter;
        private bool m_IsFireballReady;
        
        public bool IsFireballReady
        {
            get => m_IsFireballReady;
            set => m_IsFireballReady = value;
        }
        void Start()
        {
            m_FireballBtn = GameObject.Find("FireballThrowBtn").GetComponent<Button>();
            m_FireballBtn.onClick.AddListener(ThrowingFireball);
            m_FireballBtn.interactable = false;
            fireballSlider.value = 0f;
            m_IsFireballReady = true;
        }
        void Update()
        {
                if (fireballSlider.value < 1)
                {
                    m_TimeCounter += Time.deltaTime;
                    SetCooldown(m_TimeCounter);
                }
                if (fireballSlider.value == 1 && m_FireballBtn.interactable == false)
                {
                    m_IsFireballReady = true;
                    m_FireballBtn.interactable = true;
                }
        }

        private void SetCooldown ( float m_TimeCounter)
        {
            fireballSlider.value = m_TimeCounter / m_CooldownTime;
        }
        public void FireballFromKeyBoard()
        {
            ThrowFireball();
        }
        private void ThrowFireball()
        {
            if (fireballSlider.value == 1)
            {
                fireballSlider.value = 0;
                m_IsFireballReady = false;
                m_TimeCounter = 0f;
                m_FireballBtn.interactable = false;
            }
        }
        private void ThrowingFireball()
        {
            GameObject.Find("Player").GetComponent<PlayerInput>().SpellFromBtn();
            ThrowFireball();
        }
    }
}

