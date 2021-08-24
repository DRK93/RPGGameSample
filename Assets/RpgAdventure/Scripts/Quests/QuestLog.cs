using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RpgAdventure
{
    public enum QuestStatus
    {
        ACTIVE,
        FAILED,
        COMPLETED
    }
    [System.Serializable]
    public class AcceptedQuest: Quest
    {
        public QuestStatus questStatus;
        public int questTargetKilled;
        public AcceptedQuest (Quest quest)
        {
            uid = quest.uid;
            title = quest.title;
            description = quest.description;
            experience = quest.experience;
            gold = quest.gold;
            amount = quest.amount;
            targets = quest.targets;
            talkTo = quest.talkTo;
            explore = quest.explore;
            questGiver = quest.questGiver;
            type = quest.type;
            questStatus = QuestStatus.ACTIVE;
            questTargetKilled = 0;
        }
    }
    public class QuestLog : MonoBehaviour
    {
        public GameObject questTargetPanelPrefab;
        public GameObject questPanelPrefab;
        public GameObject QuestScrollPanel;
        public GameObject QuestManager;
        private float m_QuestPanelTopPosition = 0.0f;
        private int m_NumberOfQuests = 0;
        const float c_DistanceBetweenQuestPanels = 330.0f;
        public List<AcceptedQuest> quests = new List<AcceptedQuest>();

        public void AddQuest(Quest quest)
        {
            AcceptedQuest aQuest = new AcceptedQuest(quest);
            quests.Add(aQuest);
            QuestManager.GetComponent<QuestUpdate>().QuestCreateInJournal(aQuest, AddQuestPanelInJournal(aQuest));
            m_NumberOfQuests++;
        }

        public GameObject AddQuestPanelInJournal(AcceptedQuest quest)
        {
            GameObject questPanelInstance = Instantiate(questPanelPrefab, QuestScrollPanel.transform);
            RectTransform rt = questPanelInstance.GetComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, m_QuestPanelTopPosition + m_NumberOfQuests * c_DistanceBetweenQuestPanels, rt.rect.height);
            
            switch (quest.targets.Length)
            {
                case 1:
                    {
                        Destroy(questPanelInstance.transform.GetChild(14).gameObject);
                        Destroy(questPanelInstance.transform.GetChild(13).gameObject);
                        Destroy(questPanelInstance.transform.GetChild(12).gameObject);
                        Destroy(questPanelInstance.transform.GetChild(11).gameObject);
                        break;
                    }
                case 2:
                    {
                        Destroy(questPanelInstance.transform.GetChild(14).gameObject);
                        Destroy(questPanelInstance.transform.GetChild(13).gameObject);
                        Destroy(questPanelInstance.transform.GetChild(12).gameObject);
                        break;
                    }
                case 3:
                    {
                        Destroy(questPanelInstance.transform.GetChild(14).gameObject);
                        Destroy(questPanelInstance.transform.GetChild(13).gameObject);
                        break;
                    }
                case 4:
                    {
                        Destroy(questPanelInstance.transform.GetChild(14).gameObject);
                        break;
                    }
                default:
                    break;
            }
            return questPanelInstance;
        }
    }
}