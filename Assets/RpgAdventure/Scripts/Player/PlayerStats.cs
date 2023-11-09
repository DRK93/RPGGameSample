using System.Collections;
using UnityEngine;
using System;
using NGS.ExtendableSaveSystem;

namespace RpgAdventure
{
    // stats of Player character
    public class PlayerStats : CharacterStats, IMessageReceiver, ISavableComponent
    {
        public int maxLevel;
        public int[] availableLevels;
        public int currentLevel;
        public int currentExp;
        public int skillPoints;
        public int usedSkillPoints;
        public int spellDamage;
        public int spellSpeed;
        public int defeatedEnemies;
        public int questCompleted;
        public bool leveledUp = false;
        public string playerName = "Player";
        public int ExpToNextLevel
        {
            get
            { 
                return availableLevels[currentLevel+1]; 
            }
        }
        private HudManager m_HudMan;

        private void Awake()
        {
            availableLevels = new int[maxLevel];
            ComputeLevels(maxLevel);
            currentHitPoints = maxHitPoints;
            m_HudMan = FindObjectOfType<HudManager>();
        }
        private void Start()
        {
            if (FindObjectOfType<DataManager>())
            if (FindObjectOfType<DataManager>().loadingNumber == 0)
            {
                playerName = FindObjectOfType<DataManager>().playerName;
            }
        }
        private void ComputeLevels(int levelCount)
        {
            for (int i=1; i<levelCount; i++)
            {
                var level = i;
                var levelPow = Mathf.Pow(level, 2);
                var expToLevel = Convert.ToInt32(levelPow * levelCount);

                availableLevels[i] = expToLevel;
            }
        }
        public void GainExp(int exp)
        {
            Debug.Log("Gained experience " + exp);
            currentExp += exp;
            CheckIfLevelUp();
        }

        public void CheckIfLevelUp()
        {
            var currentLevelBefore = currentLevel;
            for (int i = currentLevel; i < availableLevels.Length; i++)
            {
                if (currentExp > availableLevels[i])
                {
                    currentLevel = i;
                }
            }
            if (currentLevel > currentLevelBefore)
            {
                skillPoints += (currentLevel - currentLevelBefore) * 4;
                Debug.Log("leveled up");
                leveledUp = true;
            }
        }
        public void OnReceiveMessage(MessageType type, object sender, object message)
        {
            if (type == MessageType.DEAD)
            {
                GainExp(((Damageable)sender).GetComponent<CharacterStats>().experience);
                defeatedEnemies++;
            }
        }
        public new ComponentData Serialize()
        {
            Debug.Log("Player serialize");
            ExtendedComponentData data = new ExtendedComponentData();
            data.SetTransform("transform", transform);

            data.SetString("playername", playerName);
            data.SetInt("maxhp", maxHitPoints);
            data.SetInt("exp", experience);
            data.SetInt("powr", power);
            data.SetInt("currenthp", currentHitPoints);
            data.SetInt("curlvl", currentLevel);
            data.SetInt("curexp", currentExp);
            data.SetInt("skPts", skillPoints);
            data.SetInt("usedSkPts", usedSkillPoints);
            data.SetInt("spldmg", spellDamage);
            data.SetInt("splspd", spellSpeed);
            data.SetInt("defenemy", defeatedEnemies);
            data.SetInt("qcmpl", questCompleted);

            return data;
        }
        public new void Deserialize(ComponentData data)
        {
            Debug.Log("Player deserialize");
            ExtendedComponentData unpacked = (ExtendedComponentData)data;
            unpacked.GetTransform("transform", transform);

            playerName = unpacked.GetString("playername");
            maxHitPoints = unpacked.GetInt("maxhp");
            experience = unpacked.GetInt("exp");
            power = unpacked.GetInt("powr");
            currentHitPoints = unpacked.GetInt("currenthp");
            currentLevel = unpacked.GetInt("curlvl");
            currentExp = unpacked.GetInt("curexp");
            skillPoints = unpacked.GetInt("skPts");
            usedSkillPoints = unpacked.GetInt("usedSkPts");
            spellDamage = unpacked.GetInt("spldmg");
            spellSpeed = unpacked.GetInt("splspd");
            defeatedEnemies = unpacked.GetInt("defenemy");
            questCompleted = unpacked.GetInt("qcmpl");
            
            m_HudMan.SetMaxHealth(maxHitPoints,currentHitPoints);
            m_HudMan.SetHealth(currentHitPoints);

        }
    }

}