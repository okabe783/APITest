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
        // 天候情報のView表示時に表示するUI
        [SerializeField]
        private WeatherDetailView _weatherDetailNormalView;
        [SerializeField]
        // 都市情報View表示のときに表示するUI
        private CurrentForecastItem _currentForecastStartItem;

        [Header("Loader")]
        private WeatherXmlLoader _weatherXmlLoader;
        private WeatherApiService _weatherApiService;
        private WeatherDataConverter _weatherDataConverter;
        
        private CityInfo _selectedCity;
        private List<PrefInfo> _prefList = new();
        private CancellationTokenSource _cts;
        
        // Prefs用保存キー
        private const string _lastCityIdKey = "LAST_CITY_ID";

        private void Awake()
        {
            _weatherXmlLoader = new WeatherXmlLoader();
            _weatherApiService = new WeatherApiService();
            _weatherDataConverter = new WeatherDataConverter();
        }

        private void Start()
        {
            Initialize().Forget();
            _weatherDetailNormalView.OnBackClicked += OnBackFromWeather;
        }

        private async UniTaskVoid Initialize()
        {
            // 都市情報を取得
            _prefList = await _weatherXmlLoader.LoadXmlAsync(_xmlUrl);
            _prefScrollView.SetUpPrefPanel(_prefList);
            _prefScrollView.OnCityClicked += OnCitySelected;
            LoadPrefs().Forget();
            ShowCitySelect();
        }

        // 主要都市をクリックしたときの処理
        private void OnCitySelected(CityInfo cityInfo)
        {
            _selectedCity = cityInfo;
            // Prefs に保存
            SavePrefs(cityInfo);
            // 天気表示フロー開始
            ShowWeatherFlow().Forget();
        }
        
        private async UniTaskVoid ShowWeatherFlow()
        {
            // 連打対策
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            (Root root, List<Sprite> icons) = await GetWeatherData();
            ShowForecastView(root, icons);
            ShowWeatherDetail();
        }

        private void SavePrefs(CityInfo cityInfo)
        {
            PlayerPrefs.SetString(_lastCityIdKey,cityInfo.Id);
            PlayerPrefs.Save();
        }

        private async UniTaskVoid LoadPrefs()
        {
            if(!PlayerPrefs.HasKey(_lastCityIdKey)) return;
            
            string lastCityId = PlayerPrefs.GetString(_lastCityIdKey);
            CityInfo cityInfo = Find(lastCityId);
            
            if(cityInfo == null) return;
            
            _selectedCity = cityInfo;
            // 最後に表示した都市の天気を表示
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            // 天気取得
            (Root root, List<Sprite> icons) = await GetWeatherData();
            // Current用データ生成
            CurrentForecastData currentData = _weatherDataConverter.ConvertToCurrent(root, icons[0], _selectedCity.Name);
            // Start用アイテムにセット
            _currentForecastStartItem.SetItemUI(currentData);

            // 表示制御
            ShowLastSelectedCity(true);
        }

        private CityInfo Find(string id)
        {
            foreach (PrefInfo pref in _prefList)
            {
                foreach (CityInfo city in pref.Cities)
                {
                    if(city.Id == id) return city;
                }
            }
            return null;
        }

        // 選択された都市の情報を取得
        private async UniTask<(Root root,List<Sprite> icons)> GetWeatherData()
        { 
            Root root = await _weatherApiService.Request($"{_weatherDataUrl}{_selectedCity.Id}", _cts.Token); 
            List<Sprite> icons = await LoadForecastIcons(root); 
            return (root, icons);
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

        private void ShowForecastView(Root root, List<Sprite> icons)
        {
            // 現在の天気
            CurrentForecastData currentForecastData = _weatherDataConverter.ConvertToCurrent(root,icons[0],_selectedCity.Name);
            _weatherDetailNormalView.ShowCurrent(currentForecastData);
            // 時間別天気
            List<HourlyForecastsData> hourlyList = _weatherDataConverter.ConvertToHourly(root, icons[0]);
            _weatherDetailNormalView.ShowHourlyForecastView(hourlyList);
            // 今日、明日、明後日
            List<DailyForecastData> dailyList = _weatherDataConverter.ConvertToDaily(root, icons);
            _weatherDetailNormalView.ShowDailyForecastView(dailyList);
        }
        
        // 天候情報の戻る処理
        private void OnBackFromWeather()
        {
            _cts?.Cancel();
            ShowCitySelect();
        }

        #region Viewの表示非表示

        private void ShowCitySelect()
        {
            _currentForecastStartItem.gameObject.SetActive(false);
            _prefScrollView.gameObject.SetActive(true);
            _weatherDetailNormalView.gameObject.SetActive(false);
        }

        private void ShowWeatherDetail()
        {
            ShowLastSelectedCity(false);
            _prefScrollView.gameObject.SetActive(false);
            _weatherDetailNormalView.gameObject.SetActive(true);
        }

        private void ShowLastSelectedCity(bool value)
        {
            _currentForecastStartItem.gameObject.SetActive(value);
        }

        #endregion
        

        private void OnDestroy()
        {
            _prefScrollView.OnCityClicked -= OnCitySelected;
            _weatherDetailNormalView.OnBackClicked -= OnBackFromWeather;
            _cts?.Cancel();
        }
    }
}