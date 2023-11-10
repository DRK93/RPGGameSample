using System.Collections;
using System.Collections.Generic;
using RpgAdventure.Scripts.Core;
using RpgAdventure.Scripts.Helpers;
using RpgAdventure.Scripts.Player;
using RpgAdventure.Scripts.Quests;
using RpgAdventure.Scripts.Weapons;
using UnityEngine;

namespace RpgAdventure.Scripts.DamageSystem
{
    // class manage fighting mechanic, hit points, applying damage, compute damage
    // send information about result of fighting mechanic calculation
    public partial class Damageable : MonoBehaviour
    {

        public LayerMask playerActionReceivers;
        public List<MonoBehaviour> onDamageMessageReceivers;

        private bool m_blockStance;
        private int m_CurrentHitPoints;
        private CharacterStats m_CharacterStats;
        private List<bool> invulnerables;
        private List<float> timesSinceLastHit;
        private List<int> enemiesUIds;
        private List<int> indexesToRemove;
        public int CurrentHitPoints
        {
            get { return m_CurrentHitPoints; }
            set { m_CurrentHitPoints += value;
                m_CharacterStats.currentHitPoints = m_CurrentHitPoints;
            }
        }
        public bool BlockStance
        {
            get { return m_blockStance; }
            set { m_blockStance = value; }
        }

        private void Awake()
        {
            m_CharacterStats = GetComponent<CharacterStats>();
            SetInitialHealth();

            if (0 != (playerActionReceivers.value & 1 << gameObject.layer))
            {
                onDamageMessageReceivers.Add(FindObjectOfType<QuestUpdate>());
                onDamageMessageReceivers.Add(GameObject.Find("Player").GetComponent<PlayerStats>());
            }
        }
        private void Start()
        {
            invulnerables = new List<bool>();
            timesSinceLastHit = new List<float>();
            enemiesUIds = new List<int>();
            indexesToRemove = new List<int>();
        }

        private void Update()
        {

            if(invulnerables != null)
            {
                indexesToRemove = new List<int>();
                for (int i=0; i<invulnerables.Count; i++)
                {
                    timesSinceLastHit[i] += Time.deltaTime;
                    if (timesSinceLastHit[i] >= m_CharacterStats.invulnerabilityTime)
                    {
                        invulnerables[i] = false;
                        indexesToRemove.Add(i);
                    }
                }
                if (indexesToRemove != null)
                {
                    indexesToRemove.Reverse();
                    foreach (var index in indexesToRemove)
                    {
                        invulnerables.RemoveAt(index);
                        timesSinceLastHit.RemoveAt(index);
                        enemiesUIds.RemoveAt(index);
                    }
                }
            }
        }

        public void ApplyPotion(int potionHealth)
        {
            if (m_CurrentHitPoints + potionHealth > m_CharacterStats.maxHitPoints)
            {
                m_CurrentHitPoints = m_CharacterStats.maxHitPoints;
                m_CharacterStats.currentHitPoints = m_CharacterStats.maxHitPoints;
            }
            else
            {
                m_CurrentHitPoints += potionHealth;
                m_CharacterStats.currentHitPoints += potionHealth;
            }
        }
        public void SetInitialHealth()
        {
            m_CurrentHitPoints = m_CharacterStats.maxHitPoints;
        }

        public void ApplyDamageFromSpell(DamageMessage data)
        {
            MessageType messageType;

            m_CurrentHitPoints -= data.amount;
            m_CharacterStats.currentHitPoints = m_CurrentHitPoints;
            if (m_CurrentHitPoints <= 0)
            {
                messageType = MessageType.DEAD;
            }
            else if (m_CurrentHitPoints < m_CharacterStats.maxHitPoints / 3)
            {
                messageType = MessageType.HIGHDAMAGED;
            }
            else
            {
                messageType = MessageType.DAMAGED;
            }

            for (int i = 0; i < onDamageMessageReceivers.Count; i++)
            {
                var receiver = onDamageMessageReceivers[i] as IMessageReceiver;
                receiver.OnReceiveMessage(messageType, this, data);
            }
        }
        public void ApplyDamageFromWeapon(DamageMessage data)
        {
            int enemyApplyingDmg = data.damageSource.GetComponent<CharacterStats>().uniqueID;

            if (enemiesUIds != null)
            {
                foreach (var enemy in enemiesUIds)
                {
                    if (enemyApplyingDmg == enemy)
                    {
                        return;
                    }
                }
            }
            enemiesUIds.Add(enemyApplyingDmg);
            invulnerables.Add(false);
            timesSinceLastHit.Add(0f);
            ApplyDamage(data,invulnerables[invulnerables.Count - 1]);
        }
        public void ApplyDamage(DamageMessage data, bool invulerable)
        {
            if (m_CurrentHitPoints <=0 || invulerable)
            {
                return;
            }

            MessageType messageType;
            Vector3 positionToDamager = data.damageSource.transform.position - transform.position;
            positionToDamager.y = 0;

            if (Vector3.Angle(transform.forward, positionToDamager) > m_CharacterStats.hitAngle * 0.5f)
            {
                return;
            }

            invulerable = true;

            if (m_blockStance == true && Vector3.Angle(transform.forward, positionToDamager) < m_CharacterStats.blockAngle *0.5 )
            {
                data.damager.GetComponent<MeleeWeapon>().blockedHitAudio.PlayRandomClip();
                messageType = MessageType.BLOCKED;
            }
            else
            {
                data.damager.GetComponent<MeleeWeapon>().impactAudio.PlayRandomClip();
                m_CurrentHitPoints -= data.amount;
                m_CharacterStats.currentHitPoints = m_CurrentHitPoints;
                if (m_CurrentHitPoints <= 0)
                {
                    messageType = MessageType.DEAD;
                }
                else if (m_CurrentHitPoints < m_CharacterStats.maxHitPoints / 3)
                {
                    messageType = MessageType.HIGHDAMAGED;
                }
                else
                {
                    messageType = MessageType.DAMAGED;
                }
            }
   
            for (int i = 0; i < onDamageMessageReceivers.Count; i++)
            {
                var receiver = onDamageMessageReceivers[i] as IMessageReceiver;
                receiver.OnReceiveMessage(messageType, this, data);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
            Vector3 rotatedForward = 
                Quaternion.AngleAxis(-360.0f * 0.5f, transform.up) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, 
                transform.up,
                rotatedForward,
                360.0f,
                1.0f);
        }
#endif
    }
}


