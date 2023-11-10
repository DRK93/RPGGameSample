using System.Collections;
using UnityEngine;

namespace RpgAdventure.Scripts.Quests
{
    // class define NPC as QuestGiver and manage quest mark
    public class QuestGiver : MonoBehaviour
    {
        public Quest quest;
        public Dialog.Dialog dialog;
        public GameObject questMark;
        public GameObject questAddedMark;
        private bool m_QMarkAssigned;
        public bool QMarkAssigned { get => m_QMarkAssigned; set => m_QMarkAssigned = value; }

        public void Start()
        {
            questMark.SetActive(false);
            questAddedMark.SetActive(false);
            m_QMarkAssigned = false;
        }

        public void ShowQMark()
        {
            if (m_QMarkAssigned == false)
            {
                questMark.SetActive(true);
                questAddedMark.SetActive(false);
            }
            if (m_QMarkAssigned == true)
            {
                questMark.SetActive(false);
                questAddedMark.SetActive(true);
            }
        }
        public void HideQMark()
        {
            questMark.SetActive(false);
            questAddedMark.SetActive(false);
        }
    }
}