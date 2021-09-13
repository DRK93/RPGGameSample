using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGS.ExtendableSaveSystem;

namespace RpgAdventure
{
    public class InventoryLoadState : MonoBehaviour, ISavableComponent
    {
        [SerializeField] private int m_uniqueID;
        [SerializeField] private int m_executionOrder;
        public int uniqueID => m_uniqueID;
        public int executionOrder => m_executionOrder;
        void Start()
        {

        }
        void Update()
        {

        }
        public ComponentData Serialize()
        {
            ExtendedComponentData data = new ExtendedComponentData();
            return data;
        }
        public void Deserialize(ComponentData data)
        {
            
        }

        private void LoadItem()
        {

        }
    }
}

