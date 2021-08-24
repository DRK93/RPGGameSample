using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public partial class Damageable : MonoBehaviour
    {

        [Range(0, 360.0f)]
        public float hitAngle = 360.0f;
        [Range(0, 150.0f)]
        public float blockAngle = 150.0f;
        public float invulnerabilityTime = 0.3f;
        public int maxHitPoints;
        public int experience;
        public int CurrentHitPoints { get; private set; }
        public int currentHealth;
        public bool blockStance;
        public LayerMask playerActionReceivers;
        public List<MonoBehaviour> onDamageMessageReceivers;

        private bool m_IsInvulnerable = false;
        private float m_TimeSinceLastHit = 0.0f;

        private void Awake()
        {
            SetInitialHealth();

            if (0!= (playerActionReceivers.value & 1 <<gameObject.layer))
            {
                onDamageMessageReceivers.Add(FindObjectOfType<QuestUpdate>());
                onDamageMessageReceivers.Add(FindObjectOfType<PlayerStats>());
            }

        }

        private void Update()
        {
            currentHealth = CurrentHitPoints;
          if (m_IsInvulnerable)
            {
                m_TimeSinceLastHit += Time.deltaTime;
                if (m_TimeSinceLastHit >= invulnerabilityTime)
                {
                    m_IsInvulnerable = false;
                    m_TimeSinceLastHit = 0;
                }
            }
        }

        public void ApplyPotion(int potionHealth)
        {
            if (CurrentHitPoints + potionHealth > maxHitPoints)
            {
                CurrentHitPoints = maxHitPoints;
            }
            else
            CurrentHitPoints += potionHealth;

        }
        public void SetInitialHealth()
        {
            CurrentHitPoints = maxHitPoints;
            currentHealth = CurrentHitPoints;
        }

        public void ApplyDamage(DamageMessage data)
        {
            if (CurrentHitPoints <= 0 || m_IsInvulnerable)
            {
                return;
            }

            Vector3 positionToDamager = data.damageSource.transform.position - transform.position;
            positionToDamager.y = 0;

            if (Vector3.Angle(transform.forward, positionToDamager) > hitAngle * 0.5f)
            {
                return;
            }
            
            m_IsInvulnerable = true;
            

            MessageType messageType;
            if (blockStance == true && Vector3.Angle(transform.forward, positionToDamager) < blockAngle *0.5 )
            {
                Debug.Log(Vector3.Angle(transform.forward, positionToDamager));
                messageType = MessageType.BLOCKED;
            }
            else
            {
                CurrentHitPoints -= data.amount;
                if (CurrentHitPoints <= 0)
                {
                    messageType = MessageType.DEAD;
                }
                else if (CurrentHitPoints < maxHitPoints / 3)
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
                Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, 
                transform.up,
                rotatedForward,
                hitAngle,
                1.0f);
        }
#endif
    }
}


