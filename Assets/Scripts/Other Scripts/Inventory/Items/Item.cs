using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace H1ddenGames
{
    namespace ItemSystems
    {
        public class Item : MonoBehaviour
        {
            [SerializeField] private ItemSO itemSO;

            #region Getters and Setters
            public ItemSO ItemSO { get => itemSO; set => itemSO = value; }
            #endregion

            public void PickUpItem()
            {
                InventorySystem.Instance.PlayerInventory.Items.Add(gameObject);
            }

            public void DropItem()
            {
                InventorySystem.Instance.PlayerInventory.Items.Remove(gameObject);
            }

            public void UseItem()
            {

            }

            [ContextMenu("Put item name field on prefab name.")]
            public void NameFieldToFileName()
            {
                gameObject.name = ItemSO.ItemBase.ItemName;
            }
        }
    }
}