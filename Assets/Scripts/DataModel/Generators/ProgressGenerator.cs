using System;
using System.Collections;
using System.Collections.Generic;
using DataModel.Resources;
using Saver;

namespace DataModel.Generators
{
    /// <summary>
    /// An object that produces resources over time (business)
    /// </summary>
    public class ProgressGenerator : IGenerator, IProgressable, IUpgradable
    {
        private int _index;
        private ResourceType _resourceType; //which resource is produced by generator
        private string _name;
        private int _level;
        private float _delay;
        private float _basePrice;
        private float _baseIncome;
        private IList<IUpgrade> _upgrades = new List<IUpgrade>();
        private ISaver _saver;

        public event Action<float, ResourceType> ResourceGenerated;
        public event Action<float> ProgressUpdated;
        public event Action<int> LvlChanged;
        public event Action UpgradesChanged;

        public int Index => _index;
        public ResourceType ResourceType => _resourceType;
        public int Level => _level;        
        public bool IsActive => _level > 0;
        public float Delay => _delay;
        public float Progress { get { return _progress; } set { _progress = value; } }


        public ProgressGenerator(int index, string name, int level, float progress, float delay, float basePrice, float baseIncome, ISaver saver, ResourceType resourceType = ResourceType.Cash)
        {
            _index = index;
            _resourceType = resourceType;
            _name = name;
            _level = level;
            _progress = progress;
            _delay = delay;
            _basePrice = basePrice;
            _baseIncome = baseIncome;
            _saver = saver;
            
            _maxProgress = 1.0f;

            LvlChanged += (_) => _saver.SaveGenerator(_index, _level, _progress);
            ProgressUpdated += (_) => _saver.SaveGenerator(_index, _level, _progress);
            UpgradesChanged += () =>
            {
                foreach (var upgr in _upgrades)
                    _saver.SaveUpgrade(upgr.Index);
            };
        }

        private void GenerateResource()
        {
            ResourceGenerated?.Invoke(GetIncome(), _resourceType);
        }

        public float GetIncome()
        {
            //Доход = lvl* базовый_доход * (1 + множитель_от_улучшения_1 + множитель_от_улучшения_2)
            float totalUpgradeBonus = 0;

            foreach (var upgr in _upgrades)
                if (upgr is IncomeUpgrade upgrade)
                    totalUpgradeBonus += upgrade.GetIncomeBonus();

            return _level * _baseIncome * (1 + totalUpgradeBonus);
        }        

        public float GetLevelUpCost()
        {
            //Цена уровня = cost = (lvl + 1) * базовая_стоимость
            return _basePrice * (1 + _level);
        }

        #region IProgressable
        private float _progress;
        private float _maxProgress; //хардкодим в 1.0f в данном примере

        public void AddProgress(float step)
        {
            _progress += step;

            if (_progress >= _maxProgress)
            {
                _progress = 0;
                GenerateResource();
            }
            ProgressUpdated?.Invoke(_progress);
        }
        #endregion

        #region ILeveled       
        public void LevelUp()
        {            
            _level++;
            LvlChanged?.Invoke(_level);            
        }

        public void LevelDown()
        {
            _level--;
            LvlChanged?.Invoke(_level);
        }
        #endregion

        #region IUpgradeable        
        public void AddUpgrade(IUpgrade upgrade)
        {
            if (!HasUpgrade(upgrade))
            {
                _upgrades.Add(upgrade);
                UpgradesChanged?.Invoke();
            }
        }

        public bool HasUpgrade(IUpgrade upgrade)
        {
            return _upgrades.Contains(upgrade);
        }

        public void RemoveUpgrade(IUpgrade upgrade)
        {
            if (HasUpgrade(upgrade))
            {
                _upgrades.Remove(upgrade);
                UpgradesChanged?.Invoke();
            }
        }
        #endregion
    }
}