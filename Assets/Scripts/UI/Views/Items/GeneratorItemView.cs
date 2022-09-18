using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Items
{
    public class GeneratorItemView : MonoBehaviour
    {        
        [SerializeField]
        private Text _txtName;
        [SerializeField]
        private Image _progressBar;
        [SerializeField]
        private Text _txtLevel;        
        [SerializeField]
        private Text _txtIncome;
        

        [SerializeField]
        private Button _btnLevelUp;
        [SerializeField]
        private List<Button> _btnsUpgrade;

        public Button BtnLevelUp => _btnLevelUp;
        public List<Button> BtnsUpgrade => _btnsUpgrade;


        public void InitView(string generatorName, int lvl, float income, float lvlupCost)
        {
            _txtName.text = generatorName;
            UpdateProgress(0);
            UpdateLabelLevel(lvl);
            UpdateLabelIncome(income);

            UpdateButtonLevelUpText(lvlupCost);
        }

        public void UpdateProgress(float progress)
        {
            _progressBar.fillAmount = progress;
        }

        public void UpdateLabelLevel(int lvl)
        {
            _txtLevel.text = $"LVL\n{lvl}";
        }

        public void UpdateLabelIncome(float income)
        {
            _txtIncome.text = $"Доход\n{income}";
        }

        public void UpdateButtonLevelUpText(float price)
        {
            _btnLevelUp.GetComponentInChildren<Text>().text = $"LVL UP\nЦена: {price.ToString("0")}";
        }

        public void UpdateButtonUpgradeText(int index, bool isBought, string upgradeName, float incomeBonus, float upgradePrice)
        {
            _btnsUpgrade[index].GetComponentInChildren<Text>().text = (isBought ? "Куплено" : upgradeName) + $"\nДоход: +{(100 * incomeBonus).ToString("0")} %\nЦена: {upgradePrice.ToString("0")}";            
        }
    }
}