using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace H1ddenGames
{
    namespace ItemSystems
    {
        public abstract class Item : MonoBehaviour
        {
            [SerializeField] private ItemSO itemSO;

            #region Getters and Setters
            public ItemSO ItemSO { get => itemSO; set => itemSO = value; }
            #endregion

            public virtual void PickUpItem()
            {
                InventorySystem.Instance.PlayerInventory.Items.Add(gameObject);
            }

            public virtual void DropItem()
            {
                InventorySystem.Instance.PlayerInventory.Items.Remove(gameObject);
            }

            public virtual void UseItem()
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