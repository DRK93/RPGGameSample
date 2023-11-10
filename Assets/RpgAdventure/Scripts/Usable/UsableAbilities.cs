using System.Collections;
using System.Collections.Generic;
using RpgAdventure.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;


namespace RpgAdventure.Scripts.Usable
{
    // class manage the using abilities process
    // get information form Player Input and send it further to Player Controller
    public class UsableAbilities : MonoBehaviour
    {
        public List<AbilityButton> abilityButtons;
        [SerializeField]
        private GameObject m_HealEffect;
        private PlayerController m_PlayerInst;
        private RadiantStance m_Radiant;
        private int m_abilityNumber;
        private bool m_UseAbility;
        private bool m_CanBeUsed;
        [SerializeField]
        private float m_GlobalAbilityCd;
        const int c_RadiantDamage = -3;
        const int c_PotionPower = 100;
        public bool CanBeUsed
        {
            get { return m_CanBeUsed; }
            set { m_CanBeUsed = value; }
        }
        public bool UseAbility
        {
            get { return m_UseAbility; }
            set { m_UseAbility = value; }
        }
        public int AbilityNumber
        {
            get { return m_abilityNumber; }
            set { m_abilityNumber = value; }
        }
        private void Awake()
        {
            abilityButtons = new List<AbilityButton>();
        }
        private void Start()
        {
            
            m_PlayerInst = GameObject.Find("Player").GetComponent<PlayerController>();
            m_Radiant = GameObject.Find("Player").GetComponent<RadiantStance>();
            StartCoroutine(WaitToNextAbiility());
        }

        public void UsingAbilityKeyBoard(int abNumber)
        {
            AbilityFromKeyboard(abNumber);

        }
        public void AbilityToUse(int abNumber)
        {
            
            m_CanBeUsed = false;
            StartCoroutine(WaitToNextAbiility());

            switch (abNumber)
            {
                case 1:
                    m_PlayerInst.UseAbility(abNumber);
                    break;
                case 2:
                    m_PlayerInst.UseAbility(abNumber);
                    break;
                case 3:
                    m_PlayerInst.UseAbility(abNumber);
                    break;
                case 4:
                    m_PlayerInst.UseAbility(abNumber);
                    break;
                case 5:
                    m_Radiant.UseRadiant(c_RadiantDamage);
                    break;
                case 6:
                    StartCoroutine(HealEffect());
                    m_PlayerInst.UseHealthPotion(c_PotionPower);
                    break;
                default:
                    break;
            }
        }

        private void AbilityFromKeyboard(int abNumber)
        {
            foreach (var abilityButton in abilityButtons)
            {
                if (abilityButton.abilityNumber == abNumber)
                    abilityButton.UseAbility();
            }
        }
        private IEnumerator WaitToNextAbiility()
        {
            yield return new WaitForSeconds(m_GlobalAbilityCd);
            m_CanBeUsed = true;
        }
        private IEnumerator HealEffect()
        {
            m_HealEffect.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            m_HealEffect.SetActive(false);
        }
    }
}

