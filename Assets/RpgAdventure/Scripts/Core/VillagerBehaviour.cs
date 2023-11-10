using System.Collections;
using System.Collections.Generic;
using RpgAdventure.Scripts.Quests;
using UnityEngine;

namespace RpgAdventure.Scripts.Core
{
    public class VillagerBehaviour : MonoBehaviour
    {
        public GameObject villagerName;

        void Start()
        {
            villagerName.SetActive(false);
        }

        void LateUpdate()
        {
            CheckDistanceToPlayerCamera();
        }
        private void CheckDistanceToPlayerCamera()
        {
            if (Vector3.Distance(villagerName.transform.position, Camera.main.transform.position) < 50.0f)
            {
                villagerName.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward);
                villagerName.transform.Rotate(0, 180, 0);
                villagerName.SetActive(true);
                if (transform.CompareTag("QuestGiver"))
                {
                    transform.GetComponent<QuestGiver>().ShowQMark();
                }
            }
            else
            {
                villagerName.SetActive(false);
                if (transform.CompareTag("QuestGiver"))
                {
                    transform.GetComponent<QuestGiver>().HideQMark();
                }
            }
        }
    }
}


