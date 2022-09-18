using UnityEngine;
using UI;
using DataModel.Resources;
using DataModel.Generators;
using DataModel.Generators.Config;
using UnityEditor;
using DataModel.Resources.Config;

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


    private void Awake()
    {
        if (_resourcesConfig == null)
            _resourcesConfig = AssetDatabase.LoadAssetAtPath<ResourcesConfig>(RES_CONFIG_PATH);

        if (_generatorsConfig == null)
            _generatorsConfig = AssetDatabase.LoadAssetAtPath<GeneratorsConfig>(GEN_CONFIG_PATH);
    }

    void Start()
    {
        InitResources();
        InitGenerators();
        LoadPlayerData();       
    }

    private void InitResources()
    {
        var resourcesConfigFactory = new ResourcesConfigFactory(_resourcesConfig, _uiCanvas.ResourcesView);
        var resources = resourcesConfigFactory.CreateFromConfig();

        _resourcesService = new ResourcesService(resources);

        _uiCanvas.ResourcesView.Bind(_resourcesService);
    }

    private void InitGenerators()
    {
        var generatorsProcessor = this.gameObject.AddComponent<Progressor>();

        var generatorsConfigFactory = new GeneratorsConfigFactory(_generatorsConfig, _uiCanvas.GeneratorsView, generatorsProcessor, _resourcesService);
        generatorsConfigFactory.CreateFromConfig();
    }

    private void LoadPlayerData()
    {
        //todo: load player data
        _resourcesService.AddResource(1000000, ResourceType.Cash);
    }
}