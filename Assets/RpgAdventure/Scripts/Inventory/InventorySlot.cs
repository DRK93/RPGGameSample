using System;
using UnityEngine;

namespace RpgAdventure.Scripts.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public int index;
        public string itemName;
        public GameObject itemPrefab;

        public InventorySlot(int index)
        {
            this.index = index;
        }

        public void Place(GameObject item)
        {
            itemName = item.name;
            itemPrefab = item;
        }
    }
}
