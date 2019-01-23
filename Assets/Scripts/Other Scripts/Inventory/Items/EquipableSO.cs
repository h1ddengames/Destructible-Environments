using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace H1ddenGames
{
    namespace ItemSystems
    {
        public class EquipableSO : ItemSO
        {
            [SerializeField] private int physicalAttack = 0;
            [SerializeField] private int magicalAttack = 0;
            [SerializeField] private int elementalAttack = 0; 
            [SerializeField] private int physicalDefense = 0;
            [SerializeField] private int magicalDefense = 0;
            [SerializeField] private int elementalDefense = 0;

            #region Getters and Setters
            public int PhysicalAttack { get => physicalAttack; set => physicalAttack = value; }
            public int MagicalAttack { get => magicalAttack; set => magicalAttack = value; }
            public int ElementalAttack { get => elementalAttack; set => elementalAttack = value; }
            public int PhysicalDefense { get => physicalDefense; set => physicalDefense = value; }
            public int MagicalDefense { get => magicalDefense; set => magicalDefense = value; }
            public int ElementalDefense { get => elementalDefense; set => elementalDefense = value; }
            #endregion
        }
    }
}