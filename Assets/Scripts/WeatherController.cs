using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField]
    private string _xmlUrl = "https://weather.tsukumijima.net/primary_area.xml";
    [SerializeField]
    private string _weatherDataUrl = "https://weather.tsukumijima.net/api/forecast/city/";
    [SerializeField]
    private PrefScrollView _prefScrollView;
    
    private WeatherXmlLoader _weatherXmlLoader;
    private WeatherApiService _weatherApiService;
    private CityInfo _selectedCity;
    private List<PrefInfo> _cityIDList = new();
    private CancellationTokenSource _cts;
    private void Awake()
    {
        _weatherXmlLoader = new WeatherXmlLoader();
        _weatherApiService =  new WeatherApiService();
        _cts = new CancellationTokenSource();
    }

    private void Start()
    {
        Initialize().Forget();
    }

    private async UniTaskVoid Initialize()
    {
        await GetCityIDList();
        _prefScrollView.SetUpPrefPanel(_cityIDList);
        _prefScrollView.OnCityClicked += OnCitySelected;
    }

    private async UniTask GetCityIDList()
    {
        _cityIDList = await _weatherXmlLoader.LoadXmlAsync(_xmlUrl);
    }

    private void OnCitySelected(CityInfo cityInfo)
    {
        _selectedCity = cityInfo;
        // ここに取得実装
        GetWeatherData().Forget();
    }

    // 選択された都市の情報を取得
    private async UniTaskVoid GetWeatherData()
    {
        Root root = await _weatherApiService.Request($"{_weatherDataUrl}{_selectedCity.Id}", _cts.Token);
        Debug.Log(root.publicTime);
    }

    private void OnDestroy()
    {
        _prefScrollView.OnCityClicked -= OnCitySelected;
    }
}