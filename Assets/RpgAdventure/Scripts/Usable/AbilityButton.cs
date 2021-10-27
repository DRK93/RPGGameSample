using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RpgAdventure
{
    //class to manage behaviour of ability button and send information to other class which manage the whole abilities mechanic
    public class AbilityButton : MonoBehaviour
    {
        public int abilityNumber;

        [SerializeField]
        private GameObject m_abilityPanel;
        [SerializeField]
        private Button m_abilityButton;
        [SerializeField]
        private Slider buttonSlider;
        private UsableAbilities m_UsableThings;
        [SerializeField]
        private int m_abilityLevel;
        [SerializeField]
        private float m_CooldownTime;
        private float m_Counter;
        private DialogManager m_DialogManger;
        private bool m_IsAbilityReady;
        private bool m_IsAbilityStarted;
        public bool IsAbilityReady => m_IsAbilityReady;
        public bool IsAbilityStarted
        {
            get { return m_IsAbilityStarted; }
            set { m_IsAbilityStarted = value; }
        }
        private void Start()
        {
            m_UsableThings = GetComponent<UsableAbilities>();
            m_abilityButton.onClick.AddListener(UseAbility);
            m_UsableThings.abilityButtons.Add(this);
            m_IsAbilityReady = false;
            m_abilityButton.interactable = false;
            m_DialogManger = GameObject.Find("DialogManager").GetComponent<DialogManager>();
        }

        private void Update()
        {
            if (buttonSlider.value == 1 && m_IsAbilityReady == false)
            {
                m_IsAbilityReady = true;
                m_abilityButton.interactable = true;
            }
            if(buttonSlider.value <1)
            {
                m_Counter += Time.deltaTime;
                SetCooldown(m_Counter);
            }
        }

        private void SetCooldown(float TimeCounter)
        {
            buttonSlider.value = TimeCounter / m_CooldownTime;
        }

        public void UseAbility()
        {
            if (m_DialogManger.HasActiveDialog == false)
            {
                if (m_UsableThings.CanBeUsed == true)
                {
                    if (m_IsAbilityReady == true)
                    {
                        UseAbilityUI();
                        m_UsableThings.AbilityToUse(abilityNumber);
                    }
                }
            }
        }
        
        private void UseAbilityUI()
        {
            m_Counter = 0;
            buttonSlider.value = 0;
            m_abilityButton.interactable = false;
            m_IsAbilityReady = false;
        }
    }

}


