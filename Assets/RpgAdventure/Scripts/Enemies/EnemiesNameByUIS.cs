using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    // This class is for one purpose : to show name of enemies specific genre
    // without added words/numbers like 'Clone', '(1), '(2)' etc.
    public class EnemiesNameByUIS : MonoBehaviour
    {
        public Dictionary<string, string> enemyNameList;
        void Awake()
        {
            enemyNameList = new Dictionary<string, string>();
        }
        public void AddEnemyToDictionary(string enemyUId, string enemyName)
        {
            if (!enemyNameList.ContainsKey(enemyUId))
            {
                enemyNameList.Add(enemyUId, enemyName);
            }
        }
        public string DictionaryToString(Dictionary<string, string> dictionary)
        {
            string dictionaryString = "{";
            foreach (KeyValuePair<string, string> keyValues in dictionary)
            {
                dictionaryString += keyValues.Key + " : " + keyValues.Value + ", ";
            }
            return dictionaryString.TrimEnd(',', ' ') + "}";
        }
        public string GetEnemyByName(string enemyUId)
        {
            string enemyRealName;
            string enemyRealName2 = "0";
            bool isEnemyUId = enemyNameList.TryGetValue(enemyUId, out enemyRealName);
            if (isEnemyUId)
            {
                enemyRealName2 = enemyRealName;
            }
            else
            {
            }
            return enemyRealName2;
        }
    }
}

