using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class QuestPanParameters : MonoBehaviour
    {
        public string m_QuestTitle;
        public string m_QuestDescription;
        public string m_QuestStatus;
        public int m_TargetAmount;
        public int m_CurrentDeafeted;
        public List<string> m_QuestTargets;
        public List<int> m_TargetBySpeciesDefeat;
    }
}

