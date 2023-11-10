using System.Collections;
using System.Collections.Generic;
using RpgAdventure.Scripts.Helpers;
using RpgAdventure.Scripts.Hud;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RpgAdventure.Scripts.Player
{
    // this class manage the player stats card which display player stats in game on UI elements
    // manage leveling up process - spend points to upgrade player stats

    public class PlayerStatsManager : MonoBehaviour
    {
        public Button addHealthBtn;
        public Button addPowerBtn;
        public Button addSpellDmgBtn;
        public Button addSpellSpeedBtn;
        public Button confirmBtn;
        public Button undoBtn;
        public Button closeBtn;
        public GameObject CharacterCardUI;
        public GameObject CharacterStatsPanel;

        private PlayerStats m_PlStats;
        private PauseControl m_pauseControl;
        private HudManager m_HudMan;
        private string m_PlayerName;
        private int m_Level;
        private int m_Exp;
        private int m_ToNextLevel;
        private int m_Power;
        private int m_MaxHealth;
        private int m_SpellDamage;
        private int m_SpellSpeed;
        private int m_SkillPoints;
        private int m_UsedSkillPoints;
        private int m_CurSkillPoints;
        private int m_EnemyDefeat;
        private int m_QuestCompleted;
        private int m_CurrentAddedHealth;
        private int m_CurrentAddedPower;
        private int m_CurrentAddedSpellDmg;
        private int m_CurrentAddedSpellSpd;
        

        private void Start()
        {
            m_PlStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            m_HudMan = FindObjectOfType<HudManager>();
            UpdateStatsOnCard();
            m_CurSkillPoints = 0;
            m_CurrentAddedHealth = 0;
            m_CurrentAddedPower = 0;
            m_CurrentAddedSpellDmg = 0;
            m_CurrentAddedSpellSpd = 0;

            SetTextUpdate();

            confirmBtn.onClick.AddListener(ConfirmBtn);
            undoBtn.onClick.AddListener(UndoBtn);
            closeBtn.onClick.AddListener(CloseBtn);
            addHealthBtn.onClick.AddListener(AddHealthPointBtn);
            addPowerBtn.onClick.AddListener(AddPowerPointBtn);
            addSpellDmgBtn.onClick.AddListener(AddSpellDmgPointBtn);
            addSpellSpeedBtn.onClick.AddListener(AddSpellSpeedPointBtn);
            m_pauseControl = GameObject.Find("GameMenuManager").GetComponent<PauseControl>();
            HideAllButtons();
        }
        private void Update()
        {
            if (m_PlStats.leveledUp == true)
            {
                LevelUp();
                m_PlStats.leveledUp = false;
            }
        }

        public void CardStatsKey()
        {
            UpdateStatsOnCard();

            m_pauseControl.PauseGame();
            CharacterCardUI.SetActive(true);

            if (m_SkillPoints > 0)
            {
                ShowAddButtons();
            }
        }
        private void CloseBtn()
        {
            m_pauseControl.StartGame();
            GameObject.Find("Player").GetComponent<PlayerInput>().IsAnyCardOpen = false;
            CharacterCardUI.SetActive(false);
        }

        private void UpdateStatsOnCard()
        {
            m_PlayerName = m_PlStats.playerName;
            m_Level = m_PlStats.currentLevel;
            m_Exp = m_PlStats.currentExp;
            m_ToNextLevel = m_PlStats.ExpToNextLevel;
            m_MaxHealth = m_PlStats.maxHitPoints;
            m_Power = m_PlStats.power;
            m_SpellDamage = m_PlStats.spellDamage;
            m_SpellSpeed = m_PlStats.spellSpeed;
            m_SkillPoints = m_PlStats.skillPoints;
            m_UsedSkillPoints = m_PlStats.usedSkillPoints;
            m_EnemyDefeat = m_PlStats.defeatedEnemies;
            m_QuestCompleted = m_PlStats.questCompleted;
            SetTextUpdate();
        }
        private void LevelUp()
        {
            m_Level = m_PlStats.currentLevel;
            m_Exp = m_PlStats.currentExp;
            m_ToNextLevel = m_PlStats.ExpToNextLevel;
            m_SkillPoints = m_PlStats.skillPoints;
            SetTextUpdate();
            ShowAddButtons();
        }
        private void ConfirmBtn()
        {
            m_PlStats.usedSkillPoints += m_CurSkillPoints;
            m_CurSkillPoints = 0;
            m_CurrentAddedHealth = 0;
            m_CurrentAddedPower = 0;
            m_CurrentAddedSpellDmg = 0;
            m_CurrentAddedSpellSpd = 0;
            SetTextUpdate();
            SendStatsChange();
            confirmBtn.gameObject.SetActive(false);
            undoBtn.gameObject.SetActive(false);
        }
        private void UndoBtn()
        {
            m_MaxHealth += -m_CurrentAddedHealth;
            m_Power += -m_CurrentAddedPower;
            m_SpellDamage += -m_CurrentAddedSpellDmg;
            m_SpellSpeed += -m_CurrentAddedSpellSpd;
            m_SkillPoints += m_CurSkillPoints;
            m_UsedSkillPoints += -m_CurSkillPoints;
            m_CurSkillPoints = 0;
            m_CurrentAddedHealth = 0;
            m_CurrentAddedPower = 0;
            m_CurrentAddedSpellDmg = 0;
            m_CurrentAddedSpellSpd = 0;
            SetTextUpdate();
            ShowAddButtons();
            confirmBtn.gameObject.SetActive(false);
            undoBtn.gameObject.SetActive(false);
        }
        private void AddHealthPointBtn()
        {
            m_MaxHealth += 10;
            m_CurrentAddedHealth += 10;
            m_SkillPoints += -1;
            m_UsedSkillPoints += 1;
            m_CurSkillPoints += 1;
            SetTextUpdate();
            confirmBtn.gameObject.SetActive(true);
            undoBtn.gameObject.SetActive(true);
            if (m_SkillPoints == 0)
                HideAddButtons();
        }
        private void HideAddButtons()
        {
            addHealthBtn.gameObject.SetActive(false);
            addPowerBtn.gameObject.SetActive(false);
            addSpellDmgBtn.gameObject.SetActive(false);
            addSpellSpeedBtn.gameObject.SetActive(false);
        }
        private void AddPowerPointBtn()
        {
            m_Power += 2;
            m_CurrentAddedPower += 2;
            m_SkillPoints += -1;
            m_UsedSkillPoints += 1;
            m_CurSkillPoints += 1;
            SetTextUpdate();
            confirmBtn.gameObject.SetActive(true);
            undoBtn.gameObject.SetActive(true);
            if (m_SkillPoints == 0)
                HideAddButtons();
        }
        private void AddSpellDmgPointBtn()
        {
            m_SpellDamage += 2;
            m_CurrentAddedSpellDmg += 2;
            m_SkillPoints += -1;
            m_UsedSkillPoints += 1;
            m_CurSkillPoints += 1;
            SetTextUpdate();
            confirmBtn.gameObject.SetActive(true);
            undoBtn.gameObject.SetActive(true);
            if (m_SkillPoints == 0)
                HideAddButtons();
        }
        private void AddSpellSpeedPointBtn()
        {
            m_SpellSpeed += 1;
            m_CurrentAddedSpellSpd += 1;
            m_SkillPoints += -1;
            m_UsedSkillPoints += 1;
            m_CurSkillPoints += 1;
            SetTextUpdate();
            confirmBtn.gameObject.SetActive(true);
            undoBtn.gameObject.SetActive(true);
            if (m_SkillPoints == 0)
                HideAddButtons();
        }
        private void SendStatsChange()
        {
            m_PlStats.skillPoints = m_SkillPoints;
            m_PlStats.maxHitPoints = m_MaxHealth;
            m_PlStats.power = m_Power;
            m_PlStats.spellDamage = m_SpellDamage;
            m_PlStats.spellSpeed = m_SpellSpeed;
            m_HudMan.SetMaxHealth(m_PlStats.maxHitPoints, m_PlStats.currentHitPoints);
        }

        private void HideAllButtons()
        {
            confirmBtn.gameObject.SetActive(false);
            undoBtn.gameObject.SetActive(false);
            addHealthBtn.gameObject.SetActive(false);
            addPowerBtn.gameObject.SetActive(false);
            addSpellDmgBtn.gameObject.SetActive(false);
            addSpellSpeedBtn.gameObject.SetActive(false);
        }
        private void ShowAllButtons()
        {
            confirmBtn.gameObject.SetActive(true);
            undoBtn.gameObject.SetActive(true);
            addHealthBtn.gameObject.SetActive(true);
            addPowerBtn.gameObject.SetActive(true);
            addSpellDmgBtn.gameObject.SetActive(true);
            addSpellSpeedBtn.gameObject.SetActive(true);
        }

        private void ShowAddButtons()
        {
            addHealthBtn.gameObject.SetActive(true);
            addPowerBtn.gameObject.SetActive(true);
            addSpellDmgBtn.gameObject.SetActive(true);
            addSpellSpeedBtn.gameObject.SetActive(true);
        }
        private void SetTextUpdate()
        {
            CharacterStatsPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = m_PlayerName;
            CharacterStatsPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = m_Level.ToString();
            CharacterStatsPanel.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = m_Exp.ToString();
            CharacterStatsPanel.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = m_ToNextLevel.ToString();
            CharacterStatsPanel.transform.GetChild(12).GetComponent<TextMeshProUGUI>().text = m_EnemyDefeat.ToString();
            CharacterStatsPanel.transform.GetChild(14).GetComponent<TextMeshProUGUI>().text = m_QuestCompleted.ToString();
            CharacterStatsPanel.transform.GetChild(16).GetComponent<TextMeshProUGUI>().text = m_SkillPoints.ToString();
            CharacterStatsPanel.transform.GetChild(18).GetComponent<TextMeshProUGUI>().text = m_UsedSkillPoints.ToString();
            CharacterStatsPanel.transform.GetChild(20).GetComponent<TextMeshProUGUI>().text = m_CurSkillPoints.ToString();
            CharacterStatsPanel.transform.GetChild(22).GetComponent<TextMeshProUGUI>().text = m_MaxHealth.ToString();
            CharacterStatsPanel.transform.GetChild(24).GetComponent<TextMeshProUGUI>().text = m_Power.ToString();
            CharacterStatsPanel.transform.GetChild(26).GetComponent<TextMeshProUGUI>().text = m_SpellDamage.ToString();
            CharacterStatsPanel.transform.GetChild(28).GetComponent<TextMeshProUGUI>().text = m_SpellSpeed.ToString();
        }
    }
}

