using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class RadiantStance : MonoBehaviour
    {
        public GameObject radiantEffect;
        private bool m_RadiantStatus;
    void Start()
        {
            m_RadiantStatus = false;
        }

        void Update()
        {
            bool isRadiantKeyDown = Input.GetKeyDown(KeyCode.Alpha6);
            if (isRadiantKeyDown)
            {
                if (m_RadiantStatus== false)
                {
                    radiantEffect.SetActive(true);
                    m_RadiantStatus = true;
                    if(GetComponent<MeleeWeapon>()!=null)
                        GetComponent<MeleeWeapon>().additionaDamage = 10;
                }
                else
                {
                    radiantEffect.SetActive(false);
                    m_RadiantStatus = false;
                    if (GetComponent<MeleeWeapon>() != null)
                        GetComponent<MeleeWeapon>().additionaDamage = 0;
                }
            }
        }
    }
}

