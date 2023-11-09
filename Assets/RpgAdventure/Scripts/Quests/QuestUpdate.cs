using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RpgAdventure
{
    //class to manage quest journal UI
    //adding and updating quest in quest journal
    public class QuestUpdate : MonoBehaviour, IMessageReceiver
    {
        private EnemiesNameByUIS m_EnemyDictionary2;
        private string m_enemyName;
        private PlayerStats m_PlayerStats;
        public List<GameObject> questPanels;
        public List<QuestPanParameters> questPanParameters;

        void Awake()
        {
            m_EnemyDictionary2 = GameObject.Find("Enemies/EnemyList").GetComponent<EnemiesNameByUIS>();
            m_PlayerStats = FindObjectOfType<PlayerStats>();
            questPanels = new List<GameObject>();
            questPanParameters = new List<QuestPanParameters>();
        }
        public void QuestCreateInJournal(AcceptedQuest quest, GameObject questPan)
        {
            questPan.transform.GetComponent<QuestPanParameters>().m_QuestTitle = quest.title;
            questPan.transform.GetComponent<QuestPanParameters>().m_QuestDescription = quest.description;
            questPan.transform.GetComponent<QuestPanParameters>().m_CurrentDeafeted = quest.questTargetKilled;
            questPan.transform.GetComponent<QuestPanParameters>().m_TargetAmount = quest.amount;
            questPan.transform.GetComponent<QuestPanParameters>().m_QuestStatus = quest.questStatus.ToString();
            questPan.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = questPan.transform.GetComponent<QuestPanParameters>().m_QuestTitle;
            questPan.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = questPan.transform.GetComponent<QuestPanParameters>().m_QuestDescription;
            questPan.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = questPan.transform.GetComponent<QuestPanParameters>().m_CurrentDeafeted.ToString();
            questPan.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = questPan.transform.GetComponent<QuestPanParameters>().m_TargetAmount.ToString();
            questPan.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = questPan.transform.GetComponent<QuestPanParameters>().m_QuestStatus;
            QuestTargetsCreateInJournal(quest, questPan);
            QuestPanelAdd(questPan);
        }

        public void QuestTargetsCreateInJournal(AcceptedQuest quest, GameObject questPan)
        {
            var indexForChild = 0;
            foreach (var target in quest.targets)
            {
                questPan.transform.GetComponent<QuestPanParameters>().m_QuestTargets.Add(target);
                questPan.transform.GetComponent<QuestPanParameters>().m_TargetBySpeciesDefeat.Add(0);
                m_enemyName = m_EnemyDictionary2.GetEnemyByName(target);

                questPan.transform.GetChild(10 + indexForChild).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_enemyName;
                questPan.transform.GetChild(10 + indexForChild).gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = questPan.transform.GetComponent<QuestPanParameters>().m_TargetBySpeciesDefeat[0].ToString();
                indexForChild++;
            }
        }
        private void QuestPanelAdd(GameObject questPanel)
        {
            questPanels.Add(questPanel);
            questPanParameters.Add(questPanel.transform.GetComponent<QuestPanParameters>());
        }

        public void OnReceiveMessage(MessageType type, object sender, object message)
        {
            if (type == MessageType.DEAD)
            {
                CheckQuestWhenEnemyDead((Damageable)sender, (Damageable.DamageMessage)message);
            }
        }
        private void CheckQuestWhenEnemyDead(Damageable sender, Damageable.DamageMessage message)
        {
            var questLog = message.damageSource.GetComponent<QuestLog>();
            if (questLog == null) { return; }
            foreach (var quest in questLog.quests)
            {
                if (quest.questStatus == QuestStatus.ACTIVE)
                {
                    if (quest.type == QuestType.HUNT)
                    {
                        foreach (var targetUid in quest.targets)
                        {
                            if (sender.GetComponent<UniqueId>().Uid == targetUid)
                            {
                                quest.questTargetKilled += 1;
                                if (quest.amount == quest.questTargetKilled)
                                {
                                    quest.questStatus = QuestStatus.COMPLETED;
                                    Debug.Log("Quest completed");
                                    m_PlayerStats.GainExp(quest.experience);
                                    m_PlayerStats.questCompleted++;
                                }
                                QUpdateAfterKill(quest, sender.GetComponent<UniqueId>().Uid);
                            }
                        }

                    }

                }
            }
        }

        private void QUpdateAfterKill(AcceptedQuest quest, string questTargetUId)
        {
            foreach (var quePan in questPanels)
            {
                if (quePan.transform.GetComponent<QuestPanParameters>().m_QuestTitle == quest.title)
                {
                    quePan.transform.GetComponent<QuestPanParameters>().m_CurrentDeafeted = quest.questTargetKilled;
                    quePan.transform.GetComponent<QuestPanParameters>().m_QuestStatus = quest.questStatus.ToString();
                    quePan.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = quePan.transform.GetComponent<QuestPanParameters>().m_CurrentDeafeted.ToString();
                    quePan.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = quePan.transform.GetComponent<QuestPanParameters>().m_QuestStatus;
                    var indexer = 0;
                    foreach (var queTar in quest.targets)
                    {
                        if (questTargetUId == queTar)
                        {
                            quePan.transform.GetComponent<QuestPanParameters>().m_TargetBySpeciesDefeat[indexer] += 1;
                            quePan.transform.GetChild(10 + indexer).gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = quePan.transform.GetComponent<QuestPanParameters>().m_TargetBySpeciesDefeat[indexer].ToString();
                        }
                        indexer++;
                    }
                }
            }
        }

    }
}


