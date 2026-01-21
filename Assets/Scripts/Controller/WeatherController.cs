using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WeatherApp.Loader;
using WeatherApp.View;

namespace WeatherApp.Controller
{
    public class WeatherController : MonoBehaviour
    {
        [Header("URL")]
        private const string _xmlUrl = "https://weather.tsukumijima.net/primary_area.xml";
        private const string _weatherDataUrl = "https://weather.tsukumijima.net/api/forecast/city/";
        
        [Header("View")]
        [SerializeField]
        private PrefScrollView _prefScrollView;
        [SerializeField]
        private WeatherDetailView _weatherDetailView;

        [Header("Loader")]
        private WeatherXmlLoader _weatherXmlLoader;
        private WeatherApiService _weatherApiService;
        private WeatherDataConverter _weatherDataConverter;
        
        private CityInfo _selectedCity;
        private List<PrefInfo> _prefList = new();
        private CancellationTokenSource _cts;

        private void Awake()
        {
            _weatherXmlLoader = new WeatherXmlLoader();
            _weatherApiService = new WeatherApiService();
            _weatherDataConverter = new WeatherDataConverter();
        }

        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            _prefList = await _weatherXmlLoader.LoadXmlAsync(_xmlUrl);
            _prefScrollView.SetUpPrefPanel(_prefList);
            _prefScrollView.OnCityClicked += OnCitySelected;
        }

        private void OnCitySelected(CityInfo cityInfo)
        {
            _selectedCity = cityInfo;
            
            // 連打対策
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            // ここに取得実装
            GetWeatherData().Forget();
        }

        // 選択された都市の情報を取得
        private async UniTaskVoid GetWeatherData()
        {
            try
            {
                Root root = await _weatherApiService.Request($"{_weatherDataUrl}{_selectedCity.Id}", _cts.Token);
                List<Sprite> icons = await LoadForecastIcons(root);
                Show(root, icons);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
        
        private async UniTask<List<Sprite>> LoadForecastIcons(Root root)
        {
            List<Sprite> icons = new();

            foreach (Forecast forecast in root.forecasts)
            {
                Sprite sprite = await _weatherApiService.LoadImage(forecast.image.url, _cts.Token);
                icons.Add(sprite);
            }

            return icons;
        }

        private void Show(Root root, List<Sprite> icons)
        {
            // 現在の天気
            _weatherDetailView.ShowCurrentWeatherDetail(root, _selectedCity.Name);
            // 時間別天気
            List<HourlyForecastsData> hourlyList = _weatherDataConverter.ConvertToHourly(root, icons[0]);
            _weatherDetailView.ShowHourlyForecastView(hourlyList);
            // 今日、明日、明後日
            List<DailyForecastData> dailyList = _weatherDataConverter.ConvertToDaily(root, icons);
            _weatherDetailView.ShowDailyForecastView(dailyList);
        }

        private void OnDestroy()
        {
            _prefScrollView.OnCityClicked -= OnCitySelected;
            _cts?.Cancel();
        }
    }
}