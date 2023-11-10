using System.Collections;
using System.Collections.Generic;
using RpgAdventure.Scripts.Enemies.Bandit;
using UnityEngine;
using UnityEngine.UI;

namespace RpgAdventure
{
    public class EnemyName : MonoBehaviour
    {
        public GameObject enemyName;
        public GameObject enemyHealthBar;
        private BanditBehaviour banditBhv; 
        void Start()
        {
            banditBhv = GetComponent<BanditBehaviour>();
            if (banditBhv.ThisEnemyName != null)
            { enemyName.GetComponent<TMPro.TextMeshPro>().text = banditBhv.ThisEnemyName; }
            else
            { enemyName.GetComponent<TMPro.TextMeshPro>().text = "Corpse"; }

            enemyName.SetActive(false);
            enemyHealthBar.SetActive(false);
        }

        void LateUpdate()
        {
            CheckDistanceToPlayerCamera();
        }
        private void CheckDistanceToPlayerCamera()
        {
            if (Vector3.Distance(enemyName.transform.position, Camera.main.transform.position) < 50.0f)
            {
                enemyName.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward);
                enemyName.transform.Rotate(0, 180, 0);
                enemyHealthBar.transform.LookAt(Camera.main.transform.position);
                enemyHealthBar.transform.Rotate(0, 180, 0);

                enemyName.SetActive(true);
                enemyHealthBar.SetActive(true);
            }
            else
            {
                enemyName.SetActive(false);
                enemyHealthBar.SetActive(false);
            }
        }
    }
}

