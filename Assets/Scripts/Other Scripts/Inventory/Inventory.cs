using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using H1ddenGames;

namespace H1ddenGames
{
    namespace ItemSystems
    {
        public class Inventory : MonoBehaviour
        {
            [SerializeField] private List<GameObject> items = new List<GameObject>();

            #region Getters and Setters
            public List<GameObject> Items { get => items; set => items = value; }
            #endregion
        }
    }
}