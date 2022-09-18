using UnityEngine;
using System.Collections.Generic;
using DataModel.Resources;
using UI.Views.Items;

namespace UI.Views
{
    public class ResourcesView : MonoBehaviour
    {
        [SerializeField]
        private ResourceItemView _prefab = null;
        [SerializeField]
        private RectTransform _container = null;

        private Dictionary<ResourceType, ResourceItemView> _items = new Dictionary<ResourceType, ResourceItemView>();
        private IResourcesService _resourcesService;


        public void Bind(IResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
            _resourcesService.ResourcesChanged += UpdateItemViews;            
        }

        public ResourceItemView InstantiateItemView(ResourceType resourceType)
        {
            var itemView = GameObject.Instantiate<ResourceItemView>(_prefab, _container);
            _items.Add(resourceType, itemView);

            return itemView;
        }

        private void UpdateItemViews(ResourceType resourceType, float amount)
        {
            if (_items.TryGetValue(resourceType, out var view))
                view.UpdateView(amount);
        }

        private void OnDestroy()
        {
            _resourcesService.ResourcesChanged -= UpdateItemViews;
        }
    }
}
