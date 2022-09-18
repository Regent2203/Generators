using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataModel.Resources.Config
{
    [CreateAssetMenu(menuName = "Configs/ResourcesConfig")]
    public class ResourcesConfig : ScriptableObject
    {
        [SerializeField]
        private List<ResourceInfo> _resources = new List<ResourceInfo>();        


        public IEnumerable<ResourceInfo> GetData()
        {
            var data = _resources.Distinct();            
            
            return data;
        }
    }

    [Serializable]
    public struct ResourceInfo
    {
        public ResourceType ResourceType;
        public float StartAmount;
        public string Name;
        public string DisplayFormat;
    }
}