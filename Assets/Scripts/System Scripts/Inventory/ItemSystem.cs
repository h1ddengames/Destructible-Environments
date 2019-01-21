using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using H1ddenGames;

namespace H1ddenGames
{
    namespace ItemSystems
    {
        public class ItemSystem : MonoBehaviour
        {
            public static ItemSystem Instance = null;

            [SerializeField] private List<GameObject> items = new List<GameObject>();

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

                LoadItems();   
            }

            [ContextMenu("Find all items in Resources folder.")]
            public void LoadItems()
            {
                items = Resources.LoadAll<GameObject>("Items").ToList();
            }
        }
    }
}