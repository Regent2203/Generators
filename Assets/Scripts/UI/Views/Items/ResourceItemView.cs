using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Items
{
    public class ResourceItemView : MonoBehaviour
    {
        [SerializeField]
        private Text _txtName;

        private string _resourceName;
        private string _format = "{0}: {1}";


        public void InitView(string resourceName, string format)
        {
            _resourceName = resourceName;
            _format = format;
        }

        public void UpdateView(float resValue)
        {
            _txtName.text = string.Format(_format, _resourceName, resValue.ToString("0"));
        }        
    }
}
