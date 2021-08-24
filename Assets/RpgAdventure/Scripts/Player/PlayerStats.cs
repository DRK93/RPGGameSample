using System.Collections;
using UnityEngine;
using System;

namespace RpgAdventure
{
    public class PlayerStats : MonoBehaviour, IMessageReceiver
    {
        public int maxLevel;
        public int[] availableLevels;
        public int currentLevel;
        public int currentExp;
        public int skillPoints;
        public int spellDamage;
        public float spellSpeed;
        public int power;
        public int maxHealth;
        public int expToNextLevel
        {
            get
            { 
                return availableLevels[currentLevel]; 
            }
        }

        private void Awake()
        {
            SendPlayerStats();
            availableLevels = new int[maxLevel +1];
            ComputeLevels(maxLevel);
        }
        private void ComputeLevels(int levelCount)
        {
            for (int i=0; i<levelCount+1; i++)
            {
                var level = i + 1;
                var levelPow = Mathf.Pow(level, 2);
                var expToLevel = Convert.ToInt32(levelPow * levelCount);

                availableLevels[i] = expToLevel;
            }
        }
        private void SendPlayerStats()
        {
            GetComponent<SpellSpawner>().SpellDamage = spellDamage;
            GetComponent<SpellSpawner>().SpellSpeed = spellSpeed;
            //GetComponent<PlayerController>().    = power;
            //GetComponent<PlayerController>().  = maxHealth;
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
            }
        }
        public void OnReceiveMessage(MessageType type, object sender, object message)
        {
            if (type == MessageType.DEAD)
            {
                GainExp(((Damageable)sender).experience);
            }

        }
    }
}