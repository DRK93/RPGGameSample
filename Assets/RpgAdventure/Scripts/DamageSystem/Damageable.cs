using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public partial class Damageable : MonoBehaviour
    {

        public LayerMask playerActionReceivers;
        public List<MonoBehaviour> onDamageMessageReceivers;

        private bool m_blockStance;
        private int m_CurrentHitPoints;
        private CharacterStats m_CharacterStats;
        private bool m_IsInvulnerable = false;
        private float m_TimeSinceLastHit = 0.0f;
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

        private void Update()
        {
            if (m_IsInvulnerable)
            {
                m_TimeSinceLastHit += Time.deltaTime;
                if (m_TimeSinceLastHit >= m_CharacterStats.invulnerabilityTime)
                {
                    m_IsInvulnerable = false;
                    m_TimeSinceLastHit = 0;
                }
            }
        }

        public void ApplyPotion(int potionHealth)
        {
            if (m_CurrentHitPoints + potionHealth > m_CharacterStats.maxHitPoints)
            {
                m_CurrentHitPoints = m_CharacterStats.maxHitPoints;
            }
            else
                m_CurrentHitPoints += potionHealth;
        }
        public void SetInitialHealth()
        {
            m_CurrentHitPoints = m_CharacterStats.maxHitPoints;
        }

        public void ApplyDamage(DamageMessage data)
        {
            if (m_CurrentHitPoints <= 0 || m_IsInvulnerable)
            {
                return;
            }

            Vector3 positionToDamager = data.damageSource.transform.position - transform.position;
            positionToDamager.y = 0;

            if (Vector3.Angle(transform.forward, positionToDamager) > m_CharacterStats.hitAngle * 0.5f)
            {
                return;
            }

            m_IsInvulnerable = true;
            MessageType messageType;

            if (m_blockStance == true && Vector3.Angle(transform.forward, positionToDamager) < m_CharacterStats.blockAngle *0.5 )
            {
                messageType = MessageType.BLOCKED;
            }
            else
            {
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


