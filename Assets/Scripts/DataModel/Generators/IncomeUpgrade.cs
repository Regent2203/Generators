using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataModel.Generators
{
    public interface IUpgrade
    {
        int Index { get; }
        float Price { get; }
    }

    public class IncomeUpgrade : IUpgrade
    {
        private int _index;
        private string _name;
        private float _price;
        private float _incomeBonus;

        public int Index => _index;
        public float Price => _price;


        public IncomeUpgrade(int index, string name, float price, float incomeBonus)
        {
            _index = index;
            _name = name;
            _price = price;
            _incomeBonus = incomeBonus;
        }

        public float GetIncomeBonus()
        {
            return _incomeBonus;
        }
    }
}