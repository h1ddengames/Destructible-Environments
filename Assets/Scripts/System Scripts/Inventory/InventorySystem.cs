using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using H1ddenGames;

namespace H1ddenGames
{
    namespace ItemSystems
    {
        public class InventorySystem : MonoBehaviour
        {
            public static InventorySystem Instance = null;

            [SerializeField] private Inventory playerInventory;
            [SerializeField] private List<Inventory> inventories = new List<Inventory>();

            #region Getters and Setters
            public Inventory PlayerInventory { get => playerInventory; set => playerInventory = value; }
            public List<Inventory> Inventories { get => inventories; set => inventories = value; }
            #endregion

            private void Awake()
            {
                #region Creating a Singleton
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
                #endregion

            }

            [ContextMenu("Find Player Inventory.")]
            public void FindPlayerInventory()
            {
                if (playerInventory == null)
                {
                    playerInventory = GameManager.GameManager.Instance.Player.GetComponent<Inventory>();
                }
            }
        }
    }
}