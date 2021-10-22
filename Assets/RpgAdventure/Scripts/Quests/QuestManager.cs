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
        public List<QuestGiver> QuestGivers;

        private PauseControl m_PauseControl;

        private void Awake()
        {

            QuestJournalUI.SetActive(false);
            QuestJournalLeaveBtn.onClick.AddListener(LeaveQuestJournal);
        }
        private void Start()
        {
            m_PauseControl = GameObject.Find("GameMenuManager").GetComponent<PauseControl>();
            LoadQuestFromDB();
            AssignQuest();
        }

        private void LeaveQuestJournal()
        {
            m_PauseControl.StartGame();
            QuestJournalUI.SetActive(false);
        }

        private void LoadQuestFromDB()
        {
            string pathToQuests = Application.streamingAssetsPath;
            string pathToQuests2 = Path.Combine(pathToQuests, "QuestDB.json");
            //using StreamReader reader = new StreamReader(pathToQuests +  "/QuestDB.json");
            using StreamReader reader = new StreamReader(pathToQuests2);
            //using StreamReader reader = new StreamReader(Application.dataPath + "/StreamingAssets/QuestDB.json");
            //using StreamReader reader = new StreamReader("Assets/RPGAdventure/DB/QuestDB.json");
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
                    QuestGivers.Add(questGiver);
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