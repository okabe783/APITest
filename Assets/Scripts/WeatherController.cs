using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField]
    private string _url = "https://weather.tsukumijima.net/primary_area.xml";
    [SerializeField]
    private PrefScrollView _prefScrollView;
    
    private WeatherXmlLoader _weatherXmlLoader;
    private List<PrefInfo> _cityIDList = new();
    private void Awake()
    {
        _weatherXmlLoader = new WeatherXmlLoader();
    }

    private void Start()
    {
        Initialize().Forget();
    }

    private async UniTaskVoid Initialize()
    {
        await GetCityIDList();
        _prefScrollView.SetUpPrefPanel(_cityIDList);
    }

    private async UniTask GetCityIDList()
    {
        _cityIDList = await _weatherXmlLoader.LoadXmlAsync(_url);
    }
}