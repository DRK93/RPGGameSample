using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGS.ExtendableSaveSystem;

namespace RpgAdventure
{
    public class CharacterStats : MonoBehaviour, ISavableComponent
    {
        [Range(0, 360.0f)]
        public float hitAngle = 360.0f;
        [Range(0, 150.0f)]
        public float blockAngle = 150.0f;
        public float invulnerabilityTime = 0.3f;
        public int maxHitPoints;
        public int experience;
        public int power;
        public int currentHitPoints;
        public bool isDead;


        [SerializeField] private int m_uniqueID;
        [SerializeField] private int m_executionOrder;
        public int uniqueID => m_uniqueID;
        public int executionOrder => m_executionOrder;

        public ComponentData Serialize()
        {
            isDead = false;
            ExtendedComponentData data = new ExtendedComponentData();
            data.SetTransform("transform", transform);

            data.SetInt("maxhp", maxHitPoints);
            data.SetInt("exp", experience);
            data.SetInt("powr", power);
            data.SetInt("currenthp", currentHitPoints);
            data.SetBool("dead", isDead);
            return data;
        }
        public void Deserialize(ComponentData data)
        {
            ExtendedComponentData unpacked = (ExtendedComponentData)data;
            unpacked.GetTransform("transform", transform);

            maxHitPoints = unpacked.GetInt("maxhp");
            experience = unpacked.GetInt("exp");
            power = unpacked.GetInt("powr");
            currentHitPoints = unpacked.GetInt("currenthp");
            isDead = unpacked.GetBool("dead");
        }
    }
}

