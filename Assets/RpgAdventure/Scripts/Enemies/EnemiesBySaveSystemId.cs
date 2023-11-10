using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGS.ExtendableSaveSystem;
using RpgAdventure.Scripts.Core;

namespace RpgAdventure
{
    // purpose of this class is to remove enemies from loaded save game which were defeated before saved game was saved
    public class EnemiesBySaveSystemId : MonoBehaviour, ISavableComponent
    {
        public Dictionary<int, bool> enmiesSaveSystem;
        public List<int> enemySaveId;
        public List<bool> deadStatus;
        private GameObject[] enemiesToKill;


        [SerializeField] private int m_uniqueID;
        [SerializeField] private int m_executionOrder;
        public int uniqueID => m_uniqueID;
        public int executionOrder => m_executionOrder;

        void Awake()
        {
            enmiesSaveSystem = new Dictionary<int, bool>();
            enemySaveId = new List<int>();
            deadStatus = new List<bool>();
            enemiesToKill = GameObject.FindGameObjectsWithTag("Enemy");
        }
        public void EnemyDestroy()
        {
            foreach (var enemyId in enemySaveId)
            {
                if (enmiesSaveSystem[enemyId]==true)
                {
                    Debug.Log("Destroying dead enemies from gameworld");
                    foreach (var enemy in enemiesToKill)
                    {
                        if (enemy!=null)
                        {
                            if (enemy.GetComponent<CharacterStats>().uniqueID == enemyId)
                            {
                                Destroy(enemy);
                            }
                        }
                    }
                }
            }
                
        }

        public ComponentData Serialize()
        {
            int j = 0;
            ExtendedComponentData data = new ExtendedComponentData();
            foreach (KeyValuePair<int, bool> entry in enmiesSaveSystem)
            {
                data.SetInt("key" + j,entry.Key);
                data.SetBool("bool" + j, entry.Value);
                j++;
            }
            return data;
        }

        public void Deserialize(ComponentData data)
        {
            enemySaveId = new List<int>();
            deadStatus = new List<bool>();
            ExtendedComponentData unpacked = (ExtendedComponentData)data;
            for (int j=0; j< enmiesSaveSystem.Count; j++)
            {
                enemySaveId.Add(unpacked.GetInt("key" + j));
                deadStatus.Add(unpacked.GetBool("bool" + j));
                Debug.Log(unpacked.GetBool("bool" + j));
            }
            DictionaryUpdaet();
            EnemyDestroy();
        }
        public void DictionaryUpdaet()
        {
            for (int i = 0; i < enemySaveId.Count; i++)
            {
                enmiesSaveSystem[enemySaveId[i]] = deadStatus[i];
            }
        }
    }
}

