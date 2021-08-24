using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RpgAdventure
{
    public class EnemyName : MonoBehaviour
    {
        public GameObject enemyName;
        public GameObject enemyHealthBar;
        private BanditBehaviour banditBhv; 
        // Start is called before the first frame update
        void Start()
        {
            banditBhv = GetComponent<BanditBehaviour>();
            if (banditBhv.ThisEnemyName != null)
            { enemyName.GetComponent<TMPro.TextMeshPro>().text = banditBhv.ThisEnemyName; }

            else
            { enemyName.GetComponent<TMPro.TextMeshPro>().text = "Corpse"; }
            enemyName.GetComponent<TMPro.TextMeshPro>().color = Color.red;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            enemyName.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward);
            //enemyName.transform.LookAt(Camera.main.transform.position);
            enemyName.transform.Rotate(0, 180, 0);
            enemyHealthBar.transform.LookAt(Camera.main.transform.position);
            enemyHealthBar.transform.Rotate(0, 180, 0);

        }
    }
}

