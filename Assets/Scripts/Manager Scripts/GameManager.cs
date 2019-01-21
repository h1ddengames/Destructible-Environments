using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using H1ddenGames.ItemSystems;

namespace H1ddenGames
{
    namespace GameManager
    {
        public class GameManager : MonoBehaviour
        {
            public static GameManager Instance = null;

            [SerializeField] private GameObject player;
            [SerializeField] private InventorySystem inventorySystem = InventorySystem.Instance;
            [SerializeField] private ItemSystem itemSystem = ItemSystem.Instance;

            #region Getters and Setters
            public GameObject Player { get => player; set => player = value; }
            public InventorySystem InventorySystem { get => inventorySystem; set => inventorySystem = value; }
            public ItemSystem ItemSystem { get => itemSystem; set => itemSystem = value; }
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

                FindPlayer();   
            }

            [ContextMenu("Find Player.")]
            public void FindPlayer()
            {
                if (Player == null)
                {
                    Player = GameObject.FindGameObjectWithTag("Player");
                }
            }
        }
    }
}