using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace H1ddenGames
{
    namespace ItemSystems
    {
        [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "H1ddenGames/ItemSystem/Crafting Scriptable Object")]
        public class CraftSO : ScriptableObject
        {
            [SerializeField] private List<ItemSO> listOfRequiredItems = new List<ItemSO>();
            [SerializeField] private ItemSO resultOfCrafting;

            #region Getters and Setters
            public List<ItemSO> ListOfRequiredItems { get => listOfRequiredItems; set => listOfRequiredItems = value; }
            public ItemSO ResultOfCrafting { get => resultOfCrafting; set => resultOfCrafting = value; }
            #endregion
        }
    }
}