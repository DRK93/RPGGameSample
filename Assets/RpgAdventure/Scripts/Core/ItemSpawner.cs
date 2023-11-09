using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RpgAdventure
{
    public class ItemSpawner : MonoBehaviour
    {
        public GameObject itemPrefab;
        public LayerMask targetLayers;
        public UnityEvent<ItemSpawner> onItemPickUp;

        private void Awake()
        {
            Instantiate(itemPrefab, transform);
            Destroy(transform.GetChild(0).gameObject);

            onItemPickUp.AddListener(FindObjectOfType<InventoryManager>().OnItemPickup);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (0 != (targetLayers.value & 1 << other.gameObject.layer))
            {
                onItemPickUp.Invoke(this);
            }
        }
    }
}

