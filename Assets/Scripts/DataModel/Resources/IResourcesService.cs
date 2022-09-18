using System;

namespace DataModel.Resources
{
    public interface IResourcesService
    {
        void AddResource(float amount, ResourceType type = ResourceType.Cash);
        bool TrySpendResource(float amount, ResourceType type = ResourceType.Cash);
        float GetResourceAmount(ResourceType type);

        event Action<ResourceType, float> ResourcesChanged;
    }
}