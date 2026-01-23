using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherApp.View
{
    public class WeatherDetailView : MonoBehaviour
    {
        [Header("Forecast View")]
        [SerializeField] 
        private HourlyForecastView _hourlyForecastView; 
        [SerializeField]
        private DailyForecastView _dailyForecastView;
        
        [SerializeField]
        private CurrentForecastItem _currentForecastItem;

        
        [SerializeField]
        private UIButton _backButton;

        public event Action OnBackClicked;

        private void Start()
        {
            _backButton.OnClickAddListener(() => OnBackClicked?.Invoke());
        }

        /// <summary>
        /// 現在の天気
        /// </summary>
        /// <param name="data"></param>
        public void ShowCurrent(CurrentForecastData data)
        {
            _currentForecastItem.SetItemUI(data);
        }
        
        /// <summary>
        /// 1時間ごとの天気予想図
        /// </summary>
        /// <param name="hourlyList"></param>
        public void ShowHourlyForecastView(List<HourlyForecastsData> hourlyList)
        {
            _hourlyForecastView.Show(hourlyList);
        }

        public void ShowDailyForecastView(List<DailyForecastData> dailyList)
        {
            _dailyForecastView.Show(dailyList);
        }
    }
}