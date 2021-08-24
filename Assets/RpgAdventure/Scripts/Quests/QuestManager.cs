using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace RpgAdventure
{
    public class JsonHelper
    {
        private class Wrapper<T>
        {
            public T[] array;
        }
        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
    }
    public class QuestManager : MonoBehaviour
    {
        public Quest[] quests;
        public GameObject QuestJournalUI;
        public Button QuestJournalLeaveBtn;
        

        private void Awake()
        {
            LoadQuestFromDB();
            AssignQuest();
            QuestJournalUI.SetActive(false);
            QuestJournalLeaveBtn.onClick.AddListener(LeaveQuestJournal);
        }

        private void LeaveQuestJournal()
        {
            QuestJournalUI.SetActive(false);
        }

        private void LoadQuestFromDB()
        {
            using StreamReader reader = new StreamReader("Assets/RPGAdventure/DB/QuestDB.json");
            string json = reader.ReadToEnd();
            var loadedQuests = JsonHelper.GetJsonArray<Quest>(json);
            quests = new Quest[loadedQuests.Length];
            quests = loadedQuests;
        }

        private void AssignQuest()
        {
            QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
            if (questGivers != null && questGivers.Length >0)
            {
                foreach (QuestGiver questGiver in questGivers)
                {
                    AssignQuestTo(questGiver);
                }
            }
        }

        private void AssignQuestTo(QuestGiver questGiver)
        {
            foreach ( Quest quest in quests)
            {
                if(quest.questGiver == questGiver.GetComponent<UniqueId>().Uid)
                {
                    questGiver.quest = quest;
                }
            }
        }

    }
}