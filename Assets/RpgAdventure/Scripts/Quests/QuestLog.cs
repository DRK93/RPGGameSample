using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NGS.ExtendableSaveSystem;
using RpgAdventure.Scripts.Dialog;
using TMPro;

namespace RpgAdventure.Scripts.Quests
{
    //class to manage quests assinged to Player
    //manage quest data for save and load game system 
    //add quests to quest journal from quest panel prefab
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
    public class QuestLog : MonoBehaviour, ISavableComponent
    {
        public GameObject questTargetPanelPrefab;
        public GameObject questPanelPrefab;
        public GameObject QuestScrollPanel;
        public GameObject QuestManager;
        public List<AcceptedQuest> quests = new List<AcceptedQuest>();
        private List<string> m_questUID = new List<string>();

        private float m_QuestPanelTopPosition = 0.0f;
        private int m_NumberOfQuests = 0;
        private int m_DistanceCounter = 0;

        const float c_DistanceBetweenQuestPanels = 330.0f;

        [SerializeField] private int m_uniqueID;
        [SerializeField] private int m_executionOrder;
        public int uniqueID => m_uniqueID;
        public int executionOrder => m_executionOrder;


        public void AddQuest(Quest quest)
        {
            AcceptedQuest aQuest = new AcceptedQuest(quest);
            quests.Add(aQuest);
            QuestManager.GetComponent<QuestUpdate>().QuestCreateInJournal(aQuest, AddQuestPanelInJournal(aQuest));
            m_NumberOfQuests++;
        }

        public void ClearQuestLog()
        {
            Debug.Log("Removing quests from quest log");
            for (int i = QuestScrollPanel.transform.childCount-1; i>=0; i--)
            {
                Destroy(QuestScrollPanel.transform.GetChild(i).gameObject);
            }
            m_DistanceCounter = 0;
        }

        public GameObject AddQuestPanelInJournal(AcceptedQuest quest)
        {
            GameObject questPanelInstance = Instantiate(questPanelPrefab, QuestScrollPanel.transform);
            RectTransform rt = questPanelInstance.GetComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, m_QuestPanelTopPosition + m_DistanceCounter * c_DistanceBetweenQuestPanels, rt.rect.height);
            m_DistanceCounter++;
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
        public ComponentData Serialize()
        {
            ExtendedComponentData data = new ExtendedComponentData();
            var indexer = 0;
            data.SetInt("questnumber", m_NumberOfQuests);
            foreach (var quest in quests)
            {
                data.SetString("questid" + indexer, quest.uid);
                data.SetInt("questkilled" + indexer, quest.questTargetKilled);
                data.SetString("queststatus" + indexer, quest.questStatus.ToString());

                foreach (var panel in QuestManager.GetComponent<QuestUpdate>().questPanels)
                {
                    if (quest.title == panel.GetComponent<QuestPanParameters>().m_QuestTitle)
                    {
                        data.SetInt("questcurrentkilled" + indexer, panel.GetComponent<QuestPanParameters>().m_CurrentDeafeted);

                        var indexer2 = 0;
                        foreach (var enemyOnList in panel.GetComponent<QuestPanParameters>().m_TargetBySpeciesDefeat)
                        {
                            data.SetInt("enembyspecie" + indexer + indexer2, enemyOnList);
                            indexer2++;
                        }
                    }
                }
                indexer++;
            }
            return data;
        }
        public void Deserialize(ComponentData data)
        {
            ClearQuestLog();
            ExtendedComponentData unpacked = (ExtendedComponentData)data;
            quests = new List<AcceptedQuest>();
            QuestManager.GetComponent<QuestUpdate>().questPanels = new List<GameObject>();
            m_questUID = new List<string>();
            m_NumberOfQuests = unpacked.GetInt("questnumber");

            // getting questUID
            for (int i=0; i<m_NumberOfQuests; i++)
            {
                m_questUID.Add(unpacked.GetString("questid" + i));
            }
            // getting new fresh quest as it assigned from NPC
            foreach (var questLoaded in m_questUID)
            {
                GameObject.Find("DialogManager").GetComponent<DialogManager>().RemoveQuestOption(questLoaded);
            }
            // updating quest from saved data
            var indexer = 0;
            foreach (var q in quests)
            {
                q.questTargetKilled = unpacked.GetInt("questkilled" + indexer);
                if (unpacked.GetString("queststatus" + indexer) == "ACTIVE")
                    q.questStatus = QuestStatus.ACTIVE;
                if (unpacked.GetString("queststatus" + indexer) == "COMPLETED")
                    q.questStatus = QuestStatus.COMPLETED;
                if (unpacked.GetString("queststatus" + indexer) == "FAILED")
                    q.questStatus = QuestStatus.FAILED;

                foreach (var panel in QuestManager.GetComponent<QuestUpdate>().questPanels)
                {
                    
                    if (q.title == panel.GetComponent<QuestPanParameters>().m_QuestTitle)
                    {

                        panel.GetComponent<QuestPanParameters>().m_QuestStatus = q.questStatus.ToString();
                        panel.GetComponent<QuestPanParameters>().m_CurrentDeafeted = unpacked.GetInt("questcurrentkilled" + indexer);
                        panel.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = panel.GetComponent<QuestPanParameters>().m_QuestStatus;
                        panel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = panel.transform.GetComponent<QuestPanParameters>().m_CurrentDeafeted.ToString();

                        var indexer2 = 0;
                        for (int i=0; i< panel.GetComponent<QuestPanParameters>().m_TargetBySpeciesDefeat.Count; i++)
                        {
                            panel.GetComponent<QuestPanParameters>().m_TargetBySpeciesDefeat[i] = unpacked.GetInt("enembyspecie" + indexer + indexer2);
                            panel.transform.GetChild(10 + i).gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = panel.transform.GetComponent<QuestPanParameters>().m_TargetBySpeciesDefeat[i].ToString();
                            indexer2++;
                        }
                    }
                }
                indexer++;
            }
        }
    }
}