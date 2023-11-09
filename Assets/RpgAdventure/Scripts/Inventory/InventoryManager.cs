using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NGS.ExtendableSaveSystem;

namespace RpgAdventure
{
    //class to manage inventory system, save inventory and clean weapon name
    //assign chosen weapon and hide/show inventory UI
    public class InventoryManager : MonoBehaviour, ISavableComponent
    {
        public List<InventorySlot> inventory = new List<InventorySlot>();
        public List<string> itemNames = new List<string>();
        public Transform inventoryPanel;
        public GameObject inventoryUI;

        private int m_UsedItemIndex;
        private int m_inventorySize;
        private bool m_InvetoryOpen;
        [SerializeField] private int m_uniqueID;
        [SerializeField] private int m_executionOrder;
        public int uniqueID => m_uniqueID;
        public int executionOrder => m_executionOrder;

        private void Awake()
        {
            m_inventorySize = inventoryPanel.childCount;
            CreateInventory(m_inventorySize);

            inventoryUI.SetActive(false);
            m_InvetoryOpen = false;
        }

        public void HideShowInventory()
        {
            if (m_InvetoryOpen == false)
            {
                m_InvetoryOpen = true;
                inventoryUI.SetActive(true);
            }
            else
            {
                m_InvetoryOpen = false;
                inventoryUI.SetActive(false);
            }
        }

        private void CreateInventory(int size)
        {
            for (int i=0; i<size; i++)
            {
                inventory.Add(new InventorySlot(i));
                RegisterSlotHandler(i);
            }
        }

        private void RegisterSlotHandler(int slotIndex)
        {
            var slotBtn = inventoryPanel.GetChild(slotIndex).GetComponent<Button>();
            slotBtn.onClick.AddListener(() =>{ UseItem(slotIndex); });
        }

        private void UseItem(int slotIndex)
        {

            var inventorySlot = GetSlotByIndex(slotIndex);
            if (inventorySlot.itemPrefab == null)
            {
                return;
            }
            PlayerController.Instance.UseItemFrom(inventorySlot);
            m_UsedItemIndex = slotIndex;
        }


        public void OnItemPickup(ItemSpawner spawner)
        {
            AddItemFrom(spawner);
        }
        public void AddItemFrom(ItemSpawner spawner)
        {
            var inventorySlot = GetFreeSlot();
            if (inventorySlot == null)
            {
                Debug.Log("Inventory is Full");
                return;
            }

            inventorySlot.Place(spawner.itemPrefab);
            itemNames.Add(spawner.itemPrefab.name);
            inventoryPanel.GetChild(inventorySlot.index).GetComponentInChildren<Text>().text = spawner.itemPrefab.name;
            inventoryPanel.GetChild(inventorySlot.index).GetChild(2).GetComponent<Text>().text = spawner.itemPrefab.GetComponent<MeleeWeapon>().damage.ToString();
            Destroy(spawner.gameObject);
        }

        private InventorySlot GetFreeSlot()
        {
            return inventory.Find(slot => slot.itemName == null);
        }
        private InventorySlot GetSlot(int indexer)
        {
            return inventory.Find(slot => slot.index == indexer);
        }
        private InventorySlot GetSlotByIndex(int index)
        {
            return inventory.Find(slot => slot.index == index);
        }

        public ComponentData Serialize()
        {
            ExtendedComponentData data = new ExtendedComponentData();
            var indexer = 0;
            data.SetInt("count", itemNames.Count);
            data.SetInt("used", m_UsedItemIndex);
            foreach (var itemN in itemNames)
            {
                data.SetString("item" + indexer, itemN);
                indexer++;
            }
                return data;
        }
        public void Deserialize(ComponentData data)
        {
            itemNames = new List<string>();

            ExtendedComponentData unpacked = (ExtendedComponentData)data;
            var itemCount = unpacked.GetInt("count");
            m_UsedItemIndex = unpacked.GetInt("used");
            for (int  i=0; i<itemCount; i++)
            {
                var newItemName = unpacked.GetString("item" + i);
                itemNames.Add(newItemName);
            }
            LoadItemFromName();
            UseItem(m_UsedItemIndex);
        }

        private void LoadItemFromName()
        {
            int indexer = 0;
            foreach(var itemN in itemNames)
            {
                var inventorySlot = GetSlot(indexer);
                Destroy(inventorySlot.itemPrefab);
                inventorySlot.Place(Instantiate(Resources.Load(itemN)) as GameObject);
                ItemUncloneName(inventorySlot.itemPrefab.name);
                inventoryPanel.GetChild(inventorySlot.index).GetComponentInChildren<Text>().text = ItemUncloneName(inventorySlot.itemPrefab.name);
                inventoryPanel.GetChild(inventorySlot.index).GetChild(2).GetComponent<Text>().text = inventorySlot.itemPrefab.GetComponent<MeleeWeapon>().damage.ToString();
                indexer++;
            }
        }

        private string ItemUncloneName(string nameToUnclone)
        {
            if (nameToUnclone.EndsWith(")"))
            {
                nameToUnclone = nameToUnclone.Remove(nameToUnclone.Length - 7);
            }
            return nameToUnclone;
        }
    }
}

