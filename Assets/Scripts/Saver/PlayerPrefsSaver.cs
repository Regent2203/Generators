using UnityEngine;
using DataModel.Resources;
using DataModel.Generators;
using DataModel.Generators.Config;
using System.Collections.Generic;
using System.Text;

namespace Saver
{
    public interface ISaver
    {
        void SaveResource(ResourceType resourceType, float amount);
        bool LoadResource(ResourceType resourceType, out float amount);

        void SaveGenerator(int index, int level, float progress);
        bool LoadGenerator(int index, ref GeneratorInfo info);

        public void SaveUpgrade(int index);
        public bool LoadUpgrade(int index, out bool isBought);
    }

    public class PlayerPrefsSaver : ISaver
    {
        private StringBuilder _stringBuilder = new StringBuilder();

        const string SPLITTER = ":";

        const string LABEL_RESOURCE = "res_";
        const string LABEL_GENERATOR = "gen_";
        const string LABEL_UPGRADE = "upgr_";


        public void SaveResource(ResourceType resourceType, float amount)
        {            
            PlayerPrefs.SetFloat(LABEL_RESOURCE + resourceType.ToString(), amount);
            PlayerPrefs.Save();
        }

        public bool LoadResource(ResourceType resourceType, out float amount)
        {
            amount = 0;

            if (PlayerPrefs.HasKey(LABEL_RESOURCE + resourceType.ToString()))
            {
                amount = PlayerPrefs.GetFloat(LABEL_RESOURCE + resourceType.ToString());
                return true;
            }
            else
                return false;
        }

        public void SaveGenerator(int index, int level, float progress)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(level);
            _stringBuilder.Append(SPLITTER);
            _stringBuilder.Append(progress);
            _stringBuilder.Append(SPLITTER);

            var str = _stringBuilder.ToString();
            
            PlayerPrefs.SetString(LABEL_GENERATOR + index.ToString(), str);
            PlayerPrefs.Save();
        }

        public bool LoadGenerator(int index, ref GeneratorInfo info)
        {
            if (PlayerPrefs.HasKey(LABEL_GENERATOR + index.ToString()))
            {
                var str = PlayerPrefs.GetString(LABEL_GENERATOR + index.ToString());
                var datas = str.Split(SPLITTER);
                info.Level = int.Parse(datas[0]);
                info.Progress = float.Parse(datas[1]);

                return true;
            }
            else
                return false;
        }

        public void SaveUpgrade(int index)
        {
            PlayerPrefs.SetInt(LABEL_UPGRADE + index.ToString(), 1); //because playerprefs doesn't work with bool
            PlayerPrefs.Save();
        }

        public bool LoadUpgrade(int index, out bool isBought)
        {
            isBought = false;            

            if (PlayerPrefs.HasKey(LABEL_UPGRADE + index.ToString()))
            {
                var state = PlayerPrefs.GetInt(LABEL_UPGRADE + index.ToString());
                isBought = state > 0;

                return true;
            }
            else
                return false;
        }
    }
}