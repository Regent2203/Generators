using Saver;
using System.Collections.Generic;
using UI.Views;
using UI.Views.Items;

namespace DataModel.Resources.Config
{
    public class ResourcesConfigFactory
    {
        private ResourcesConfig _config;
        private ResourcesView _resourcesView;

        private ISaver _saver;


        public ResourcesConfigFactory(ResourcesConfig resourcesConfig, ResourcesView resourcesView, ISaver saver)
        {
            _config = resourcesConfig;
            _resourcesView = resourcesView;
            _saver = saver;
        }

        public IDictionary<ResourceType, float> CreateFromConfig()
        {
            var configData = _config.GetData();
            var resources = new Dictionary<ResourceType, float>();

            foreach (var confResInfo in configData)
            {
                var resInfo = confResInfo;
                if (_saver.LoadResource(resInfo.ResourceType, out var newAmount))
                    resInfo.StartAmount = newAmount;

                
                var resource = CreateResource(resInfo);                    
                resources.Add(resource.Item1, resource.Item2);

                var resourceItemView = CreateResourceItemView(resInfo);
            }

            return resources;
        }

        private (ResourceType, float) CreateResource(ResourceInfo info)
        {
            return (info.ResourceType, info.StartAmount);
        }

        private ResourceItemView CreateResourceItemView(ResourceInfo info)
        {
            var itemView = _resourcesView.InstantiateItemView(info.ResourceType);
            itemView.InitView(info.Name, info.DisplayFormat);
            itemView.UpdateView(info.StartAmount);

            return itemView;
        }
    }
}
        