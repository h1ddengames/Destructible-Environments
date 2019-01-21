using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace H1ddenGames
{
    namespace ItemSystems
    {
        public abstract class ItemSO : ScriptableObject
        {
            [SerializeField] private ItemBase itemBase;
            [SerializeField] private ItemInventoryBase itemInventoryBase;

            #region Getters and Setters
            public ItemBase ItemBase { get => itemBase; set => itemBase = value; }
            public ItemInventoryBase ItemInventoryBase { get => itemInventoryBase; set => itemInventoryBase = value; } 
            #endregion

            [ContextMenu("Put file name on item name field.")]
            public void FileNameToNameField()
            {
                string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
                ItemBase.ItemName = Path.GetFileNameWithoutExtension(assetPath);
            }

            [ContextMenu("Put item name field on file name.")]
            public void NameFieldToFileName()
            {
                string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
                AssetDatabase.RenameAsset(assetPath, ItemBase.ItemName);
            }

            [ContextMenu("Give a random string to ItemID.")]
            public void GiveRandomItemID()
            {
                ItemBase.ItemID = GenerateRandomString(8);
            }

            public static string GenerateRandomString(int length)
            {
                System.Random random = new System.Random();
                string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                StringBuilder result = new StringBuilder(length);
                for (int i = 0; i < length; i++)
                {
                    result.Append(characters[random.Next(characters.Length)]);
                }
                return result.ToString();
            }
        }

        [System.Serializable]
        public class ItemBase
        {
            [SerializeField] private string itemID = ItemSO.GenerateRandomString(8);
            [SerializeField] private string itemName = "Item Name";
            [SerializeField, TextArea(3, 15)] private string itemDescription = "This is the description of the item.";
            [SerializeField] private int itemLevel = 1;

            #region Getters and Setters
            public string ItemID { get => itemID; set => itemID = value; }
            public string ItemName { get => itemName; set => itemName = value; }
            public string ItemDescription { get => itemDescription; set => itemDescription = value; }
            public int ItemLevel { get => itemLevel; set => itemLevel = value; }
            #endregion
        }

        [System.Serializable]
        public class ItemInventoryBase
        {
            [SerializeField] private bool itemIsDroppable;
            [SerializeField] private bool itemIsTradeable;
            [SerializeField] private bool itemIsSellable;

            #region Getters and Setters
            public bool ItemIsDroppable { get => itemIsDroppable; set => itemIsDroppable = value; }
            public bool ItemIsTradeable { get => itemIsTradeable; set => itemIsTradeable = value; }
            public bool ItemIsSellable { get => itemIsSellable; set => itemIsSellable = value; }
            #endregion
        }
    }
}