using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RpgAdventure
{
    public class HealthPotionDrink : MonoBehaviour
    {
        public GameObject m_HealParticle;
        public Slider m_HealthPotionSlider;

        private Button UsePotionBtn;
        private PlayerController m_PlayerInst;
        
        private float m_CooldownTime = 10.0f;
        private float m_TimeCounter;
        private bool m_potionIsReady = false;
        private bool m_StartHealing = false;
        public bool IsStartHealing
        { get { return m_StartHealing; } set { m_StartHealing = value; } }

        
        public bool IsPotionReady => m_potionIsReady;

        void Start()
        {
            UsePotionBtn = GameObject.Find("PotionDrinkBtn").GetComponent<Button>();
            UsePotionBtn.onClick.AddListener(UsePotion);
            UsePotionBtn.interactable = false;
            m_HealthPotionSlider.value = 0;
            m_PlayerInst = GameObject.Find("Player").GetComponent<PlayerController>();
            //m_HealParticle = GameObject.Find("HealParticleEffect");
        }

        // Update is called once per frame
        void Update()
        {
            if (m_HealthPotionSlider.value == 1 && m_potionIsReady == false)
            {
                m_potionIsReady = true;
                UsePotionBtn.interactable = true;
            }
            if (m_HealthPotionSlider.value <= 1)
            {
                m_TimeCounter += Time.deltaTime;
                SetCooldown(m_TimeCounter);
            }
            if (m_StartHealing == true)
            {
                m_StartHealing = false;
                UsePotion();
                Debug.Log("Start healing from keyboard");
            }
        }

        private void SetCooldown( float TimeCounter )
        {
            m_HealthPotionSlider.value = TimeCounter / m_CooldownTime;
        }

        private void UsePotion()
        {
            Debug.Log("UsingPotion");
            m_HealthPotionSlider.value = 0;
            m_TimeCounter = 0;
            m_potionIsReady = false;
            UsePotionBtn.interactable = false;
            m_PlayerInst.UseHealthPotion();
            StartCoroutine(HealEffect());
        }

        private IEnumerator HealEffect()
        {
            m_HealParticle.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            m_HealParticle.SetActive(false);
        }
    }


}
