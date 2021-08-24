using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class EnemiesNameByUIS : MonoBehaviour
    {
        public Dictionary<string, string> enemyNameList;
        
        // Start is called before the first frame update
        void Awake()
        {
            enemyNameList = new Dictionary<string, string>();
            //Debug.Log("Hello, your enemy list is here awake");
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(enemyNameList.ToString());
            //Debug.Log(DictionaryToString(enemyNameList));
        }
        public void AddEnemyToDictionary(string enemyUId, string enemyName)
        {
            //Debug.Log(enemyName);
            if (!enemyNameList.ContainsKey(enemyUId))
            {
                enemyNameList.Add(enemyUId, enemyName);
                //Debug.Log(enemyNameList.Count);
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
                //Debug.Log("There is that enemy: " + enemyRealName2);
            }
            else
            {
                //Debug.Log("No enemy withat id");
            }
            return enemyRealName2;
        }
    }
}

