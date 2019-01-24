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
        [CreateAssetMenu(fileName = "New Item", menuName = "H1ddenGames/ItemSystem/Item Scriptable Object")]
        public class ItemSO : ScriptableObject
        {
            [SerializeField] private ItemBase itemBase;
            [SerializeField, Space(20)] private ItemInventoryBase itemInventoryBase;
            [SerializeField, Space(20)] private ItemStatsBase itemStatsBase;
            [SerializeField, Space(20)] private ItemDetailsBase itemDetailsBase;

            #region Getters and Setters
            public ItemBase ItemBase { get => itemBase; set => itemBase = value; }
            public ItemInventoryBase ItemInventoryBase { get => itemInventoryBase; set => itemInventoryBase = value; }
            public ItemStatsBase ItemStatsBase { get => itemStatsBase; set => itemStatsBase = value; }
            public ItemDetailsBase ItemDetailsBase { get => itemDetailsBase; set => itemDetailsBase = value; }
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
            [SerializeField] private List<String> itemTags = new List<string>();
            
            #region Getters and Setters
            public string ItemID { get => itemID; set => itemID = value; }
            public string ItemName { get => itemName; set => itemName = value; }
            public string ItemDescription { get => itemDescription; set => itemDescription = value; }
            public List<string> ItemTags { get => itemTags; set => itemTags = value; }
            #endregion
        }

        [System.Serializable]
        public class ItemInventoryBase
        {
            [SerializeField] private bool itemIsDroppable;
            [SerializeField] private bool itemIsTradeable;
            [SerializeField] private bool itemIsSellable;
            [SerializeField, Space(10)] private InventoryTab inventoryTab;
            [SerializeField, Space(10)] private ItemRarity itemRarity;

            #region Getters and Setters
            public bool ItemIsDroppable { get => itemIsDroppable; set => itemIsDroppable = value; }
            public bool ItemIsTradeable { get => itemIsTradeable; set => itemIsTradeable = value; }
            public bool ItemIsSellable { get => itemIsSellable; set => itemIsSellable = value; }
            public InventoryTab InventoryTab { get => inventoryTab; set => inventoryTab = value; }
            public ItemRarity ItemRarity { get => itemRarity; set => itemRarity = value; }
            #endregion
        }

        [System.Serializable]
        public class ItemStatsBase
        {
            [SerializeField] private MainStats itemMainStats;
            [SerializeField] private AttackStats itemAttackStats;
            [SerializeField] private DefenseStats itemDefenseStats;

            #region Getters and Setters 
            public MainStats ItemMainStats { get => itemMainStats; set => itemMainStats = value; }
            public AttackStats ItemAttackStats { get => itemAttackStats; set => itemAttackStats = value; }
            public DefenseStats ItemDefenseStats { get => itemDefenseStats; set => itemDefenseStats = value; }
            #endregion
        }

        [System.Serializable]
        public class ItemDetailsBase
        {
            [SerializeField] private int itemLevel = 1;
            [SerializeField, Space(10)] private MainStats itemStatRequirements;

            #region Getters and Setters
            public int ItemLevel { get => itemLevel; set => itemLevel = value; }
            public MainStats ItemStatRequirements { get => itemStatRequirements; set => itemStatRequirements = value; }
            #endregion
        }

        [System.Serializable]
        public class MainStats
        {
            [SerializeField] private int intelligence = 0;
            [SerializeField] private int luck = 0;
            [SerializeField] private int dexterity = 0;
            [SerializeField] private int strength = 0;

            #region Getters and Setters
            public int Intelligence { get => intelligence; set => intelligence = value; }
            public int Luck { get => luck; set => luck = value; }
            public int Dexterity { get => dexterity; set => dexterity = value; }
            public int Strength { get => strength; set => strength = value; }
            #endregion
        }

        [System.Serializable]
        public class AttackStats
        {
            [SerializeField] private float physicalAttack = 0f;
            [SerializeField] private float magicalAttack = 0f;
            [SerializeField] private float elementalAttack = 0f;

            #region Getters and Setters
            public float PhysicalAttack { get => physicalAttack; set => physicalAttack = value; }
            public float MagicalAttack { get => magicalAttack; set => magicalAttack = value; }
            public float ElementalAttack { get => elementalAttack; set => elementalAttack = value; }
            #endregion
        }

        [System.Serializable]
        public class DefenseStats
        {
            [SerializeField] private float physicalDefense = 0f;
            [SerializeField] private float magicalDefense = 0f;
            [SerializeField] private float elementalDefense = 0f;

            #region Getters and Setters
            public float PhysicalDefense { get => physicalDefense; set => physicalDefense = value; }
            public float MagicalDefense { get => magicalDefense; set => magicalDefense = value; }
            public float ElementalDefense { get => elementalDefense; set => elementalDefense = value; }
            #endregion
        }

        public enum InventoryTab
        {
            None, Accessory, Armor, Craft, Quest, Use, Weapon
        }

        public enum ItemRarity
        {
            None, Normal, Epic, Unique, Legendary, Ancient, One_Of_A_Kind
        }
    }
}