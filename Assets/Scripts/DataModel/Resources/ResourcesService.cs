using System.Collections.Generic;
using UnityEngine;
using System;

namespace DataModel.Resources
{
    public class ResourcesService : IResourcesService
    {
        private IDictionary<ResourceType, float> _resources;

        public event Action<ResourceType, float> ResourcesChanged;


        public ResourcesService (IDictionary<ResourceType, float> resources)
        {
            _resources = resources;
        }

        public void AddResource(float amountAdd, ResourceType type = ResourceType.Cash)
        {
            if (_resources.ContainsKey(type))
            {
                _resources[type] += amountAdd;
                ResourcesChanged?.Invoke(type, _resources[type]);
            }
            else
                Debug.LogError($"Resource of type '{type}' has not been found! Check config.");
        }

        public bool TrySpendResource(float amountRequired, ResourceType type = ResourceType.Cash)
        {
            if (_resources.TryGetValue(type, out var amountTotal) && (amountTotal >= amountRequired))
            {
                _resources[type] -= amountRequired;
                ResourcesChanged?.Invoke(type, _resources[type]);
                return true;
            }
            else
            {
                Debug.LogError($"Resource of type '{type}' has not been found! Check config.");
                return false;
            }
        }

        public float GetResourceAmount(ResourceType type)
        {
            if (_resources.ContainsKey(type))
            {
                return _resources[type];
            }
            else
            {
                Debug.LogError($"Resource of type '{type}' has not been found! Check config.");
                return 0;
            }
        }
    }
}