using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class AreaSpell : MonoBehaviour
    {
        [SerializeField]
        private float m_LifeTime = 8.0f;
        [SerializeField]
        private int m_SpellDmg = 10;
        private float counter;
        private GameObject m_Owner;
        private List<GameObject> enemyInArea;
        private List<int> enemyUniqId;
        private List<float> counterPerEnemy;

        void Start()
        {
            counter = 0f;
            enemyInArea = new List<GameObject>();
            enemyUniqId = new List<int>();
            counterPerEnemy = new List<float>();
        }

        void Update()
        {
            counter += Time.deltaTime;
            if (counter >= m_LifeTime)
                Destroy(this.gameObject);

            if (counter > 0.8 )
            {
                for (int i = 0; i < counterPerEnemy.Count; i++)
                {
                    counterPerEnemy[i] += Time.deltaTime;
                    if (counterPerEnemy[i] > 1)
                    {
                        counterPerEnemy[i] -= 1;
                        if (enemyInArea[i] != null)
                            ApplyDamageTo(enemyInArea[i]);
                    }
                }
            }
        }

        private void ApplyDamageTo(GameObject enemy)
            {
                Damageable damageable = enemy.GetComponent<Damageable>();
                m_Owner = GameObject.Find("Player");
                if (damageable != null)
                {
                    Damageable.DamageMessage data;
                    data.amount = m_SpellDmg + GameObject.Find("Player").GetComponent<PlayerStats>().spellDamage;
                    data.damager = this;
                    data.damageSource = m_Owner;
                    data.tool = 2;
                    damageable.ApplyDamage(data);
                    enemy.GetComponent<BanditBehaviour>().DetectionRadiusChange();
                }
        }
        public void EnteringSpellArea(GameObject enemyIn)
        {
            enemyInArea.Add(enemyIn);
            enemyUniqId.Add(enemyIn.GetComponent<CharacterStats>().uniqueID);
            counterPerEnemy.Add(0);
        }

        public void ExitSpellArea(GameObject enemyOut)
        {
            for(int i=0; i<enemyInArea.Count; i++)
            if (enemyOut.GetComponent<CharacterStats>().uniqueID == enemyInArea[i].GetComponent<CharacterStats>().uniqueID)
            {
                enemyInArea.RemoveAt(i);
                enemyUniqId.RemoveAt(i);
                counterPerEnemy.RemoveAt(i);
            }
        }
    }
}

