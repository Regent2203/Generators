using UnityEngine;
using System.Collections.Generic;
using DataModel.Generators;
using UI.Views.Items;

namespace UI.Views
{
    public class GeneratorsView : MonoBehaviour
    {
        [SerializeField]
        private GeneratorItemView _prefab = null;
        [SerializeField]
        private RectTransform _container = null;

        //private List<GeneratorItemView> _items;
        //private IGeneratorsService _generatorsService;

        /*
        public void Bind(IGeneratorsService generatorsService)
        {
            _generatorsService = generatorsService;
        }*/

        public GeneratorItemView InstantiateItemView()
        {
            var itemView = GameObject.Instantiate<GeneratorItemView>(_prefab, _container);
            //_items.Add(itemView);

            return itemView;
        }
    }
}
