using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataModel.Generators
{
    public interface IUpgrade
    {
        float Price { get; }
    }

    public class IncomeUpgrade : IUpgrade
    {
        private string _name;
        private float _price;
        private float _incomeBonus;

        public float Price => _price;


        public IncomeUpgrade(string name, float price, float incomeBonus)
        {
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