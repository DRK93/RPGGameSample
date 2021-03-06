using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RpgAdventure
{

    public class InventoryManager : MonoBehaviour
    {
        public List<InventorySlot> inventory = new List<InventorySlot>();
        public Transform inventoryPanel;
        public GameObject inventoryUI;
        private int m_inventorySize;
        private bool m_InvetoryOpen;

        private void Awake()
        {
            m_inventorySize = inventoryPanel.childCount;
            CreateInventory(m_inventorySize);

            inventoryUI.SetActive(false);
            m_InvetoryOpen = false;
            Input.GetKeyDown(KeyCode.I);
        }
        private void Update()
        {
            bool isKeyForInventory = Input.GetKeyDown(KeyCode.I);
            if (isKeyForInventory)
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
            inventoryPanel.GetChild(inventorySlot.index).GetComponentInChildren<Text>().text = spawner.itemPrefab.name;
            inventoryPanel.GetChild(inventorySlot.index).GetChild(2).GetComponent<Text>().text = spawner.itemPrefab.GetComponent<MeleeWeapon>().damage.ToString();
            Destroy(spawner.gameObject);
        }

        private InventorySlot GetFreeSlot()
        {
            return inventory.Find(slot => slot.itemName == null);
        }
        private InventorySlot GetSlotByIndex(int index)
        {
            return inventory.Find(slot => slot.index == index);
        }
    }
}

