using Saver;
using System.Collections.Generic;
using UI.Views;
using UI.Views.Items;
using DataModel.Resources;

namespace DataModel.Generators.Config
{
    public class GeneratorsConfigFactory
    {
        private GeneratorsConfig _config;
        private GeneratorsView _generatorsView;
        private Progressor _generatorsProgressor;        

        private IResourcesService _resourcesService;

        private ISaver _saver;


        public GeneratorsConfigFactory(GeneratorsConfig generatorsConfig, GeneratorsView generatorsView, Progressor generatorsProcessor, IResourcesService resourcesService, ISaver saver)
        {
            _config = generatorsConfig;
            _generatorsView = generatorsView;
            _generatorsProgressor = generatorsProcessor;

            _resourcesService = resourcesService;
            _saver = saver;
        }

        public void CreateFromConfig()
        {
            var configData = _config.GetData();
            var tempUpgradesList = new List<(IncomeUpgrade, bool)>();

            int i = 0;
            int j = 0;
            foreach (var confGenInfo in configData)
            {
                var genInfo = confGenInfo;
                _saver.LoadGenerator(i, ref genInfo);


                var generator = CreateGenerator(i, genInfo);
                _generatorsProgressor.AddProgressable(generator, generator.Delay);
                
                var itemView = CreateGeneratorItemView(genInfo);
                
                foreach (var confUpgrInfo in genInfo.Upgrades)
                {
                    var upgrInfo = confUpgrInfo;
                    if (_saver.LoadUpgrade(j, out var isBought))
                        upgrInfo.IsBought = isBought;

                    var upgrade = CreateGeneratorUpgrade(j, upgrInfo);
                    tempUpgradesList.Add((upgrade, upgrInfo.IsBought));

                    if (upgrInfo.IsBought)
                        generator.AddUpgrade(upgrade);
                    j++;
                }

                
                var income = generator.GetIncome();
                if (genInfo.BaseIncome != income)
                    itemView.UpdateLabelIncome(income);

                Bind(generator, itemView, tempUpgradesList);                
                tempUpgradesList.Clear();
                i++;
            }
        }

        private ProgressGenerator CreateGenerator(int index, GeneratorInfo info)
        {
            var result = new ProgressGenerator(index, info.Name, info.Level, info.Progress, info.Delay, info.BasePrice, info.BaseIncome, _saver);

            return result;
        }

        private GeneratorItemView CreateGeneratorItemView(GeneratorInfo info)
        {
            var itemView = _generatorsView.InstantiateItemView();
            itemView.InitView(info.Name, info.Level, info.BaseIncome, info.BasePrice);

            int i = 0;
            foreach (var upgrInfo in info.Upgrades)
            {
                itemView.UpdateButtonUpgradeText(i, false, upgrInfo.Name, upgrInfo.IncomeBonus, upgrInfo.UpgradePrice);
                i++;
            }

            return itemView;
        }

        private IncomeUpgrade CreateGeneratorUpgrade(int index, GeneratorUpgradeInfo info)
        {
            var result = new IncomeUpgrade(index, info.Name, info.UpgradePrice, info.IncomeBonus);

            return result;
        }


        private void Bind(ProgressGenerator generator, GeneratorItemView itemView, IList<(IncomeUpgrade, bool)> upgrades)
        {
            //captions
            generator.LvlChanged += itemView.UpdateLabelLevel;
            generator.LvlChanged += (_) => itemView.UpdateLabelIncome(generator.GetIncome());
            generator.LvlChanged += (_) => itemView.UpdateButtonLevelUpText(generator.GetLevelUpCost());

            generator.UpgradesChanged += () => itemView.UpdateLabelIncome(generator.GetIncome());
            
            generator.ProgressUpdated += itemView.UpdateProgress;
            generator.ResourceGenerated += _resourcesService.AddResource;


            //button events
            itemView.BtnLevelUp.onClick.AddListener(() =>
            {
                if (_resourcesService.TrySpendResource(generator.GetLevelUpCost()))
                    generator.LevelUp();
            });

            for (int i = 0; i < upgrades.Count; i++) //both upgrades and BtnsUpgrade in this example contain two elements
            {
                var upgrade = upgrades[i].Item1;
                var index = i;

                if (upgrades[i].Item2)
                {
                    OnUpgradeBought(index, upgrade);
                }
                else
                    itemView.BtnsUpgrade[i].onClick.AddListener(() =>
                    {
                        if (generator.Level > 0 && !generator.HasUpgrade(upgrade))
                            if (_resourcesService.TrySpendResource(upgrade.Price))
                            {
                                generator.AddUpgrade(upgrade);

                                OnUpgradeBought(index, upgrade);
                            }
                    });
            }

            void OnUpgradeBought(int index, IncomeUpgrade upgrade)
            {
                itemView.UpdateButtonUpgradeText(index, true, "", upgrade.GetIncomeBonus(), upgrade.Price);
                itemView.BtnsUpgrade[index].interactable = false;
            }
        }
    }
}