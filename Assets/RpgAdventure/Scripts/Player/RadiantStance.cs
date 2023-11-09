using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    //class for manage the ability of radiant state in which
    //other class use SkinnedMeshRender to generate radiant effect
    public class RadiantStance : MonoBehaviour
    {
        public GameObject radiantEffect;
        private PlayerController m_PlayerContr;
        [SerializeField]
        private float m_Duration;
        private float m_Counter;
        private bool m_RadiantStatus;
        private int m_RadiantDamage;
    void Start()
        {
            m_RadiantStatus = false;
            m_PlayerContr = GameObject.Find("Player").GetComponent<PlayerController>();
            m_Counter = 0;
            m_RadiantDamage = -3;
        }

        void Update()
        {
            if (m_RadiantStatus == true)
            {
                m_Counter += Time.deltaTime;
            }
            if( m_Counter > 1)
            {
                m_PlayerContr.UseHealthPotion(m_RadiantDamage);
                m_Counter -= 1;
            }
        }
        public void UseRadiant(int damageNumber)
        {
            if(PauseControl.gameIsPaused)
            {
                return;
            }
            else
            {
                m_RadiantDamage = damageNumber;
                StartCoroutine(RadiantEffect());
            }
        }

        private IEnumerator RadiantEffect()
        {
            StartRadiant();
            yield return new WaitForSeconds(m_Duration);
            EndRadiant();
        }
        private void StartRadiant()
        {
            m_Counter = 0;
            radiantEffect.SetActive(true);
            m_RadiantStatus = true;
            if (GetComponent<MeleeWeapon>() != null)
                GetComponent<MeleeWeapon>().additionaDamage = 10;
        }

        private void EndRadiant()
        {
            m_Counter = 0;
            radiantEffect.SetActive(false);
            m_RadiantStatus = false;
            if (GetComponent<MeleeWeapon>() != null)
                GetComponent<MeleeWeapon>().additionaDamage = 0;
        }
    }
}

