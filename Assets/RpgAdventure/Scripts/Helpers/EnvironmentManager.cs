using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGS.ExtendableSaveSystem;

namespace RpgAdventure.Scripts.Helpers
{
    public class EnvironmentManager : MonoBehaviour, ISavableComponent
    {

        [SerializeField] private int m_uniqueID;
        [SerializeField] private int m_executionOrder;
        public int uniqueID => m_uniqueID;
        public int executionOrder => m_executionOrder;
        private bool IsActive;
        public GameObject forceField;
        public void Deserialize(ComponentData data)
        {
            ExtendedComponentData unpacked = (ExtendedComponentData)data;
            IsActive = unpacked.GetBool("IsActive");
            Debug.Log("Is Active: " + IsActive);
            if (IsActive == false)
            {
                Destroy(forceField);
            }
        }

        public ComponentData Serialize()
        {
            ExtendedComponentData data = new ExtendedComponentData();
            Debug.Log("Is Active: " + IsActive);
            if (forceField == null)
                IsActive = false;
            else
                IsActive = true;
            data.SetBool("IsActive", IsActive);
            return data;
        }

    }
}

