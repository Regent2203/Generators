using DataModel.Resources;
using System;

namespace DataModel.Generators
{
    public interface IGenerator
    {
        ResourceType ResourceType { get; }        

        event Action<float, ResourceType> ResourceGenerated;
    }

    public interface IProgressable
    {
        bool IsActive { get; }
        float Delay { get; }
        void AddProgress(float step);
        
        public event Action<float> ProgressUpdated;
    }

    public interface ILeveled
    {
        int Level { get; }
        void LevelUp();
        void LevelDown();

        event Action<int> LvlChanged;
    }

    public interface IUpgradable
    {
        void AddUpgrade(IUpgrade upgrade);
        bool HasUpgrade(IUpgrade upgrade);
        void RemoveUpgrade(IUpgrade upgrade);

        event Action UpgradesChanged;
    }
}