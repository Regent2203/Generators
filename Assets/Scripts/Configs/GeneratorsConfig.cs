using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataModel.Generators.Config
{
    [CreateAssetMenu(menuName = "Configs/GeneratorsConfig")]
    public class GeneratorsConfig : ScriptableObject
    {
        [SerializeField]
        private List<GeneratorInfo> _generators = new List<GeneratorInfo>();

        public IList<GeneratorInfo> GetData()
        {
            return _generators;
        }
    }

    [Serializable]
    public struct GeneratorInfo
    {
        public string Name;
        public int Level;
        public float Progress;
        public float Delay;
        public float BasePrice;
        public float BaseIncome;
        public List<GeneratorUpgradeInfo> Upgrades;
    }

    [Serializable]
    public struct GeneratorUpgradeInfo
    {
        public string Name;
        public float UpgradePrice;
        public float IncomeBonus;
        //public float DelayBonus; //should be less than zero
        public bool IsBought;
    }
}