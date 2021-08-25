using System.Collections;
using UnityEngine;
using System;

namespace RpgAdventure
{
    public class PlayerStats : CharacterStats, IMessageReceiver
    {
        public int maxLevel;
        public int[] availableLevels;
        public int currentLevel;
        public int currentExp;
        public int skillPoints;
        public int spellDamage;
        public int spellSpeed;
        public int defeatedEnemies;
        public int questCompleted;
        public bool leveledUp = false;
        public int ExpToNextLevel
        {
            get
            { 
                return availableLevels[currentLevel+1]; 
            }
        }

        private void Awake()
        {
            availableLevels = new int[maxLevel];
            ComputeLevels(maxLevel);
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
    }
}