using DataModel.Resources;
using System.Collections.Generic;
using UI.Views;
using UI.Views.Items;

namespace DataModel.Generators.Config
{
    public class GeneratorsConfigFactory
    {
        private GeneratorsConfig _config;
        private GeneratorsView _generatorsView;
        private Progressor _generatorsProgressor;        

        private IResourcesService _resourcesService;


        public GeneratorsConfigFactory(GeneratorsConfig generatorsConfig, GeneratorsView generatorsView, Progressor generatorsProcessor, IResourcesService resourcesService)
        {
            _config = generatorsConfig;
            _generatorsView = generatorsView;
            _generatorsProgressor = generatorsProcessor;

            _resourcesService = resourcesService;
        }

        public void CreateFromConfig()
        {
            var configData = _config.GetData();
            var tempUpgradesList = new List<IncomeUpgrade>();

            foreach (var genInfo in configData)
            {
                var generator = CreateGenerator(genInfo);
                _generatorsProgressor.AddProgressable(generator, generator.Delay);

                var itemView = CreateGeneratorItemView(genInfo);

                foreach (var upgrInfo in genInfo.Upgrades)
                {
                    var upgrade = CreateGeneratorUpgrade(upgrInfo);
                    tempUpgradesList.Add(upgrade);
                }

                Bind(generator, itemView, tempUpgradesList);
                tempUpgradesList.Clear();
            }
        }

        private ProgressGenerator CreateGenerator(GeneratorInfo info)
        {
            var result = new ProgressGenerator(info.Name, info.Level, info.Delay, info.BasePrice, info.BaseIncome);

            return result;
        }

        private GeneratorItemView CreateGeneratorItemView(GeneratorInfo info)
        {
            var itemView = _generatorsView.InstantiateItemView();
            itemView.InitView(info.Name, 0, info.BaseIncome, info.BasePrice);

            int i = 0;
            foreach (var upgrInfo in info.Upgrades)
            {
                itemView.UpdateButtonUpgradeText(i, false, upgrInfo.Name, upgrInfo.IncomeBonus, upgrInfo.UpgradePrice);
                i++;
            }

            return itemView;
        }

        private IncomeUpgrade CreateGeneratorUpgrade(GeneratorUpgradeInfo info)
        {
            var result = new IncomeUpgrade(info.Name, info.UpgradePrice, info.IncomeBonus);

            return result;
        }


        private void Bind(ProgressGenerator generator, GeneratorItemView itemView, IList<IncomeUpgrade> upgrades)
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
                var upgrade = upgrades[i];
                var index = i;

                itemView.BtnsUpgrade[i].onClick.AddListener(() =>
                {
                    if (generator.Level > 0 && !generator.HasUpgrade(upgrade))
                        if (_resourcesService.TrySpendResource(upgrade.Price))
                        {
                            generator.AddUpgrade(upgrade);

                            itemView.UpdateButtonUpgradeText(index, true, "", upgrade.GetIncomeBonus(), upgrade.Price);
                            itemView.BtnsUpgrade[index].interactable = false;
                        }
                });
            }
        }
    }
}