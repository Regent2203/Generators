using UnityEngine;
using UnityEditor;
using UI;
using DataModel.Resources;
using DataModel.Generators;
using DataModel.Generators.Config;
using DataModel.Resources.Config;
using Saver;

public class Services : MonoBehaviour
{
    [SerializeField]
    private UICanvas _uiCanvas;

    [Space]
    [Header("Configs")]
    [SerializeField]
    private ResourcesConfig _resourcesConfig;
    [SerializeField]
    private GeneratorsConfig _generatorsConfig;

    const string RES_CONFIG_PATH = "Assets/Configs/ResourcesConfig.asset"; //либо через инспектор
    const string GEN_CONFIG_PATH = "Assets/Configs/GeneratorsConfig.asset"; //либо через инспектор


    private IResourcesService _resourcesService;
    private ISaver _saver;


    private void Awake()
    {
#if !UNITY_ANDROID
        if (_resourcesConfig == null)
            _resourcesConfig = AssetDatabase.LoadAssetAtPath<ResourcesConfig>(RES_CONFIG_PATH);

        if (_generatorsConfig == null)
            _generatorsConfig = AssetDatabase.LoadAssetAtPath<GeneratorsConfig>(GEN_CONFIG_PATH);
#endif
    }

    void Start()
    {
        InitSaver();
        InitResources();
        InitGenerators();
        //_resourcesService.AddResource(1000000, ResourceType.Cash);      
    }

    private void InitSaver()
    {
        _saver = new PlayerPrefsSaver();
    }

    private void InitResources()
    {
        var resourcesConfigFactory = new ResourcesConfigFactory(_resourcesConfig, _uiCanvas.ResourcesView, _saver);
        var resources = resourcesConfigFactory.CreateFromConfig();

        _resourcesService = new ResourcesService(resources);
        _resourcesService.ResourcesChanged += _saver.SaveResource;

        _uiCanvas.ResourcesView.Bind(_resourcesService);
    }

    private void InitGenerators()
    {
        var generatorsProcessor = this.gameObject.AddComponent<Progressor>();

        var generatorsConfigFactory = new GeneratorsConfigFactory(_generatorsConfig, _uiCanvas.GeneratorsView, generatorsProcessor, _resourcesService, _saver);
        generatorsConfigFactory.CreateFromConfig();
    }
}