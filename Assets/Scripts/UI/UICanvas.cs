using UI.Views;
using UnityEngine;

namespace UI
{
    public class UICanvas : MonoBehaviour
    {
        [SerializeField]
        private ResourcesView _resourcesView;

        [SerializeField]
        private GeneratorsView _generatorsView;

        public ResourcesView ResourcesView => _resourcesView;
        public GeneratorsView GeneratorsView => _generatorsView;
    }
}